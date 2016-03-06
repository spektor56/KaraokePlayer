using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CdgLib.SubCode;

namespace CdgLib
{
    public class GraphicsFile
    {
        private const int PacketSize = 24;
        private IEnumerable<Packet> _packets;

        private GraphicsFile()
        {
        }

        public bool Transparent => true;

        public long Duration => _packets.Count()/PacketSize*1000/300;

        public static async Task<GraphicsFile> LoadAsync(string fileName)
        {
            var graphicsFile = new GraphicsFile {_packets = await LoadFileAsync(fileName)};
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
            //duration of one packet is 1/300 seconds (4 packets per sector, 75 sectors per second)
            //p=t*3/10  t=p*10/3 t=milliseconds, p=packets
            var numberOfSubCodePackets = (int) (time*3/10);
            var graphic = new Graphic(_packets.Take(numberOfSubCodePackets));
            var image = graphic.ToBitmap();
            return image;
        }
    }
}