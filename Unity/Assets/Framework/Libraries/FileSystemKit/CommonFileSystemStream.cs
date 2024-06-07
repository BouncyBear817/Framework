// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/7 10:35:7
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    /// <summary>
    /// 通用文件系统流
    /// </summary>
    public sealed class CommonFileSystemStream : FileSystemStream, IDisposable
    {
        private readonly FileStream mFileStream;

        public CommonFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            switch (access)
            {
                case FileSystemAccess.Read:
                    mFileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                case FileSystemAccess.Write:
                    mFileStream = new FileStream(fullPath, createNew ? FileMode.Create : FileMode.Open, FileAccess.Write, FileShare.Read);
                    break;
                case FileSystemAccess.ReadWrite:
                    mFileStream = new FileStream(fullPath, createNew ? FileMode.Create : FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                    break;
                default:
                    throw new Exception("Access is invalid.");
            }
        }

        /// <summary>
        /// 文件系统流的位置
        /// </summary>
        protected internal override long Position
        {
            get => mFileStream.Position;
            set => mFileStream.Position = value;
        }

        /// <summary>
        /// 文件系统流的长度
        /// </summary>
        protected internal override long Length => mFileStream.Length;

        /// <summary>
        /// 设置文件系统流的长度
        /// </summary>
        /// <param name="length">文件系统流的长度</param>
        protected internal override void SetLength(long length)
        {
            mFileStream.SetLength(length);
        }

        /// <summary>
        /// 定位文件系统流的位置
        /// </summary>
        /// <param name="offset">文件系统流位置的偏移</param>
        /// <param name="origin">文件系统流位置的方式</param>
        protected internal override void Seek(long offset, SeekOrigin origin)
        {
            mFileStream.Seek(offset, origin);
        }

        /// <summary>
        /// 从文件系统中读取一个字节
        /// </summary>
        /// <returns>读取的字节，若到达文件末尾，返回-1</returns>
        protected internal override int ReadByte()
        {
            return mFileStream.ReadByte();
        }

        /// <summary>
        /// 从文件系统中读取二进制流
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>实际读取了多少字节</returns>
        protected internal override int Read(byte[] buffer, int startIndex, int length)
        {
            return mFileStream.Read(buffer, startIndex, length);
        }

        /// <summary>
        /// 向文件系统流中写入一个字节
        /// </summary>
        /// <param name="value">写入的字节</param>
        protected internal override void WriteByte(byte value)
        {
            mFileStream.WriteByte(value);
        }

        /// <summary>
        /// 向文件系统流中写入二进制流
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        protected internal override void Write(byte[] buffer, int startIndex, int length)
        {
            mFileStream.Write(buffer, startIndex, length);
        }

        /// <summary>
        /// 将文件系统流更新到存储介质中
        /// </summary>
        protected internal override void Flush()
        {
            mFileStream.Flush();
        }

        /// <summary>
        /// 关闭文件系统流
        /// </summary>
        protected internal override void Close()
        {
            mFileStream.Close();
        }

        public void Dispose()
        {
            mFileStream.Dispose();
        }
    }
}