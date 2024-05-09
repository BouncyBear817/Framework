namespace Framework
{
    /// <summary>
    /// 资源包版本资源列表序列化器
    /// </summary>
    public sealed class ResourcePackVersionListSerializer : FrameworkSerializer<ResourcePackVersionList>
    {
        private static readonly byte[] sHeader = new byte[]
        {
            (byte)'F', (byte)'K', (byte)'V'
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