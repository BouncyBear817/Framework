// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 16:42:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// 校验相关的实用函数
        /// </summary>
        public static partial class Verifier
        {
            private const int CachedBytesLength = 0x1000;

            private static readonly byte[] sCachedBytes = new byte[CachedBytesLength];
            private static readonly Crc32 sAlgorithm = new Crc32();

            /// <summary>
            /// 计算二进制流的Crc32
            /// </summary>
            /// <param name="bytes">二进制流</param>
            /// <returns>计算后的Crc32</returns>
            /// <exception cref="Exception"></exception>
            public static int GetCrc32(byte[] bytes)
            {
                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                return GetCrc32(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// 计算二进制流的Crc32
            /// </summary>
            /// <param name="bytes">二进制流</param>
            /// <param name="offset">二进制流的偏移</param>
            /// <param name="length">二进制流的长度</param>
            /// <returns>计算后的Crc32</returns>
            /// <exception cref="Exception"></exception>
            public static int GetCrc32(byte[] bytes, int offset, int length)
            {
                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (offset < 0 || length < 0 || offset + length > bytes.Length)
                {
                    throw new Exception("Offset or length is invalid.");
                }

                sAlgorithm.HashCore(bytes, offset, length);
                var result = (int)sAlgorithm.HashFinal();
                sAlgorithm.Initialize();
                return result;
            }

            /// <summary>
            /// 计算二进制流的Crc32
            /// </summary>
            /// <param name="stream">二进制流</param>
            /// <returns>计算后的Crc32</returns>
            /// <exception cref="Exception"></exception>
            public static int GetCrc32(Stream stream)
            {
                if (stream == null)
                {
                    throw new Exception("Stream is invalid.");
                }

                while (true)
                {
                    var bytesRead = stream.Read(sCachedBytes, 0, CachedBytesLength);
                    if (bytesRead > 0)
                    {
                        sAlgorithm.HashCore(sCachedBytes, 0, bytesRead);
                    }
                    else
                    {
                        break;
                    }
                }

                var result = (int)sAlgorithm.HashFinal();
                sAlgorithm.Initialize();
                Array.Clear(sCachedBytes, 0, CachedBytesLength);
                return result;
            }

            /// <summary>
            /// 计算二进制流的Crc32
            /// </summary>
            /// <param name="stream">二进制流</param>
            /// <param name="code">异或二进制流</param>
            /// <param name="length">二进制流的长度</param>
            /// <returns>计算后的Crc32</returns>
            /// <exception cref="Exception"></exception>
            public static int GetCrc32(Stream stream, byte[] code, int length)
            {
                if (stream == null)
                {
                    throw new Exception("Stream is invalid.");
                }

                if (code == null)
                {
                    throw new Exception("Code is invalid.");
                }

                var codeLength = code.Length;
                if (codeLength <= 0)
                {
                    throw new Exception("Code length is invalid.");
                }

                var bytesLength = (int)stream.Length;
                if (length < 0 || length > bytesLength)
                {
                    length = bytesLength;
                }

                var codeIndex = 0;
                while (true)
                {
                    var bytesRead = stream.Read(sCachedBytes, 0, CachedBytesLength);
                    if (bytesRead > 0)
                    {
                        if (length > 0)
                        {
                            for (int i = 0; i < bytesRead && i < length; i++)
                            {
                                sCachedBytes[i] ^= code[codeIndex++];
                                codeIndex %= codeLength;
                            }

                            length -= bytesRead;
                        }

                        sAlgorithm.HashCore(sCachedBytes, 0, bytesRead);
                    }
                    else
                    {
                        break;
                    }
                }

                var result = (int)sAlgorithm.HashFinal();
                sAlgorithm.Initialize();
                Array.Clear(sCachedBytes, 0, CachedBytesLength);
                return result;
            }

            /// <summary>
            /// 获取Crc32数值的二进制数组
            /// </summary>
            /// <param name="crc32">Crc32数值</param>
            /// <returns>Crc32数值的二进制数组</returns>
            public static byte[] GetCrc32Bytes(int crc32)
            {
                return new[]
                {
                    (byte)((crc32 >> 24) & 0xff),
                    (byte)((crc32 >> 16) & 0xff),
                    (byte)((crc32 >> 8) & 0xff),
                    (byte)(crc32 & 0xff)
                };
            }

            /// <summary>
            /// 获取Crc32数值的二进制数组
            /// </summary>
            /// <param name="crc32">Crc32数值</param>
            /// <param name="bytes">Crc32数值的二进制数组</param>
            public static void GetCrc32Bytes(int crc32, byte[] bytes)
            {
                GetCrc32Bytes(crc32, bytes, 0);
            }

            /// <summary>
            /// 获取Crc32数值的二进制数组
            /// </summary>
            /// <param name="crc32">Crc32数值</param>
            /// <param name="bytes">Crc32数值的二进制数组</param>
            /// <param name="offset">二进制数组的起始位置 </param>
            /// <exception cref="Exception"></exception>
            public static void GetCrc32Bytes(int crc32, byte[] bytes, int offset)
            {
                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (offset < 0 || offset + 4 > bytes.Length)
                {
                    throw new Exception("Offset or length is invalid.");
                }

                bytes[offset] = (byte)((crc32 >> 24) & 0xff);
                bytes[offset + 1] = (byte)((crc32 >> 16) & 0xff);
                bytes[offset + 2] = (byte)((crc32 >> 8) & 0xff);
                bytes[offset + 3] = (byte)(crc32 & 0xff);
            }
        }
    }
}