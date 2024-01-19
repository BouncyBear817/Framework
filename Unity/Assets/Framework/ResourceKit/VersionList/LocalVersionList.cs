using System;

namespace Framework
{
    /// <summary>
    /// 本地版本资源列表
    /// </summary>
    public struct LocalVersionList
    {
        private static readonly int[] sEmptyIntArray = {
        };
        
        private readonly bool mIsValid;
        private readonly Resource[] mResources;
        private readonly FileSystem[] mFileSystems;

        public LocalVersionList(Resource[] resources, FileSystem[] fileSystems)
        {
            mIsValid = true;
            mResources = resources;
            mFileSystems = fileSystems;
        }

        /// <summary>
        /// 本地版本资源列表是否有效
        /// </summary>
        public bool IsValid => mIsValid;

        /// <summary>
        /// 本地版本资源列表包含的资源集合
        /// </summary>
        /// <exception cref="Exception"></exception>
        public Resource[] Resources => mIsValid ? mResources : throw new Exception("LocalVersionList data is invalid.");

        /// <summary>
        /// 本地版本资源列表包含的文件系统集合
        /// </summary>
        /// <exception cref="Exception"></exception>
        public FileSystem[] FileSystems => mIsValid ? mFileSystems : throw new Exception("LocalVersionList data is invalid.");

        /// <summary>
        /// 版本列表文件系统
        /// </summary>
        public struct FileSystem
        {
            private readonly string mName;
            private readonly int[] mResourceIndexes;

            public FileSystem(string name, int[] resourceIndexes)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }
                
                mName = name;
                mResourceIndexes = resourceIndexes ?? sEmptyIntArray;
            }

            /// <summary>
            /// 版本列表文件系统名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 版本列表文件系统中的资源索引集合
            /// </summary>
            public int[] ResourceIndexes => mResourceIndexes;
        }

        /// <summary>
        /// 版本列表资源
        /// </summary>
        public struct Resource
        {
            private readonly string mName;
            private readonly string mVariant;
            private readonly string mExtension;
            private readonly byte mLoadType;
            private readonly int mLength;
            private readonly int mHashCode;

            public Resource(string name, string variant, string extension, byte loadType, int length, int hashCode)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }
                
                mName = name;
                mVariant = variant;
                mExtension = extension;
                mLoadType = loadType;
                mLength = length;
                mHashCode = hashCode;
            }

            /// <summary>
            /// 版本列表资源名称
            /// </summary>
            public string Name => mName;
            /// <summary>
            /// 版本列表资源变体
            /// </summary>
            public string Variant => mVariant;
            /// <summary>
            /// 版本列表资源扩展名称
            /// </summary>
            public string Extension => mExtension;
            /// <summary>
            /// 版本列表资源加载方式
            /// </summary>
            public byte LoadType => mLoadType;
            /// <summary>
            /// 版本列表资源大小
            /// </summary>
            public int Length => mLength;
            /// <summary>
            /// 版本列表资源哈希值
            /// </summary>
            public int HashCode => mHashCode;
        }
    }
}