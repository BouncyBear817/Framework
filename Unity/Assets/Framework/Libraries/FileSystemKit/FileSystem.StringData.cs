// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/6 11:13:59
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Framework
{
    public sealed partial class FileSystem : IFileSystem
    {
        /// <summary>
        /// 字符串数据
        /// </summary>
        private struct StringData
        {
            private static readonly byte[] sCachedBytes = new byte[byte.MaxValue + 1];

            private readonly byte mLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = byte.MaxValue)]
            private readonly byte[] mBytes;

            public StringData(byte length, byte[] bytes)
            {
                mLength = length;
                mBytes = bytes;
            }

            public string GetString(byte[] encryptBytes)
            {
                if (mLength <= 0)
                {
                    return null;
                }

                Array.Copy(mBytes, 0, sCachedBytes, 0, mLength);
                Utility.Encryption.GetSelfXorBytes(sCachedBytes, 0, mLength, encryptBytes);
                return Utility.Converter.GetString(sCachedBytes, 0, mLength);
            }

            public StringData SetString(string value, byte[] encryptBytes)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return Clear();
                }

                var length = Utility.Converter.GetBytes(value, sCachedBytes);
                if (length > byte.MaxValue)
                {
                    throw new Exception($"String ({value}) is too long.");
                }

                Utility.Encryption.GetSelfXorBytes(sCachedBytes, encryptBytes);
                Array.Copy(sCachedBytes, 0, mBytes, 0, mLength);
                return new StringData((byte)length, mBytes);
            }

            public StringData Clear()
            {
                return new StringData(0, mBytes);
            }
        }
    }
}