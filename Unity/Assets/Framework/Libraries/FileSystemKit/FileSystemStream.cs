// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 15:51:49
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    /// <summary>
    /// 文件系统流
    /// </summary>
    public abstract class FileSystemStream
    {
        /// <summary>
        /// 缓存的二进制流长度
        /// </summary>
        protected const int CachedBytesLength = 0x1000;

        /// <summary>
        /// 缓存的二进制流
        /// </summary>
        protected static readonly byte[] sCachedBytes = new byte[CachedBytesLength];

        /// <summary>
        /// 文件系统流的位置
        /// </summary>
        protected internal abstract long Position { get; set; }

        /// <summary>
        /// 文件系统流的长度
        /// </summary>
        protected internal abstract long Length { get; }

        /// <summary>
        /// 设置文件系统流的长度
        /// </summary>
        /// <param name="length">文件系统流的长度</param>
        protected internal abstract void SetLength(long length);

        /// <summary>
        /// 定位文件系统流的位置
        /// </summary>
        /// <param name="offset">文件系统流位置的偏移</param>
        /// <param name="origin">文件系统流位置的方式</param>
        protected internal abstract void Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// 从文件系统中读取一个字节
        /// </summary>
        /// <returns>读取的字节，若到达文件末尾，返回-1</returns>
        protected internal abstract int ReadByte();

        /// <summary>
        /// 从文件系统中读取二进制流
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>实际读取了多少字节</returns>
        protected internal abstract int Read(byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统中读取二进制流
        /// </summary>
        /// <param name="stream">二进制流</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>实际读取了多少字节</returns>
        protected internal int Read(Stream stream, int length)
        {
            var bytesRead = 0;
            var bytesLeft = length;
            while ((bytesRead = Read(sCachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
            {
                bytesLeft -= bytesRead;
                stream.Write(sCachedBytes, 0, bytesRead);
            }

            Array.Clear(sCachedBytes, 0, CachedBytesLength);
            return length - bytesLeft;
        }

        /// <summary>
        /// 向文件系统流中写入一个字节
        /// </summary>
        /// <param name="value">写入的字节</param>
        protected internal abstract void WriteByte(byte value);

        /// <summary>
        /// 向文件系统流中写入二进制流
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        protected internal abstract void Write(byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 向文件系统流中写入二进制流
        /// </summary>
        /// <param name="stream">二进制流</param>
        /// <param name="length">二进制流的长度</param>
        protected internal void Write(Stream stream, int length)
        {
            var bytesRead = 0;
            var bytesLeft = length;
            while ((bytesRead = stream.Read(sCachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
            {
                bytesLeft -= bytesRead;
                Write(sCachedBytes, 0, bytesRead);
            }

            Array.Clear(sCachedBytes, 0, CachedBytesLength);
        }

        /// <summary>
        /// 将文件系统流更新到存储介质中
        /// </summary>
        protected internal abstract void Flush();

        /// <summary>
        /// 关闭文件系统流
        /// </summary>
        protected internal abstract void Close();
    }
}