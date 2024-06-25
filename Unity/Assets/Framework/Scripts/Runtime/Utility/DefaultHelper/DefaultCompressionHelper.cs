// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/17 15:53:57
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using Framework;
using ICSharpCode.SharpZipLib.GZip;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认压缩解压缩辅助器
    /// </summary>
    public class DefaultCompressionHelper : Utility.Compression.ICompressionHelper
    {
        private const int CachedBytesLength = 0x1000;

        private readonly byte[] mCachedBytes = new byte[CachedBytesLength];

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="bytes">要压缩的二进制流</param>
        /// <param name="offset">二进制流的偏移</param>
        /// <param name="length">二进制流的长度</param>
        /// <param name="compressStream">压缩后的二进制流</param>
        /// <returns>是否成功压缩数据</returns>
        public bool Compress(byte[] bytes, int offset, int length, Stream compressStream)
        {
            if (bytes == null || compressStream == null)
            {
                return false;
            }

            if (offset < 0 || length < 0 || offset + length > bytes.Length)
            {
                return false;
            }

            try
            {
                var gZipOutputStream = new GZipOutputStream(compressStream);
                gZipOutputStream.Write(bytes, offset, length);
                gZipOutputStream.Finish();
                ProcessHeader(compressStream);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="stream">要压缩的二进制流</param>
        /// <param name="compressStream">压缩后的二进制流</param>
        /// <returns>是否成功压缩数据</returns>
        public bool Compress(Stream stream, Stream compressStream)
        {
            if (stream == null || compressStream == null)
            {
                return false;
            }

            try
            {
                var gZipOutputStream = new GZipOutputStream(compressStream);
                var bytesRead = 0;
                while ((bytesRead = stream.Read(mCachedBytes, 0, CachedBytesLength)) > 0)
                {
                    gZipOutputStream.Write(mCachedBytes, 0, bytesRead);
                }

                gZipOutputStream.Finish();
                ProcessHeader(compressStream);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Array.Clear(mCachedBytes, 0, CachedBytesLength);
            }
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="bytes">要解压缩的二进制流</param>
        /// <param name="offset">二进制流的偏移</param>
        /// <param name="length">二进制流的长度</param>
        /// <param name="decompressStream">解压缩后的二进制流</param>
        /// <returns>是否成功解压缩数据</returns>
        public bool Decompress(byte[] bytes, int offset, int length, Stream decompressStream)
        {
            if (bytes == null || decompressStream == null)
            {
                return false;
            }

            if (offset < 0 || length < 0 || offset + length > bytes.Length)
            {
                return false;
            }

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(bytes, offset, length, false);
                using (var gZipInputStream = new GZipInputStream(memoryStream))
                {
                    int bytesRead = 0;
                    while ((bytesRead = gZipInputStream.Read(mCachedBytes, 0, CachedBytesLength)) > 0)
                    {
                        decompressStream.Write(mCachedBytes, 0, bytesRead);
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }

                Array.Clear(mCachedBytes, 0, CachedBytesLength);
            }
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="stream">要解压缩的二进制流</param>
        /// <param name="decompressStream">解压缩后的二进制流</param>
        /// <returns>是否成功解压缩数据</returns>
        public bool Decompress(Stream stream, Stream decompressStream)
        {
            if (stream == null || decompressStream == null)
            {
                return false;
            }

            try
            {
                var gZipInputStream = new GZipInputStream(stream);
                var bytesRead = 0;
                while ((bytesRead = gZipInputStream.Read(mCachedBytes, 0, CachedBytesLength)) > 0)
                {
                    decompressStream.Write(mCachedBytes, 0, bytesRead);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Array.Clear(mCachedBytes, 0, CachedBytesLength);
            }
        }

        private static void ProcessHeader(Stream compressStream)
        {
            if (compressStream.Length >= 8L)
            {
                var current = compressStream.Position;
                compressStream.Position = 4L;
                compressStream.WriteByte(25);
                compressStream.WriteByte(134);
                compressStream.WriteByte(2);
                compressStream.WriteByte(32);
                compressStream.Position = current;
            }
        }
    }
}