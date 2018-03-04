using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CdgLib.SubCode;

namespace CdgLib
{
    public class GraphicsFile
    {
        
        private Graphic _graphic;

        private GraphicsFile()
        {
        }

        public long Duration => _graphic.Duration;

        public bool Transparent => true;

        public static async Task<GraphicsFile> LoadAsync(string fileName)
        {
            var graphicsFile = new GraphicsFile { _graphic = new Graphic(await LoadFileAsync(fileName))};
            return graphicsFile;
        }

        private static async Task<IEnumerable<Packet>> LoadFileAsync(string fileName)
        {
            using (var fileStream = new GraphicFileStream(fileName))
            {
                return await fileStream.ReadFile();
            }
        }

        public Bitmap RenderAtTime(long time)
        {

            return _graphic.ToBitmap(time);
        }
    }
}