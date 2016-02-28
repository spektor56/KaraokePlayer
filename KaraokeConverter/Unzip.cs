using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace KaraokeConverter
{
    public class Unzip
    {
        public static string UnzipMP3GFiles(string zipFilename, string outputPath)
        {
            string functionReturnValue = null;
            functionReturnValue = "";
            try
            {
                var myZip = new FastZip();
                myZip.ExtractZip(zipFilename, outputPath, "");
                var myDirInfo = new DirectoryInfo(outputPath);
                var myFileInfo = myDirInfo.GetFiles("*.cdg", SearchOption.AllDirectories);
                if (myFileInfo.Length > 0)
                {
                    functionReturnValue = myFileInfo[0].FullName;
                }
            }
            catch (Exception ex)
            {
            }
            return functionReturnValue;
        }
    }
}