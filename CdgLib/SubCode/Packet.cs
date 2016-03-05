using System;

namespace CdgLib.SubCode
{
    public class Packet
    {
        public Command Command { get; }

        public Instruction Instruction { get; }

        public byte[] ParityQ { get; } = new byte[2];

        public byte[] Data { get; } = new byte[16];

        public byte[] ParityP { get; } = new byte[4];


        public Packet(byte[] data)
        {
            Command = (Command)(data[0] & 0x3F);
            Instruction = (Instruction)(data[1] & 0x3F);
            Array.Copy(data, 2, ParityQ, 0, 2);
            Array.Copy(data, 4, Data, 0, 16);
            Array.Copy(data, 20, ParityP, 0, 4);
        }

        public void ApplyTransform(int[,] data)
        {
            if (Command != Command.Graphic) return;
            switch (Instruction)
            {
                case Instruction.MemoryPreset:
                    MemoryPreset(data);
                    break;
                case Instruction.BorderPreset:
                    BorderPreset(data);
                    break;
                case Instruction.TileBlockNormal:
                    TileBlock(data,false);
                    break;
                case Instruction.ScrollPreset:
                    Scroll(data,false);
                    break;
                case Instruction.ScrollCopy:
                    Scroll(data,true);
                    break;
                case Instruction.DefineTransparentColor:
                    DefineTransparentColour(data);
                    break;
                case Instruction.LoadColorTableLower:
                    LoadColorTable(data,0);
                    break;
                case Instruction.LoadColorTableUpper:
                    LoadColorTable(data,1);
                    break;
                case Instruction.TileBlockXor:
                    TileBlock(data,true);
                    break;
            }
        }


        private void MemoryPreset(int[,] data)
        {
            var colour = Data[0] & 0xf;
            var repeat = Data[1] & 0xf;

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
                for (int rowIndex = 0; rowIndex < data.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < data.GetLength(1); columnIndex++)
                    {
                        data[rowIndex, columnIndex] = (byte) colour;
                    }
                }
            }
        }

        private void BorderPreset(int[,] data)
        {
            int rowIndex;
            int columnIndex;

            var colour = Data[0] & 0xf;

            //The border area is the area contained with a rectangle 
            //defined by (0,0,300,216) minus the interior pixels which are contained
            //within a rectangle defined by (6,12,294,204).

            for (rowIndex = 0; rowIndex < data.GetLength(0); rowIndex++)
            {
                for (columnIndex = 0; columnIndex < 6; columnIndex++)
                {
                    data[rowIndex, columnIndex] = (byte) colour;
                }

                for (columnIndex = data.GetLength(1) - 6; columnIndex < data.GetLength(1); columnIndex++)
                {
                    data[rowIndex, columnIndex] = (byte) colour;
                }
            }

            for (columnIndex = 6; columnIndex < data.GetLength(1) - 6; columnIndex++)
            {
                for (rowIndex = 0; rowIndex < 12; rowIndex++)
                {
                    data[rowIndex, columnIndex] = (byte) colour;
                }

                for (rowIndex = data.GetLength(1) - 12; rowIndex < data.GetLength(1); rowIndex++)
                {
                    data[rowIndex, columnIndex] = (byte) colour;
                }
            }
        }


        private void LoadColorTable(int[,] data,int table)
        {
            for (var i = 0; i < 8; i++)
            {
                //[---high byte---]   [---low byte----]
                //7 6 5 4 3 2 1 0     7 6 5 4 3 2 1 0
                //X X r r r r g g     X X g g b b b b

                var byte0 = Data[2*i];
                var byte1 = Data[2*i + 1];
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


        private void TileBlock(int[,] data,bool bXor)
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
                        xorCol = pixel == 0 ? colour0 : colour1;

                        //Get the colour index currently at this location, and xor with it 
                        currentColourIndex = _mPixelColours[rowIndex + i, columnIndex + j];
                        newCol = currentColourIndex ^ xorCol;
                    }
                    else
                    {
                        newCol = pixel == 0 ? colour0 : colour1;
                    }

                    //Set the pixel with the new colour. We set both the surfarray
                    //containing actual RGB values, as well as our array containing
                    //the colour indexes into our colour table. 
                    _mPixelColours[rowIndex + i, columnIndex + j] = (byte) newCol;
                }
            }
        }

        private void DefineTransparentColour(int[,] data)
        {
            _mTransparentColour = Data[0] & 0xf;
        }


        private void Scroll(int[,] data,bool copy)
        {
            //Decode the scroll command parameters
            var colour = Data[0] & 0xf;
            var horizontalScroll = Data[1] & 0x3f;
            var verticalScroll = Data[2] & 0x3f;

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
                    temp[(rowIndex + vInc)%FullHeight, (columnIndex + hInc)%FullWidth] = _mPixelColours[rowIndex, columnIndex];
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
                            temp[rowIndex, columnIndex] = (byte) colour;
                        }
                    }
                }
                else if (verticalScrollPixels < 0)
                {
                    for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                    {
                        for (rowIndex = FullHeight + verticalScrollPixels; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte) colour;
                        }
                    }
                }


                if (horizontalScrollPixels > 0)
                {
                    for (columnIndex = 0; columnIndex <= horizontalScrollPixels - 1; columnIndex++)
                    {
                        for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte) colour;
                        }
                    }
                }
                else if (horizontalScrollPixels < 0)
                {
                    for (columnIndex = FullWidth + horizontalScrollPixels; columnIndex <= FullWidth - 1; columnIndex++)
                    {
                        for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
                        {
                            temp[rowIndex, columnIndex] = (byte) colour;
                        }
                    }
                }
            }

            //Now copy the temporary buffer back to our array

            for (rowIndex = 0; rowIndex <= FullHeight - 1; rowIndex++)
            {
                for (columnIndex = 0; columnIndex <= FullWidth - 1; columnIndex++)
                {
                    _mPixelColours[rowIndex, columnIndex] = temp[rowIndex, columnIndex];
                }
            }
        }
    }
}