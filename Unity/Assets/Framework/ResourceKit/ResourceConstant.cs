namespace Framework
{
    public struct ResourceConstant
    {
        private readonly string mReadOnlyPath;
        private readonly string mReadWritePath;
        private readonly ResourceMode mResourceMode;
        private readonly string mApplicableVersion;
        private readonly string mUpdatePrefixUrl;

        public ResourceConstant(string readOnlyPath, string readWritePath, ResourceMode resourceMode,
            string applicableVersion, string updatePrefixUrl)
        {
            mReadOnlyPath = readOnlyPath;
            mReadWritePath = readWritePath;
            mResourceMode = resourceMode;
            mApplicableVersion = applicableVersion;
            mUpdatePrefixUrl = updatePrefixUrl;
        }

        /// <summary>
        /// 资源只读路径
        /// </summary>
        public string ReadOnlyPath => mReadOnlyPath;

        /// <summary>
        /// 资源读写路径
        /// </summary>
        public string ReadWritePath => mReadWritePath;

        /// <summary>
        /// 资源模式
        /// </summary>
        public ResourceMode ResourceMode => mResourceMode;

        /// <summary>
        /// 当前资源适用的版号
        /// </summary>
        public string ApplicableVersion => mApplicableVersion;

        /// <summary>
        /// 更新前缀地址
        /// </summary>
        public string UpdatePrefixUrl => mUpdatePrefixUrl;
    }
}