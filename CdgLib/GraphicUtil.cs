using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace CdgLib
{
    /// <summary>
    /// </summary>
    public class GraphicUtil
    {
        /// <summary>
        ///     Bitmaps to stream.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static Stream BitmapToStream(string filename)
        {
            var oldBmp = (Bitmap)Image.FromFile(filename);
            var oldData = oldBmp.LockBits(new Rectangle(0, 0, oldBmp.Width, oldBmp.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            var length = oldData.Stride * oldBmp.Height;
            var stream = new byte[length];
            Marshal.Copy(oldData.Scan0, stream, 0, length);
            oldBmp.UnlockBits(oldData);
            oldBmp.Dispose();
            return new MemoryStream(stream);
        }


        /// <summary>
        ///     Streams to bitmap.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap StreamToBitmap(ref Stream stream, int width, int height)
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

        /// <summary>
        ///     Gets the CDG size bitmap.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static Bitmap GetCdgSizeBitmap(string filename)
        {
            var bm = new Bitmap(filename);
            return ResizeBitmap(ref bm, CDGFile.CDG_FULL_WIDTH, CDGFile.CDG_FULL_HEIGHT);
        }

        /// <summary>
        ///     Resizes the bitmap.
        /// </summary>
        /// <param name="bm">The bm.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(ref Bitmap bm, int width, int height)
        {
            var thumb = new Bitmap(width, height);
            using (bm)
            {
                using (var g = Graphics.FromImage(thumb))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bm, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bm.Width, bm.Height),
                        GraphicsUnit.Pixel);
                }
            }
            return thumb;
        }

        /// <summary>
        ///     Merges the images with transparency.
        /// </summary>
        /// <param name="picture1">The pic1.</param>
        /// <param name="picture2">The pic2.</param>
        /// <returns></returns>
        public static Bitmap MergeImagesWithTransparency(Bitmap picture1, Bitmap picture2)
        {
            Bitmap mergedImage;
            var bm = new Bitmap(picture1.Width, picture1.Height);
            using (var gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(picture1, 0, 0);
                picture2.MakeTransparent(picture2.GetPixel(1, 1));
                gr.DrawImage(picture2, 0, 0);
                mergedImage = bm;
            }
            return mergedImage;
        }
    }
}
