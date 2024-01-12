namespace Framework
{
    /// <summary>
    /// 版本资源列表信息
    /// </summary>
    public struct VersionListInfo
    {
        private readonly int mLength;
        private readonly int mHashCode;
        private readonly int mCompressedLength;
        private readonly int mCompressedHashCode;

        public VersionListInfo(int mLength, int mHashCode, int mCompressedLength, int mCompressedHashCode)
        {
            this.mLength = mLength;
            this.mHashCode = mHashCode;
            this.mCompressedLength = mCompressedLength;
            this.mCompressedHashCode = mCompressedHashCode;
        }
        
        /// <summary>
        /// 版本资源列表大小
        /// </summary>
        public int Length => mLength;
        
        /// <summary>
        /// 版本资源列表哈希值
        /// </summary>
        public int HashCode => mHashCode;

        /// <summary>
        /// 版本资源列表压缩后大小
        /// </summary>
        public int CompressedLength => mCompressedLength;

        /// <summary>
        /// 版本资源列表压缩后哈希值
        /// </summary>
        public int CompressedHashCode => mCompressedHashCode;
    }
}