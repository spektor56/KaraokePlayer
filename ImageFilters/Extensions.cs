using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilters
{
    public static class Extensions
    {
        public static Bitmap ChangeFormat(this Bitmap image, PixelFormat format)
        {
            var newFormatImage = new Bitmap(image.Width, image.Height, format);
            using (var gr = Graphics.FromImage(newFormatImage))
            {
                gr.DrawImage(image, new Rectangle(0, 0, newFormatImage.Width, newFormatImage.Height));
            }

            return newFormatImage;
        }

        public static int[] ToIntArray(this Bitmap image)
        {

            var rectangle = new Rectangle(0, 0, image.Width, image.Height);
            var bitmapData = image.LockBits(rectangle, ImageLockMode.ReadWrite, image.PixelFormat);
            var bitmapPointer = bitmapData.Scan0;

            if (bitmapData.Stride < 0)
            {
                bitmapPointer += bitmapData.Stride * (image.Height - 1);
            }

            var intCount = bitmapData.Stride * image.Height / 4;
            var values = new int[intCount];

            Marshal.Copy(bitmapPointer, values, 0, intCount);
            image.UnlockBits(bitmapData);

            return values;
        }

        public static Bitmap ToBitmap(this int[] bitmapData, int width, int height, PixelFormat format)
        {
            var newImage = new Bitmap(width, height, format);
            var rectangle = new Rectangle(0, 0, newImage.Width, newImage.Height);
            var newBitmapData = newImage.LockBits(rectangle, ImageLockMode.ReadWrite, format);
            var newBitmapPointer = newBitmapData.Scan0;
            var intCount = newBitmapData.Stride * newImage.Height / 4;
            Marshal.Copy(bitmapData, 0, newBitmapPointer, intCount);
            newImage.UnlockBits(newBitmapData);

            return newImage;
        }
    }
}
