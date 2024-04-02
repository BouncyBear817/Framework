/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:22
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 加载场景成功事件
    /// </summary>
    public class LoadSceneSuccessEventArgs : FrameworkEventArgs
    {
        public LoadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 加载场景持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载场景成功事件
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="duration">加载场景持续时间</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>加载场景成功事件</returns>
        public static LoadSceneSuccessEventArgs Create(string sceneAssetName, float duration, object userData)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneSuccessEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.Duration = duration;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载场景成功事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载场景失败事件
    /// </summary>
    public class LoadSceneFailureEventArgs : FrameworkEventArgs
    {
        public LoadSceneFailureEventArgs()
        {
            SceneAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载场景失败事件
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>加载场景失败事件</returns>
        public static LoadSceneFailureEventArgs Create(string sceneAssetName, string errorMessage, object userData)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneFailureEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载场景失败事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载场景更新事件
    /// </summary>
    public class LoadSceneUpdateEventArgs : FrameworkEventArgs
    {
        public LoadSceneUpdateEventArgs()
        {
            SceneAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 加载场景进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载场景更新事件
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="progress">加载场景进度</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>加载场景更新事件</returns>
        public static LoadSceneUpdateEventArgs Create(string sceneAssetName, float progress, object userData)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneUpdateEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.Progress = progress;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载场景更新事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载场景依赖资源事件
    /// </summary>
    public class LoadSceneDependencyAssetEventArgs : FrameworkEventArgs
    {
        public LoadSceneDependencyAssetEventArgs()
        {
            SceneAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 加载场景依赖资源名称
        /// </summary>
        public string DependencyAssetName { get; private set; }

        /// <summary>
        /// 已加载依赖资源数量
        /// </summary>
        public int LoadedCount { get; private set; }

        /// <summary>
        /// 总共加载依赖资源数量
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载场景依赖资源事件
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="dependencyAssetName">加载场景依赖资源名称</param>
        /// <param name="loadedCount">已加载依赖资源数量</param>
        /// <param name="totalCount">总共加载依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>加载场景依赖资源事件</returns>
        public static LoadSceneDependencyAssetEventArgs Create(string sceneAssetName, string dependencyAssetName,
            int loadedCount, int totalCount, object userData)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneDependencyAssetEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.DependencyAssetName = dependencyAssetName;
            eventArgs.LoadedCount = loadedCount;
            eventArgs.TotalCount = totalCount;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载场景依赖资源事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}