using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using CdgLib.SubCode;

namespace CdgLib
{
    public class Graphic
    {
        private const int ColourTableSize = 16;
        private const int TileHeight = 12;
        private const int TileWidth = 6;
        public const int FullWidth = 300;
        public const int FullHeight = 216;

        private readonly int[] _colourTable = new int[ColourTableSize];
        private readonly byte[,] _pixelColours = new byte[FullHeight, FullWidth];
        private int[,] _graphicData;

        public Graphic(IEnumerable<Packet> packets)
        {
            foreach (var packet in packets)
            {
                Process(packet);
            }
        }

        public Bitmap ToBitmap()
        {
            Bitmap myBitmap;
            using (var bitmapStream = new MemoryStream())
            {
                foreach (var colourValue in _graphicData)
                {
                    var colour = BitConverter.GetBytes(colourValue);
                    bitmapStream.Write(colour, 0, 4);
                }
                myBitmap = StreamToBitmap(bitmapStream, FullWidth, FullHeight);
            }
            myBitmap.MakeTransparent(myBitmap.GetPixel(1, 1));
    
            return myBitmap;
        }

        private Bitmap StreamToBitmap(Stream stream, int width, int height)
        {
            //create a new bitmap
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            stream.Seek(0, SeekOrigin.Begin);
            //copy the stream of pixel
            for (var n = 0; n <= stream.Length - 1; n++)
            {
                var myByte = new byte[1];
                stream.Read(myByte, 0, 1);
                Marshal.WriteByte(bmpData.Scan0, n, myByte[0]);
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private void Process(Packet packet)
        {
            if (packet.Command != Command.Graphic) return;
            switch (packet.Instruction)
            {
                case Instruction.MemoryPreset:
                    MemoryPreset(packet);
                    break;
                case Instruction.BorderPreset:
                    BorderPreset(packet);
                    break;
                case Instruction.TileBlockNormal:
                    TileBlock(packet, false);
                    break;
                case Instruction.ScrollPreset:
                    Scroll(packet, false);
                    break;
                case Instruction.ScrollCopy:
                    Scroll(packet, true);
                    break;
                case Instruction.DefineTransparentColor:
                    DefineTransparentColour(packet);
                    break;
                case Instruction.LoadColorTableLower:
                    LoadColorTable(packet, 0);
                    break;
                case Instruction.LoadColorTableUpper:
                    LoadColorTable(packet, 1);
                    break;
                case Instruction.TileBlockXor:
                    TileBlock(packet, true);
                    break;
            }
        }


        private void MemoryPreset(Packet packet)
        {
            var colour = packet.Data[0] & 0xf;
            var repeat = packet.Data[1] & 0xf;

            //we have a reliable packet stream, so the repeat command 
            //is executed only the first time
            if (repeat == 0)
            {
                //Note that this may be done before any load colour table
                //commands by some CDGs. So the load colour table itself
                //actual recalculates the RGB values for all pixels when
                //the colour table changes.

                //Set the preset colour for every pixel. Must be stored in 
                //the pixel colour table indeces array
                for (int rowIndex = 0; rowIndex < _pixelColours.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < _pixelColours.GetLength(1); columnIndex++)
                    {
                        _pixelColours[rowIndex, columnIndex] = (byte)colour;
                    }
                }
            }
        }

        private void BorderPreset(Packet packet)
        {
            int rowIndex;
            int columnIndex;

            var colour = packet.Data[0] & 0xf;

            //The border area is the area contained with a rectangle 
            //defined by (0,0,300,216) minus the interior pixels which are contained
            //within a rectangle defined by (6,12,294,204).

            for (rowIndex = 0; rowIndex < _pixelColours.GetLength(0); rowIndex++)
            {
                for (columnIndex = 0; columnIndex < 6; columnIndex++)
                {
                    _pixelColours[rowIndex, columnIndex] = (byte)colour;
                }

                for (columnIndex = _pixelColours.GetLength(1) - 6; columnIndex < _pixelColours.GetLength(1); columnIndex++)
                {
                    _pixelColours[rowIndex, columnIndex] = (byte)colour;
                }
            }

            for (columnIndex = 6; columnIndex < _pixelColours.GetLength(1) - 6; columnIndex++)
            {
                for (rowIndex = 0; rowIndex < 12; rowIndex++)
                {
                    _pixelColours[rowIndex, columnIndex] = (byte)colour;
                }

                for (rowIndex = _pixelColours.GetLength(1) - 12; rowIndex < _pixelColours.GetLength(1); rowIndex++)
                {
                    _pixelColours[rowIndex, columnIndex] = (byte)colour;
                }
            }
        }


        private void LoadColorTable(Packet packet, int table)
        {
            for (var i = 0; i < 8; i++)
            {
                //[---high byte---]   [---low byte----]
                //7 6 5 4 3 2 1 0     7 6 5 4 3 2 1 0
                //X X r r r r g g     X X g g b b b b
                var highByte = packet.Data[2 * i];
                var lowByte = packet.Data[2 * i + 1];

                var red = (highByte & 0x3f) >> 2;
                var green = ((highByte & 0x3) << 2) | ((lowByte & 0x3f) >> 4);
                var blue = lowByte & 0xf;

                //4 bit colour to 8 bit colour
                red *= 17;
                green *= 17;
                blue *= 17;

                _colourTable[i + table*8] = Color.FromArgb(red, green, blue).ToArgb(); 
            }
        }


        private void TileBlock(Packet packet, bool bXor)
        {
            var colour0 = packet.Data[0] & 0xf;
            var colour1 = packet.Data[1] & 0xf;
            var rowIndex = (packet.Data[2] & 0x1f) * 12;
            var columnIndex = (packet.Data[3] & 0x3f) * 6;

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
                var myByte = packet.Data[4 + i] & 0x3f;
                for (var j = 0; j <= 5; j++)
                {
                    var pixel = (myByte >> (5 - j)) & 0x1;
                    var newCol = 0;
                    if (bXor)
                    {
                        //Tile Block XOR 
                        var xorCol = pixel == 0 ? colour0 : colour1;

                        //Get the colour index currently at this location, and xor with it 
                        int currentColourIndex = _pixelColours[rowIndex + i, columnIndex + j];
                        newCol = currentColourIndex ^ xorCol;
                    }
                    else
                    {
                        newCol = pixel == 0 ? colour0 : colour1;
                    }

                    //Set the pixel with the new colour. We set both the surfarray
                    //containing actual RGB values, as well as our array containing
                    //the colour indexes into our colour table. 
                    _pixelColours[rowIndex + i, columnIndex + j] = (byte)newCol;
                }
            }
        }

