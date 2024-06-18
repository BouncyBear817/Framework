/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/2 15:14:57
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Runtime
{
    /// <summary>
    /// 场景组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Scene")]
    public sealed class SceneComponent : FrameworkComponent
    {
        private const int DefaultPriority = 0;

        private readonly SortedDictionary<string, int> mSceneOrder = new SortedDictionary<string, int>();
        private ISceneManager mSceneManager = null;
        private EventComponent mEventComponent = null;
        private Scene mFrameworkScene = default(Scene);

        [SerializeField] private Camera mMainCamera = null;
        [SerializeField] private bool mEnableLoadSceneUpdateEvent = false;
        [SerializeField] private bool mEnableLoadSceneDependencyAssetEvent = false;

        public Camera MainCamera => mMainCamera;

        protected override void Awake()
        {
            base.Awake();

            mSceneManager = FrameworkEntry.GetModule<ISceneManager>();
            if (mSceneManager == null)
            {
                Log.Fatal("Scene manager is invalid.");
                return;
            }

            mSceneManager.LoadSceneSuccess += OnLoadSceneSuccess;
            mSceneManager.LoadSceneFailure += OnLoadSceneFailure;
            if (mEnableLoadSceneUpdateEvent)
            {
                mSceneManager.LoadSceneUpdate += OnLoadSceneUpdate;
            }

            if (mEnableLoadSceneDependencyAssetEvent)
            {
                mSceneManager.LoadSceneDependencyAsset += OnLoadSceneDependencyAsset;
            }

            mSceneManager.UnloadSceneSuccess += OnUnloadSceneSuccess;
            mSceneManager.UnloadSceneFailure += OnUnloadSceneFailure;

            mFrameworkScene = SceneManager.GetSceneAt(MainEntryHelper.FrameworkSceneId);
            if (!mFrameworkScene.IsValid())
            {
                Log.Error("Framework scene is invalid.");
            }
        }

        private void Start()
        {
            var baseComponent = MainEntryHelper.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
            }

            if (baseComponent.EditorResourceMode)
            {
                mSceneManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                mSceneManager.SetResourceManager(FrameworkEntry.GetModule<IResourceManager>());
            }
        }

        /// <summary>
        /// 获取场景名称
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景名称</returns>
        public static string GetSceneName(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                Log.Error("Scene asset name is invalid.");
                return null;
            }

            var sceneNamePosition = sceneAssetName.LastIndexOf('/');
            if (sceneNamePosition + 1 >= sceneAssetName.Length)
            {
                Log.Error("Scene asset name is invalid.");
                return null;
            }

            var sceneName = sceneAssetName.Substring(sceneNamePosition + 1);
            sceneNamePosition = sceneName.LastIndexOf(".unity", StringComparison.Ordinal);
            if (sceneNamePosition > 0)
            {
                sceneName = sceneName.Substring(0, sceneNamePosition);
            }

            return sceneName;
        }

        /// <summary>
        /// 获取场景是否已经加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否已经加载</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            return mSceneManager.SceneIsLoaded(sceneAssetName);
        }

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <returns>所有已加载场景名称</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return mSceneManager.GetLoadedSceneAssetNames();
        }

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <param name="results">所有已加载场景名称</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            mSceneManager.GetLoadedSceneAssetNames(results);
        }

        /// <summary>
        /// 获取场景是否正在加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在加载</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            return mSceneManager.SceneIsLoading(sceneAssetName);
        }

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <returns>所有正在加载的场景名称</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return mSceneManager.GetLoadingSceneAssetNames();
        }

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <param name="results">所有正在加载的场景名称</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            mSceneManager.GetLoadingSceneAssetNames(results);
        }

        /// <summary>
        /// 获取场景是否正在卸载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在卸载</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            return mSceneManager.SceneIsUnloading(sceneAssetName);
        }

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <returns>所有正在卸载场景的名称</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return mSceneManager.GetUnloadingSceneAssetNames();
        }

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <param name="results">所有正在卸载场景的名称</param>
        public void GetUnloadingSceneAssetNames(List<string> results)
        {
            mSceneManager.GetUnloadingSceneAssetNames(results);
        }

        /// <summary>
        /// 检查场景资源是否存在
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景资源是否存在</returns>
        public bool HasScene(string sceneAssetName)
        {
            return mSceneManager.HasScene(sceneAssetName);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="priority">场景优先级</param>
        /// <param name="userData">自定义数据</param>
        public void LoadScene(string sceneAssetName, int priority = 0, object userData = null)
        {
            mSceneManager.LoadScene(sceneAssetName, priority, userData);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="userData">自定义数据</param>
        public void UnloadScene(string sceneAssetName, object userData = null)
        {
            mSceneManager.UnloadScene(sceneAssetName, userData);
            mSceneOrder.Remove(sceneAssetName);
        }

        /// <summary>
        /// 设置场景顺序
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="sceneOrder">场景顺序</param>
        public void SetSceneOrder(string sceneAssetName, int sceneOrder)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                Log.Error("Scene asset name is invalid.");
                return;
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                Log.Error($"Scene asset name is invalid.");
                return;
            }

            if (SceneIsLoading(sceneAssetName))
            {
                mSceneOrder[sceneAssetName] = sceneOrder;
                return;
            }

            if (SceneIsLoaded(sceneAssetName))
            {
                mSceneOrder[sceneAssetName] = sceneOrder;
                RefreshSceneOrder();
                return;
            }

            Log.Error($"Scene ({sceneAssetName}) is not loading or loaded.");
        }

        private void RefreshMainCamera()
        {
            mMainCamera = Camera.main;
        }

        private void RefreshSceneOrder()
        {
            if (mSceneOrder.Count > 0)
            {
                string maxSceneName = null;
                int maxSceneOrder = 0;
                foreach (var (sceneName, sceneOrder) in mSceneOrder)
                {
                    if (SceneIsLoading(sceneName))
                    {
                        continue;
                    }

                    if (maxSceneName == null)
                    {
                        maxSceneName = sceneName;
                        maxSceneOrder = sceneOrder;
                        continue;
                    }

                    if (sceneOrder > maxSceneOrder)
                    {
                        maxSceneName = sceneName;
                        maxSceneOrder = sceneOrder;
                    }
                }

                if (maxSceneName == null)
                {
                    SetActiveScene(mFrameworkScene);
                    return;
                }

                var scene = SceneManager.GetSceneByName(GetSceneName(maxSceneName));
                if (!scene.IsValid())
                {
                    Log.Error($"Active scene ({maxSceneName}) is invalid.");
                    return;
                }

                SetActiveScene(scene);
            }
            else
            {
                SetActiveScene(mFrameworkScene);
            }
        }

        private void SetActiveScene(Scene scene)
        {
            var lastActiveScene = SceneManager.GetActiveScene();
            if (lastActiveScene != scene)
            {
                SceneManager.SetActiveScene(scene);
                mEventComponent.Fire(this, ActiveSceneChangedEventArgs.Create(lastActiveScene, scene));
            }

            RefreshMainCamera();
        }

        private void OnLoadSceneSuccess(object sender, Framework.LoadSceneSuccessEventArgs e)
        {
            mSceneOrder.TryAdd(e.SceneAssetName, 0);
            mEventComponent.Fire(this, LoadSceneSuccessEventArgs.Create(e));
            RefreshSceneOrder();
        }

        private void OnLoadSceneFailure(object sender, Framework.LoadSceneFailureEventArgs e)
        {
            Log.Warning($"Load scene failure, scene asset name ({e.SceneAssetName}), error message ({e.ErrorMessage}).");
            mEventComponent.Fire(this, LoadSceneFailureEventArgs.Create(e));
        }

        private void OnLoadSceneUpdate(object sender, Framework.LoadSceneUpdateEventArgs e)
        {
            mEventComponent.Fire(this, LoadSceneUpdateEventArgs.Create(e));
        }

        private void OnLoadSceneDependencyAsset(object sender, Framework.LoadSceneDependencyAssetEventArgs e)
        {
            mEventComponent.Fire(this, LoadSceneDependencyAssetEventArgs.Create(e));
        }

        private void OnUnloadSceneSuccess(object sender, Framework.UnloadSceneSuccessEventArgs e)
        {
            mEventComponent.Fire(this, UnloadSceneSuccessEventArgs.Create(e));
            mSceneOrder.Remove(e.SceneAssetName);
            RefreshSceneOrder();
        }

        private void OnUnloadSceneFailure(object sender, Framework.UnloadSceneFailureEventArgs e)
        {
            Log.Warning($"Unload scene failure, scene asset name ({e.SceneAssetName}).");
            mEventComponent.Fire(this, UnloadSceneFailureEventArgs.Create(e));
        }
    }
}