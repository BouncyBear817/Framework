namespace Framework
{
    /// <summary>
    /// 本地只读区版本资源列表序列化器
    /// </summary>
    public sealed class ReadOnlyVersionListSerializer : FrameworkSerializer<LocalVersionList>
    {
        private static readonly byte[] sHeader = new byte[]
        {
            (byte)'F', (byte)'R', (byte)'V'
        };

        /// <summary>
        /// 获取数据头标识
        /// </summary>
        /// <returns>数据头标识</returns>
        protected override byte[] GetHeader()
        {
            return sHeader;
        }
    }
    
    /// <summary>
    /// 本地读写区版本资源序列化器
    /// </summary>
    public sealed class ReadWriteVersionListSerializer : FrameworkSerializer<LocalVersionList>
    {
        private static readonly byte[] sHeader = new byte[]
        {
            (byte)'F', (byte)'W', (byte)'V'
        };

        /// <summary>
        /// 获取数据头标识
        /// </summary>
        /// <returns>数据头标识</returns>
        protected override byte[] GetHeader()
        {
            return sHeader;
        }
    }
}