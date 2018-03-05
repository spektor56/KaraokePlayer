using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilters
{
    public class Xbrz
    {
        [DllImport("xbrz.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void scale(Int32 factor, Int32[] src, Int32[] trg, Int32 srcWidth, Int32 srcHeight, Int32 yFirst, Int32 yLast);

        [DllImport("xbrz.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void bilinearScale(Int32[] src, Int32 srcWidth, Int32 srcHeight, Int32[] trg, Int32 trgWidth, Int32 trgHeight);

        [DllImport("xbrz.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void nearestNeighborScale(Int32[] src, Int32 srcWidth, Int32 srcHeight, Int32[] trg, Int32 trgWidth, Int32 trgHeight);

        public static Bitmap ScaleImage(Bitmap bitmap, int scaleFactor)
        {
            var rgbValues = bitmap.ToIntArray();
            var scaledRbgValues = new int[rgbValues.Length * (scaleFactor * scaleFactor)];
            scale(scaleFactor, rgbValues, scaledRbgValues, bitmap.Width, bitmap.Height, 0, int.MaxValue);
            return scaledRbgValues.ToBitmap(bitmap.Width * scaleFactor, bitmap.Height * scaleFactor, bitmap.PixelFormat);
        }
    }
}
