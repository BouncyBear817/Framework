namespace Framework
{
    /// <summary>
    /// 资源序列化器
    /// </summary>
    public struct ResourceSerializer
    {
        /// <summary>
        /// 本地只读区版本资源列表序列化器
        /// </summary>
        public ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer { get; set; }
        /// <summary>
        /// 本地读写区版本资源序列化器
        /// </summary>
        public ReadWriteVersionListSerializer ReadWriteVersionListSerializer { get; set; }
        /// <summary>
        /// 单机模式版本资源列表序列化器
        /// </summary>
        public PackageVersionListSerializer PackageVersionListSerializer { get; set; }
        /// <summary>
        /// 资源包版本资源列表序列化器
        /// </summary>
        public ResourcePackVersionListSerializer ResourcePackVersionListSerializer { get; set; }
        /// <summary>
        /// 可更新模式版本资源列表序列化器
        /// </summary>
        public UpdatableVersionListSerializer UpdatableVersionListSerializer { get; set; }
    }
}