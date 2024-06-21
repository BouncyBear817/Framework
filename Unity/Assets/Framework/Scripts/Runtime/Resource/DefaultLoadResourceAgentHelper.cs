// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 14:31:5
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认加载资源代理辅助器
    /// </summary>
    public class DefaultLoadResourceAgentHelper : LoadResourceAgentHelperBase, IDisposable
    {
        private string mFileFullPath = null;
        private string mFileName = null;
        private string mBytesFullPath = null;
        private string mAssetName = null;
        private float mLastProgress = 0f;
        private bool mDisposed = false;
        private UnityWebRequest mUnityWebRequest = null;
        private AssetBundleCreateRequest mFileAssetBundleCreateRequest = null;
        private AssetBundleCreateRequest mBytesAssetBundleCreateRequest = null;
        private AssetBundleRequest mAssetBundleRequest = null;
        private AsyncOperation mAsyncOperation = null;

        private EventHandler<LoadResourceAgentHelperUpdateEventArgs> mLoadResourceAgentHelperUpdateEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> mLoadResourceAgentHelperReadFileCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> mLoadResourceAgentHelperReadBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> mLoadResourceAgentHelperParseBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> mLoadResourceAgentHelperLoadCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperErrorEventArgs> mLoadResourceAgentHelperErrorEventHandler = null;


        /// <summary>
        /// 加载资源代理辅助器更新事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
        {
            add => mLoadResourceAgentHelperUpdateEventHandler += value;
            remove => mLoadResourceAgentHelperUpdateEventHandler -= value;
        }

        /// <summary>
        /// 加载资源代理辅助器读取资源文件完成事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
        {
            add => mLoadResourceAgentHelperReadFileCompleteEventHandler += value;
            remove => mLoadResourceAgentHelperReadFileCompleteEventHandler -= value;
        }

        /// <summary>
        /// 加载资源代理辅助器读取资源二进制流完成事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
        {
            add => mLoadResourceAgentHelperReadBytesCompleteEventHandler += value;
            remove => mLoadResourceAgentHelperReadBytesCompleteEventHandler -= value;
        }

        /// <summary>
        /// 加载资源代理辅助器将资源二进制流转换为加载对象事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
        {
            add => mLoadResourceAgentHelperParseBytesCompleteEventHandler += value;
            remove => mLoadResourceAgentHelperParseBytesCompleteEventHandler -= value;
        }

        /// <summary>
        /// 加载资源代理辅助器加载资源完成事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
        {
            add => mLoadResourceAgentHelperLoadCompleteEventHandler += value;
            remove => mLoadResourceAgentHelperLoadCompleteEventHandler -= value;
        }

        /// <summary>
        /// 加载资源代理辅助器错误事件
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
        {
            add => mLoadResourceAgentHelperErrorEventHandler += value;
            remove => mLoadResourceAgentHelperErrorEventHandler -= value;
        }

        /// <summary>
        /// 读取资源文件
        /// </summary>
        /// <param name="fullPath">资源完整路径</param>
        public override void ReadFile(string fullPath)
        {
            if (mLoadResourceAgentHelperReadFileCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            mFileFullPath = fullPath;

            mFileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fullPath);
        }

        /// <summary>
        /// 读取资源文件
        /// </summary>
        /// <param name="fileSystem">资源的文件系统</param>
        /// <param name="name">资源名称</param>
        public override void ReadFile(IFileSystem fileSystem, string name)
        {
            if (mLoadResourceAgentHelperReadFileCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            var fileInfo = fileSystem.GetFileInfo(name);
            mFileFullPath = fileSystem.FullPath;
            mFileName = name;

            mFileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fileSystem.FullPath, 0u, (ulong)fileInfo.Offset);
        }

        /// <summary>
        /// 读取资源二进制流
        /// </summary>
        /// <param name="fullPath">资源完整路径</param>
        public override void ReadBytes(string fullPath)
        {
            if (mLoadResourceAgentHelperReadBytesCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            mBytesFullPath = fullPath;

            mUnityWebRequest = UnityWebRequest.Get(Utility.Path.GetRemotePath(fullPath));
            mUnityWebRequest.SendWebRequest();
        }

        /// <summary>
        /// 读取资源二进制流
        /// </summary>
        /// <param name="fileSystem">资源的文件系统</param>
        /// <param name="name">资源名称</param>
        public override void ReadBytes(IFileSystem fileSystem, string name)
        {
            if (mLoadResourceAgentHelperReadBytesCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            var bytes = fileSystem.ReadFile(name);
            var eventArgs = LoadResourceAgentHelperReadBytesCompleteEventArgs.Create(bytes);
            mLoadResourceAgentHelperReadBytesCompleteEventHandler(this, eventArgs);
            ReferencePool.Release(eventArgs);
        }

        /// <summary>
        /// 将资源二进制流转换为加载对象
        /// </summary>
        /// <param name="bytes">资源二进制流</param>
        public override void ParseBytes(byte[] bytes)
        {
            if (mLoadResourceAgentHelperParseBytesCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            mBytesAssetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bytes);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="isScene">资源是否为场景</param>
        public override void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
        {
            if (mLoadResourceAgentHelperLoadCompleteEventHandler == null || mLoadResourceAgentHelperUpdateEventHandler == null ||
                mLoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            var assetBundle = resource as AssetBundle;
            if (assetBundle == null)
            {
                var errorMessage = "Can not load asset bundle from loaded resource which is not an asset bundle.";
                var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.TypeError, errorMessage);
                mLoadResourceAgentHelperErrorEventHandler(this, eventArgs);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                var errorMessage = "Can not load asset from asset bundle which child name is invalid.";
                var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, errorMessage);
                mLoadResourceAgentHelperErrorEventHandler(this, eventArgs);
                return;
            }

            mAssetName = assetName;
            if (isScene)
            {
                var sceneNamePositionStart = assetName.LastIndexOf('/');
                int sceneNamePositionEnd = assetName.LastIndexOf('.');
                if (sceneNamePositionStart <= 0 || sceneNamePositionEnd <= 0 || sceneNamePositionStart > sceneNamePositionEnd)
                {
                    var errorMessage = $"Scene name ({assetName}) is invalid.";
                    var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, errorMessage);
                    mLoadResourceAgentHelperErrorEventHandler(this, eventArgs);
                    return;
                }

                var sceneName = assetName.Substring(sceneNamePositionStart + 1, sceneNamePositionEnd - sceneNamePositionStart - 1);
                mAsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                if (assetType != null)
                {
                    mAssetBundleRequest = assetBundle.LoadAssetAsync(assetName, assetType);
                }
                else
                {
                    mAssetBundleRequest = assetBundle.LoadAssetAsync(assetName);
                }
            }
        }

        /// <summary>
        /// 重置加载资源代理辅助器
        /// </summary>
        public override void Reset()
        {
            mFileFullPath = null;
            mFileName = null;
            mBytesFullPath = null;
            mAssetName = null;
            mLastProgress = 0f;

            if (mUnityWebRequest != null)
            {
                mUnityWebRequest.Dispose();
                mUnityWebRequest = null;
            }

            mFileAssetBundleCreateRequest = null;
            mBytesAssetBundleCreateRequest = null;
            mAssetBundleRequest = null;
            mAsyncOperation = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (mDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (mUnityWebRequest != null)
                {
                    mUnityWebRequest.Dispose();
                    mUnityWebRequest = null;
                }
            }

            mDisposed = true;
        }

        private void Update()
        {
            UpdateUnityWebRequest();

            UpdateFileAssetBundleCreateRequest();
            UpdateBytesAssetBundleCreateRequest();
            UpdateAssetBundleRequest();
            UpdateAsyncOperation();
        }

        private void UpdateUnityWebRequest()
        {
            if (mUnityWebRequest != null)
            {
                if (mUnityWebRequest.isDone)
                {
                    if (string.IsNullOrEmpty(mUnityWebRequest.error))
                    {
                        var eventArgs = LoadResourceAgentHelperReadBytesCompleteEventArgs.Create(mUnityWebRequest.downloadHandler.data);
                        mLoadResourceAgentHelperReadBytesCompleteEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);

                        mUnityWebRequest.Dispose();
                        mUnityWebRequest = null;
                        mBytesFullPath = null;
                        mLastProgress = 0f;
                    }
                    else
                    {
                        var isError = mUnityWebRequest.result != UnityWebRequest.Result.Success;
                        var errorMessage = isError ? mUnityWebRequest.error : null;
                        var errorMessageAppend = $"Can not load asset bundle ({mBytesFullPath}) with error message ({errorMessage}).";
                        var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, errorMessageAppend);
                        mLoadResourceAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
                else if (Math.Abs(mUnityWebRequest.downloadProgress - mLastProgress) > 0f)
                {
                    mLastProgress = mUnityWebRequest.downloadProgress;
                    var eventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.ReadResource, mUnityWebRequest.downloadProgress);
                    mLoadResourceAgentHelperUpdateEventHandler?.Invoke(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }

        private void UpdateFileAssetBundleCreateRequest()
        {
            if (mFileAssetBundleCreateRequest != null)
            {
                if (mFileAssetBundleCreateRequest.isDone)
                {
                    var assetBundle = mFileAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        var eventArgs = LoadResourceAgentHelperReadFileCompleteEventArgs.Create(assetBundle);
                        mLoadResourceAgentHelperReadFileCompleteEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);

                        mFileAssetBundleCreateRequest = null;
                        mLastProgress = 0f;
                    }
                    else
                    {
                        var fileName = mFileName == null ? mFileFullPath : $"{mFileFullPath} | {mFileName}";
                        var errorMessageAppend = $"Can not load asset bundle from file ({fileName}) which is not a valid asset bundle.";
                        var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, errorMessageAppend);
                        mLoadResourceAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
                else if (Math.Abs(mFileAssetBundleCreateRequest.progress - mLastProgress) > 0f)
                {
                    mLastProgress = mFileAssetBundleCreateRequest.progress;
                    var eventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadResource, mFileAssetBundleCreateRequest.progress);
                    mLoadResourceAgentHelperUpdateEventHandler?.Invoke(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }

        private void UpdateBytesAssetBundleCreateRequest()
        {
            if (mBytesAssetBundleCreateRequest != null)
            {
                if (mBytesAssetBundleCreateRequest.isDone)
                {
                    var assetBundle = mBytesAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        var eventArgs = LoadResourceAgentHelperParseBytesCompleteEventArgs.Create(assetBundle);
                        mLoadResourceAgentHelperParseBytesCompleteEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);

                        mBytesAssetBundleCreateRequest = null;
                        mLastProgress = 0f;
                    }
                    else
                    {
                        var errorMessageAppend = $"Can not load asset bundle from memory which is not a valid asset bundle.";
                        var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, errorMessageAppend);
                        mLoadResourceAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
                else if (Math.Abs(mBytesAssetBundleCreateRequest.progress - mLastProgress) > 0f)
                {
                    mLastProgress = mBytesAssetBundleCreateRequest.progress;
                    var eventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadResource, mBytesAssetBundleCreateRequest.progress);
                    mLoadResourceAgentHelperUpdateEventHandler?.Invoke(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }

        private void UpdateAssetBundleRequest()
        {
            if (mAssetBundleRequest != null)
            {
                if (mAssetBundleRequest.isDone)
                {
                    var asset = mAssetBundleRequest.asset;
                    if (asset != null)
                    {
                        var eventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(asset);
                        mLoadResourceAgentHelperLoadCompleteEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);

                        mAssetName = null;
                        mAssetBundleRequest = null;
                        mLastProgress = 0f;
                    }
                    else
                    {
                        var errorMessageAppend = $"Can not load asset ({mAssetName}) from asset bundle which is not exist.";
                        var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, errorMessageAppend);
                        mLoadResourceAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
                else if (Math.Abs(mAssetBundleRequest.progress - mLastProgress) > 0f)
                {
                    mLastProgress = mAssetBundleRequest.progress;
                    var eventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadAsset, mAssetBundleRequest.progress);
                    mLoadResourceAgentHelperUpdateEventHandler?.Invoke(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }

        private void UpdateAsyncOperation()
        {
            if (mAsyncOperation != null)
            {
                if (mAsyncOperation.isDone)
                {
                    if (mAsyncOperation.allowSceneActivation)
                    {
                        var sceneAsset = new SceneAsset();
                        var eventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(sceneAsset);
                        mLoadResourceAgentHelperLoadCompleteEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);

                        mAssetName = null;
                        mAsyncOperation = null;
                        mLastProgress = 0f;
                    }
                    else
                    {
                        var errorMessageAppend = $"Can not load scene asset ({mAssetName}) from asset bundle.";
                        var eventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, errorMessageAppend);
                        mLoadResourceAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
                else if (Math.Abs(mAsyncOperation.progress - mLastProgress) > 0f)
                {
                    mLastProgress = mAsyncOperation.progress;
                    var eventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadScene, mAsyncOperation.progress);
                    mLoadResourceAgentHelperUpdateEventHandler?.Invoke(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }
    }
}