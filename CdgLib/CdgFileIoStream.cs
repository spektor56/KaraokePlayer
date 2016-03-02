using System.IO;

namespace CdgLib
{
    /// <summary>
    /// </summary>
    public class CdgFileIoStream
    {
        private Stream _cdgFile;

        /// <summary>
        /// </summary>
        public CdgFileIoStream()
        {
            _cdgFile = null;
        }

        /// <summary>
        ///     Reads the specified buf.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="bufSize">The buf_size.</param>
        /// <returns></returns>
        public int Read(ref byte[] buf, int bufSize)
        {
            return _cdgFile.Read(buf, 0, bufSize);
        }

        /// <summary>
        ///     Writes the specified buf.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="bufSize">The buf_size.</param>
        /// <returns></returns>
        public int Write(ref byte[] buf, int bufSize)
        {
            _cdgFile.Write(buf, 0, bufSize);
            return 1;
        }

        /// <summary>
        ///     Seeks the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="whence">The whence.</param>
        /// <returns></returns>
        public int Seek(int offset, SeekOrigin whence)
        {
            return (int) _cdgFile.Seek(offset, whence);
        }

        /// <summary>
        ///     EOFs this instance.
        /// </summary>
        /// <returns></returns>
        public bool Eof()
        {
            return _cdgFile.Position >= _cdgFile.Length;
        }

        /// <summary>
        ///     Getsizes this instance.
        /// </summary>
        /// <returns></returns>
        public int Getsize()
        {
            return (int) _cdgFile.Length;
        }

        /// <summary>
        ///     Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public bool Open(string filename)
        {
            Close();
            _cdgFile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return _cdgFile != null;
        }

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            if (_cdgFile != null)
            {
                _cdgFile.Close();
                _cdgFile = null;
            }
        }
    }
}