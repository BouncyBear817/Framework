// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/6 11:13:59
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Runtime.InteropServices;

namespace Framework
{
    public sealed partial class FileSystem : IFileSystem
    {
        /// <summary>
        /// 头数据
        /// </summary>
        private struct HeaderData
        {
            private const int HeaderLength = 3;
            private const int FileSystemVersion = 0;
            private const int EncryptBytesLength = 4;

            private static readonly byte[] Header = new byte[HeaderLength] { (byte)'F', (byte)'H', (byte)'D' };

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = HeaderLength)]
            private readonly byte[] mHeader;

            private readonly byte mVersion;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = EncryptBytesLength)]
            private readonly byte[] mEncryptBytes;

            private readonly int mMaxFileCount;
            private readonly int mMaxBlockCount;
            private readonly int mBlockCount;

            public HeaderData(int maxFileCount, int maxBlockCount) : this(FileSystemVersion, new byte[EncryptBytesLength], maxFileCount, maxBlockCount, 0)
            {
                Utility.Random.GetRandomBytes(mEncryptBytes);
            }

            public HeaderData(byte version, byte[] encryptBytes, int maxFileCount, int maxBlockCount, int blockCount) : this()
            {
                mVersion = version;
                mEncryptBytes = encryptBytes;
                mMaxFileCount = maxFileCount;
                mMaxBlockCount = maxBlockCount;
                mBlockCount = blockCount;
            }

            public bool IsValid => mHeader.Length == HeaderLength && mHeader[0] == Header[0] && mHeader[1] == Header[1] && mHeader[2] == Header[2] &&
                                   mVersion == FileSystemVersion && mEncryptBytes.Length == EncryptBytesLength &&
                                   mMaxFileCount > 0 && mMaxBlockCount > 0 && mMaxFileCount <= mMaxBlockCount && mBlockCount > 0 && mBlockCount <= mMaxBlockCount;

            public byte Version => mVersion;

            public int MaxFileCount => mMaxFileCount;

            public int MaxBlockCount => mMaxBlockCount;

            public int BlockCount => mBlockCount;

            public byte[] EncryptBytes => mEncryptBytes;

            public HeaderData SetBlockCount(int blockCount)
            {
                return new HeaderData(mVersion, mEncryptBytes, mMaxFileCount, mMaxBlockCount, blockCount);
            }
        }
    }
}