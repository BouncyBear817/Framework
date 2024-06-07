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
        private sealed partial class ResourceVerifier
        {
            /// <summary>
            /// 资源校验信息
            /// </summary>
            private struct VerifyInfo
            {
                private readonly ResourceName mResourceName;
                private readonly string mFileSystemName;
                private readonly LoadType mLoadType;
                private readonly int mLength;
                private readonly int mHashCode;

                public VerifyInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length,
                    int hashCode)
                {
                    mResourceName = resourceName;
                    mFileSystemName = fileSystemName;
                    mLoadType = loadType;
                    mLength = length;
                    mHashCode = hashCode;
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
            }
        }
    }
}