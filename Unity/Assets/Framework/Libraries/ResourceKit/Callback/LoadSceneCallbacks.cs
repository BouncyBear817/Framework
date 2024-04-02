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
    /// 加载场景成功回调函数
    /// </summary>
    public delegate void LoadSceneSuccessCallback(string sceneAssetName, float duration, object userData);

    /// <summary>
    /// 加载场景更新回调函数
    /// </summary>
    public delegate void LoadSceneUpdateCallback(string sceneAssetName, float progress, object userData);

    /// <summary>
    /// 加载场景依赖资源回调函数
    /// </summary>
    public delegate void LoadSceneDependencyAssetCallback(string sceneAssetName, string dependencyAssetName,
        int loadedCount, int totalCount, object userData);

    /// <summary>
    /// 加载场景失败回调函数
    /// </summary>
    public delegate void LoadSceneFailureCallback(string sceneAssetName, LoadResourceStatus status, string errorMessage,
        object userData);

    /// <summary>
    /// 加载场景回调函数集
    /// </summary>
    public sealed class LoadSceneCallbacks
    {
        private readonly LoadSceneSuccessCallback mLoadSceneSuccessCallback;
        private readonly LoadSceneUpdateCallback mLoadSceneUpdateCallback;
        private readonly LoadSceneDependencyAssetCallback mLoadSceneDependencyAssetCallback;
        private readonly LoadSceneFailureCallback mLoadSceneFailureCallback;

        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,
            LoadSceneFailureCallback loadSceneFailureCallback = null,
            LoadSceneUpdateCallback loadSceneUpdateCallback = null,
            LoadSceneDependencyAssetCallback loadSceneDependencyAssetCallback = null)
        {
            mLoadSceneSuccessCallback = loadSceneSuccessCallback ??
                                        throw new Exception("Load scene success callback is invalid.");
            mLoadSceneUpdateCallback = loadSceneUpdateCallback;
            mLoadSceneDependencyAssetCallback = loadSceneDependencyAssetCallback;
            mLoadSceneFailureCallback = loadSceneFailureCallback;
        }

        /// <summary>
        /// 加载场景成功回调函数
        /// </summary>
        public LoadSceneSuccessCallback LoadSceneSuccessCallback => mLoadSceneSuccessCallback;

        /// <summary>
        /// 加载场景更新回调函数
        /// </summary>
        public LoadSceneUpdateCallback LoadSceneUpdateCallback => mLoadSceneUpdateCallback;

        /// <summary>
        /// 加载场景依赖资源回调函数
        /// </summary>
        public LoadSceneDependencyAssetCallback LoadSceneDependencyAssetCallback => mLoadSceneDependencyAssetCallback;

        /// <summary>
        /// 加载场景失败回调函数
        /// </summary>
        public LoadSceneFailureCallback LoadSceneFailureCallback => mLoadSceneFailureCallback;
    }
}