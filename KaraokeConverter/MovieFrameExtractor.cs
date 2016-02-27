using System.Drawing;
using System.IO;
namespace KaraokeConverter
{
    public class MovieFrameExtractor
    {

        public static Bitmap GetBitmap(double position, string movieFileName, int width, int height)
        {

            DexterLib.MediaDetClass det = new DexterLib.MediaDetClass();
            det.Filename = movieFileName;
            det.CurrentStream = 0;
            double len = det.StreamLength;
            if (position > len)
            {
                return null;
            }

            string myTempFile = System.IO.Path.GetTempFileName();
            det.WriteBitmapBits(position, width, height, myTempFile);
            Bitmap myBMP = null;
            using (FileStream lStream = new FileStream(myTempFile, FileMode.Open, FileAccess.Read))
            {
                myBMP = (Bitmap)Image.FromStream(lStream);
            }
            System.IO.File.Delete(myTempFile);
            return myBMP;

        }

    }
}
