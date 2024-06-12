using System;

namespace Framework
{
    /// <summary>
    /// 可更新模式版本资源列表
    /// </summary>
    public struct UpdatableVersionList
    {
        private static readonly int[] sEmptyIntArray =
        {
        };

        private static readonly Asset[] sEmptyAssetArray =
        {
        };

        private static readonly Resource[] sEmptyResourceArray =
        {
        };

        private static readonly FileSystem[] sEmptyFileSystemArray =
        {
        };

        private static readonly ResourceGroup[] sEmptyResourceGroupArray =
        {
        };

        private readonly bool mIsValid;
        private readonly string mApplicableVersion;
        private readonly int mInternalResourceVersion;
        private readonly Asset[] mAssets;
        private readonly Resource[] mResources;
        private readonly FileSystem[] mFileSystems;
        private readonly ResourceGroup[] mResourceGroups;

        public UpdatableVersionList(string applicableVersion, int internalResourceVersion, Asset[] assets,
            Resource[] resources, FileSystem[] fileSystems, ResourceGroup[] resourceGroups) : this()
        {
            mIsValid = true;
            mApplicableVersion = applicableVersion;
            mInternalResourceVersion = internalResourceVersion;
            mAssets = assets ?? sEmptyAssetArray;
            mResources = resources ?? sEmptyResourceArray;
            mFileSystems = fileSystems ?? sEmptyFileSystemArray;
            mResourceGroups = resourceGroups ?? sEmptyResourceGroupArray;
        }

        /// <summary>
        /// 单机模式版本资源列表是否有效
        /// </summary>
        public bool IsValid => mIsValid;

        /// <summary>
        /// 资源适用的版号
        /// </summary>
        public string ApplicableVersion =>
            mIsValid ? mApplicableVersion : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 资源的内部版本号
        /// </summary>
        public int InternalResourceVersion => mIsValid
            ? mInternalResourceVersion
            : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 资源集合
        /// </summary>
        public Asset[] Assets => mIsValid ? mAssets : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 资源集合
        /// </summary>
        public Resource[] Resources =>
            mIsValid ? mResources : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 文件系统集合
        /// </summary>
        public FileSystem[] FileSystems =>
            mIsValid ? mFileSystems : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 资源组集合
        /// </summary>
        public ResourceGroup[] ResourceGroups =>
            mIsValid ? mResourceGroups : throw new Exception("UpdatableVersionList data is invalid.");

        /// <summary>
        /// 版本列表资源
        /// </summary>
        public struct Asset
        {
            private readonly string mName;
            private readonly int[] mDependencyAssetIndexes;

            public Asset(string name, int[] dependencyAssetIndexes)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }

                mName = name;
                mDependencyAssetIndexes = dependencyAssetIndexes ?? sEmptyIntArray;
            }

            /// <summary>
            /// 版本列表资源名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 版本列表资源包含的依赖资源索引集合
            /// </summary>
            public int[] DependencyAssetIndexes => mDependencyAssetIndexes;
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
            private readonly int mCompressedLength;
            private readonly int mCompressedHashCode;
            private readonly int[] mAssetIndexes;

            public Resource(string name, string variant, string extension, byte loadType, int length, int hashCode,
                int compressedLength, int compressedHashCode, int[] assetIndexes)
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
                mCompressedLength = compressedLength;
                mCompressedHashCode = compressedHashCode;
                mAssetIndexes = assetIndexes ?? sEmptyIntArray;
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
            /// 版本列表资源压缩后大小
            /// </summary>
            public int CompressedLength => mCompressedLength;

            /// <summary>
            /// 版本列表资源压缩后哈希值
            /// </summary>
            public int CompressedHashCode => mCompressedHashCode;

            /// <summary>
            /// 版本列表资源哈希值
            /// </summary>
            public int HashCode => mHashCode;

            /// <summary>
            /// 版本列表资源包含的额资源索引集合
            /// </summary>
            public int[] AssetIndexes => mAssetIndexes;
        }

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
        /// 版本列表资源组
        /// </summary>
        public struct ResourceGroup
        {
            private readonly string mName;
            private readonly int[] mResourceIndexes;

            public ResourceGroup(string name, int[] resourceIndexes)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }

                mName = name;
                mResourceIndexes = resourceIndexes ?? sEmptyIntArray;
            }

            /// <summary>
            /// 版本列表资源组名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 版本列表资源组中的资源索引集合
            /// </summary>
            public int[] ResourceIndexes => mResourceIndexes;
        }
    }
}