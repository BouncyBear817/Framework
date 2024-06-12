/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.IO;
using Framework;

namespace Runtime
{
    /// <summary>
    /// 对BinaryReader与BinaryWriter的扩展方法
    /// </summary>
    public static class BinaryExtension
    {
        private static readonly byte[] sCachedBytes = new byte[byte.MaxValue + 1];

        /// <summary>
        /// 从二进制流读取编码后的32位有符号整数
        /// </summary>
        /// <param name="binaryReader">读取的二进制流</param>
        /// <returns>32位有符号整数</returns>
        public static int Read7BitEncodedInt32(this BinaryReader binaryReader)
        {
            var value = 0;
            var shift = 0;
            byte b;
            do
            {
                if (shift >= 35)
                {
                    throw new Exception("7 bit encoded int is invalid.");
                }

                b = binaryReader.ReadByte();
                value |= (b & 0x7f) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            return value;
        }

        /// <summary>
        /// 向二进制流写入编码后的32位有符号整数
        /// </summary>
        /// <param name="binaryWriter">写入的二进制流</param>
        /// <param name="value">写入的32位有符号整数</param>
        /// <returns></returns>
        public static void Write7BitEncodedInt32(this BinaryWriter binaryWriter, int value)
        {
            var num = (uint)value;
            while (num >= 0x80)
            {
                binaryWriter.Write((byte)(num | 0x80));
                num >>= 7;
            }

            binaryWriter.Write((byte)num);
        }

        /// <summary>
        /// 从二进制流读取编码后的32位有符号整数
        /// </summary>
        /// <param name="binaryReader">读取的二进制流</param>
        /// <returns>32位有符号整数</returns>
        public static uint Read7BitEncodedUInt32(this BinaryReader binaryReader)
        {
            return (uint)Read7BitEncodedInt32(binaryReader);
        }

        /// <summary>
        /// 向二进制流写入编码后的32位有符号整数
        /// </summary>
        /// <param name="binaryWriter">写入的二进制流</param>
        /// <param name="value">写入的32位有符号整数</param>
        /// <returns></returns>
        public static void Write7BitEncodedUInt32(this BinaryWriter binaryWriter, uint value)
        {
            Write7BitEncodedInt32(binaryWriter, (int)value);
        }

        /// <summary>
        /// 从二进制流读取编码后的64位有符号整数
        /// </summary>
        /// <param name="binaryReader">读取的二进制流</param>
        /// <returns>64位有符号整数</returns>
        public static long Read7BitEncodedInt64(this BinaryReader binaryReader)
        {
            var value = 0L;
            var shift = 0;
            byte b;
            do
            {
                if (shift >= 70)
                {
                    throw new Exception("7 bit encoded int is invalid.");
                }

                b = binaryReader.ReadByte();
                value |= (b & 0x7fL) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            return value;
        }

        /// <summary>
        /// 向二进制流写入编码后的64位有符号整数
        /// </summary>
        /// <param name="binaryWriter">写入的二进制流</param>
        /// <param name="value">写入的64位有符号整数</param>
        /// <returns></returns>
        public static void Write7BitEncodedInt64(this BinaryWriter binaryWriter, long value)
        {
            var num = (ulong)value;
            while (num >= 0x80)
            {
                binaryWriter.Write((byte)(num | 0x80));
                num >>= 7;
            }

            binaryWriter.Write((byte)num);
        }

        /// <summary>
        /// 从二进制流读取编码后的64位有符号整数
        /// </summary>
        /// <param name="binaryReader">读取的二进制流</param>
        /// <returns>64位有符号整数</returns>
        public static ulong Read7BitEncodedUInt64(this BinaryReader binaryReader)
        {
            return (ulong)Read7BitEncodedInt64(binaryReader);
        }

        /// <summary>
        /// 向二进制流写入编码后的64位有符号整数
        /// </summary>
        /// <param name="binaryWriter">写入的二进制流</param>
        /// <param name="value">写入的64位有符号整数</param>
        /// <returns></returns>
        public static void Write7BitEncodedUInt64(this BinaryWriter binaryWriter, ulong value)
        {
            Write7BitEncodedInt64(binaryWriter, (long)value);
        }

        /// <summary>
        /// 从二进制流读取加密字符串
        /// </summary>
        /// <param name="binaryReader">读取的二进制流</param>
        /// <param name="encryptBytes">密钥数组</param>
        /// <returns>加密字符串</returns>
        public static string ReadEncryptedString(this BinaryReader binaryReader, byte[] encryptBytes)
        {
            var length = binaryReader.ReadByte();
            if (length <= 0)
            {
                return null;
            }

            if (length > byte.MaxValue)
            {
                throw new Exception("String is too long.");
            }

            for (int i = 0; i < length; i++)
            {
                sCachedBytes[i] = binaryReader.ReadByte();
            }

            Utility.Encryption.GetSelfXorBytes(sCachedBytes, 0, length, encryptBytes);
            var value = Utility.Converter.GetString(sCachedBytes, 0, length);
            Array.Clear(sCachedBytes, 0, length);
            return value;
        }

        /// <summary>
        /// 向二进制流写入加密字符串
        /// </summary>
        /// <param name="binaryWriter">写入的二进制流</param>
        /// <param name="value">写入的字符串</param>
        /// <param name="encryptBytes">密钥数组</param>
        public static void WriteEncryptedString(this BinaryWriter binaryWriter, string value, byte[] encryptBytes)
        {
            if (string.IsNullOrEmpty(value))
            {
                binaryWriter.Write((byte)0);
                return;
            }

            var length = Utility.Converter.GetBytes(value, sCachedBytes);
            if (length > byte.MaxValue)
            {
                throw new Exception($"String ({value}) is too long.");
            }

            Utility.Encryption.GetSelfXorBytes(sCachedBytes, encryptBytes);
            binaryWriter.Write((byte)length);
            binaryWriter.Write(sCachedBytes, 0, length);
        }
    }
}