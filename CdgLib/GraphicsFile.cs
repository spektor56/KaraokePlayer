using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CdgLib.SubCode;

namespace CdgLib
{
    public class GraphicsFile : FileStream
    {
        private const int ColourTableSize = 16;

        private const int CdgPacketSize = 24;
        private const int TileHeight = 12;
        private const int TileWidth = 6;
        public const int FullWidth = 300;
        public const int FullHeight = 216;

        private readonly int[] _mColourTable = new int[ColourTableSize];
        private readonly byte[,] _mPixelColours = new byte[FullHeight, FullWidth];


        private long _previousPosition;

        public GraphicsFile(string path) : base(path, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
        }

        public bool Transparent => true;

        public long Duration => Length/CdgPacketSize*1000/300;

        public async Task<Bitmap> RenderAtTime(long position = -1)
        {
            if (position < 0)
            {
                position = _previousPosition + CdgPacketSize;
            }

            if (position < _previousPosition)
            {
               // Reset();
            }

            //duration of one packet is 1/300 seconds (4 packets per sector, 75 sectors per second)
            //p=t*3/10  t=p*10/3 t=milliseconds, p=packets
            var timeToRender = position - _previousPosition;
            _previousPosition += timeToRender;
            var numberOfSubCodePackets = timeToRender*3/10;
      
            var subCodePackets = await ReadSubCodeAsync(numberOfSubCodePackets);
            foreach (var subCodePacket in subCodePackets)
            {
               // ProcessPacket(subCodePacket);
            }
            
            RenderSurface();
            


        }



        private async Task<IEnumerable<Packet>> ReadSubCodeAsync(long numberOfPackets)
        {
            var subCodePackets = new List<Packet>();
            var buffer = new byte[CdgPacketSize*numberOfPackets];
            var bytesRead = await ReadAsync(buffer, 0, buffer.Length);

            for (var i = 0; i < bytesRead/CdgPacketSize; i++)
            {
                var subCodePacket = new Packet(buffer.Skip(i* CdgPacketSize).Take(CdgPacketSize).ToArray());
                subCodePackets.Add(subCodePacket);
            }
            return subCodePackets;
        }



        private void RenderSurface()
        {
            if (_mPSurface == null)
                return;
            for (var ri = 0; ri <= FullHeight - 1; ri++)
            {
                for (var ci = 0; ci <= FullWidth - 1; ci++)
                {
                    if (ri < TileHeight || ri >= FullHeight - TileHeight || ci < TileWidth ||
                        ci >= FullWidth - TileWidth)
                    {
                     //   _mPSurface.RgbData[ri, ci] = _mColourTable[_mBorderColourIndex];
                    }
                    else
                    {
                        _mPSurface.RgbData[ri, ci] = _mColourTable[_mPixelColours[ri + _mVOffset, ci + _mHOffset]];
                    }
                }
            }
        }
    }
}