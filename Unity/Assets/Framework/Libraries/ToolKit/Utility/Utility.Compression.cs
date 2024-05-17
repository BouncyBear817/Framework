/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/16 16:39:35
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.IO;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// 压缩解压缩的实用函数
        /// </summary>
        public static partial class Compression
        {
            private static ICompressionHelper sCompressionHelper = null;

            /// <summary>
            /// 设置压缩解压缩的辅助器
            /// </summary>
            /// <param name="compressionHelper">压缩解压缩的辅助器</param>
            public static void SetCompressionHelper(ICompressionHelper compressionHelper)
            {
                sCompressionHelper = compressionHelper;
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="bytes">要压缩的二进制流</param>
            /// <returns>压缩后的二进制流</returns>
            public static byte[] Compress(byte[] bytes)
            {
                return Compress(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="bytes">要压缩的二进制流</param>
            /// <param name="offset">二进制流的偏移</param>
            /// <param name="length">二进制流的长度</param>
            /// <returns>压缩后的二进制流</returns>
            public static byte[] Compress(byte[] bytes, int offset, int length)
            {
                using (var compressStream = new MemoryStream())
                {
                    if (Compress(bytes, offset, length, compressStream))
                    {
                        return compressStream.ToArray();
                    }

                    return null;
                }
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="bytes">要压缩的二进制流</param>
            /// <param name="compressStream">压缩后的二进制流</param>
            /// <returns>是否成功压缩数据</returns>
            public static bool Compress(byte[] bytes, Stream compressStream)
            {
                return Compress(bytes, 0, bytes.Length, compressStream);
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="bytes">要压缩的二进制流</param>
            /// <param name="offset">二进制流的偏移</param>
            /// <param name="length">二进制流的长度</param>
            /// <param name="compressStream">压缩后的二进制流</param>
            /// <returns>是否成功压缩数据</returns>
            public static bool Compress(byte[] bytes, int offset, int length, Stream compressStream)
            {
                if (sCompressionHelper == null)
                {
                    throw new Exception("Compression helper is invalid.");
                }

                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (offset < 0 || offset + length > bytes.Length)
                {
                    throw new Exception("Offset or length is invalid.");
                }

                if (compressStream == null)
                {
                    throw new Exception("Compress stream is invalid.");
                }

                try
                {
                    return sCompressionHelper.Compress(bytes, offset, length, compressStream);
                }
                catch (Exception e)
                {
                    throw new Exception($"Can not compress with exception ({e}).");
                }
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="stream">要压缩的二进制流</param>
            /// <returns>压缩后的二进制流</returns>
            public static byte[] Compress(Stream stream)
            {
                using (var compressStream = new MemoryStream())
                {
                    if (Compress(stream, compressStream))
                    {
                        return compressStream.ToArray();
                    }

                    return null;
                }
            }

            /// <summary>
            /// 压缩数据
            /// </summary>
            /// <param name="stream">要压缩的二进制流</param>
            /// <param name="compressStream">压缩后的二进制流</param>
            /// <returns>是否成功压缩数据</returns>
            public static bool Compress(Stream stream, Stream compressStream)
            {
                if (sCompressionHelper == null)
                {
                    throw new Exception("Compression helper is invalid.");
                }

                if (stream == null)
                {
                    throw new Exception("Stream is invalid.");
                }

                if (compressStream == null)
                {
                    throw new Exception(" Compress stream is invalid.");
                }

                try
                {
                    return sCompressionHelper.Compress(stream, compressStream);
                }
                catch (Exception e)
                {
                    throw new Exception($"Can not compress with exception ({e}).");
                }
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="bytes">要解压缩的二进制流</param>
            /// <returns>解压缩后的二进制流</returns>
            public static byte[] Decompress(byte[] bytes)
            {
                return Decompress(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="bytes">要解压缩的二进制流</param>
            /// <param name="offset">二进制流的偏移</param>
            /// <param name="length">二进制流的长度</param>
            /// <returns>解压缩后的二进制流</returns>
            public static byte[] Decompress(byte[] bytes, int offset, int length)
            {
                using (var decompressStream = new MemoryStream())
                {
                    if (Decompress(bytes, offset, length, decompressStream))
                    {
                        return decompressStream.ToArray();
                    }

                    return null;
                }
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="bytes">要解压缩的二进制流</param>
            /// <param name="decompressStream"></param>
            /// <returns>是否成功解压缩数据</returns>
            public static bool Decompress(byte[] bytes, Stream decompressStream)
            {
                return Decompress(bytes, 0, bytes.Length, decompressStream);
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="bytes">要解压缩的二进制流</param>
            /// <param name="offset">二进制流的偏移</param>
            /// <param name="length">二进制流的长度</param>
            /// <param name="decompressStream">解压缩后的二进制流</param>
            /// <returns>是否成功解压缩数据</returns>
            public static bool Decompress(byte[] bytes, int offset, int length, Stream decompressStream)
            {
                if (sCompressionHelper == null)
                {
                    throw new Exception("Compression helper is invalid.");
                }

                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (offset < 0 || offset + length > bytes.Length)
                {
                    throw new Exception("Offset or length is invalid.");
                }

                if (decompressStream == null)
                {
                    throw new Exception("Decompress stream is invalid.");
                }

                try
                {
                    return sCompressionHelper.Decompress(bytes, offset, length, decompressStream);
                }
                catch (Exception e)
                {
                    throw new Exception($"Can not decompress with exception ({e}).");
                }
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="stream">要解压缩的二进制流</param>
            /// <returns>解压缩后的二进制流</returns>
            public static byte[] Decompress(Stream stream)
            {
                using (var decompressStream = new MemoryStream())
                {
                    if (Decompress(stream, decompressStream))
                    {
                        return decompressStream.ToArray();
                    }

                    return null;
                }
            }

            /// <summary>
            /// 解压缩数据
            /// </summary>
            /// <param name="stream">要解压缩的二进制流</param>
            /// <param name="decompressStream">解压缩后的二进制流</param>
            /// <returns>是否成功解压缩数据</returns>
            public static bool Decompress(Stream stream, Stream decompressStream)
            {
                if (sCompressionHelper == null)
                {
                    throw new Exception("Compression helper is invalid.");
                }

                if (stream == null)
                {
                    throw new Exception("Stream is invalid.");
                }

                if (decompressStream == null)
                {
                    throw new Exception(" Decompress stream is invalid.");
                }

                try
                {
                    return sCompressionHelper.Decompress(stream, decompressStream);
                }
                catch (Exception e)
                {
                    throw new Exception($"Can not decompress with exception ({e}).");
                }
            }
        }
    }
}