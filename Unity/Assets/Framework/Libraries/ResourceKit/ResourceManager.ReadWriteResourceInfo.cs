using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private struct ReadWriteResourceInfo
        {
            private readonly string mFileSystemName;
            private readonly LoadType mLoadType;
            private readonly int mLength;
            private readonly int mHashCode;

            public ReadWriteResourceInfo(string fileSystemName, LoadType loadType, int length, int hashCode)
            {
                mFileSystemName = fileSystemName;
                mLoadType = loadType;
                mLength = length;
                mHashCode = hashCode;
            }

            public bool UseFileSystem => !string.IsNullOrEmpty(mFileSystemName);

            public string FileSystemName => mFileSystemName;

            public LoadType LoadType => mLoadType;

            public int Length => mLength;

            public int HashCode => mHashCode;
        }
    }
}