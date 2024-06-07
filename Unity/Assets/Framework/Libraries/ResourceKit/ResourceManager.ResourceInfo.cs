// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/20 16:18:43
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        private sealed class ResourceInfo
        {
            private readonly ResourceName mResourceName;
            private readonly string mFileSystemName;
            private readonly LoadType mLoadType;
            private readonly int mLength;
            private readonly int mHashCode;
            private readonly int mCompressedLength;
            private readonly bool mStorageInReadOnly;
            private bool mReady;

            public ResourceInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length,
                int hashCode, int compressedLength, bool storageInReadOnly, bool ready)
            {
                mResourceName = resourceName;
                mFileSystemName = fileSystemName;
                mLoadType = loadType;
                mLength = length;
                mHashCode = hashCode;
                mCompressedLength = compressedLength;
                mStorageInReadOnly = storageInReadOnly;
                mReady = ready;
            }

            public ResourceInfo(ResourceChecker.CheckInfo checkInfo)
            {
                mResourceName = checkInfo.ResourceName;
                mFileSystemName = checkInfo.FileSystemName;
                mLoadType = checkInfo.LoadType;
                mLength = checkInfo.Length;
                mHashCode = checkInfo.HashCode;
                mCompressedLength = checkInfo.CompressedLength;
                mStorageInReadOnly = true;
                mReady = true;
            }

            /// <summary>
            /// 资源名称
            /// </summary>
            public ResourceName ResourceName => mResourceName;

            /// <summary>
            /// 是否使用文件系统
            /// </summary>
            public bool UseFileSystem => !string.IsNullOrEmpty(mFileSystemName);

            /// <summary>
            /// 文件系统名称
            /// </summary>
            public string FileSystemName => mFileSystemName;

            /// <summary>
            /// 资源是否通过二进制方式加载
            /// </summary>
            public bool IsLoadFromBinary => mLoadType == LoadType.LoadFromBinary ||
                                            mLoadType == LoadType.LoadFromBinaryAndQuickDecrypt ||
                                            mLoadType == LoadType.LoadFromBinaryDecrypt;

            /// <summary>
            /// 资源加载方式
            /// </summary>
            public LoadType LoadType => mLoadType;

            /// <summary>
            /// 资源大小
            /// </summary>
            public int Length => mLength;

            /// <summary>
            /// 资源哈希值
            /// </summary>
            public int HashCode => mHashCode;

            /// <summary>
            /// 压缩后资源大小
            /// </summary>
            public int CompressedLength => mCompressedLength;

            /// <summary>
            /// 资源是否存在只读区
            /// </summary>
            public bool StorageInReadOnly => mStorageInReadOnly;

            /// <summary>
            /// 资源是否准备完毕
            /// </summary>
            public bool Ready => mReady;

            /// <summary>
            /// 标记资源准备完毕
            /// </summary>
            public void MarkReady()
            {
                mReady = true;
            }
        }
    }
}