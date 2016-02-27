using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokePlayer
{
    class Unzip
    {

        public static string UnzipMP3GFiles(string zipFilename, string outputPath)
        {
            string functionReturnValue = null;
            functionReturnValue = "";
            try
            {
                ICSharpCode.SharpZipLib.Zip.FastZip myZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                myZip.ExtractZip(zipFilename, outputPath, "");
                DirectoryInfo myDirInfo = new DirectoryInfo(outputPath);
                FileInfo[] myFileInfo = myDirInfo.GetFiles("*.cdg", SearchOption.AllDirectories);
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