        private void DefineTransparentColour(Packet packet)
        {
            //_mTransparentColour = packet.Data[0] & 0xf;
        }


        private void Scroll(Packet packet, bool copy)
        {
            //Decode the scroll command parameters
            var colour = packet.Data[0] & 0xf;
            var horizontalScroll = packet.Data[1] & 0x3f;
            var verticalScroll = packet.Data[2] & 0x3f;

            var horizontalScrollCommand = (horizontalScroll & 0x30) >> 4;
            var horizontalOffset = horizontalScroll & 0x7;
            var verticalScrollCommand = (verticalScroll & 0x30) >> 4;
            var verticalOffset = verticalScroll & 0xf;


            _mHOffset = horizontalOffset < 5 ? horizontalOffset : 5;
            _mVOffset = verticalOffset < 11 ? verticalOffset : 11;

            //Scroll Vertical - Calculate number of pixels

            var verticalScrollPixels = 0;
            switch (verticalScrollCommand)
            {
                case 2:
                    verticalScrollPixels = -12;
                    break;
                case 1:
                    verticalScrollPixels = 12;
                    break;
            }

            //Scroll Horizontal- Calculate number of pixels

            var horizontalScrollPixels = 0;
            switch (horizontalScrollCommand)
            {
                case 2:
                    horizontalScrollPixels = -6;
                    break;
                case 1:
                    horizontalScrollPixels = 6;
                    break;
            }

            if (horizontalScrollPixels == 0 && verticalScrollPixels == 0)
            {
                return;
            }

            //Perform the actual scroll.

            var temp = new byte[FullHeight + 1, FullWidth + 1];
            var vInc = verticalScrollPixels + FullHeight;
            var hInc = horizontalScrollPixels + FullWidth;
            var rowIndex = 0;
            //row index
            var columnIndex = 0;
            //column index

            for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
            {
                for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                {
                    temp[(rowIndex + vInc) % FullHeight, (columnIndex + hInc) % FullWidth] = _pixelColours[rowIndex, columnIndex];
                }
            }


            //if copy is false, we were supposed to fill in the new pixels
            //with a new colour. Go back and do that now.


            if (copy == false)
            {
                if (verticalScrollPixels > 0)
                {
                    for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                    {
                        for (rowIndex = 0; rowIndex <= verticalScrollPixels - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte)colour;
                        }
                    }
                }
                else if (verticalScrollPixels < 0)
                {
                    for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                    {
                        for (rowIndex = FullHeight + verticalScrollPixels; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte)colour;
                        }
                    }
                }


                if (horizontalScrollPixels > 0)
                {
                    for (columnIndex = 0; columnIndex <= horizontalScrollPixels - 1; columnIndex++)
                    {
                        for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte)colour;
                        }
                    }
                }
                else if (horizontalScrollPixels < 0)
                {
                    for (columnIndex = FullWidth + horizontalScrollPixels; columnIndex <= FullWidth - 1; columnIndex++)
                    {
                        for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte)colour;
                        }
                    }
                }
            }

            //Now copy the temporary buffer back to our array

            for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
            {
                for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                {
                    _pixelColours[rowIndex, columnIndex] = temp[rowIndex, columnIndex];
                }
            }
        }

    }
}
