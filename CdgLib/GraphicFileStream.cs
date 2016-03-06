using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CdgLib.SubCode;

namespace CdgLib
{
    public class GraphicFileStream : FileStream
    {
        private const int PacketSize = 24;

        public GraphicFileStream(string path) : base(path, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
        }

        public async Task<IEnumerable<Packet>> ReadFile()
        {
            Position = 0;
            var subCodePackets = new List<Packet>();
            var buffer = new byte[Length];
            var bytesRead = await ReadAsync(buffer, 0, buffer.Length);

            for (var i = 0; i < bytesRead/PacketSize; i++)
            {
                var subCodePacket = new Packet(buffer.Skip(i*PacketSize).Take(PacketSize).ToArray());
                subCodePackets.Add(subCodePacket);
            }
            return subCodePackets;
        }

        private async Task<IEnumerable<Packet>> ReadSubCodeAsync(long numberOfPackets)
        {
            var subCodePackets = new List<Packet>();
            var buffer = new byte[PacketSize*numberOfPackets];
            var bytesRead = await ReadAsync(buffer, 0, buffer.Length);

            for (var i = 0; i < bytesRead/PacketSize; i++)
            {
                var subCodePacket = new Packet(buffer.Skip(i*PacketSize).Take(PacketSize).ToArray());
                subCodePackets.Add(subCodePacket);
            }
            return subCodePackets;
        }
    }
}