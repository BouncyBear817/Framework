/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:35
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 卸载场景成功事件
    /// </summary>
    public class UnloadSceneSuccessEventArgs : FrameworkEventArgs
    {
        public UnloadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            UserData = null;
        }

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
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>卸载场景成功事件</returns>
        public static UnloadSceneSuccessEventArgs Create(string sceneAssetName, object userData)
        {
            var eventArgs = ReferencePool.Acquire<UnloadSceneSuccessEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.UserData = userData;
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
    public class UnloadSceneFailureEventArgs : FrameworkEventArgs
    {
        public UnloadSceneFailureEventArgs()
        {
            SceneAssetName = null;
            UserData = null;
        }

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
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>卸载场景成功事件</returns>
        public static UnloadSceneFailureEventArgs Create(string sceneAssetName, object userData)
        {
            var eventArgs = ReferencePool.Acquire<UnloadSceneFailureEventArgs>();
            eventArgs.SceneAssetName = sceneAssetName;
            eventArgs.UserData = userData;
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