using System;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 实用函数集
    /// </summary>
    public static partial class Utility
    {
        public static class Converter
        {
            /// <summary>
            /// 将字节数组转换成UTF8编码的字符串
            /// </summary>
            /// <param name="bytes">字节数组</param>
            /// <returns>转换后的字符串</returns>
            public static string GetString(byte[] bytes)
            {
                return GetString(bytes, Encoding.UTF8);
            }

            /// <summary>
            /// 将字节数组转换成指定编码的字符串
            /// </summary>
            /// <param name="bytes">字节数组</param>
            /// <param name="encoding">指定编码</param>
            /// <returns>转换后的字符串</returns>
            /// <exception cref="Exception"></exception>
            public static string GetString(byte[] bytes, Encoding encoding)
            {
                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (encoding == null)
                {
                    throw new Exception("Encoding is invalid.");
                }

                return encoding.GetString(bytes);
            }

            /// <summary>
            /// 将字节数组转换成UTF8编码的字符串
            /// </summary>
            /// <param name="bytes">字节数组</param>
            /// <param name="startIndex">字节数组起始位置</param>
            /// <param name="length">字节数组长度</param>
            /// <returns>转换后的字符串</returns>
            public static string GetString(byte[] bytes, int startIndex, int length)
            {
                return GetString(bytes, startIndex, length, Encoding.UTF8);
            }

            /// <summary>
            /// 将字节数组转换成指定编码的字符串
            /// </summary>
            /// <param name="bytes">字节数组</param>
            /// <param name="startIndex">字节数组起始位置</param>
            /// <param name="length">字节数组长度</param>
            /// <param name="encoding">指定编码</param>
            /// <returns>转换后的字符串</returns>
            /// <exception cref="Exception"></exception>
            public static string GetString(byte[] bytes, int startIndex, int length, Encoding encoding)
            {
                if (bytes == null)
                {
                    throw new Exception("Bytes is invalid.");
                }

                if (encoding == null)
                {
                    throw new Exception("Encoding is invalid.");
                }

                return encoding.GetString(bytes, startIndex, length);
            }
        }
    }
}