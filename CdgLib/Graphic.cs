using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CdgLib.SubCode;

namespace CdgLib
{
    public class Graphic
    {
        private int _mPresetColourIndex;
        private int _mBorderColourIndex;

        private long _mDuration;
        private int _mHOffset;
        private Bitmap _mImage;

        private CdgFileIoStream _mPStream;
        private readonly Surface _mPSurface;
        private int _mTransparentColour;
        private int _mVOffset;

        private const byte Command = 0x9;
        private const byte CdgMask = 0x3f;
        private const int CdgDisplayWidth = 294;
        private const int CdgDisplayHeight = 204;
        private int[,] _graphicData;
        public Graphic(IEnumerable<Packet> packets)
        {
            
        }

        public Bitmap ToBitmap()
        {
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
            myBitmap.MakeTransparent(myBitmap.GetPixel(1, 1));
    
            return myBitmap;
        }


        private void ProcessPacket(Packet packetPacket)
        {
            if ((packetPacket.Command[0] & CdgMask) != Command) return;
            var instructionCode = (Instruction)(packetPacket.Instruction[0] & CdgMask);
            switch (instructionCode)
            {
                case Instruction.MemoryPreset:
                    MemoryPreset(packetPacket);
                    break;
                case Instruction.BorderPreset:
                    BorderPreset(packetPacket);
                    break;
                case Instruction.TileBlockNormal:
                    TileBlock(packetPacket, false);
                    break;
                case Instruction.ScrollPreset:
                    Scroll(packetPacket, false);
                    break;
                case Instruction.ScrollCopy:
                    Scroll(packetPacket, true);
                    break;
                case Instruction.DefineTransparentColor:
                    DefineTransparentColour(packetPacket);
                    break;
                case Instruction.LoadColorTableLower:
                    LoadColorTable(packetPacket, 0);
                    break;
                case Instruction.LoadColorTableUpper:
                    LoadColorTable(packetPacket, 1);
                    break;
                case Instruction.TileBlockXor:
                    TileBlock(packetPacket, true);
                    break;
            }
        }


        private void MemoryPreset(Packet pack)
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
                        _mPixelColours[ri, ci] = (byte)colour;
                    }
                }
            }
        }

        private void Reset()
        {
            Position = 0;
            Array.Clear(_mPixelColours, 0, _mPixelColours.Length);
            Array.Clear(_mColourTable, 0, _mColourTable.Length);

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

        private void BorderPreset(Packet pack)
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
                    _mPixelColours[ri, ci] = (byte)colour;
                }

                for (ci = FullWidth - 6; ci <= FullWidth - 1; ci++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }
            }

            for (ci = 6; ci <= FullWidth - 7; ci++)
            {
                for (ri = 0; ri <= 11; ri++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }

                for (ri = FullHeight - 12; ri <= FullHeight - 1; ri++)
                {
                    _mPixelColours[ri, ci] = (byte)colour;
                }
            }
        }


        private void LoadColorTable(Packet pack, int table)
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


        private void TileBlock(Packet pack, bool bXor)
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
                    _mPixelColours[rowIndex + i, columnIndex + j] = (byte)newCol;
                }
            }
        }

        private void DefineTransparentColour(Packet pack)
        {
            _mTransparentColour = pack.Data[0] & 0xf;
        }


        private void Scroll(Packet pack, bool copy)
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
                    temp[(ri + vInc) % FullHeight, (ci + hInc) % FullWidth] = _mPixelColours[ri, ci];
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
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }
                else if (vScrollPixels < 0)
                {
                    for (ci = 0; ci <= FullWidth - 1; ci++)
                    {
                        for (ri = FullHeight + vScrollPixels; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }


                if (hScrollPixels > 0)
                {
                    for (ci = 0; ci <= hScrollPixels - 1; ci++)
                    {
                        for (ri = 0; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
                        }
                    }
                }
                else if (hScrollPixels < 0)
                {
                    for (ci = FullWidth + hScrollPixels; ci <= FullWidth - 1; ci++)
                    {
                        for (ri = 0; ri <= FullHeight - 1; ri++)
                        {
                            temp[ri, ci] = (byte)colour;
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
    }
}
