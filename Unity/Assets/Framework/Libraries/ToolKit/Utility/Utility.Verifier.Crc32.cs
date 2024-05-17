// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 16:42:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public static partial class Utility
    {
        public static partial class Verifier
        {
            /// <summary>
            /// Crc32算法
            /// </summary>
            private sealed class Crc32
            {
                private const int TableLength = 256;
                private const uint DefaultPolynomial = 0xedb88320;
                private const uint DefaultSeed = 0xffffffff;

                private readonly uint mSeed;
                private readonly uint[] mTable;
                private uint mHash;

                public Crc32() : this(DefaultPolynomial, DefaultSeed)
                {
                }

                public Crc32(uint polynomial, uint seed)
                {
                    mSeed = seed;
                    mTable = InitializeTable(polynomial);
                    mHash = seed;
                }

                public void Initialize()
                {
                    mHash = mSeed;
                }

                public void HashCore(byte[] bytes, int offset, int length)
                {
                    mHash = CalculateHash(mTable, mHash, bytes, offset, length);
                }

                public uint HashFinal()
                {
                    return ~mHash;
                }

                private static uint CalculateHash(uint[] table, uint value, byte[] bytes, int offset, int length)
                {
                    var last = offset + length;
                    for (int i = 0; i < last; i++)
                    {
                        unchecked
                        {
                            value = (value >> 8) ^ table[bytes[i] ^ value & 0xff];
                        }
                    }

                    return value;
                }

                private static uint[] InitializeTable(uint polynomial)
                {
                    var table = new uint[TableLength];
                    for (int i = 0; i < TableLength; i++)
                    {
                        var entry = (uint)i;
                        for (int j = 0; j < 8; j++)
                        {
                            if ((entry & 1) == 1)
                            {
                                entry = (entry >> 1) ^ polynomial;
                            }
                            else
                            {
                                entry >>= 1;
                            }

                            table[i] = entry;
                        }
                    }

                    return table;
                }
            }
        }
    }
}