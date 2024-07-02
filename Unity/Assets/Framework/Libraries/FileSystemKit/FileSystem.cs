// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/6 11:1:34
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Framework
{
    /// <summary>
    /// 文件系统
    /// </summary>
    public sealed partial class FileSystem : IFileSystem
    {
        private const int ClusterSize = 1024 * 4;
        private const int CachedBytesLength = 0x1000;

        private static readonly string[] sEmptyStringArray = new string[] { };
        private static readonly byte[] sCachedBytes = new byte[CachedBytesLength];
        private static readonly int sHeaderDataSize = Marshal.SizeOf(typeof(HeaderData));
        private static readonly int sBlockDataSize = Marshal.SizeOf(typeof(BlockData));
        private static readonly int sStringDataSize = Marshal.SizeOf(typeof(StringData));

        private readonly string mFullPath;
        private readonly FileSystemAccess mAccess;
        private readonly FileSystemStream mStream;
        private readonly Dictionary<string, int> mFileDatas;
        private readonly List<BlockData> mBlockDatas;
        private readonly MultiDictionary<int, int> mFreeBlockIndexes;
        private readonly SortedDictionary<int, StringData> mStringDatas;
        private readonly Queue<int> mFreeStringIndexes;
        private readonly Queue<StringData> mFreeStringDatas;

        private HeaderData mHeaderData;
        private int mBlockDataOffset;
        private int mStringDataOffset;
        private int mFileDataOffset;

        public FileSystem(string fullPath, FileSystemAccess access, FileSystemStream stream)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified)
            {
                throw new Exception("File system access is invalid.");
            }

            if (stream == null)
            {
                throw new Exception("File system stream is invalid.");
            }

            mFullPath = fullPath;
            mAccess = access;
            mStream = stream;
            mFileDatas = new Dictionary<string, int>(StringComparer.Ordinal);
            mBlockDatas = new List<BlockData>();
            mFreeBlockIndexes = new MultiDictionary<int, int>();
            mStringDatas = new SortedDictionary<int, StringData>();
            mFreeStringIndexes = new Queue<int>();
            mFreeStringDatas = new Queue<StringData>();

            mHeaderData = default(HeaderData);
            mBlockDataOffset = 0;
            mStringDataOffset = 0;
            mFileDataOffset = 0;

            Utility.Marshal.EnsureCachedHGlobalSize(CachedBytesLength);
        }

        /// <summary>
        /// 文件系统完整地址
        /// </summary>
        public string FullPath => mFullPath;

        /// <summary>
        /// 文件系统访问方式
        /// </summary>
        public FileSystemAccess Access => mAccess;

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount => mFileDatas.Count;

        /// <summary>
        /// 最大文件数量
        /// </summary>
        public int MaxFileCount => mHeaderData.MaxFileCount;

        /// <summary>
        /// 创建文件系统
        /// </summary>
        /// <param name="fullPath">文件系统完整地址</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="stream">文件系统流</param>
        /// <param name="maxFileCount">最大文件数量</param>
        /// <param name="maxBlockCount">最大块数量</param>
        /// <returns>文件系统</returns>
        /// <exception cref="Exception"></exception>
        public static FileSystem Create(string fullPath, FileSystemAccess access, FileSystemStream stream, int maxFileCount, int maxBlockCount)
        {
            if (maxFileCount < 0)
            {
                throw new Exception("Max file count is invalid.");
            }

            if (maxBlockCount < 0)
            {
                throw new Exception("Max block count is invalid.");
            }

            if (maxFileCount < maxBlockCount)
            {
                throw new Exception("Max file count can not larger than max block count.");
            }

            var fileSystem = new FileSystem(fullPath, access, stream)
            {
                mHeaderData = new HeaderData(maxFileCount, maxBlockCount)
            };
            CalcOffsets(fileSystem);
            Utility.Marshal.StructureToBytes(fileSystem.mHeaderData, sHeaderDataSize, sCachedBytes);

            try
            {
                stream.Write(sCachedBytes, 0, sHeaderDataSize);
                stream.SetLength(fileSystem.mFileDataOffset);
                return fileSystem;
            }
            catch
            {
                fileSystem.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// 加载文件系统
        /// </summary>
        /// <param name="fullPath">文件系统完整地址</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="stream">文件系统流</param>
        /// <returns>文件系统</returns>
        public static FileSystem Load(string fullPath, FileSystemAccess access, FileSystemStream stream)
        {
            var fileSystem = new FileSystem(fullPath, access, stream);

            stream.Read(sCachedBytes, 0, sHeaderDataSize);
            fileSystem.mHeaderData = Utility.Marshal.BytesToStructure<HeaderData>(sHeaderDataSize, sCachedBytes);
            if (!fileSystem.mHeaderData.IsValid)
            {
                fileSystem.Shutdown();
                return null;
            }

            CalcOffsets(fileSystem);

            if (fileSystem.mBlockDatas.Capacity < fileSystem.mHeaderData.BlockCount)
            {
                fileSystem.mBlockDatas.Capacity = fileSystem.mHeaderData.BlockCount;
            }

            for (int i = 0; i < fileSystem.mHeaderData.BlockCount; i++)
            {
                stream.Read(sCachedBytes, 0, sBlockDataSize);
                var blockData = Utility.Marshal.BytesToStructure<BlockData>(sBlockDataSize, sCachedBytes);
                fileSystem.mBlockDatas.Add(blockData);
            }

            for (int i = 0; i < fileSystem.mBlockDatas.Count; i++)
            {
                var blockData = fileSystem.mBlockDatas[i];
                if (blockData.Using)
                {
                    var stringData = fileSystem.ReadStringData(blockData.StringIndex);
                    fileSystem.mStringDatas.Add(blockData.StringIndex, stringData);
                    fileSystem.mFileDatas.Add(stringData.GetString(fileSystem.mHeaderData.EncryptBytes), i);
                }
            }

            var index = 0;

            foreach (var (i, stringData) in fileSystem.mStringDatas)
            {
                while (index < i)
                {
                    fileSystem.mFreeStringIndexes.Enqueue(index++);
                }

                index++;
            }

            return fileSystem;
        }

        /// <summary>
        /// 关闭并清理文件系统
        /// </summary>
        public void Shutdown()
        {
            mStream.Close();

            mFileDatas.Clear();
            mBlockDatas.Clear();
            mFreeBlockIndexes.Clear();
            mStringDatas.Clear();
            mFreeStringIndexes.Clear();
            mFreeStringDatas.Clear();

            mBlockDataOffset = 0;
            mStringDataOffset = 0;
            mFileDataOffset = 0;
        }

        /// <summary>
        /// 获取指定文件信息
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>文件信息</returns>
        public FileInfo GetFileInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (!mFileDatas.TryGetValue(name, out var blockIndex))
            {
                return default(FileInfo);
            }

            var blockData = mBlockDatas[blockIndex];
            return new FileInfo(name, GetClusterOffset(blockData.ClusterIndex), blockData.Length);
        }


        /// <summary>
        /// 获取所有文件信息
        /// </summary>
        /// <returns>所有文件信息</returns>
        public FileInfo[] GetAllFileInfos()
        {
            var index = 0;
            var results = new FileInfo[mFileDatas.Count];
            foreach (var (name, blockIndex) in mFileDatas)
            {
                var blockData = mBlockDatas[blockIndex];
                results[index++] = new FileInfo(name, GetClusterOffset(blockData.ClusterIndex), blockData.Length);
            }

            return results;
        }

        /// <summary>
        /// 获取所有文件信息
        /// </summary>
        /// <param name="results">所有文件信息</param>
        public void GetAllFileInfos(List<FileInfo> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();

            foreach (var (name, blockIndex) in mFileDatas)
            {
                var blockData = mBlockDatas[blockIndex];
                results.Add(new FileInfo(name, GetClusterOffset(blockData.ClusterIndex), blockData.Length));
            }
        }

        /// <summary>
        /// 检查是否存在指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>是否存在指定文件</returns>
        public bool HasFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            return mFileDatas.ContainsKey(name);
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>文件的二进制流</returns>
        public byte[] ReadFile(string name)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return null;
            }

            var length = fileInfo.Length;
            var buffer = new byte[length];
            if (length > 0)
            {
                mStream.Position = fileInfo.Offset;
                mStream.Read(buffer, 0, length);
            }

            return buffer;
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFile(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return ReadFile(name, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <param name="startIndex">文件的二进制流的起始位置</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFile(string name, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return ReadFile(name, buffer, startIndex, buffer.Length - startIndex);
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <param name="startIndex">文件的二进制流的起始位置</param>
        /// <param name="length">文件的二进制流的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFile(string name, byte[] buffer, int startIndex, int length)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new Exception("Start index or length is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            mStream.Position = fileInfo.Offset;
            if (length > fileInfo.Length)
            {
                length = fileInfo.Length;
            }

            if (length > 0)
            {
                return mStream.Read(buffer, startIndex, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">文件的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFile(string name, Stream stream)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (stream == null)
            {
                throw new Exception("Stream is invalid.");
            }

            if (!stream.CanWrite)
            {
                throw new Exception("Stream is not writable.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            var length = fileInfo.Length;
            if (length > 0)
            {
                mStream.Position = fileInfo.Offset;
                return mStream.Read(stream, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="length">片段的长度</param>
        /// <returns>片段的二进制流</returns>
        public byte[] ReadFileSegment(string name, int length)
        {
            return ReadFileSegment(name, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="length">片段的长度</param>
        /// <returns>片段的二进制流</returns>
        public byte[] ReadFileSegment(string name, int offset, int length)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new Exception("Offset is invalid.");
            }

            if (length < 0)
            {
                throw new Exception("Length is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return null;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            var leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            var buffer = new byte[length];
            if (length > 0)
            {
                mStream.Position = fileInfo.Offset + offset;
                mStream.Read(buffer, 0, length);
            }

            return buffer;
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return ReadFileSegment(name, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, byte[] buffer, int length)
        {
            return ReadFileSegment(name, 0, buffer, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="startIndex">片段的起始位置</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, byte[] buffer, int startIndex, int length)
        {
            return ReadFileSegment(name, 0, buffer, startIndex, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return ReadFileSegment(name, offset, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer, int length)
        {
            return ReadFileSegment(name, offset, buffer, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="startIndex">片段的起始位置</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer, int startIndex, int length)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new Exception("Offset is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new Exception("Start index or length is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            var leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            if (length > 0)
            {
                mStream.Position = fileInfo.Offset + offset;
                return mStream.Read(buffer, 0, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, Stream stream, int length)
        {
            return ReadFileSegment(name, 0, stream, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="stream">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        public int ReadFileSegment(string name, int offset, Stream stream, int length)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new Exception("Offset is invalid.");
            }

            if (stream == null)
            {
                throw new Exception("Stream is invalid.");
            }

            if (!stream.CanWrite)
            {
                throw new Exception("Stream is not writable.");
            }

            if (length < 0)
            {
                throw new Exception("Length is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            var leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            if (length > 0)
            {
                mStream.Position = fileInfo.Offset + offset;
                return mStream.Read(stream, length);
            }

            return 0;
        }

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <returns>是否成功写入指定的文件</returns>
        public bool WriteFile(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return WriteFile(name, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <returns>是否成功写入指定的文件</returns>
        public bool WriteFile(string name, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return WriteFile(name, buffer, startIndex, buffer.Length - startIndex);
        }

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>是否成功写入指定的文件</returns>
        public bool WriteFile(string name, byte[] buffer, int startIndex, int length)
        {
            if (mAccess != FileSystemAccess.Write && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (name.Length > byte.MaxValue)
            {
                throw new Exception($"Name ({name}) is too long.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new Exception("Start index or length is invalid.");
            }

            var hasFile = false;
            var oldBlockIndex = -1;
            if (mFileDatas.TryGetValue(name, out oldBlockIndex))
            {
                hasFile = true;
            }

            if (!hasFile && mFileDatas.Count >= mHeaderData.MaxFileCount)
            {
                return false;
            }

            var blockIndex = AllocBlock(length);
            if (blockIndex < 0)
            {
                return false;
            }

            if (length > 0)
            {
                mStream.Position = GetClusterOffset(mBlockDatas[blockIndex].ClusterIndex);
                mStream.Write(buffer, startIndex, length);
            }

            ProcessWriteFile(name, hasFile, oldBlockIndex, blockIndex, length);
            mStream.Flush();
            return true;
        }

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">文件内容的二进制流</param>
        /// <returns>是否成功写入指定的文件</returns>
        public bool WriteFile(string name, Stream stream)
        {
            if (mAccess != FileSystemAccess.Write && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (name.Length > byte.MaxValue)
            {
                throw new Exception($"Name ({name}) is too long.");
            }

            if (stream == null)
            {
                throw new Exception("Stream is invalid.");
            }

            if (!stream.CanRead)
            {
                throw new Exception("Stream is not readable.");
            }

            var hasFile = false;
            var oldBlockIndex = -1;
            if (mFileDatas.TryGetValue(name, out oldBlockIndex))
            {
                hasFile = true;
            }

            if (!hasFile && mFileDatas.Count >= mHeaderData.MaxFileCount)
            {
                return false;
            }

            var length = (int)(stream.Length - stream.Position);
            var blockIndex = AllocBlock(length);
            if (blockIndex < 0)
            {
                return false;
            }

            if (length > 0)
            {
                mStream.Position = GetClusterOffset(mBlockDatas[blockIndex].ClusterIndex);
                mStream.Write(stream, length);
            }

            ProcessWriteFile(name, hasFile, oldBlockIndex, blockIndex, length);
            mStream.Flush();
            return true;
        }

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="filePath">存储写入文件内容的文件路径</param>
        /// <returns>是否成功写入指定的文件</returns>
        public bool WriteFile(string name, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("File path is invalid.");
            }

            if (!File.Exists(filePath))
            {
                return false;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return WriteFile(name, fileStream);
            }
        }

        /// <summary>
        /// 将指定文件另存为物理文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="filePath">存储写入文件内容的文件路径</param>
        /// <returns>是否成功将指定文件另存为物理文件</returns>
        public bool SaveAsFile(string name, string filePath)
        {
            if (mAccess != FileSystemAccess.Read && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("File path is invalid.");
            }

            var fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return false;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var length = fileInfo.Length;
                if (length > 0)
                {
                    mStream.Position = fileInfo.Offset;
                    return mStream.Read(fileStream, length) == length;
                }

                return true;
            }
        }

        /// <summary>
        /// 重命名指定文件
        /// </summary>
        /// <param name="oldName">文件的旧名称</param>
        /// <param name="newName">文件的新名称</param>
        /// <returns>是否成功重命名指定文件</returns>
        public bool RenameFile(string oldName, string newName)
        {
            if (mAccess != FileSystemAccess.Write && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not writable.");
            }

            if (string.IsNullOrEmpty(oldName))
            {
                throw new Exception("Old name is invalid.");
            }

            if (string.IsNullOrEmpty(newName))
            {
                throw new Exception("New name is invalid.");
            }

            if (newName.Length > byte.MaxValue)
            {
                throw new Exception($"New name ({newName}) is too long.");
            }

            if (oldName == newName)
            {
                return true;
            }

            if (mFileDatas.ContainsKey(newName))
            {
                return false;
            }

            if (!mFileDatas.TryGetValue(oldName, out var blockIndex))
            {
                return false;
            }

            var stringIndex = mBlockDatas[blockIndex].StringIndex;
            var stringData = mStringDatas[stringIndex].SetString(newName, mHeaderData.EncryptBytes);
            mStringDatas[stringIndex] = stringData;
            WriteStringData(stringIndex, stringData);
            mFileDatas.Add(newName, blockIndex);
            mFileDatas.Remove(oldName);
            mStream.Flush();
            return true;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>是否成功删除指定文件</returns>
        public bool DeleteFile(string name)
        {
            if (mAccess != FileSystemAccess.Write && mAccess != FileSystemAccess.ReadWrite)
            {
                throw new Exception("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (!mFileDatas.TryGetValue(name, out var blockIndex))
            {
                return false;
            }

            mFileDatas.Remove(name);

            var blockData = mBlockDatas[blockIndex];
            var stringIndex = blockData.StringIndex;
            var stringData = mStringDatas[stringIndex].Clear();
            mFreeStringIndexes.Enqueue(stringIndex);
            mFreeStringDatas.Enqueue(stringData);
            mStringDatas.Remove(stringIndex);
            WriteStringData(stringIndex, stringData);

            blockData = blockData.Free();
            mBlockDatas[blockIndex] = blockData;
            if (!TryCombineFreeBlocks(blockIndex))
            {
                mFreeBlockIndexes.Add(blockData.Length, blockIndex);
                WriteBlockData(blockIndex);
            }

            mStream.Flush();
            return true;
        }

        private static void CalcOffsets(FileSystem fileSystem)
        {
            fileSystem.mBlockDataOffset = sHeaderDataSize;
            fileSystem.mStringDataOffset = fileSystem.mBlockDataOffset + sBlockDataSize * fileSystem.mHeaderData.MaxBlockCount;
            fileSystem.mFileDataOffset = (int)GetUpBoundClusterOffset(fileSystem.mStringDataOffset + sStringDataSize * fileSystem.mHeaderData.MaxFileCount);
        }

        private static long GetUpBoundClusterOffset(long offset)
        {
            return (offset - 1L + ClusterSize) / ClusterSize * ClusterSize;
        }

        private static int GetUpBoundClusterCount(long length)
        {
            return (int)((length - 1L + ClusterSize) / ClusterSize);
        }

        private static long GetClusterOffset(int clusterIndex)
        {
            return (long)ClusterSize * clusterIndex;
        }

        private void ProcessWriteFile(string name, bool hasFile, int oldBlockIndex, int blockIndex, int length)
        {
            var blockData = mBlockDatas[blockIndex];
            if (hasFile)
            {
                var oldBlockData = mBlockDatas[oldBlockIndex];
                blockData = new BlockData(oldBlockData.StringIndex, blockData.ClusterIndex, length);
                mBlockDatas[blockIndex] = blockData;
                WriteBlockData(blockIndex);

                oldBlockData = oldBlockData.Free();
                mBlockDatas[oldBlockIndex] = oldBlockData;
                if (!TryCombineFreeBlocks(oldBlockIndex))
                {
                    mFreeBlockIndexes.Add(oldBlockData.Length, oldBlockIndex);
                    WriteBlockData(oldBlockIndex);
                }

                mFileDatas[name] = blockIndex;
            }
            else
            {
                var stringIndex = AllocString(name);
                blockData = new BlockData(stringIndex, blockData.ClusterIndex, length);
                mBlockDatas[blockIndex] = blockData;
                WriteBlockData(blockIndex);

                mFileDatas.Add(name, blockIndex);
            }
        }

        private bool TryCombineFreeBlocks(int freeBlockIndex)
        {
            var freeBlockData = mBlockDatas[freeBlockIndex];
            if (freeBlockData.Length <= 0)
            {
                return false;
            }

            var previousFreeBlockIndex = -1;
            var nextFreeBlockIndex = -1;
            var nextBlockDataClusterIndex = freeBlockData.ClusterIndex + GetUpBoundClusterCount(freeBlockData.Length);
            foreach (var (length, blockIndexes) in mFreeBlockIndexes)
            {
                if (length <= 0)
                {
                    continue;
                }

                var blockDataClusterCount = GetUpBoundClusterCount(length);
                foreach (var blockIndex in blockIndexes)
                {
                    var blockData = mBlockDatas[blockIndex];
                    if (blockData.ClusterIndex + blockDataClusterCount == freeBlockData.ClusterIndex)
                    {
                        previousFreeBlockIndex = blockIndex;
                    }
                    else if (blockData.ClusterIndex == nextBlockDataClusterIndex)
                    {
                        nextFreeBlockIndex = blockIndex;
                    }
                }
            }

            if (previousFreeBlockIndex < 0 && nextFreeBlockIndex < 0)
            {
                return false;
            }

            mFreeBlockIndexes.Remove(freeBlockData.Length, freeBlockIndex);
            if (previousFreeBlockIndex > 0)
            {
                var previousFreeBlockData = mBlockDatas[previousFreeBlockIndex];
                mFreeBlockIndexes.Remove(previousFreeBlockData.Length, previousFreeBlockIndex);
                freeBlockData = new BlockData(previousFreeBlockData.ClusterIndex, previousFreeBlockData.Length + freeBlockData.Length);
                mBlockDatas[previousFreeBlockIndex] = BlockData.Empty;
                mFreeBlockIndexes.Add(0, previousFreeBlockIndex);
                WriteBlockData(previousFreeBlockIndex);
            }

            if (nextFreeBlockIndex > 0)
            {
                var nextFreeBlockData = mBlockDatas[nextFreeBlockIndex];
                mFreeBlockIndexes.Remove(nextFreeBlockData.Length, nextFreeBlockIndex);
                freeBlockData = new BlockData(nextFreeBlockData.ClusterIndex, nextFreeBlockData.Length + freeBlockData.Length);
                mBlockDatas[nextFreeBlockIndex] = BlockData.Empty;
                mFreeBlockIndexes.Add(0, nextFreeBlockIndex);
                WriteBlockData(nextFreeBlockIndex);
            }

            mBlockDatas[freeBlockIndex] = freeBlockData;
            mFreeBlockIndexes.Add(freeBlockData.Length, freeBlockIndex);
            WriteBlockData(freeBlockIndex);
            return true;
        }

        private int GetEmptyBlockIndex()
        {
            if (mFreeBlockIndexes.TryGetValue(0, out var lengthRange))
            {
                var blockIndex = lengthRange.First.Value;
                mFreeBlockIndexes.Remove(0, blockIndex);
                return blockIndex;
            }

            if (mBlockDatas.Count < mHeaderData.MaxBlockCount)
            {
                var blockIndex = mBlockDatas.Count;
                mBlockDatas.Add(BlockData.Empty);
                WriteHeaderData();
                return blockIndex;
            }

            return -1;
        }

        private int AllocBlock(int length)
        {
            if (length <= 0)
            {
                return GetEmptyBlockIndex();
            }

            length = (int)GetUpBoundClusterOffset(length);

            var lengthFound = -1;
            var lengthRange = default(LinkedList<int>);
            foreach (var (key, value) in mFreeBlockIndexes)
            {
                if (key < length)
                {
                    continue;
                }

                if (lengthFound >= 0 && lengthFound < key)
                {
                    continue;
                }

                lengthFound = key;
                lengthRange = value;
            }

            if (lengthFound >= 0)
            {
                if (lengthFound > length && mBlockDatas.Count >= mHeaderData.MaxBlockCount)
                {
                    return -1;
                }

                var blockIndex = lengthRange.First.Value;
                mFreeBlockIndexes.Remove(lengthFound, blockIndex);
                if (lengthFound > length)
                {
                    var blockData = mBlockDatas[blockIndex];
                    mBlockDatas[blockIndex] = new BlockData(blockData.ClusterIndex, length);
                    WriteBlockData(blockIndex);

                    var deltaLength = lengthFound - length;
                    var anotherBlockIndex = GetEmptyBlockIndex();
                    mBlockDatas[anotherBlockIndex] = new BlockData(blockData.ClusterIndex + GetUpBoundClusterCount(length), deltaLength);
                    mFreeBlockIndexes.Add(deltaLength, anotherBlockIndex);
                    WriteBlockData(anotherBlockIndex);
                }

                return blockIndex;
            }
            else
            {
                var blockIndex = GetEmptyBlockIndex();
                if (blockIndex < 0)
                {
                    return -1;
                }

                var fileLength = mStream.Length;
                try
                {
                    mStream.SetLength(fileLength + length);
                }
                catch
                {
                    return -1;
                }

                mBlockDatas[blockIndex] = new BlockData(GetUpBoundClusterCount(fileLength), length);
                WriteBlockData(blockIndex);
                return blockIndex;
            }
        }

        private int AllocString(string value)
        {
            var stringIndex = -1;
            var stringData = default(StringData);

            if (mFreeStringIndexes.Count > 0)
            {
                stringIndex = mFreeStringIndexes.Dequeue();
            }
            else
            {
                stringIndex = mStringDatas.Count;
            }

            if (mFreeStringDatas.Count > 0)
            {
                stringData = mFreeStringDatas.Dequeue();
            }
            else
            {
                var bytes = new byte[byte.MaxValue];
                Utility.Random.GetRandomBytes(bytes);
                stringData = new StringData(0, bytes);
            }

            stringData = stringData.SetString(value, mHeaderData.EncryptBytes);
            mStringDatas.Add(stringIndex, stringData);
            return stringIndex;
        }

        private void WriteBlockData(int blockIndex)
        {
            Utility.Marshal.StructureToBytes(mBlockDatas[blockIndex], sBlockDataSize, sCachedBytes);
            mStream.Position = mBlockDataOffset + sBlockDataSize * blockIndex;
            mStream.Write(sCachedBytes, 0, sBlockDataSize);
        }

        private void WriteHeaderData()
        {
            mHeaderData = mHeaderData.SetBlockCount(mBlockDatas.Count);
            Utility.Marshal.StructureToBytes(mHeaderData, sHeaderDataSize, sCachedBytes);
            mStream.Position = 0L;
            mStream.Write(sCachedBytes, 0, sHeaderDataSize);
        }

        private StringData ReadStringData(int stringIndex)
        {
            mStream.Position = mStringDataOffset + sStringDataSize * stringIndex;
            mStream.Read(sCachedBytes, 0, sStringDataSize);
            return Utility.Marshal.BytesToStructure<StringData>(sStringDataSize, sCachedBytes);
        }

        private void WriteStringData(int stringIndex, StringData stringData)
        {
            Utility.Marshal.StructureToBytes(stringData, sStringDataSize, sCachedBytes);
            mStream.Position = mStringDataOffset + sStringDataSize * stringIndex;
            mStream.Write(sCachedBytes, 0, sStringDataSize);
        }
    }
}