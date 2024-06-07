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
                /// 远程资源状态信息
                /// </summary>
                private struct RemoteVersionInfo
                {
                    private readonly bool mExist;
                    private readonly string mFileSystemName;
                    private readonly LoadType mLoadType;
                    private readonly int mLength;
                    private readonly int mHashCode;
                    private readonly int mCompressedLength;
                    private readonly int mCompressHashCode;

                    public RemoteVersionInfo(string fileSystemName, LoadType loadType, int length, int hashCode,
                        int compressedLength, int compressHashCode) : this()
                    {
                        mExist = true;
                        mFileSystemName = fileSystemName;
                        mLoadType = loadType;
                        mLength = length;
                        mHashCode = hashCode;
                        mCompressedLength = compressedLength;
                        mCompressHashCode = compressHashCode;
                    }

                    public bool UseFileSystem => !string.IsNullOrEmpty(mFileSystemName);

                    public bool Exist => mExist;

                    public string FileSystemName => mFileSystemName;

                    public LoadType LoadType => mLoadType;

                    public int Length => mLength;

                    public int HashCode => mHashCode;

                    public int CompressedLength => mCompressedLength;

                    public int CompressHashCode => mCompressHashCode;
                }
            }
        }
    }
}