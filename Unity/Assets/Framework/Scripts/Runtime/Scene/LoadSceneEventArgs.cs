/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:22
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 加载场景成功事件
    /// </summary>
    public class LoadSceneSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadSceneSuccessEventArgs).GetHashCode();

        public LoadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }


        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>加载场景成功事件</returns>
        public static LoadSceneSuccessEventArgs Create(Framework.LoadSceneSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneSuccessEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.Duration = e.Duration;
            eventArgs.UserData = e.UserData;
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
    public class LoadSceneFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadSceneFailureEventArgs).GetHashCode();

        public LoadSceneFailureEventArgs()
        {
            SceneAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>加载场景失败事件</returns>
        public static LoadSceneFailureEventArgs Create(Framework.LoadSceneFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneFailureEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
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
    public class LoadSceneUpdateEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadSceneUpdateEventArgs).GetHashCode();

        public LoadSceneUpdateEventArgs()
        {
            SceneAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>加载场景更新事件</returns>
        public static LoadSceneUpdateEventArgs Create(Framework.LoadSceneUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneUpdateEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.Progress = e.Progress;
            eventArgs.UserData = e.UserData;
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
    public class LoadSceneDependencyAssetEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadSceneDependencyAssetEventArgs).GetHashCode();

        public LoadSceneDependencyAssetEventArgs()
        {
            SceneAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>加载场景依赖资源事件</returns>
        public static LoadSceneDependencyAssetEventArgs Create(Framework.LoadSceneDependencyAssetEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadSceneDependencyAssetEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.DependencyAssetName = e.DependencyAssetName;
            eventArgs.LoadedCount = e.LoadedCount;
            eventArgs.TotalCount = e.TotalCount;
            eventArgs.UserData = e.UserData;
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