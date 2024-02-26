/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 卸载场景成功回调函数
    /// </summary>
    public delegate void UnloadSceneSuccessCallback(string sceneAssetName, object userData);

    /// <summary>
    /// 卸载场景失败回调函数
    /// </summary>
    public delegate void UnloadSceneFailureCallback(string sceneAssetName, string errorMessage, object userData);

    /// <summary>
    /// 卸载场景回调函数集
    /// </summary>
    public sealed class UnloadSceneCallbacks
    {
        private readonly UnloadSceneSuccessCallback mUnloadSceneSuccessCallback;
        private readonly UnloadSceneFailureCallback mUnloadSceneFailureCallback;

        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback,
            UnloadSceneFailureCallback unloadSceneFailureCallback = null)
        {
            mUnloadSceneSuccessCallback = unloadSceneSuccessCallback ??
                                          throw new Exception("Unload scene success callback is invalid.");
            mUnloadSceneFailureCallback = unloadSceneFailureCallback;
        }

        /// <summary>
        /// 卸载场景成功回调函数
        /// </summary>
        public UnloadSceneSuccessCallback UnloadSceneSuccessCallback => mUnloadSceneSuccessCallback;

        /// <summary>
        /// 卸载场景失败回调函数
        /// </summary>
        public UnloadSceneFailureCallback UnloadSceneFailureCallback => mUnloadSceneFailureCallback;
    }
}