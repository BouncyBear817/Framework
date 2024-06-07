// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/27 15:36:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceUpdater
        {
            /// <summary>
            /// 资源更新信息
            /// </summary>
            private sealed class UpdateInfo
            {
                private readonly ResourceName mResourceName;
                private readonly string mFileSystemName;
                private readonly LoadType mLoadType;
                private readonly int mLength;
                private readonly int mHashCode;
                private readonly int mCompressedLength;
                private readonly int mCompressedHashCode;
                private readonly string mResourcePath;

                private bool mDownloading;
                private int mRetryCount;

                public UpdateInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length,
                    int hashCode, int compressedLength, int compressedHashCode, string resourcePath)
                {
                    mResourceName = resourceName;
                    mFileSystemName = fileSystemName;
                    mLoadType = loadType;
                    mLength = length;
                    mHashCode = hashCode;
                    mCompressedLength = compressedLength;
                    mCompressedHashCode = compressedHashCode;
                    mResourcePath = resourcePath;
                    mDownloading = false;
                    mRetryCount = 0;
                }

                /// <summary>
                /// 资源名称
                /// </summary>
                public ResourceName ResourceName => mResourceName;

                /// <summary>
                /// 文件系统名称
                /// </summary>
                public string FileSystemName => mFileSystemName;

                /// <summary>
                /// 是否使用文件系统
                /// </summary>
                public bool UseFileSystem => !string.IsNullOrEmpty(mFileSystemName);

                /// <summary>
                /// 资源加载方式类型
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
                /// 资源压缩后大小
                /// </summary>
                public int CompressedLength => mCompressedLength;

                /// <summary>
                /// 资源压缩后哈希值
                /// </summary>
                public int CompressedHashCode => mCompressedHashCode;

                /// <summary>
                /// 资源路径
                /// </summary>
                public string ResourcePath => mResourcePath;

                /// <summary>
                /// 资源下载状态
                /// </summary>
                public bool Downloading
                {
                    get => mDownloading;
                    set => mDownloading = value;
                }

                /// <summary>
                /// 资源下载重试次数
                /// </summary>
                public int RetryCount
                {
                    get => mRetryCount;
                    set => mRetryCount = value;
                }
            }
        }
    }
}