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

        private int[,] _graphicData;

        public Graphic(IEnumerable<Packet> packets)
        {
            foreach (var packet in packets)
            {
                packet.ApplyTransform(ref _graphicData);
            }
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


      


    }
}
