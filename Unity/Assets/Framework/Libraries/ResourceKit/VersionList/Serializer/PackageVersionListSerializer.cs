namespace Framework
{
    /// <summary>
    /// 单机模式版本资源列表序列化器
    /// </summary>
    public sealed class PackageVersionListSerializer : FrameworkSerializer<PackageVersionList>
    {
        private static readonly byte[] sHeader = new byte[]
        {
            (byte)'F', (byte)'P', (byte)'V'
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