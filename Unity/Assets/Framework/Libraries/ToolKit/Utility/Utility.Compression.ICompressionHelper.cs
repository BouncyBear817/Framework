// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 16:39:35
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;

namespace Framework
{
    public static partial class Utility
    {
        public static partial class Compression
        {
            /// <summary>
            /// 压缩解压缩辅助器接口
            /// </summary>
            public interface ICompressionHelper
            {
                /// <summary>
                /// 压缩数据
                /// </summary>
                /// <param name="bytes">要压缩的二进制流</param>
                /// <param name="offset">二进制流的偏移</param>
                /// <param name="length">二进制流的长度</param>
                /// <param name="compressStream">压缩后的二进制流</param>
                /// <returns>是否成功压缩数据</returns>
                bool Compress(byte[] bytes, int offset, int length, Stream compressStream);

                /// <summary>
                /// 压缩数据
                /// </summary>
                /// <param name="stream">要压缩的二进制流</param>
                /// <param name="compressStream">压缩后的二进制流</param>
                /// <returns>是否成功压缩数据</returns>
                bool Compress(Stream stream, Stream compressStream);

                /// <summary>
                /// 解压缩数据
                /// </summary>
                /// <param name="bytes">要解压缩的二进制流</param>
                /// <param name="offset">二进制流的偏移</param>
                /// <param name="length">二进制流的长度</param>
                /// <param name="decompressStream">解压缩后的二进制流</param>
                /// <returns>是否成功解压缩数据</returns>
                bool Decompress(byte[] bytes, int offset, int length, Stream decompressStream);

                /// <summary>
                /// 解压缩数据
                /// </summary>
                /// <param name="stream">要解压缩的二进制流</param>
                /// <param name="decompressStream">解压缩后的二进制流</param>
                /// <returns>是否成功解压缩数据</returns>
                bool Decompress(Stream stream, Stream decompressStream);
            }
        }
    }
}