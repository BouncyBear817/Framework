// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 16:41:13
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// 加密解密相关的实用函数
        /// </summary>
        public static class Encryption
        {
            public const int QuickEncryptLength = 220;

            /// <summary>
            /// 将bytes使用code做异或运算的快速版本
            /// </summary>
            /// <param name="bytes">原始二进制流</param>
            /// <param name="code">异或二进制流</param>
            /// <returns>异或后的二进制流</returns>
            public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
            {
                return GetXorBytes(bytes, 0, QuickEncryptLength, code);
            }

            /// <summary>
            /// 将bytes使用code做异或运算的快速版本，此方法将复用并改写传入的bytes作为返回值，无需额外分配内存空间
            /// </summary>
            /// <param name="bytes">原始或异或后二进制流</param>
            /// <param name="code">异或二进制流</param>
            public static void GetQuickSelfXorBytes(byte[] bytes, byte[] code)
            {
                GetSelfXorBytes(bytes, 0, QuickEncryptLength, code);
            }

            /// <summary>
            /// 将bytes使用code做异或运算
            /// </summary>
            /// <param name="bytes">原始二进制流</param>
            /// <param name="code">异或二进制流</param>
            /// <returns>异或后的二进制流</returns>
            public static byte[] GetXorBytes(byte[] bytes, byte[] code)
            {
                if (bytes == null)
                {
                    return null;
                }

                return GetXorBytes(bytes, 0, bytes.Length, code);
            }

            /// <summary>
            /// 将bytes使用code做异或运算，此方法将复用并改写传入的bytes作为返回值，无需额外分配内存空间
            /// </summary>
            /// <param name="bytes">原始或异或后二进制流</param>
            /// <param name="code">异或二进制流</param>
            public static void GetSelfXorBytes(byte[] bytes, byte[] code)
            {
                if (bytes == null)
                {
                    return;
                }

                GetSelfXorBytes(bytes, 0, bytes.Length, code);
            }

            /// <summary>
            /// 将bytes使用code做异或运算
            /// </summary>
            /// <param name="bytes">原始二进制流</param>
            /// <param name="startIndex">异或计算的开始位置</param>
            /// <param name="length">异或计算的长度</param>
            /// <param name="code">异或二进制流</param>
            /// <returns>异或后的二进制流</returns>
            public static byte[] GetXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
            {
                if (bytes == null)
                {
                    return null;
                }

                var bytesLength = bytes.Length;
                var results = new byte[bytesLength];
                Array.Copy(bytes, 0, results, 0, bytesLength);
                GetSelfXorBytes(results, startIndex, length, code);
                return results;
            }

            /// <summary>
            /// 将bytes使用code做异或运算，此方法将复用并改写传入的bytes作为返回值，无需额外分配内存空间
            /// </summary>
            /// <param name="bytes">原始或异或后二进制流</param>
            /// <param name="startIndex">异或计算的开始位置</param>
            /// <param name="length">异或计算的长度</param>
            /// <param name="code">异或二进制流</param>
            /// <exception cref="Exception"></exception>
            public static void GetSelfXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
            {
                if (bytes == null || code == null)
                {
                    throw new Exception("Bytes or code is invalid.");
                }

                var codeLength = code.Length;
                if (codeLength <= 0)
                {
                    throw new Exception("Code length is invalid.");
                }

                if (startIndex < 0 || length < 0 || startIndex + length > bytes.Length)
                {
                    throw new Exception("Start index or length is invalid.");
                }

                var codeIndex = startIndex % codeLength;
                for (var i = startIndex; i < length; i++)
                {
                    bytes[i] ^= code[codeIndex++];
                    codeIndex %= codeLength;
                }
            }
        }
    }
}