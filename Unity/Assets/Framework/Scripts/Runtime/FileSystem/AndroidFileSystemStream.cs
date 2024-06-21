// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 10:31:23
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// Android文件系统流
    /// </summary>
    public sealed class AndroidFileSystemStream : FileSystemStream
    {
        private const string SplitFlag = "!/assets/";

        private static readonly int SplitLength = SplitFlag.Length;

        private static readonly AndroidJavaObject sAssetManager = null;
        private static readonly IntPtr sInternalReadMethodId = IntPtr.Zero;
        private static readonly jvalue[] sInternalReadArgs = null;

        private readonly AndroidJavaObject mFileStream;
        private readonly IntPtr mFileStreamRawObject;

        static AndroidFileSystemStream()
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (unityPlayer == null)
            {
                throw new Exception("Unity player is invalid.");
            }

            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (currentActivity == null)
            {
                throw new Exception("Current activity is invalid.");
            }

            var assetManager = currentActivity.Call<AndroidJavaObject>("getAssets");
            if (assetManager == null)
            {
                throw new Exception("Asset manager is invalid.");
            }

            sAssetManager = assetManager;

            var inputStreamClassPtr = AndroidJNI.FindClass("java/io/InputStream");
            sInternalReadMethodId = AndroidJNI.GetMethodID(inputStreamClassPtr, "read", "([BII)I");
            sInternalReadArgs = new jvalue[3];

            AndroidJNI.DeleteLocalRef(inputStreamClassPtr);
            currentActivity.Dispose();
            unityPlayer.Dispose();
        }

        public AndroidFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            if (access != FileSystemAccess.Read)
            {
                throw new Exception($"({access}) is not supported in AndroidFileSystemStream.");
            }

            if (createNew)
            {
                throw new Exception("Create new is not supported in AndroidFileSystemStream.");
            }

            var position = fullPath.LastIndexOf(SplitFlag, StringComparison.Ordinal);
            if (position < 0)
            {
                throw new Exception("Can not find split flag in full path.");
            }

            var fileName = fullPath.Substring(position + SplitLength);
            mFileStream = InternalOpen(fileName);

            if (mFileStream == null)
            {
                throw new Exception($"Open file ({fullPath}) from android asset manager failure.");
            }

            mFileStreamRawObject = mFileStream.GetRawObject();
        }

        /// <summary>
        /// 文件系统流的位置
        /// </summary>
        protected internal override long Position
        {
            get => throw new Exception("Get position is not supported in AndroidFileSystemStream.");
            set => Seek(value, SeekOrigin.Begin);
        }

        /// <summary>
        /// 文件系统流的长度
        /// </summary>
        protected internal override long Length => InternalAvailable();

        /// <summary>
        /// 设置文件系统流的长度
        /// </summary>
        /// <param name="length">文件系统流的长度</param>
        protected internal override void SetLength(long length)
        {
            throw new Exception("SetLength is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 定位文件系统流的位置
        /// </summary>
        /// <param name="offset">文件系统流位置的偏移</param>
        /// <param name="origin">文件系统流位置的方式</param>
        protected internal override void Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.End)
            {
                Seek(Length + offset, SeekOrigin.Begin);
                return;
            }

            if (origin == SeekOrigin.Begin)
            {
                InternalReset();
            }

            while (offset > 0)
            {
                var skip = InternalSkip(offset);
                if (skip < 0)
                {
                    return;
                }

                offset -= skip;
            }
        }

        /// <summary>
        /// 从文件系统中读取一个字节
        /// </summary>
        /// <returns>读取的字节，若到达文件末尾，返回-1</returns>
        protected internal override int ReadByte()
        {
            return InternalRead();
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
            var bytesRead = InternalRead(length, out var result);
            Array.Copy(result, 0, buffer, startIndex, bytesRead);
            return bytesRead;
        }

        /// <summary>
        /// 向文件系统流中写入一个字节
        /// </summary>
        /// <param name="value">写入的字节</param>
        protected internal override void WriteByte(byte value)
        {
            throw new Exception("WriteByte is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 向文件系统流中写入二进制流
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        protected internal override void Write(byte[] buffer, int startIndex, int length)
        {
            throw new Exception("Write is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 将文件系统流更新到存储介质中
        /// </summary>
        protected internal override void Flush()
        {
            throw new Exception("Flush is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 关闭文件系统流
        /// </summary>
        protected internal override void Close()
        {
            InternalClose();
            mFileStream.Dispose();
        }

        private AndroidJavaObject InternalOpen(string fileName)
        {
            return sAssetManager.Call<AndroidJavaObject>("open", fileName);
        }

        private int InternalAvailable()
        {
            return mFileStream.Call<int>("available");
        }

        private void InternalClose()
        {
            mFileStream.Call("close");
        }

        private int InternalRead()
        {
            return mFileStream.Call<int>("read");
        }

        private int InternalRead(int length, out byte[] result)
        {
#if UNITY_2019_2_OR_NEWER
#pragma warning disable CS0618
#endif
            var resultPtr = AndroidJNI.NewByteArray(length);
#if UNITY_2019_2_OR_NEWER
#pragma warning disable CS0618
#endif
            var offset = 0;
            var bytesLeft = length;
            while (bytesLeft > 0)
            {
                sInternalReadArgs[0] = new jvalue() { l = resultPtr };
                sInternalReadArgs[1] = new jvalue() { i = offset };
                sInternalReadArgs[2] = new jvalue() { i = bytesLeft };
                var bytesRead = AndroidJNI.CallIntMethod(mFileStreamRawObject, sInternalReadMethodId, sInternalReadArgs);
                if (bytesRead <= 0)
                {
                    break;
                }

                offset += bytesRead;
                bytesLeft -= bytesRead;
            }
#if UNITY_2019_2_OR_NEWER
#pragma warning disable CS0618
#endif
            result = AndroidJNI.FromByteArray(resultPtr);
#if UNITY_2019_2_OR_NEWER
#pragma warning disable CS0618
#endif
            AndroidJNI.DeleteLocalRef(resultPtr);
            return offset;
        }

        private void InternalReset()
        {
            mFileStream.Call("reset");
        }

        private long InternalSkip(long offset)
        {
            return mFileStream.Call<long>("skip", offset);
        }
    }
}