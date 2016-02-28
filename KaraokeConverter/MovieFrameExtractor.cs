using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using DexterLib;
using DirectShowLib.DMO;
using NReco.VideoConverter;

namespace KaraokeConverter
{
    public class MovieFrameExtractor
    {
        public static Bitmap GetBitmap(double position, string movieFileName, int width, int height)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var ffProbe = new NReco.VideoInfo.FFProbe();
            var mediaInfo = ffProbe.GetMediaInfo(movieFileName);
            var videoDuration = (int)mediaInfo.Duration.TotalSeconds;

            var ffMpeg = new FFMpegConverter();
            using (var ms = new MemoryStream())
            {
                ffMpeg.GetVideoThumbnail(movieFileName, ms, (float)position % videoDuration);
                var bitmap = new Bitmap(ms);
                stopWatch.Stop();
                Debug.Print(stopWatch.ElapsedMilliseconds.ToString());
                return bitmap;
            }
        }
    }
}