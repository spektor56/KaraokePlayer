using System.Drawing;

namespace CdgLib
{
    public class Surface
    {
        public int[,] RgbData = new int[CdgFile.FullHeight, CdgFile.FullWidth];

        public int MapRgbColour(int red, int green, int blue)
        {
            return Color.FromArgb(red, green, blue).ToArgb();
        }
    }
}