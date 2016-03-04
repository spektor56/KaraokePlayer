using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

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
        public const int FullWidth = 300;
        public const int FullHeight = 216;
        private const int CdgDisplayWidth = 294;
        private const int CdgDisplayHeight = 204;
        private readonly int[] _mColourTable = new int[ColourTableSize];
        private readonly byte[,] _mPixelColours = new byte[FullHeight, FullWidth];
        private int _mBorderColourIndex;
        private long _mDuration;
        private int _mHOffset;
        private Bitmap _mImage;
        private int _mPresetColourIndex;
        private CdgFileIoStream _mPStream;
        private readonly Surface _mPSurface;
        private int _mTransparentColour;
        private int _mVOffset;
        private long _previousPosition;

        public CdgFile(string path, FileMode mode, FileAccess fileAccess) : base(path, mode, fileAccess, FileShare.Read)
        {
            _mPSurface = new Surface();
        }

        public bool Transparent => true;

        public long Duration => Length/CdgPacketSize*1000/300;

        public async Task<Bitmap> Render(long position = -1)
        {
            if (position < 0)
            {
                position = _previousPosition + CdgPacketSize;
            }

            if (position < _previousPosition)
            {
                Reset();
            }

            //duration of one packet is 1/300 seconds (4 packets per sector, 75 sectors per second)
            //p=t*3/10  t=p*10/3 t=milliseconds, p=packets
            var timeToRender = position - _previousPosition;
            _previousPosition += timeToRender;
            var numberOfSubCodePackets = timeToRender*3/10;
      
            var subCodePackets = await ReadSubCodeAsync(numberOfSubCodePackets);
            foreach (var subCodePacket in subCodePackets)
            {
                ProcessPacket(subCodePacket);
            }
            
            RenderSurface();
            Bitmap myBitmap;
            using (var bitmapStream = new MemoryStream())
            {
                foreach (var colourValue in _mPSurface.RgbData)
                {
                    var colour = BitConverter.GetBytes(colourValue);
                    bitmapStream.Write(colour, 0, 4);
                }
                myBitmap = GraphicUtil.StreamToBitmap(bitmapStream, FullWidth, FullHeight);
            }

            if (Transparent)
            {
                myBitmap.MakeTransparent(myBitmap.GetPixel(1, 1));
            }
            return myBitmap;

        }

        private void Reset()
        {
            Position = 0;
            Array.Clear(_mPixelColours, 0, _mPixelColours.Length);
            Array.Clear(_mColourTable, 0, _mColourTable.Length);

            _mPresetColourIndex = 0;
            _mBorderColourIndex = 0;
            _mTransparentColour = 0;
            _mHOffset = 0;
            _mVOffset = 0;

            _mDuration = 0;
            _previousPosition = 0;

            //clear surface 
            if (_mPSurface.RgbData != null)
            {
                Array.Clear(_mPSurface.RgbData, 0, _mPSurface.RgbData.Length);
            }
        }

        private async Task<IEnumerable<SubCodePacket>> ReadSubCodeAsync(long numberOfPackets)
        {
            var subCodePackets = new List<SubCodePacket>();
            var buffer = new byte[CdgPacketSize*numberOfPackets];
            var bytesRead = await ReadAsync(buffer, 0, buffer.Length);

            for (var i = 0; i < bytesRead/CdgPacketSize; i++)
            {
                var subCodePacket = new SubCodePacket();
                Array.Copy(buffer, i*CdgPacketSize + 0, subCodePacket.Command, 0, 1);
                Array.Copy(buffer, i*CdgPacketSize + 1, subCodePacket.Instruction, 0, 1);
                Array.Copy(buffer, i*CdgPacketSize + 2, subCodePacket.ParityQ, 0, 2);
                Array.Copy(buffer, i*CdgPacketSize + 4, subCodePacket.Data, 0, 16);
                Array.Copy(buffer, i*CdgPacketSize + 20, subCodePacket.ParityP, 0, 4);
                subCodePackets.Add(subCodePacket);
            }
            return subCodePackets;
        }


        private void ProcessPacket(SubCodePacket subCodePacketPacket)
        {
            if ((subCodePacketPacket.Command[0] & CdgMask) != CdgCommand) return;
            var instructionCode = subCodePacketPacket.Instruction[0] & CdgMask;
            switch (instructionCode)
            {
                case CdgInstMemoryPreset:
                    MemoryPreset(subCodePacketPacket);
                    break;
                case CdgInstBorderPreset:
                    BorderPreset(subCodePacketPacket);
                    break;
                case CdgInstTileBlock:
                    TileBlock(subCodePacketPacket, false);
                    break;
                case CdgInstScrollPreset:
                    Scroll(subCodePacketPacket, false);
                    break;
                case CdgInstScrollCopy:
                    Scroll(subCodePacketPacket, true);
                    break;
                case CdgInstDefTranspCol:
                    DefineTransparentColour(subCodePacketPacket);
                    break;
                case CdgInstLoadColTblLo:
                    LoadColorTable(subCodePacketPacket, 0);
                    break;
                case CdgInstLoadColTblHigh:
                    LoadColorTable(subCodePacketPacket, 1);
                    break;
                case CdgInstTileBlockXor:
                    TileBlock(subCodePacketPacket, true);
                    break;
            }
        }


        private void MemoryPreset(SubCodePacket pack)
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

                for (ri = 0; ri <= FullHeight - 1; ri++)
                {
                    for (ci = 0; ci <= FullWidth - 1; ci++)
                    {
                        _mPixelColours[ri, ci] = (byte) colour;
                    }
                }
            }
        }


        private void BorderPreset(SubCodePacket pack)
        {
            var colour = 0;
            var ri = 0;
            var ci = 0;

            colour = pack.Data[0] & 0xf;
            _mBorderColourIndex = colour;

            //The border area is the area contained with a rectangle 
            //defined by (0,0,300,216) minus the interior pixels which are contained
            //within a rectangle defined by (6,12,294,204).

            for (ri = 0; ri <= FullHeight - 1; ri++)
            {
                for (ci = 0; ci <= 5; ci++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }

                for (ci = FullWidth - 6; ci <= FullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }
            }

            for (ci = 6; ci <= FullWidth - 7; ci++)
            {
                for (ri = 0; ri <= 11; ri++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }

                for (ri = FullHeight - 12; ri <= FullHeight - 1; ri++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }
            }
        }


        private void LoadColorTable(SubCodePacket pack, int table)
        {
            for (var i = 0; i <= 7; i++)
            {
                //[---high byte---]   [---low byte----]
                //7 6 5 4 3 2 1 0     7 6 5 4 3 2 1 0
                //X X r r r r g g     X X g g b b b b

                var byte0 = pack.Data[2*i];
                var byte1 = pack.Data[2*i + 1];
                var red = (byte0 & 0x3f) >> 2;
                var green = ((byte0 & 0x3) << 2) | ((byte1 & 0x3f) >> 4);
                var blue = byte1 & 0xf;

                red *= 17;
                green *= 17;
                blue *= 17;

                if (_mPSurface != null)
                {
                    _mColourTable[i + table*8] = _mPSurface.MapRgbColour(red, green, blue);
                }
            }
        }


        private void TileBlock(SubCodePacket pack, bool bXor)
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
            rowIndex = (pack.Data[2] & 0x1f)*12;
            columnIndex = (pack.Data[3] & 0x3f)*6;

            if (rowIndex > FullHeight - TileHeight)
                return;
            if (columnIndex > FullWidth - TileWidth)
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
                    _mPixelColours[rowIndex + i, columnIndex + j] = (byte) newCol;
                }
            }
        }

        private void DefineTransparentColour(SubCodePacket pack)
        {
            _mTransparentColour = pack.Data[0] & 0xf;
        }


        private void Scroll(SubCodePacket pack, bool copy)
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

            var temp = new byte[FullHeight + 1, FullWidth + 1];
            var vInc = vScrollPixels + FullHeight;
            var hInc = hScrollPixels + FullWidth;
            var ri = 0;
            //row index
            var ci = 0;
            //column index

            for (ri = 0; ri <= FullHeight - 1; ri++)
            {
                for (ci = 0; ci <= FullWidth - 1; ci++)
                {
                    temp[(ri + vInc)%FullHeight, (ci + hInc)%FullWidth] = _mPixelColours[ri, ci];
                }
            }


            //if copy is false, we were supposed to fill in the new pixels
            //with a new colour. Go back and do that now.


            if (copy == false)
            {
                if (vScrollPixels > 0)
                {
                    for (ci = 0; ci <= FullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= vScrollPixels - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }
                else if (vScrollPixels < 0)
                {
                    for (ci = 0; ci <= FullWidth - 1; ci++)
                    {
                        for (ri = FullHeight + vScrollPixels; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }


                if (hScrollPixels > 0)
                {
                    for (ci = 0; ci <= hScrollPixels - 1; ci++)
                    {
                        for (ri = 0; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }
                else if (hScrollPixels < 0)
                {
                    for (ci = FullWidth + hScrollPixels; ci <= FullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }
            }

            //Now copy the temporary buffer back to our array

            for (ri = 0; ri <= FullHeight - 1; ri++)
            {
                for (ci = 0; ci <= FullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = temp[ri, ci];
                }
            }
        }

        private void RenderSurface()
        {
            if (_mPSurface == null)
                return;
            for (var ri = 0; ri <= FullHeight - 1; ri++)
            {
                for (var ci = 0; ci <= FullWidth - 1; ci++)
                {
                    if (ri < TileHeight || ri >= FullHeight - TileHeight || ci < TileWidth ||
                        ci >= FullWidth - TileWidth)
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