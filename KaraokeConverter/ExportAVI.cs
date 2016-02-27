using AviFile;
using CdgLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraokeConverter
{
    public class ExportAVI
    {


        public void CDGtoAVI(string aviFileName, string cdgFileName, string mp3FileName, double frameRate, string backgroundFileName = "")
        {
            Bitmap backgroundBmp = null;
            Bitmap mergedBMP = null;
            VideoStream aviStream = null;
            CDGFile myCDGFile = new CDGFile(cdgFileName);
            myCDGFile.renderAtPosition(0);
            Bitmap bitmap__1 = (Bitmap)myCDGFile.RgbImage;
            if (!string.IsNullOrEmpty(backgroundFileName))
            {
                try
                {
                    if (IsMovie(backgroundFileName))
                        backgroundBmp = MovieFrameExtractor.GetBitmap(0, backgroundFileName, CDGFile.CDG_FULL_WIDTH, CDGFile.CDG_FULL_HEIGHT);
                    if (IsGraphic(backgroundFileName))
                        backgroundBmp = GraphicUtil.GetCdgSizeBitmap(backgroundFileName);
                }
                catch (Exception ex)
                {
                }
            }
            AviManager aviManager = new AviManager(aviFileName, false);
            if (backgroundBmp != null)
            {
                mergedBMP = GraphicUtil.MergeImagesWithTransparency(backgroundBmp, bitmap__1);
                aviStream = aviManager.AddVideoStream(true, frameRate, mergedBMP);
                mergedBMP.Dispose();
                if (IsMovie(backgroundFileName))
                    backgroundBmp.Dispose();
            }
            else {
                aviStream = aviManager.AddVideoStream(true, frameRate, bitmap__1);
            }

            int count = 0;
            double frameInterval = 1000 / frameRate;
            long totalDuration = myCDGFile.getTotalDuration();
            double position = 0;
            while (position <= totalDuration)
            {
                count += 1;
                position = count * frameInterval;
                myCDGFile.renderAtPosition(Convert.ToInt64(position));
                bitmap__1 = (Bitmap)myCDGFile.RgbImage;
                if (!string.IsNullOrEmpty(backgroundFileName))
                {
                    if (IsMovie(backgroundFileName))
                        backgroundBmp = MovieFrameExtractor.GetBitmap(position / 1000, backgroundFileName, CDGFile.CDG_FULL_WIDTH, CDGFile.CDG_FULL_HEIGHT);
                }
                if (backgroundBmp != null)
                {
                    mergedBMP = GraphicUtil.MergeImagesWithTransparency(backgroundBmp, bitmap__1);
                    aviStream.AddFrame(mergedBMP);
                    mergedBMP.Dispose();
                    if (IsMovie(backgroundFileName))
                        backgroundBmp.Dispose();
                }
                else {
                    aviStream.AddFrame(bitmap__1);
                }
                bitmap__1.Dispose();
                int percentageDone = (int)((position / totalDuration) * 100);
                if (Status != null)
                {
                    Status(percentageDone.ToString());
                }
                Application.DoEvents();
            }
            myCDGFile.Dispose();
            aviManager.Close();
            if (backgroundBmp != null)
                backgroundBmp.Dispose();
            AddMP3toAVI(aviFileName, mp3FileName);
        }

        public static void AddMP3toAVI(string aviFileName, string mp3FileName)
        {
            /*
			string newAVIFileName = Regex.Replace(aviFileName, "\\.avi$", "MUX.avi", RegexOptions.IgnoreCase);
			string cmdLineArgs = "-ovc copy -oac copy -audiofile \"" + mp3FileName + "\" -o \"" + newAVIFileName + "\" \"" + aviFileName + "\"";
			using (Process myProcess = new Process()) {
				string myCMD = "\"" + System.AppDomain.CurrentDomain.BaseDirectory() + "mencoder.exe \"" + cmdLineArgs;
				myProcess.StartInfo.FileName = "\"" + System.AppDomain.CurrentDomain.BaseDirectory() + "mencoder.exe\"";
				myProcess.StartInfo.Arguments = cmdLineArgs;
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.Start();
				myProcess.PriorityClass = ProcessPriorityClass.Normal;
				myProcess.WaitForExit();
			}
			if (File.Exists(newAVIFileName)) {
				File.Delete(aviFileName);
				File.Move(newAVIFileName, aviFileName);
			}
            */
        }

        public static bool IsMovie(string filename)
        {
            return Regex.IsMatch(filename, "^.+(\\.avi|\\.mpg|\\.wmv)$", RegexOptions.IgnoreCase);
        }

        public static bool IsGraphic(string filename)
        {
            return Regex.IsMatch(filename, "^.+(\\.jpg|\\.bmp|\\.png|\\.tif|\\.tiff|\\.gif|\\.wmf)$", RegexOptions.IgnoreCase);
        }

        public event StatusEventHandler Status;
        public delegate void StatusEventHandler(string message);


    }
}
