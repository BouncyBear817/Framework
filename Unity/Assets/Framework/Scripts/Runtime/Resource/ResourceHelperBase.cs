// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 14:26:47
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 资源辅助器基类
    /// </summary>
    public abstract class ResourceHelperBase : MonoBehaviour, IResourceHelper
    {
        /// <summary>
        /// 从指定路径加载数据流
        /// </summary>
        /// <param name="fileUri">文件路径</param>
        /// <param name="loadBytesCallbacks">加载数据流回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public abstract void LoadBytes(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public abstract void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="objectToRelease">待释放的资源</param>
        public abstract void Release(object objectToRelease);
    }
}