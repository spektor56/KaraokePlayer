using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CdgLib
{
    public class CdgFile : FileStream
    {
        private const int ColourTableSize = 16;
        private const byte CdgCommand = 0x9;
        private const int CdgInstMemoryPreset = 1;
        private const int CdgInstBorderPreset = 2;
        private const int CdgInstTileBlock = 6;
        private const int CdgInstScrollPreset = 20;
        private const int CdgInstScrollCopy = 24;
        private const int CdgInstDefTranspCol = 28;
        private const int CdgInstLoadColTblLo = 30;
        private const int CdgInstLoadColTblHigh = 31;
        private const int CdgInstTileBlockXor = 38;
        private const byte CdgMask = 0x3f;
        private const int CdgPacketSize = 24;
        private const int TileHeight = 12;
        private const int TileWidth = 6;
        public const int CdgFullWidth = 300;
        public const int CdgFullHeight = 216;
        private const int CdgDisplayWidth = 294;
        private const int CdgDisplayHeight = 204;
        private readonly byte[,] _mPixelColours = new byte[CdgFullHeight, CdgFullWidth];
        private readonly int[] _mColourTable = new int[ColourTableSize];
        private int _mPresetColourIndex;
        private int _mBorderColourIndex;
        private int _mTransparentColour;
        private int _mHOffset;
        private int _mVOffset;
        private CdgFileIoStream _mPStream;
        private Surface _mPSurface;
        private long _previousPosition;
        private long _mDuration;
        private Bitmap _mImage;
        public bool Transparent => true;

        public Image RgbImage
        {
            get
            {
                Stream temp = new MemoryStream();
                try
                {
                    var i = 0;
                    for (var ri = 0; ri <= CdgFullHeight - 1; ri++)
                    {
                        for (var ci = 0; ci <= CdgFullWidth - 1; ci++)
                        {
                            var argbInt = _mPSurface.RgbData[ri, ci];
                            var myByte = new byte[4];
                            myByte = BitConverter.GetBytes(argbInt);
                            temp.Write(myByte, 0, 4);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Do nothing (empty bitmap will be returned)
                }
                var myBitmap = GraphicUtil.StreamToBitmap(ref temp, CdgFullWidth, CdgFullHeight);
                if (Transparent)
                {
                    myBitmap.MakeTransparent(myBitmap.GetPixel(1, 1));
                }
                return myBitmap;
            }
        }

        //Png Export
        public void SavePng(string filename)
        {
            RgbImage.Save(filename, ImageFormat.Png);
        }

        //New
        public CdgFile(string path, FileMode mode, FileAccess fileAccess) : base(path, mode, fileAccess)
        {
            _mPSurface = new Surface();
        }


        private long Duration => Length / CdgPacketSize * 1000 / 300;

        public long GetTotalDuration()
        {
            return _mDuration;
        }

        public async Task<bool> RenderAtPosition(long position)
        {
            if (position < _previousPosition)
            {
                Position = 0;
                _previousPosition = 0;
            }

            //duration of one packet is 1/300 seconds (4 packets per sector, 75 sectors per second)
            //p=t*3/10  t=p*10/3 t=milliseconds, p=packets
            var timeToRender = position - _previousPosition;
            _previousPosition += timeToRender;
            var numberOfSubCodePackets = timeToRender*3/10;
            while (numberOfSubCodePackets > 0)
            {
                var pack = await ReadSubCode();
                ProcessPacket(pack);
                numberOfSubCodePackets -= 1;
            }

            Render();
            return true;
        }

        private async Task<SubCodePacket> ReadSubCode()
        {
            var subCode = new SubCodePacket();
            byte[] buffer = new byte[24];
            var bytesRead = await ReadAsync(buffer, 0, 24);

            if (bytesRead < 24)
            {
                return new SubCodePacket();
            }

            Array.Copy(buffer, 0, subCode.Command, 0, 1);
            Array.Copy(buffer, 1, subCode.Instruction, 0, 1);
            Array.Copy(buffer, 2, subCode.ParityQ, 0, 2);
            Array.Copy(buffer, 4, subCode.Data, 0, 16);
            Array.Copy(buffer, 20, subCode.ParityP, 0, 4);

            return subCode;
        }


        private void ProcessPacket(SubCodePacket subCodePacketPacket)
        {
            if ((subCodePacketPacket.Command[0] & CdgMask) != CdgCommand) return;
            var instructionCode = subCodePacketPacket.Instruction[0] & CdgMask;
            switch (instructionCode)
            {
                case CdgInstMemoryPreset:
                    MemoryPreset(ref subCodePacketPacket);
                    break;
                case CdgInstBorderPreset:
                    BorderPreset(ref subCodePacketPacket);
                    break;
                case CdgInstTileBlock:
                    TileBlock(ref subCodePacketPacket, false);
                    break;
                case CdgInstScrollPreset:
                    Scroll(ref subCodePacketPacket, false);
                    break;
                case CdgInstScrollCopy:
                    Scroll(ref subCodePacketPacket, true);
                    break;
                case CdgInstDefTranspCol:
                    DefineTransparentColour(ref subCodePacketPacket);
                    break;
                case CdgInstLoadColTblLo:
                    LoadColorTable(ref subCodePacketPacket, 0);
                    break;
                case CdgInstLoadColTblHigh:
                    LoadColorTable(ref subCodePacketPacket, 1);
                    break;
                case CdgInstTileBlockXor:
                    TileBlock(ref subCodePacketPacket, true);
                    break;
            }
        }


        private void MemoryPreset(ref SubCodePacket pack)
        {
            var colour = 0;
            var ri = 0;
            var ci = 0;
            var repeat = 0;

            colour = pack.Data[0] & 0xf;
            repeat = pack.Data[1] & 0xf;

            //Our new interpretation of CD+G Revealed is that memory preset
            //commands should also change the border
            _mPresetColourIndex = colour;
            _mBorderColourIndex = colour;

            //we have a reliable data stream, so the repeat command 
            //is executed only the first time


            if (repeat == 0)
            {
                //Note that this may be done before any load colour table
                //commands by some CDGs. So the load colour table itself
                //actual recalculates the RGB values for all pixels when
                //the colour table changes.

                //Set the preset colour for every pixel. Must be stored in 
                //the pixel colour table indeces array

                for (ri = 0; ri <= CdgFullHeight - 1; ri++)
                {
                    for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                    {
                        _mPixelColours[ri, ci] = (byte)colour;
                    }
                }
            }
        }


        private void BorderPreset(ref SubCodePacket pack)
        {
            var colour = 0;
            var ri = 0;
            var ci = 0;

            colour = pack.Data[0] & 0xf;
            _mBorderColourIndex = colour;

            //The border area is the area contained with a rectangle 
            //defined by (0,0,300,216) minus the interior pixels which are contained
            //within a rectangle defined by (6,12,294,204).

            for (ri = 0; ri <= CdgFullHeight - 1; ri++)
            {
                for (ci = 0; ci <= 5; ci++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }

                for (ci = CdgFullWidth - 6; ci <= CdgFullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }
            }

            for (ci = 6; ci <= CdgFullWidth - 7; ci++)
            {
                for (ri = 0; ri <= 11; ri++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }

                for (ri = CdgFullHeight - 12; ri <= CdgFullHeight - 1; ri++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }
            }
        }


        private void LoadColorTable(ref SubCodePacket pack, int table)
        {
            for (var i = 0; i <= 7; i++)
            {
                //[---high byte---]   [---low byte----]
                //7 6 5 4 3 2 1 0     7 6 5 4 3 2 1 0
                //X X r r r r g g     X X g g b b b b

                var byte0 = pack.Data[2 * i];
                var byte1 = pack.Data[2 * i + 1];
                var red = (byte0 & 0x3f) >> 2;
                var green = ((byte0 & 0x3) << 2) | ((byte1 & 0x3f) >> 4);
                var blue = byte1 & 0xf;

                red *= 17;
                green *= 17;
                blue *= 17;

                if (_mPSurface != null)
                {
                    _mColourTable[i + table * 8] = _mPSurface.MapRgbColour(red, green, blue);
                }
            }
        }


        private void TileBlock(ref SubCodePacket pack, bool bXor)
        {
            var colour0 = 0;
            var colour1 = 0;
            var columnIndex = 0;
            var rowIndex = 0;
            var myByte = 0;
            var pixel = 0;
            var xorCol = 0;
            var currentColourIndex = 0;
            var newCol = 0;

            colour0 = pack.Data[0] & 0xf;
            colour1 = pack.Data[1] & 0xf;
            rowIndex = (pack.Data[2] & 0x1f) * 12;
            columnIndex = (pack.Data[3] & 0x3f) * 6;

            if (rowIndex > CdgFullHeight - TileHeight)
                return;
            if (columnIndex > CdgFullWidth - TileWidth)
                return;

            //Set the pixel array for each of the pixels in the 12x6 tile.
            //Normal = Set the colour to either colour0 or colour1 depending
            //on whether the pixel value is 0 or 1.
            //XOR = XOR the colour with the colour index currently there.


            for (var i = 0; i <= 11; i++)
            {
                myByte = pack.Data[4 + i] & 0x3f;
                for (var j = 0; j <= 5; j++)
                {
                    pixel = (myByte >> (5 - j)) & 0x1;
                    if (bXor)
                    {
                        //Tile Block XOR 
                        if (pixel == 0)
                        {
                            xorCol = colour0;
                        }
                        else
                        {
                            xorCol = colour1;
                        }

                        //Get the colour index currently at this location, and xor with it 
                        currentColourIndex = _mPixelColours[rowIndex + i, columnIndex + j];
                        newCol = currentColourIndex ^ xorCol;
                    }
                    else
                    {
                        if (pixel == 0)
                        {
                            newCol = colour0;
                        }
                        else
                        {
                            newCol = colour1;
                        }
                    }

                    //Set the pixel with the new colour. We set both the surfarray
                    //containing actual RGB values, as well as our array containing
                    //the colour indexes into our colour table. 
                    _mPixelColours[rowIndex + i, columnIndex + j] = (byte)newCol;
                }
            }
        }

        private void DefineTransparentColour(ref SubCodePacket pack)
        {
            _mTransparentColour = pack.Data[0] & 0xf;
        }


        private void Scroll(ref SubCodePacket pack, bool copy)
        {
            var colour = 0;
            var hScroll = 0;
            var vScroll = 0;
            var hSCmd = 0;
            var hOffset = 0;
            var vSCmd = 0;
            var vOffset = 0;
            var vScrollPixels = 0;
            var hScrollPixels = 0;

            //Decode the scroll command parameters
            colour = pack.Data[0] & 0xf;
            hScroll = pack.Data[1] & 0x3f;
            vScroll = pack.Data[2] & 0x3f;

            hSCmd = (hScroll & 0x30) >> 4;
            hOffset = hScroll & 0x7;
            vSCmd = (vScroll & 0x30) >> 4;
            vOffset = vScroll & 0xf;


            _mHOffset = hOffset < 5 ? hOffset : 5;
            _mVOffset = vOffset < 11 ? vOffset : 11;

            //Scroll Vertical - Calculate number of pixels

            vScrollPixels = 0;
            if (vSCmd == 2)
            {
                vScrollPixels = -12;
            }
            else if (vSCmd == 1)
            {
                vScrollPixels = 12;
            }

            //Scroll Horizontal- Calculate number of pixels

            hScrollPixels = 0;
            if (hSCmd == 2)
            {
                hScrollPixels = -6;
            }
            else if (hSCmd == 1)
            {
                hScrollPixels = 6;
            }

            if (hScrollPixels == 0 && vScrollPixels == 0)
            {
                return;
            }

            //Perform the actual scroll.

            var temp = new byte[CdgFullHeight + 1, CdgFullWidth + 1];
            var vInc = vScrollPixels + CdgFullHeight;
            var hInc = hScrollPixels + CdgFullWidth;
            var ri = 0;
            //row index
            var ci = 0;
            //column index

            for (ri = 0; ri <= CdgFullHeight - 1; ri++)
            {
                for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                {
                    temp[(ri + vInc) % CdgFullHeight, (ci + hInc) % CdgFullWidth] = _mPixelColours[ri, ci];
                }
            }


            //if copy is false, we were supposed to fill in the new pixels
            //with a new colour. Go back and do that now.


            if (copy == false)
            {
                if (vScrollPixels > 0)
                {
                    for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= vScrollPixels - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }
                else if (vScrollPixels < 0)
                {
                    for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                    {
                        for (ri = CdgFullHeight + vScrollPixels; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }


                if (hScrollPixels > 0)
                {
                    for (ci = 0; ci <= hScrollPixels - 1; ci++)
                    {
                        for (ri = 0; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }
                else if (hScrollPixels < 0)
                {
                    for (ci = CdgFullWidth + hScrollPixels; ci <= CdgFullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }
            }

            //Now copy the temporary buffer back to our array

            for (ri = 0; ri <= CdgFullHeight - 1; ri++)
            {
                for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = temp[ri, ci];
                }
            }
        }

        private void Render()
        {
            if (_mPSurface == null)
                return;
            for (var ri = 0; ri <= CdgFullHeight - 1; ri++)
            {
                for (var ci = 0; ci <= CdgFullWidth - 1; ci++)
                {
                    if (ri < TileHeight || ri >= CdgFullHeight - TileHeight || ci < TileWidth ||
                        ci >= CdgFullWidth - TileWidth)
                    {
                        _mPSurface.RgbData[ri, ci] = _mColourTable[_mBorderColourIndex];
                    }
                    else
                    {
                        _mPSurface.RgbData[ri, ci] = _mColourTable[_mPixelColours[ri + _mVOffset, ci + _mHOffset]];
                    }
                }
            }
        }

    }
}