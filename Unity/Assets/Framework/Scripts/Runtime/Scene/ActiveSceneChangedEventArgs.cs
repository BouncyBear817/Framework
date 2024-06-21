/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/2 15:6:29
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine.SceneManagement;

namespace Framework.Runtime
{
    /// <summary>
    /// 激活场景被改变事件
    /// </summary>
    public class ActiveSceneChangedEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(ActiveSceneChangedEventArgs).GetHashCode();

        public ActiveSceneChangedEventArgs()
        {
            LastActiveScene = default(Scene);
            ActiveScene = default(Scene);
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 上一个被激活的场景
        /// </summary>
        public Scene LastActiveScene { get; private set; }

        /// <summary>
        /// 当前被激活的场景
        /// </summary>
        public Scene ActiveScene { get; private set; }

        /// <summary>
        /// 创建激活场景被改变事件
        /// </summary>
        /// <param name="lastActiveScene">上一个被激活的场景</param>
        /// <param name="activeScene">当前被激活的场景</param>
        /// <returns></returns>
        public static ActiveSceneChangedEventArgs Create(Scene lastActiveScene, Scene activeScene)
        {
            var eventArgs = ReferencePool.Acquire<ActiveSceneChangedEventArgs>();
            eventArgs.LastActiveScene = lastActiveScene;
            eventArgs.ActiveScene = activeScene;
            return eventArgs;
        }

        /// <summary>
        /// 清理激活场景被改变事件
        /// </summary>
        public override void Clear()
        {
            LastActiveScene = default(Scene);
            ActiveScene = default(Scene);
        }
    }
}