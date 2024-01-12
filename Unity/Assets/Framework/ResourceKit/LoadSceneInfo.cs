namespace Framework
{
    /// <summary>
    /// 加载场景信息
    /// </summary>
    public struct LoadSceneInfo
    {
        private readonly string mSceneAssetName;
        private readonly int mPriority;
        private readonly object mUserData;

        public LoadSceneInfo(string sceneAssetName) : this()
        {
            this.mSceneAssetName = sceneAssetName;
        }

        public LoadSceneInfo(string sceneAssetName, int priority) : this()
        {
            this.mSceneAssetName = sceneAssetName;
            this.mPriority = priority;
        }

        public LoadSceneInfo(string sceneAssetName, object userData) : this()
        {
            this.mSceneAssetName = sceneAssetName;
            this.mUserData = userData;
        }

        public LoadSceneInfo(string sceneAssetName, int priority, object userData = null)
        {
            this.mSceneAssetName = sceneAssetName;
            this.mPriority = priority;
            this.mUserData = userData;
        }

        public string SceneAssetName => mSceneAssetName;
        public int Priority => mPriority;
        public object UserData => mUserData;
    }
}