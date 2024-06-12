/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:35
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 卸载场景成功事件
    /// </summary>
    public class UnloadSceneSuccessEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadSceneDependencyAssetEventArgs).GetHashCode();

        public UnloadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建卸载场景成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>卸载场景成功事件</returns>
        public static UnloadSceneSuccessEventArgs Create(Framework.UnloadSceneSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<UnloadSceneSuccessEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理卸载场景成功事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 卸载场景失败事件
    /// </summary>
    public class UnloadSceneFailureEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadSceneDependencyAssetEventArgs).GetHashCode();

        public UnloadSceneFailureEventArgs()
        {
            SceneAssetName = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 场景资源名称
        /// </summary>
        public string SceneAssetName { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建卸载场景失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>卸载场景成功事件</returns>
        public static UnloadSceneFailureEventArgs Create(Framework.UnloadSceneFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<UnloadSceneFailureEventArgs>();
            eventArgs.SceneAssetName = e.SceneAssetName;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理卸载场景失败事件
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            UserData = null;
        }
    }
}