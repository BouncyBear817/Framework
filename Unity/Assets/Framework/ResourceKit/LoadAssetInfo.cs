using System;

namespace Framework
{
    /// <summary>
    /// 加载资源的信息
    /// </summary>
    public struct LoadAssetInfo
    {
        private readonly string mAssetName;
        private readonly Type mAssetType;
        private readonly int mPriority;
        private readonly object mUserData;

        public LoadAssetInfo(string assetName) : this()
        {
            this.mAssetName = assetName;
        }

        public LoadAssetInfo(string assetName, Type assetType) : this()
        {
            this.mAssetName = assetName;
            this.mAssetType = assetType;
        }

        public LoadAssetInfo(string assetName, int priority) : this()
        {
            this.mAssetName = assetName;
            this.mPriority = priority;
        }

        public LoadAssetInfo(string assetName, object userData) : this()
        {
            this.mAssetName = assetName;
            this.mUserData = userData;
        }

        public LoadAssetInfo(string assetName, Type assetType, object userData) : this()
        {
            this.mAssetName = assetName;
            this.mAssetType = assetType;
            this.mUserData = userData;
        }

        public LoadAssetInfo(string assetName, int priority, object userData) : this()
        {
            this.mAssetName = assetName;
            this.mPriority = priority;
            this.mUserData = userData;
        }

        public LoadAssetInfo(string assetName, Type assetType, int priority, object userData)
        {
            this.mAssetName = assetName;
            this.mAssetType = assetType;
            this.mPriority = priority;
            this.mUserData = userData;
        }

        /// <summary>
        /// 加载资源名称
        /// </summary>
        public string AssetName => mAssetName;

        /// <summary>
        /// 加载资源类型
        /// </summary>
        public Type AssetType => mAssetType;

        /// <summary>
        /// 加载资源优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// 加载资源用户数据
        /// </summary>
        public object UserData => mUserData;
    }
}