// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/13 11:6:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Framework.Runtime
{
    /// <summary>
    /// 编辑器资源组件
    /// </summary>
    [DisallowMultipleComponent]
    public class EditorResourceComponent : MonoBehaviour, IResourceManager
    {
        private const int DefaultPriority = 0;
        private static readonly int AssetsStringLength = "Assets".Length;

        [SerializeField] private bool mEnableCachedAssets = true;
        [SerializeField] private int mLoadAssetCountPerFrame = 1;
        [SerializeField] private float mMinLoadAssetRandomDelaySeconds = 0f;
        [SerializeField] private float mMaxLoadAssetRandomDelaySeconds = 0f;

        private string mReadOnlyPath = null;
        private string mReadWritePath = null;
        private Dictionary<string, Object> mCachedAssets = null;
        private LinkedList<LoadAssetInfo> mLoadAssetInfos = null;
        private LinkedList<LoadSceneInfo> mLoadSceneInfos = null;
        private LinkedList<UnloadSceneInfo> mUnloadSceneInfos = null;

        /// <summary>
        /// 只读区地址
        /// </summary>
        public string ReadOnlyPath => mReadOnlyPath;

        /// <summary>
        /// 读写区地址
        /// </summary>
        public string ReadWritePath => mReadWritePath;

        /// <summary>
        /// 资源模式
        /// </summary>
        public ResourceMode ResourceMode => ResourceMode.Unspecified;

        /// <summary>
        /// 当前变体
        /// </summary>
        public string CurrentVariant => null;

        /// <summary>
        /// 当前资源适用的版号
        /// </summary>
        public string ApplicableVersion => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 当前内部资源版本号
        /// </summary>
        public int InternalResourceVersion => throw new NotSupportedException("InternalResourceVersion");

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        public string UpdatePrefixUri
        {
            get => throw new NotSupportedException("UpdatePrefixUri");
            set => throw new NotSupportedException("UpdatePrefixUri");
        }

        /// <summary>
        /// 每更新多少字节的资源，重新生成一次版本资源列表
        /// </summary>
        public int GenerateReadWriteVersionListLength
        {
            get => throw new NotSupportedException("GenerateReadWriteVersionListLength");
            set => throw new NotSupportedException("GenerateReadWriteVersionListLength");
        }

        /// <summary>
        /// 正在应用资源包路径
        /// </summary>
        public string ApplyingResourcePackPath => throw new NotSupportedException("ApplyingResourcePackPath");

        /// <summary>
        /// 等待应用资源的数量
        /// </summary>
        public int ApplyingWaitingCount => throw new NotSupportedException("ApplyingWaitingCount");

        /// <summary>
        /// 资源更新重试次数
        /// </summary>
        public int UpdateRetryCount
        {
            get => throw new NotSupportedException("UpdateRetryCount");
            set => throw new NotSupportedException("UpdateRetryCount");
        }

        /// <summary>
        /// 正在更新的资源组
        /// </summary>
        public IResourceGroup UpdatingResourceGroup => throw new NotSupportedException("UpdatingResourceGroup");

        /// <summary>
        /// 等待更新资源的数量
        /// </summary>
        public int UpdateWaitingCount => throw new NotSupportedException("UpdateWaitingCount");

        /// <summary>
        /// 使用时下载的等待更新资源的数量
        /// </summary>
        public int UpdateWaitingWhilePlayingCount => throw new NotSupportedException("UpdateWaitingWhilePlayingCount");

        /// <summary>
        /// 候选更新资源的数量
        /// </summary>
        public int UpdateCandidateCount => throw new NotSupportedException("UpdateCandidateCount");

        /// <summary>
        /// 加载资源代理总数量
        /// </summary>
        public int LoadTotalAgentCount => throw new NotSupportedException("LoadTotalAgentCount");

        /// <summary>
        /// 可用的加载资源代理的数量
        /// </summary>
        public int LoadAvailableAgentCount => throw new NotSupportedException("LoadAvailableAgentCount");

        /// <summary>
        /// 工作中加载资源代理的数量
        /// </summary>
        public int LoadWorkingAgentCount => throw new NotSupportedException("LoadWorkingAgentCount");

        /// <summary>
        /// 等待加载资源任务的数量
        /// </summary>
        public int LoadWaitingTaskCount => mLoadAssetInfos.Count;

        /// <summary>
        /// 资源数量
        /// </summary>
        public int AssetCount => throw new NotSupportedException("AssetCount");

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get => throw new NotSupportedException("AssetAutoReleaseInterval");
            set => throw new NotSupportedException("AssetAutoReleaseInterval");
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int AssetCapacity
        {
            get => throw new NotSupportedException("AssetCapacity");
            set => throw new NotSupportedException("AssetCapacity");
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int AssetPriority
        {
            get => throw new NotSupportedException("AssetPriority");
            set => throw new NotSupportedException("AssetPriority");
        }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int ResourceCount => throw new NotSupportedException("ResourceCount");

        /// <summary>
        /// 资源组数量
        /// </summary>
        public int ResourceGroupCount => throw new NotSupportedException("ResourceGroupCount");

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get => throw new NotSupportedException("ResourceAutoReleaseInterval");
            set => throw new NotSupportedException("ResourceAutoReleaseInterval");
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int ResourceCapacity
        {
            get => throw new NotSupportedException("ResourceCapacity");
            set => throw new NotSupportedException("ResourceCapacity");
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int ResourcePriority
        {
            get => throw new NotSupportedException("ResourcePriority");
            set => throw new NotSupportedException("ResourcePriority");
        }

        /// <summary>
        /// 单机模式版本资源列表序列化器
        /// </summary>
        public PackageVersionListSerializer PackageVersionListSerializer => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 可更新模式版本资源列表序列化器
        /// </summary>
        public UpdatableVersionListSerializer UpdatableVersionListSerializer => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 本地只读区版本资源列表序列化器
        /// </summary>
        public ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 本地读写区版本资源序列化器
        /// </summary>
        public ReadWriteVersionListSerializer ReadWriteVersionListSerializer => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 资源包版本资源列表序列化器
        /// </summary>
        public ResourcePackVersionListSerializer ResourcePackVersionListSerializer => throw new NotSupportedException("ApplicableVersion");

        /// <summary>
        /// 资源校验开始事件
        /// </summary>
        public event EventHandler<Framework.ResourceVerifyStartEventArgs> ResourceVerifyStart;

        /// <summary>
        /// 资源校验成功事件
        /// </summary>
        public event EventHandler<Framework.ResourceVerifySuccessEventArgs> ResourceVerifySuccess;

        /// <summary>
        /// 资源校验失败事件
        /// </summary>
        public event EventHandler<Framework.ResourceVerifyFailureEventArgs> ResourceVerifyFailure;

        /// <summary>
        /// 资源更新开始事件
        /// </summary>
        public event EventHandler<Framework.ResourceUpdateStartEventArgs> ResourceUpdateStart;

        /// <summary>
        /// 资源更新改变事件
        /// </summary>
        public event EventHandler<Framework.ResourceUpdateChangedEventArgs> ResourceUpdateChanged;

        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        public event EventHandler<Framework.ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;

        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        public event EventHandler<Framework.ResourceUpdateFailureEventArgs> ResourceUpdateFailure;

        /// <summary>
        /// 资源更新全部完成事件
        /// </summary>
        public event EventHandler<Framework.ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;

        /// <summary>
        /// 资源应用开始事件
        /// </summary>
        public event EventHandler<Framework.ResourceApplyStartEventArgs> ResourceApplyStart;

        /// <summary>
        /// 资源应用成功事件
        /// </summary>
        public event EventHandler<Framework.ResourceApplySuccessEventArgs> ResourceApplySuccess;

        /// <summary>
        /// 资源应用失败事件
        /// </summary>
        public event EventHandler<Framework.ResourceApplyFailureEventArgs> ResourceApplyFailure;

        private void Awake()
        {
            mReadOnlyPath = null;
            mReadWritePath = null;
            mCachedAssets = new Dictionary<string, Object>(StringComparer.Ordinal);
            mLoadAssetInfos = new LinkedList<LoadAssetInfo>();
            mLoadSceneInfos = new LinkedList<LoadSceneInfo>();
            mUnloadSceneInfos = new LinkedList<UnloadSceneInfo>();

            var baseComponent = GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Error("Can not find base component.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                baseComponent.EditorResourceHelper = this;
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (mLoadAssetInfos.Count > 0)
            {
                var count = 0;
                var current = mLoadAssetInfos.First;
                while (current != null && count < mLoadAssetCountPerFrame)
                {
                    var loadAssetInfo = current.Value;
                    var elapseSeconds = (float)(DateTime.UtcNow - loadAssetInfo.StartTime).TotalSeconds;
                    if (elapseSeconds >= loadAssetInfo.DelaySeconds)
                    {
                        var asset = GetCachedAsset(loadAssetInfo.AssetName);
                        if (asset == null)
                        {
#if UNITY_EDITOR
                            if (loadAssetInfo.AssetType != null)
                            {
                                asset = UnityEditor.AssetDatabase.LoadAssetAtPath(loadAssetInfo.AssetName, loadAssetInfo.AssetType);
                            }
                            else
                            {
                                asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(loadAssetInfo.AssetName);
                            }

                            if (mEnableCachedAssets && asset != null)
                            {
                                mCachedAssets.Add(loadAssetInfo.AssetName, asset);
                            }
#endif
                        }

                        if (asset != null)
                        {
                            loadAssetInfo.LoadAssetCallbacks?.LoadAssetSuccessCallback?.Invoke(loadAssetInfo.AssetName, asset, elapseSeconds, loadAssetInfo.UserData);
                        }
                        else
                        {
                            loadAssetInfo.LoadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(loadAssetInfo.AssetName, LoadResourceStatus.AssetError,
                                "Can not load this asset form asset database.", loadAssetInfo.UserData);
                        }

                        var next = current.Next;
                        mLoadAssetInfos.Remove(loadAssetInfo);
                        current = next;
                        count++;
                    }
                }
            }

            if (mLoadSceneInfos.Count > 0)
            {
                var current = mLoadSceneInfos.First;
                while (current != null)
                {
                    var loadSceneInfo = current.Value;
                    if (loadSceneInfo.AsyncOperation.isDone)
                    {
                        if (loadSceneInfo.AsyncOperation.allowSceneActivation)
                        {
                            var elapseSeconds = (float)(DateTime.UtcNow - loadSceneInfo.StartTime).TotalSeconds;
                            loadSceneInfo.LoadSceneCallbacks?.LoadSceneSuccessCallback?.Invoke(loadSceneInfo.SceneAssetName, elapseSeconds, loadSceneInfo.UserData);
                        }
                        else
                        {
                            loadSceneInfo.LoadSceneCallbacks?.LoadSceneFailureCallback?.Invoke(loadSceneInfo.SceneAssetName, LoadResourceStatus.AssetError,
                                "Can not load this scene form asset database", loadSceneInfo.UserData);
                        }

                        var next = current.Next;
                        mLoadSceneInfos.Remove(loadSceneInfo);
                        current = next;
                    }
                    else
                    {
                        loadSceneInfo.LoadSceneCallbacks?.LoadSceneUpdateCallback?.Invoke(loadSceneInfo.SceneAssetName, loadSceneInfo.AsyncOperation.progress,
                            loadSceneInfo.UserData);

                        current = current.Next;
                    }
                }
            }

            if (mUnloadSceneInfos.Count > 0)
            {
                var current = mUnloadSceneInfos.First;
                while (current != null)
                {
                    var UnloadSceneInfo = current.Value;
                    if (UnloadSceneInfo.AsyncOperation.isDone)
                    {
                        if (UnloadSceneInfo.AsyncOperation.allowSceneActivation)
                        {
                            UnloadSceneInfo.UnloadSceneCallbacks?.UnloadSceneSuccessCallback?.Invoke(UnloadSceneInfo.SceneAssetName, UnloadSceneInfo.UserData);
                        }
                        else
                        {
                            UnloadSceneInfo.UnloadSceneCallbacks?.UnloadSceneFailureCallback?.Invoke(UnloadSceneInfo.SceneAssetName, UnloadSceneInfo.UserData);
                        }

                        var next = current.Next;
                        mUnloadSceneInfos.Remove(UnloadSceneInfo);
                        current = next;
                    }
                    else
                    {
                        current = current.Next;
                    }
                }
            }
        }

        /// <summary>
        /// 设置只读区路径
        /// </summary>
        /// <param name="readOnlyPath">只读区路径</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                Log.Error("Read-only path is invalid.");
                return;
            }

            mReadOnlyPath = readOnlyPath;
        }

        /// <summary>
        /// 设置读写区路径
        /// </summary>
        /// <param name="readWritePath">读写区路径</param>
        public void SetReadWritePath(string readWritePath)
        {
            if (string.IsNullOrEmpty(readWritePath))
            {
                Log.Error("Read-write path is invalid.");
                return;
            }

            mReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourceMode">资源模式</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            throw new NotSupportedException("SetResourceMode");
        }

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant">当前变体</param>
        public void SetCurrentVariant(string currentVariant)
        {
            throw new NotSupportedException("SetCurrentVariant");
        }

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            throw new NotSupportedException("SetObjectPoolManager");
        }

        /// <summary>
        /// 设置文件系统管理器
        /// </summary>
        /// <param name="fileSystemManager">文件系统管理器</param>
        public void SetFileSystemManager(IFileSystemManager fileSystemManager)
        {
            throw new NotSupportedException("SetFileSystemManager");
        }

        /// <summary>
        /// 设置下载管理器
        /// </summary>
        /// <param name="downloadManager">下载管理器</param>
        public void SetDownloadManager(IDownloadManager downloadManager)
        {
            throw new NotSupportedException("SetDownloadManager");
        }

        /// <summary>
        /// 设置解密资源回调函数
        /// </summary>
        /// <param name="decryptResourceCallback">解密资源回调函数</param>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            throw new NotSupportedException("SetDecryptResourceCallback");
        }

        /// <summary>
        /// 加载资源辅助器
        /// </summary>
        /// <param name="resourceHelper">资源辅助器</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            throw new NotSupportedException("SetResourceHelper");
        }

        /// <summary>
        /// 增加加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            throw new NotSupportedException("AddLoadResourceAgentHelper");
        }

        /// <summary>
        /// 使用单机模式并初始化资源
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时回调</param>
        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            throw new NotSupportedException("InitResources");
        }

        /// <summary>
        /// 使用可更新模式并检查版本资源列表
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            throw new NotSupportedException("CheckVersionList");
        }

        /// <summary>
        /// 使用可更新模式并更新版本资源列表
        /// </summary>
        /// <param name="versionListInfo">版本资源列表信息</param>
        /// <param name="updateVersionListCallbacks">更新版本资源列表回调函数集</param>
        public void UpdateVersionList(VersionListInfo versionListInfo, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            throw new NotSupportedException("UpdateVersionList");
        }

        /// <summary>
        /// 使用可更新模式并校验资源
        /// </summary>
        /// <param name="verifyResourceLengthPerFrame">每帧校验资源大小，以字节为单位</param>
        /// <param name="verifyResourcesCompleteCallback">校验资源完成回调函数</param>
        public void VerifyResource(int verifyResourceLengthPerFrame, VerifyResourcesCompleteCallback verifyResourcesCompleteCallback)
        {
            throw new NotSupportedException("VerifyResource");
        }

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若否则移除其它变体的资源</param>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数</param>
        public void CheckResource(bool ignoreOtherVariant, CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            throw new NotSupportedException("CheckResource");
        }

        /// <summary>
        /// 使用可更新模式并应用资源
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="applyResourcesCompleteCallback">使用可更新模式并应用资源完成时的回调函数</param>
        public void ApplyResource(string resourcePackPath, ApplyResourcesCompleteCallback applyResourcesCompleteCallback)
        {
            throw new NotSupportedException("ApplyResource");
        }

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            throw new NotSupportedException("UpdateResources");
        }

        /// <summary>
        /// 使用可更新模式并更新指定资源组的资源
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(string resourceGroupName, UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            throw new NotSupportedException("UpdateResources");
        }

        /// <summary>
        /// 停止更新资源
        /// </summary>
        public void StopUpdateResource()
        {
            throw new NotSupportedException("StopUpdateResource");
        }

        /// <summary>
        /// 校验资源包
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <returns>是否成功校验资源包</returns>
        public bool VerifyResourcePack(string resourcePackPath)
        {
            throw new NotSupportedException("VerifyResourcePack");
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <returns>所有加载资源任务的信息</returns>
        public TaskInfo[] GetAllLoadAssetInfos()
        {
            throw new NotSupportedException("GetAllLoadAssetInfos");
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <param name="results">所有加载资源任务的信息</param>
        public void GetAllLoadAssetInfos(List<TaskInfo> results)
        {
            throw new NotSupportedException("GetAllLoadAssetInfos");
        }

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns>检查资源是否存在的结果</returns>
        public HasAssetResult HasAsset(string assetName)
        {
#if UNITY_EDITOR
            var obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetName);
            if (obj == null)
            {
                return HasAssetResult.NotExist;
            }

            var result = obj.GetType() == typeof(UnityEditor.DefaultAsset) ? HasAssetResult.BinaryOnDisk : HasAssetResult.AssetOnDisk;
            obj = null;
            UnityEditor.EditorUtility.UnloadUnusedAssetsImmediate();
            return result;
#else
            return HasAssetResult.NotExist;
#endif
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="loadAssetInfo">加载资源的信息</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        public void LoadAsset(Framework.LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (loadAssetCallbacks == null)
            {
                Log.Error("Load asset callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(loadAssetInfo.AssetName))
            {
                loadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(loadAssetInfo.AssetName, LoadResourceStatus.NotExist, "Asset name is invalid.",
                    loadAssetInfo.UserData);
                return;
            }

            if (string.IsNullOrEmpty(loadAssetInfo.AssetName) || !loadAssetInfo.AssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                loadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(loadAssetInfo.AssetName, LoadResourceStatus.NotExist, $"Asset name ({loadAssetInfo.AssetName}) is invalid.",
                    loadAssetInfo.UserData);
                return;
            }

            if (!HasFile(loadAssetInfo.AssetName))
            {
                loadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(loadAssetInfo.AssetName, LoadResourceStatus.NotExist, $"Asset name ({loadAssetInfo.AssetName}) is not exist.",
                    loadAssetInfo.UserData);
                return;
            }

            mLoadAssetInfos.AddLast(new LoadAssetInfo(loadAssetInfo.AssetName, loadAssetInfo.AssetType, loadAssetInfo.Priority, DateTime.UtcNow,
                mMinLoadAssetRandomDelaySeconds + (float)Utility.Random.GetRandomDouble() * (mMaxLoadAssetRandomDelaySeconds - mMinLoadAssetRandomDelaySeconds), loadAssetCallbacks,
                loadAssetInfo.UserData));
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源</param>
        public void UnloadAsset(object asset)
        {
            //Do nothing in editor resource mode.
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="loadSceneInfo">加载场景信息</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        public void LoadScene(Framework.LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks)
        {
            if (loadSceneCallbacks == null)
            {
                Log.Error("Load scene callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(loadSceneInfo.SceneAssetName))
            {
                loadSceneCallbacks?.LoadSceneFailureCallback?.Invoke(loadSceneInfo.SceneAssetName, LoadResourceStatus.NotExist,
                    "Scene asset name is invalid.",
                    loadSceneInfo.UserData);
                return;
            }

            if (!loadSceneInfo.SceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) ||
                !loadSceneInfo.SceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                loadSceneCallbacks?.LoadSceneFailureCallback?.Invoke(loadSceneInfo.SceneAssetName, LoadResourceStatus.NotExist,
                    $"Scene asset name ({loadSceneInfo.SceneAssetName}) is invalid.",
                    loadSceneInfo.UserData);
                return;
            }

            if (!HasFile(loadSceneInfo.SceneAssetName))
            {
                loadSceneCallbacks?.LoadSceneFailureCallback?.Invoke(loadSceneInfo.SceneAssetName, LoadResourceStatus.NotExist,
                    $"Scene asset name ({loadSceneInfo.SceneAssetName}) is not exist.",
                    loadSceneInfo.UserData);
                return;
            }

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadSceneInfo.SceneAssetName, LoadSceneMode.Additive);
            if (asyncOperation == null)
            {
                return;
            }

            mLoadSceneInfos.AddLast(new LoadSceneInfo(asyncOperation, loadSceneInfo.SceneAssetName, loadSceneInfo.Priority, DateTime.UtcNow, loadSceneCallbacks,
                loadSceneInfo.UserData));
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                Log.Error("Scene asset name is invalid.");
                return;
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) ||
                !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                Log.Error($"Scene asset name ({sceneAssetName}) is invalid.");
                return;
            }

            if (unloadSceneCallbacks == null)
            {
                Log.Error("Unload scene callbacks is invalid.");
                return;
            }

            if (!HasFile(sceneAssetName))
            {
                Log.Error($"Scene ({sceneAssetName}) is not exist.");
                return;
            }

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneAssetName);
            if (asyncOperation == null)
            {
                return;
            }

            mUnloadSceneInfos.AddLast(new UnloadSceneInfo(asyncOperation, sceneAssetName, unloadSceneCallbacks, userData));
        }

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的实际路径</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（非文件系统）中，否则为空</remarks>
        public string GetBinaryPath(string binaryAssetName)
        {
            if (!HasFile(binaryAssetName))
            {
                return null;
            }

            return Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + binaryAssetName;
        }

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="storageInReadOnly">二进制资源是否存储在只读区内</param>
        /// <param name="storageInFileSystem">二进制资源是否存储在文件系统内</param>
        /// <param name="relativePath">文件系统相对于只读区或读写区的相对路径</param>
        /// <param name="fileName">二进制资源在文件系统中的名称（仅用于存储在文件系统中，否则为空）</param>
        /// <returns>是否成功获取二进制资源的实际路径</returns>
        public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName)
        {
            throw new NotSupportedException("GetBinaryPath");
        }

        /// <summary>
        /// 获取二进制资源的长度
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的长度</returns>
        public int GetBinaryLength(string binaryAssetName)
        {
            var binaryPath = GetBinaryPath(binaryAssetName);
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                return -1;
            }

            return (int)new System.IO.FileInfo(binaryPath).Length;
        }

        /// <summary>
        /// 加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData = null)
        {
            throw new NotSupportedException("LoadBinary");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>存储加载二进制资源的二进制流</returns>
        public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 检查资源组是否存在
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>资源组是否存在</returns>
        public bool HasResourceGroup(string resourceGroupName)
        {
            throw new NotSupportedException("HasResourceGroup");
        }

        /// <summary>
        /// 获取指定资源组
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>指定资源组</returns>
        public IResourceGroup GetResourceGroup(string resourceGroupName = null)
        {
            throw new NotSupportedException("GetResourceGroup");
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <returns>所有资源组</returns>
        public IResourceGroup[] GetAllResourceGroups()
        {
            throw new NotSupportedException("GetAllResourceGroups");
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <param name="results">所有资源组</param>
        public void GetAllResourceGroups(List<IResourceGroup> results)
        {
            throw new NotSupportedException("GetAllResourceGroups");
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(params string[] resourceGroupNames)
        {
            throw new NotSupportedException("GetResourceGroupCollection");
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(List<string> resourceGroupNames)
        {
            throw new NotSupportedException("GetResourceGroupCollection");
        }

        private bool HasFile(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            if (HasCachedAsset(assetName))
            {
                return true;
            }

            var assetFullName = Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + assetName;
            if (string.IsNullOrEmpty(assetFullName))
            {
                return false;
            }

            var splitedAssetFullName = assetFullName.Split('/');
            var currentPath = Path.GetPathRoot(assetFullName);
            for (int i = 0; i < splitedAssetFullName.Length - 1; i++)
            {
                var directoryNames = Directory.GetDirectories(currentPath, splitedAssetFullName[i]);
                if (directoryNames.Length != 1)
                {
                    return false;
                }

                currentPath = directoryNames[0];
            }

            var fileNames = Directory.GetFiles(currentPath, splitedAssetFullName[splitedAssetFullName.Length - 1]);
            if (fileNames.Length != 1)
            {
                return false;
            }

            var fileFullName = Utility.Path.GetRegularPath(fileNames[0]);
            if (fileFullName == null)
            {
                return false;
            }

            if (assetFullName != fileFullName)
            {
                if (assetFullName.ToLowerInvariant() == fileFullName.ToLowerInvariant())
                {
                    Log.Warning(
                        $"The real path of the specific asset ({assetName}) is Assets({fileFullName.Substring(Application.dataPath.Length)}). Check the case of letters in the path.");
                }

                return false;
            }

            return true;
        }

        private bool HasCachedAsset(string assetName)
        {
            if (!mEnableCachedAssets)
            {
                return false;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            return mCachedAssets.ContainsKey(assetName);
        }

        private Object GetCachedAsset(string assetName)
        {
            if (!mEnableCachedAssets)
            {
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return null;
            }

            return mCachedAssets.GetValueOrDefault(assetName);
        }

        private struct LoadAssetInfo
        {
            private readonly string mAssetName;
            private readonly Type mAssetType;
            private readonly int mPriority;
            private readonly DateTime mStartTime;
            private readonly float mDelaySeconds;
            private readonly LoadAssetCallbacks mLoadAssetCallbacks;
            private readonly object mUserData;

            public LoadAssetInfo(string assetName, Type assetType, int priority, DateTime startTime, float delaySeconds, LoadAssetCallbacks loadAssetCallbacks, object userData)
            {
                mAssetName = assetName;
                mAssetType = assetType;
                mPriority = priority;
                mStartTime = startTime;
                mDelaySeconds = delaySeconds;
                mLoadAssetCallbacks = loadAssetCallbacks;
                mUserData = userData;
            }

            public string AssetName => mAssetName;

            public Type AssetType => mAssetType;

            public int Priority => mPriority;

            public DateTime StartTime => mStartTime;

            public float DelaySeconds => mDelaySeconds;

            public LoadAssetCallbacks LoadAssetCallbacks => mLoadAssetCallbacks;

            public object UserData => mUserData;
        }

        private struct LoadSceneInfo
        {
            private readonly AsyncOperation mAsyncOperation;
            private readonly string mSceneAssetName;
            private readonly int mPriority;
            private readonly DateTime mStartTime;
            private readonly LoadSceneCallbacks mLoadSceneCallbacks;
            private readonly object mUserData;

            public LoadSceneInfo(AsyncOperation asyncOperation, string sceneAssetName, int priority, DateTime startTime, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                mAsyncOperation = asyncOperation;
                mSceneAssetName = sceneAssetName;
                mPriority = priority;
                mStartTime = startTime;
                mLoadSceneCallbacks = loadSceneCallbacks;
                mUserData = userData;
            }

            public AsyncOperation AsyncOperation => mAsyncOperation;

            public string SceneAssetName => mSceneAssetName;

            public int Priority => mPriority;

            public DateTime StartTime => mStartTime;

            public LoadSceneCallbacks LoadSceneCallbacks => mLoadSceneCallbacks;

            public object UserData => mUserData;
        }

        public struct UnloadSceneInfo
        {
            private readonly AsyncOperation mAsyncOperation;
            private readonly string mSceneAssetName;
            private readonly UnloadSceneCallbacks mUnloadSceneCallbacks;
            private readonly object mUserData;

            public UnloadSceneInfo(AsyncOperation asyncOperation, string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
            {
                mAsyncOperation = asyncOperation;
                mSceneAssetName = sceneAssetName;
                mUnloadSceneCallbacks = unloadSceneCallbacks;
                mUserData = userData;
            }

            public AsyncOperation AsyncOperation => mAsyncOperation;

            public string SceneAssetName => mSceneAssetName;

            public UnloadSceneCallbacks UnloadSceneCallbacks => mUnloadSceneCallbacks;

            public object UserData => mUserData;
        }
    }
}