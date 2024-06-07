using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceChecker
        {
            internal sealed partial class CheckInfo
            {
                /// <summary>
                /// 本地资源状态信息
                /// </summary>
                private struct LocalVersionInfo
                {
                    private readonly bool mExist;
                    private readonly string mFileSystemName;
                    private readonly LoadType mLoadType;
                    private readonly int mLength;
                    private readonly int mHashCode;

                    public LocalVersionInfo(string fileSystemName, LoadType loadType, int length, int hashCode) : this()
                    {
                        mExist = true;
                        mFileSystemName = fileSystemName;
                        mLoadType = loadType;
                        mLength = length;
                        mHashCode = hashCode;
                    }

                    public bool UseFileSystem => !string.IsNullOrEmpty(mFileSystemName);

                    public bool Exist => mExist;

                    public string FileSystemName => mFileSystemName;

                    public LoadType LoadType => mLoadType;

                    public int Length => mLength;

                    public int HashCode => mHashCode;
                }
            }
        }
    }
}