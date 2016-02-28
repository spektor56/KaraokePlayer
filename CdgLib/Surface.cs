using System.Drawing;

namespace CdgLib
{
    public class Surface
    {
        public int[,] RgbData = new int[CdgFile.CdgFullHeight, CdgFile.CdgFullWidth];

        public int MapRgbColour(int red, int green, int blue)
        {
            return Color.FromArgb(red, green, blue).ToArgb();
        }
    }
}