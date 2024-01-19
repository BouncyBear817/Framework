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

        public VersionListInfo(int length, int hashCode, int compressedLength, int compressedHashCode)
        {
            this.mLength = length;
            this.mHashCode = hashCode;
            this.mCompressedLength = compressedLength;
            this.mCompressedHashCode = compressedHashCode;
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