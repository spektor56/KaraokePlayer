using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Schema;

namespace CdgLib
{
    public class CdgFile : IDisposable
    {
        private const int ColourTableSize = 16;

        // To detect redundant calls
        private bool _disposedValue;

        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

 
        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _mPStream.Close();
                }
                _mPStream = null;
                _mPSurface = null;
            }
            _disposedValue = true;
        }

        #region "Constants"

        //CDG Command Code

        private const byte CdgCommand = 0x9;
        //CDG Instruction Codes
        private const int CdgInstMemoryPreset = 1;
        private const int CdgInstBorderPreset = 2;
        private const int CdgInstTileBlock = 6;
        private const int CdgInstScrollPreset = 20;
        private const int CdgInstScrollCopy = 24;
        private const int CdgInstDefTranspCol = 28;
        private const int CdgInstLoadColTblLo = 30;
        private const int CdgInstLoadColTblHigh = 31;

        private const int CdgInstTileBlockXor = 38;
        //Bitmask for all CDG fields
        private const byte CdgMask = 0x3f;
        private const int CdgPacketSize = 24;
        private const int TileHeight = 12;

        private const int TileWidth = 6;
        //This is the size of the display as defined by the CDG specification.
        //The pixels in this region can be painted, and scrolling operations
        //rotate through this number of pixels.
        public const int CdgFullWidth = 300;

        public const int CdgFullHeight = 216;
        //This is the size of the screen that is actually intended to be
        //visible.  It is the center area of CDG_FULL.  
        private const int CdgDisplayWidth = 294;

        private const int CdgDisplayHeight = 204;

        #endregion

        #region "Private Declarations"

        private readonly byte[,] _mPixelColours = new byte[CdgFullHeight, CdgFullWidth];
        private readonly int[] _mColourTable = new int[ColourTableSize];
        private int _mPresetColourIndex;
        private int _mBorderColourIndex;

        private int _mTransparentColour;
        private int _mHOffset;

        private int _mVOffset;
        private CdgFileIoStream _mPStream;
        private Surface _mPSurface;
        private long _mPositionMs;

        private long _mDuration;

        private Bitmap _mImage;

        #endregion

        #region "Properties"

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

        #endregion

        #region "Public Methods"

        //Png Export
        public void SavePng(string filename)
        {
            RgbImage.Save(filename, ImageFormat.Png);
        }

        //New
        public CdgFile(string cdgFileName)
        {
            _mPStream = new CdgFileIoStream();
            _mPStream.Open(cdgFileName);
            _mPSurface = new Surface();
            if (_mPStream != null && _mPSurface != null)
            {
                Reset();
                _mDuration = _mPStream.Getsize()/CdgPacketSize*1000/300;
            }
        }

        public long GetTotalDuration()
        {
            return _mDuration;
        }

        public bool RenderAtPosition(long ms)
        {
            var pack = new CdgPacket();
            long numPacks = 0;
            var res = true;

            if (_mPStream == null)
            {
                return false;
            }

            if (ms < _mPositionMs)
            {
                if (_mPStream.Seek(0, SeekOrigin.Begin) < 0)
                    return false;
                _mPositionMs = 0;
            }

            //duration of one packet is 1/300 seconds (4 packets per sector, 75 sectors per second)

            numPacks = ms - _mPositionMs;
            numPacks /= 10;

            _mPositionMs += numPacks*10;
            numPacks *= 3;

            //TODO: double check logic due to inline while loop fucntionality
            //AndAlso m_pSurface.rgbData Is Nothing
            while (numPacks > 0)
            {
                res = ReadPacket(ref pack);
                ProcessPacket(ref pack);
                numPacks -= 1;
            }

            Render();
            return res;
        }

        #endregion

        #region "Private Methods"

        private void Reset()
        {
            Array.Clear(_mPixelColours, 0, _mPixelColours.Length);
            Array.Clear(_mColourTable, 0, _mColourTable.Length);

            _mPresetColourIndex = 0;
            _mBorderColourIndex = 0;
            _mTransparentColour = 0;
            _mHOffset = 0;
            _mVOffset = 0;

            _mDuration = 0;
            _mPositionMs = 0;

            //clear surface 
            if (_mPSurface.RgbData != null)
            {
                Array.Clear(_mPSurface.RgbData, 0, _mPSurface.RgbData.Length);
            }
        }

        private bool ReadPacket(ref CdgPacket pack)
        {
            if (_mPStream == null || _mPStream.Eof())
            {
                return false;
            }

            var read = 0;

            read += _mPStream.Read(ref pack.Command, 1);
            read += _mPStream.Read(ref pack.Instruction, 1);
            read += _mPStream.Read(ref pack.ParityQ, 2);
            read += _mPStream.Read(ref pack.Data, 16);
            read += _mPStream.Read(ref pack.ParityP, 4);

            return read == 24;
        }


        private void ProcessPacket(ref CdgPacket pack)
        {
            var instCode = 0;

            if ((pack.Command[0] & CdgMask) == CdgCommand)
            {
                instCode = pack.Instruction[0] & CdgMask;
                switch (instCode)
                {
                    case CdgInstMemoryPreset:
                        MemoryPreset(ref pack);


                        break;
                    case CdgInstBorderPreset:
                        BorderPreset(ref pack);


                        break;
                    case CdgInstTileBlock:
                        TileBlock(ref pack, false);


                        break;
                    case CdgInstScrollPreset:
                        Scroll(ref pack, false);


                        break;
                    case CdgInstScrollCopy:
                        Scroll(ref pack, true);


                        break;
                    case CdgInstDefTranspCol:
                        DefineTransparentColour(ref pack);


                        break;
                    case CdgInstLoadColTblLo:
                        LoadColorTable(ref pack, 0);


                        break;
                    case CdgInstLoadColTblHigh:
                        LoadColorTable(ref pack, 1);


                        break;
                    case CdgInstTileBlockXor:
                        TileBlock(ref pack, true);


                        break;
                    default:
                        //Ignore the unsupported commands

                        break;
                }
            }
        }


        private void MemoryPreset(ref CdgPacket pack)
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
                        _mPixelColours[ri, ci] = (byte) colour;
                    }
                }
            }
        }


        private void BorderPreset(ref CdgPacket pack)
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
                    _mPixelColours[ri, ci] = (byte) colour;
                }

                for (ci = CdgFullWidth - 6; ci <= CdgFullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }
            }

            for (ci = 6; ci <= CdgFullWidth - 7; ci++)
            {
                for (ri = 0; ri <= 11; ri++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }

                for (ri = CdgFullHeight - 12; ri <= CdgFullHeight - 1; ri++)
                {
                    _mPixelColours[ri, ci] = (byte) colour;
                }
            }
        }


        private void LoadColorTable(ref CdgPacket pack, int table)
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


        private void TileBlock(ref CdgPacket pack, bool bXor)
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
                    _mPixelColours[rowIndex + i, columnIndex + j] = (byte) newCol;
                }
            }
        }

        private void DefineTransparentColour(ref CdgPacket pack)
        {
            _mTransparentColour = pack.Data[0] & 0xf;
        }


        private void Scroll(ref CdgPacket pack, bool copy)
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
                    temp[(ri + vInc)%CdgFullHeight, (ci + hInc)%CdgFullWidth] = _mPixelColours[ri, ci];
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
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }
                else if (vScrollPixels < 0)
                {
                    for (ci = 0; ci <= CdgFullWidth - 1; ci++)
                    {
                        for (ri = CdgFullHeight + vScrollPixels; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }


                if (hScrollPixels > 0)
                {
                    for (ci = 0; ci <= hScrollPixels - 1; ci++)
                    {
                        for (ri = 0; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
                        }
                    }
                }
                else if (hScrollPixels < 0)
                {
                    for (ci = CdgFullWidth + hScrollPixels; ci <= CdgFullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= CdgFullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte) colour;
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

        #endregion
    }
}