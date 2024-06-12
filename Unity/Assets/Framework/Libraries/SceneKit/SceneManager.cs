/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:54
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public class SceneManager : FrameworkModule, ISceneManager
    {
        private readonly List<string> mLoadedSceneAssetNames;
        private readonly List<string> mLoadingSceneAssetNames;
        private readonly List<string> mUnloadingSceneAssetNames;
        private readonly LoadSceneCallbacks mLoadSceneCallbacks;
        private readonly UnloadSceneCallbacks mUnloadSceneCallbacks;

        private IResourceManager mResourceManager;
        public EventHandler<LoadSceneSuccessEventArgs> mLoadSceneSuccessEventHandler;
        public EventHandler<LoadSceneFailureEventArgs> mLoadSceneFailureEventHandler;
        public EventHandler<LoadSceneUpdateEventArgs> mLoadSceneUpdateEventHandler;
        public EventHandler<LoadSceneDependencyAssetEventArgs> mLoadSceneDependencyAssetEventHandler;
        public EventHandler<UnloadSceneSuccessEventArgs> mUnloadSceneSuccessEventHandler;
        public EventHandler<UnloadSceneFailureEventArgs> mUnloadSceneFailureEventHandler;

        public SceneManager()
        {
            mLoadedSceneAssetNames = new List<string>();
            mLoadingSceneAssetNames = new List<string>();
            mUnloadingSceneAssetNames = new List<string>();
            mLoadSceneCallbacks = new LoadSceneCallbacks(LoadSceneSuccessCallback, LoadSceneFailureCallback, LoadSceneUpdateCallback, LoadSceneDependencyAssetCallback);
            mUnloadSceneCallbacks = new UnloadSceneCallbacks(UnloadSceneSuccessCallback, UnloadSceneFailureCallback);

            mResourceManager = null;
            mLoadSceneSuccessEventHandler = null;
            mLoadSceneFailureEventHandler = null;
            mLoadSceneUpdateEventHandler = null;
            mLoadSceneDependencyAssetEventHandler = null;
            mUnloadSceneSuccessEventHandler = null;
            mUnloadSceneFailureEventHandler = null;
        }

        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public override int Priority => 2;

        /// <summary>
        /// 加载场景成功事件
        /// </summary>
        public event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess
        {
            add => mLoadSceneSuccessEventHandler += value;
            remove => mLoadSceneSuccessEventHandler -= value;
        }

        /// <summary>
        /// 加载场景失败事件
        /// </summary>
        public event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure
        {
            add => mLoadSceneFailureEventHandler += value;
            remove => mLoadSceneFailureEventHandler -= value;
        }

        /// <summary>
        /// 加载场景更新事件
        /// </summary>
        public event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate
        {
            add => mLoadSceneUpdateEventHandler += value;
            remove => mLoadSceneUpdateEventHandler -= value;
        }

        /// <summary>
        /// 加载场景依赖资源事件
        /// </summary>
        public event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset
        {
            add => mLoadSceneDependencyAssetEventHandler += value;
            remove => mLoadSceneDependencyAssetEventHandler -= value;
        }

        /// <summary>
        /// 卸载场景成功事件
        /// </summary>
        public event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess
        {
            add => mUnloadSceneSuccessEventHandler += value;
            remove => mUnloadSceneSuccessEventHandler -= value;
        }

        /// <summary>
        /// 卸载场景失败事件
        /// </summary>
        public event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure
        {
            add => mUnloadSceneFailureEventHandler += value;
            remove => mUnloadSceneFailureEventHandler -= value;
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            var loadedAssetNames = mLoadedSceneAssetNames.ToArray();
            foreach (var assetName in loadedAssetNames)
            {
                if (SceneIsUnloading(assetName))
                {
                    continue;
                }

                UnloadScene(assetName);
            }

            mLoadedSceneAssetNames.Clear();
            mLoadingSceneAssetNames.Clear();
            mUnloadingSceneAssetNames.Clear();
        }


        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            mResourceManager = resourceManager;
        }

        /// <summary>
        /// 获取场景是否已经加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否已经加载</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            return mLoadedSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <returns>所有已加载场景名称</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return mLoadedSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <param name="results">所有已加载场景名称</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            results.AddRange(mLoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在加载</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            return mLoadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <returns>所有正在加载的场景名称</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return mLoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <param name="results">所有正在加载的场景名称</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            results.AddRange(mLoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在卸载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在卸载</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            return mUnloadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <returns>所有正在卸载场景的名称</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return mUnloadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <param name="results">所有正在卸载场景的名称</param>
        public void GetUnloadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            results.AddRange(mUnloadingSceneAssetNames);
        }

        /// <summary>
        /// 检查场景资源是否存在
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景资源是否存在</returns>
        public bool HasScene(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                throw new Exception($"Scene asset name is invalid.");
            }

            return mResourceManager.HasAsset(sceneAssetName) != HasAssetResult.NotExist;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="priority">场景优先级</param>
        /// <param name="userData">自定义数据</param>
        public void LoadScene(string sceneAssetName, int priority = 0, object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                throw new Exception($"Scene asset name is invalid.");
            }

            if (mResourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is being unloaded.");
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is being loading.");
            }

            if (SceneIsLoaded(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is already loaded.");
            }

            mLoadingSceneAssetNames.Add(sceneAssetName);
            mResourceManager.LoadScene(new LoadSceneInfo(sceneAssetName, priority, userData), mLoadSceneCallbacks);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="userData">自定义数据</param>
        public void UnloadScene(string sceneAssetName, object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new Exception("Scene asset name is invalid.");
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                throw new Exception($"Scene asset name is invalid.");
            }

            if (mResourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is being unloaded.");
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is being loading.");
            }

            if (!SceneIsLoaded(sceneAssetName))
            {
                throw new Exception($"Scene asset ({sceneAssetName}) is not loaded yet.");
            }

            mUnloadingSceneAssetNames.Add(sceneAssetName);
            mResourceManager.UnloadScene(sceneAssetName, mUnloadSceneCallbacks, userData);
        }

        private void LoadSceneSuccessCallback(string sceneAssetName, float duration, object userData)
        {
            mLoadingSceneAssetNames.Remove(sceneAssetName);
            mLoadedSceneAssetNames.Add(sceneAssetName);

            if (mLoadSceneSuccessEventHandler != null)
            {
                var eventArgs = LoadSceneSuccessEventArgs.Create(sceneAssetName, duration, userData);
                mLoadSceneSuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadSceneFailureCallback(string sceneAssetName, LoadResourceStatus status,
            string errorMessage, object userData)
        {
            mLoadingSceneAssetNames.Remove(sceneAssetName);

            var error = $"Load scene failure, scene asset name ({sceneAssetName}), status ({status}),error message ({errorMessage}).";
            if (mLoadSceneFailureEventHandler != null)
            {
                var eventArgs = LoadSceneFailureEventArgs.Create(sceneAssetName, error, userData);
                mLoadSceneFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
                return;
            }

            throw new Exception(error);
        }

        private void LoadSceneUpdateCallback(string sceneAssetName, float progress, object userData)
        {
            if (mLoadSceneUpdateEventHandler != null)
            {
                var eventArgs = LoadSceneUpdateEventArgs.Create(sceneAssetName, progress, userData);
                mLoadSceneUpdateEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadSceneDependencyAssetCallback(string sceneAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (mLoadSceneDependencyAssetEventHandler != null)
            {
                var eventArgs = LoadSceneDependencyAssetEventArgs.Create(sceneAssetName, dependencyAssetName, loadedCount, totalCount, userData);
                mLoadSceneDependencyAssetEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void UnloadSceneSuccessCallback(string sceneAssetName, object userData)
        {
            mUnloadingSceneAssetNames.Remove(sceneAssetName);
            mLoadedSceneAssetNames.Remove(sceneAssetName);

            if (mUnloadSceneSuccessEventHandler != null)
            {
                var eventArgs = UnloadSceneSuccessEventArgs.Create(sceneAssetName, userData);
                mUnloadSceneSuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void UnloadSceneFailureCallback(string sceneAssetName, object userData)
        {
            mUnloadingSceneAssetNames.Remove(sceneAssetName);

            if (mUnloadSceneFailureEventHandler != null)
            {
                var eventArgs = UnloadSceneFailureEventArgs.Create(sceneAssetName, userData);
                mUnloadSceneFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
                return;
            }

            throw new Exception($"Unload scene failure, scene asset name ({sceneAssetName}).");
        }
    }
}