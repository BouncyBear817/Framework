using System;
namespace Framework
{
    /// <summary>
    /// 资源包版本列表
    /// </summary>
    public struct ResourcePackVersionList
    {
        private readonly bool mIsValid;
        private readonly int mOffset;
        private readonly long mLength;
        private readonly int mHashCode;
        private readonly Resource[] mResources;

        public ResourcePackVersionList(int offset, long length, int hashCode, Resource[] resources) : this()
        {
            mIsValid = true;
            mOffset = offset;
            mLength = length;
            mHashCode = hashCode;
            mResources = resources;
        }

        /// <summary>
        /// 资源包版本列表是否有效
        /// </summary>
        public bool IsValid => mIsValid;
        /// <summary>
        /// 资源包版本列表资源数据偏移
        /// </summary>
        /// <exception cref="Exception"></exception>
        public int Offset => mIsValid ? mOffset : throw new Exception("ResourcePackVersionList data is invalid.");
        /// <summary>
        /// 资源包版本列表资源数据大小
        /// </summary>
        /// <exception cref="Exception"></exception>
        public long Length => mIsValid ? mLength : throw new Exception("ResourcePackVersionList data is invalid.");
        /// <summary>
        /// 资源包版本列表资源数据哈希值
        /// </summary>
        /// <exception cref="Exception"></exception>
        public int HashCode => mIsValid ? mHashCode : throw new Exception("ResourcePackVersionList data is invalid.");
        /// <summary>
        /// 资源包版本列表包含的资源集合
        /// </summary>
        /// <exception cref="Exception"></exception>
        public Resource[] Resources => mIsValid ? mResources : throw new Exception("ResourcePackVersionList data is invalid.");

        /// <summary>
        /// 版本列表资源
        /// </summary>
        public struct Resource
        {
            private readonly string mName;
            private readonly string mVariant;
            private readonly string mExtension;
            private readonly byte mLoadType;
            private readonly long mOffset;
            private readonly int mLength;
            private readonly int mHashCode;
            private readonly int mCompressedLength;
            private readonly int mCompressedHashCode;

            public Resource(string name, string variant, string extension, byte loadType, long offset, int length, int hashCode, int compressedLength, int compressedHashCode)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }

                mName = name;
                mVariant = variant;
                mExtension = extension;
                mLoadType = loadType;
                mOffset = offset;
                mLength = length;
                mHashCode = hashCode;
                mCompressedLength = compressedLength;
                mCompressedHashCode = compressedHashCode;
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
            /// 版本列表资源偏移
            /// </summary>
            public long Offset => mOffset;
            /// <summary>
            /// 版本列表资源大小
            /// </summary>
            public int Length => mLength;
            /// <summary>
            /// 版本列表资源哈希值
            /// </summary>
            public int HashCode => mHashCode;
            /// <summary>
            /// 版本列表资源压缩后大小
            /// </summary>
            public int CompressedLength => mCompressedLength;
            /// <summary>
            /// 版本列表资源压缩后哈希值
            /// </summary>
            public int CompressedHashCode => mCompressedHashCode;
        }
    }
}