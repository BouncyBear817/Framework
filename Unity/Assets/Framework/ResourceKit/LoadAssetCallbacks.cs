/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/29 16:18:55
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 加载资源成功回调函数
    /// </summary>
    public delegate void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData);
    
    /// <summary>
    /// 加载资源失败回调函数
    /// </summary>
    public delegate void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage,
        object userData);
    
    /// <summary>
    /// 加载资源更新回调函数
    /// </summary>
    public delegate void LoadAssetUpdateCallback(string assetName, float progress, object userData);
    
    /// <summary>
    /// 加载资源依赖回调函数
    /// </summary>
    public delegate void LoadAssetDependencyCallback(string assetName, string dependencyAssetName, int loadedCount,
        int totalCount, object userData);
    
    /// <summary>
    /// 加载资源回调函数集
    /// </summary>
    public sealed class LoadAssetCallbacks
    {
        private readonly LoadAssetSuccessCallback mLoadAssetSuccessCallback;
        private readonly LoadAssetFailureCallback mLoadAssetFailureCallback;
        private readonly LoadAssetUpdateCallback mLoadAssetUpdateCallback;
        private readonly LoadAssetDependencyCallback mLoadAssetDependencyCallback;

        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,
            LoadAssetFailureCallback loadAssetFailureCallback = null,
            LoadAssetUpdateCallback loadAssetUpdateCallback = null,
            LoadAssetDependencyCallback loadAssetDependencyCallback = null)
        {
            mLoadAssetSuccessCallback = loadAssetSuccessCallback ?? throw new Exception("load asset callback is invalid.");
            mLoadAssetFailureCallback = loadAssetFailureCallback;
            mLoadAssetUpdateCallback = loadAssetUpdateCallback;
            mLoadAssetDependencyCallback = loadAssetDependencyCallback;
        }
        
        /// <summary>
        /// 加载资源成功回调函数
        /// </summary>
        public LoadAssetSuccessCallback LoadAssetSuccessCallback => mLoadAssetSuccessCallback;
        
        /// <summary>
        /// 加载资源失败回调函数
        /// </summary>
        public LoadAssetFailureCallback LoadAssetFailureCallback => mLoadAssetFailureCallback;

        /// <summary>
        /// 加载资源更新回调函数
        /// </summary>
        public LoadAssetUpdateCallback LoadAssetUpdateCallback => mLoadAssetUpdateCallback;

        /// <summary>
        /// 加载资源依赖回调函数
        /// </summary>
        public LoadAssetDependencyCallback LoadAssetDependencyCallback => mLoadAssetDependencyCallback;
    }
}