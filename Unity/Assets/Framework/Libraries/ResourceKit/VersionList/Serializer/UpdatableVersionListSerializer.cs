namespace Framework
{
    /// <summary>
    /// 可更新模式版本资源列表序列化器
    /// </summary>
    public sealed class UpdatableVersionListSerializer : FrameworkSerializer<UpdatableVersionList>
    {
        private static readonly byte[] sHeader = new byte[]
        {
            (byte)'F', (byte)'U', (byte)'V'
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