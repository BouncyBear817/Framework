// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 16:45:59
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 资源组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Resource")]
    public sealed class ResourceComponent : FrameworkComponent
    {
        private const int DefaultPriority = 0;
        private const int OneMegaBytes = 1024 * 1024;

        private IResourceManager mResourceManager = null;
        private EventComponent mEventComponent = null;
        private bool mEditorResourceMode = false;
        private bool mForceUnloadUnusedAssets = false;
        private bool mPreorderUnloadUnusedAssets = false;
        private bool mPerformGCCollect = false;
        private AsyncOperation mAsyncOperation = null;
        private float mLastUnloadUnusedAssetsOperationElapseSeconds = 0f;
        private ResourceHelperBase mResourceHelper = null;

        [SerializeField] private ResourceMode mResourceMode = ResourceMode.StandalonePackage;
        [SerializeField] private ReadWritePathType mReadWritePathType = ReadWritePathType.Unspecified;
        [SerializeField] private float mMinUnloadUnusedAssetsInterval = 60f;
        [SerializeField] private float mMaxUnloadUnusedAssetsInterval = 300f;
        [SerializeField] private float mAssetAutoReleaseInterval = 60f;
        [SerializeField] private int mAssetCapacity = 64;
        [SerializeField] private int mAssetPriority = 0;
        [SerializeField] private float mResourceAutoReleaseInterval = 60f;
        [SerializeField] private int mResourceCapacity = 64;
        [SerializeField] private int mResourcePriority = 0;
        [SerializeField] private string mUpdatePrefixUri = null;
        [SerializeField] private int mGenerateReadWriteVersionListLength = OneMegaBytes;
        [SerializeField] private int mUpdateRetryCount = 3;
        [SerializeField] private Transform mInstanceRoot = null;
        [SerializeField] private string mResourceHelperTypeName = "Framework.Runtime.DefaultResourceHelper";
        [SerializeField] private ResourceHelperBase mCustomResourceHelper = null;
        [SerializeField] private string mLoadResourceAgentHelperTypeName = "Framework.Runtime.DefaultLoadResourceAgentHelper";
        [SerializeField] private LoadResourceAgentHelperBase mCustomLoadResourceAgentHelper = null;
        [SerializeField] private int mLoadResourceAgentHelperCount = 3;

        /// <summary>
        /// 只读区地址
        /// </summary>
        public string ReadOnlyPath => mResourceManager.ReadOnlyPath;

        /// <summary>
        /// 读写区地址
        /// </summary>
        public string ReadWritePath => mResourceManager.ReadWritePath;

        /// <summary>
        /// 资源模式
        /// </summary>
        public ResourceMode ResourceMode => mResourceManager.ResourceMode;

        /// <summary>
        /// 读写区路径类型
        /// </summary>
        public ReadWritePathType ReadWritePathType => mReadWritePathType;

        /// <summary>
        /// 获取无用资源释放的等待时长，以秒为单位
        /// </summary>
        public float LastUnloadUnusedAssetsOperationElapseSeconds
        {
            get => mLastUnloadUnusedAssetsOperationElapseSeconds;
            set => mLastUnloadUnusedAssetsOperationElapseSeconds = value;
        }

        /// <summary>
        /// 获取无用资源释放的最小间隔时长，以秒为单位
        /// </summary>
        public float MinUnloadUnusedAssetsInterval
        {
            get => mMinUnloadUnusedAssetsInterval;
            set => mMinUnloadUnusedAssetsInterval = value;
        }

        /// <summary>
        /// 获取无用资源释放的最大间隔时长，以秒为单位
        /// </summary>
        public float MaxUnloadUnusedAssetsInterval
        {
            get => mMaxUnloadUnusedAssetsInterval;
            set => mMaxUnloadUnusedAssetsInterval = value;
        }

        /// <summary>
        /// 当前变体
        /// </summary>
        public string CurrentVariant => mResourceManager.CurrentVariant;

        /// <summary>
        /// 当前资源适用的版号
        /// </summary>
        public string ApplicableVersion => mResourceManager.ApplicableVersion;

        /// <summary>
        /// 当前内部资源版本号
        /// </summary>
        public int InternalResourceVersion => mResourceManager.InternalResourceVersion;

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        public string UpdatePrefixUri
        {
            get => mResourceManager.UpdatePrefixUri;
            set => mResourceManager.UpdatePrefixUri = value;
        }

        /// <summary>
        /// 每更新多少字节的资源，重新生成一次版本资源列表
        /// </summary>
        public int GenerateReadWriteVersionListLength
        {
            get => mResourceManager.GenerateReadWriteVersionListLength;
            set => mResourceManager.GenerateReadWriteVersionListLength = mGenerateReadWriteVersionListLength = value;
        }

        /// <summary>
        /// 正在应用资源包路径
        /// </summary>
        public string ApplyingResourcePackPath => mResourceManager.ApplyingResourcePackPath;

        /// <summary>
        /// 等待应用资源的数量
        /// </summary>
        public int ApplyingWaitingCount => mResourceManager.ApplyingWaitingCount;

        /// <summary>
        /// 资源更新重试次数
        /// </summary>
        public int UpdateRetryCount
        {
            get => mResourceManager.UpdateRetryCount;
            set => mResourceManager.UpdateRetryCount = mUpdateRetryCount = value;
        }

        /// <summary>
        /// 正在更新的资源组
        /// </summary>
        public IResourceGroup UpdatingResourceGroup => mResourceManager.UpdatingResourceGroup;

        /// <summary>
        /// 等待更新资源的数量
        /// </summary>
        public int UpdateWaitingCount => mResourceManager.UpdateWaitingCount;

        /// <summary>
        /// 使用时下载的等待更新资源的数量
        /// </summary>
        public int UpdateWaitingWhilePlayingCount => mResourceManager.UpdateWaitingWhilePlayingCount;

        /// <summary>
        /// 候选更新资源的数量
        /// </summary>
        public int UpdateCandidateCount => mResourceManager.UpdateCandidateCount;

        /// <summary>
        /// 加载资源代理总数量
        /// </summary>
        public int LoadTotalAgentCount => mResourceManager.LoadTotalAgentCount;

        /// <summary>
        /// 可用的加载资源代理的数量
        /// </summary>
        public int LoadAvailableAgentCount => mResourceManager.LoadAvailableAgentCount;

        /// <summary>
        /// 工作中加载资源代理的数量
        /// </summary>
        public int LoadWorkingAgentCount => mResourceManager.LoadWorkingAgentCount;

        /// <summary>
        /// 等待加载资源任务的数量
        /// </summary>
        public int LoadWaitingTaskCount => mResourceManager.LoadWaitingTaskCount;

        /// <summary>
        /// 资源数量
        /// </summary>
        public int AssetCount => mResourceManager.AssetCount;

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get => mResourceManager.AssetAutoReleaseInterval;
            set => mResourceManager.AssetAutoReleaseInterval = mAssetAutoReleaseInterval = value;
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int AssetCapacity
        {
            get => mResourceManager.AssetCapacity;
            set => mResourceManager.AssetCapacity = mAssetCapacity = value;
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int AssetPriority
        {
            get => mResourceManager.AssetPriority;
            set => mResourceManager.AssetPriority = mAssetPriority = value;
        }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int ResourceCount => mResourceManager.ResourceCount;

        /// <summary>
        /// 资源组数量
        /// </summary>
        public int ResourceGroupCount => mResourceManager.ResourceGroupCount;

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get => mResourceManager.ResourceAutoReleaseInterval;
            set => mResourceManager.ResourceAutoReleaseInterval = mResourceAutoReleaseInterval = value;
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int ResourceCapacity
        {
            get => mResourceManager.ResourceCapacity;
            set => mResourceManager.ResourceCapacity = mResourceCapacity = value;
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int ResourcePriority
        {
            get => mResourceManager.ResourcePriority;
            set => mResourceManager.ResourcePriority = mResourcePriority = value;
        }

        /// <summary>
        /// 单机模式版本资源列表序列化器
        /// </summary>
        public PackageVersionListSerializer PackageVersionListSerializer => mResourceManager.PackageVersionListSerializer;

        /// <summary>
        /// 可更新模式版本资源列表序列化器
        /// </summary>
        public UpdatableVersionListSerializer UpdatableVersionListSerializer => mResourceManager.UpdatableVersionListSerializer;

        /// <summary>
        /// 本地只读区版本资源列表序列化器
        /// </summary>
        public ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer => mResourceManager.ReadOnlyVersionListSerializer;

        /// <summary>
        /// 本地读写区版本资源序列化器
        /// </summary>
        public ReadWriteVersionListSerializer ReadWriteVersionListSerializer => mResourceManager.ReadWriteVersionListSerializer;

        /// <summary>
        /// 资源包版本资源列表序列化器
        /// </summary>
        public ResourcePackVersionListSerializer ResourcePackVersionListSerializer => mResourceManager.ResourcePackVersionListSerializer;

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
                return;
            }

            mEditorResourceMode = baseComponent.EditorResourceMode;
            mResourceManager = mEditorResourceMode ? baseComponent.EditorResourceHelper : FrameworkEntry.GetModule<IResourceManager>();
            if (mResourceManager == null)
            {
                Log.Fatal("Resource manager is invalid.");
                return;
            }

            mResourceManager.ResourceVerifyStart += OnResourceVerifyStart;
            mResourceManager.ResourceVerifySuccess += OnResourceVerifySuccess;
            mResourceManager.ResourceVerifyFailure += OnResourceVerifyFailure;
            mResourceManager.ResourceApplyStart += OnResourceApplyStart;
            mResourceManager.ResourceApplySuccess += OnResourceApplySuccess;
            mResourceManager.ResourceApplyFailure += OnResourceApplyFailure;
            mResourceManager.ResourceUpdateStart += OnResourceUpdateStart;
            mResourceManager.ResourceUpdateChanged += OnResourceUpdateChanged;
            mResourceManager.ResourceUpdateSuccess += OnResourceUpdateSuccess;
            mResourceManager.ResourceUpdateFailure += OnResourceUpdateFailure;
            mResourceManager.ResourceUpdateAllComplete += OnResourceUpdateAllComplete;

            mResourceManager.SetReadOnlyPath(Application.streamingAssetsPath);
            if (mReadWritePathType == ReadWritePathType.TemporaryCache)
            {
                mResourceManager.SetReadWritePath(Application.temporaryCachePath);
            }
            else
            {
                if (mReadWritePathType == ReadWritePathType.Unspecified)
                {
                    mReadWritePathType = ReadWritePathType.PersistentData;
                }

                mResourceManager.SetReadWritePath(Application.persistentDataPath);
            }

            if (mEditorResourceMode)
            {
                return;
            }

            SetResourceMode(mResourceMode);
            mResourceManager.SetObjectPoolManager(FrameworkEntry.GetModule<IObjectPoolManager>());
            mResourceManager.SetFileSystemManager(FrameworkEntry.GetModule<IFileSystemManager>());
            mResourceManager.SetDownloadManager(FrameworkEntry.GetModule<IDownloadManager>());
            mResourceManager.AssetAutoReleaseInterval = mAssetAutoReleaseInterval;
            mResourceManager.AssetCapacity = mAssetCapacity;
            mResourceManager.AssetPriority = mAssetPriority;
            mResourceManager.ResourceAutoReleaseInterval = mResourceAutoReleaseInterval;
            mResourceManager.ResourceCapacity = mResourceCapacity;
            mResourceManager.ResourcePriority = mResourcePriority;

            if (mResourceMode == ResourceMode.Updatable || mResourceMode == ResourceMode.UpdatableWhilePlaying)
            {
                mResourceManager.UpdatePrefixUri = mUpdatePrefixUri;
                mResourceManager.GenerateReadWriteVersionListLength = mGenerateReadWriteVersionListLength;
                mResourceManager.UpdateRetryCount = mUpdateRetryCount;
            }

            mResourceHelper = Helper.CreateHelper(mResourceHelperTypeName, mCustomResourceHelper);
            if (mResourceHelper == null)
            {
                Log.Error("Resource helper is invalid.");
                return;
            }

            mResourceHelper.name = "Resource Helper";
            var trans = mResourceHelper.transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;

            mResourceManager.SetResourceHelper(mResourceHelper);

            if (mInstanceRoot == null)
            {
                mInstanceRoot = new GameObject("Load Resource Agent Instances").transform;
                mInstanceRoot.SetParent(transform);
                mInstanceRoot.localScale = Vector3.one;
            }

            for (var i = 0; i < mLoadResourceAgentHelperCount; i++)
            {
                AddLoadResourceAgentHelper(i);
            }
        }

        private void Update()
        {
            mLastUnloadUnusedAssetsOperationElapseSeconds += Time.unscaledDeltaTime;
            if (mAsyncOperation == null && (mForceUnloadUnusedAssets || mLastUnloadUnusedAssetsOperationElapseSeconds >= mMaxUnloadUnusedAssetsInterval ||
                                            mPreorderUnloadUnusedAssets && mLastUnloadUnusedAssetsOperationElapseSeconds >= mMinUnloadUnusedAssetsInterval))
            {
                Log.Info("Unload unused assets...");
                mForceUnloadUnusedAssets = false;
                mPreorderUnloadUnusedAssets = false;
                mLastUnloadUnusedAssetsOperationElapseSeconds = 0f;
                mAsyncOperation = Resources.UnloadUnusedAssets();
            }

            if (mAsyncOperation != null && mAsyncOperation.isDone)
            {
                mAsyncOperation = null;
                if (mPerformGCCollect)
                {
                    Log.Info("GC.Collect...");
                    mPerformGCCollect = false;
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourceMode">资源模式</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            mResourceManager.SetResourceMode(resourceMode);
            switch (resourceMode)
            {
                case ResourceMode.StandalonePackage:
                    mResourceManager.PackageVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V0);
                    mResourceManager.PackageVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V1);
                    mResourceManager.PackageVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V2);
                    break;
                case ResourceMode.Updatable:
                case ResourceMode.UpdatableWhilePlaying:
                    mResourceManager.UpdatableVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V0);
                    mResourceManager.UpdatableVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V1);
                    mResourceManager.UpdatableVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V2);

                    mResourceManager.UpdatableVersionListSerializer.RegisterTryGetValueCallback(0, BuiltinVersionListSerializer.UpdatableVersionListTryGetValueCallback_V0);
                    mResourceManager.UpdatableVersionListSerializer.RegisterTryGetValueCallback(1, BuiltinVersionListSerializer.UpdatableVersionListTryGetValueCallback_V1_V2);
                    mResourceManager.UpdatableVersionListSerializer.RegisterTryGetValueCallback(2, BuiltinVersionListSerializer.UpdatableVersionListTryGetValueCallback_V1_V2);

                    mResourceManager.ReadOnlyVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V0);
                    mResourceManager.ReadOnlyVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V1);
                    mResourceManager.ReadOnlyVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V2);

                    mResourceManager.ReadWriteVersionListSerializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.LocalVersionListSerializeCallback_V0);
                    mResourceManager.ReadWriteVersionListSerializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.LocalVersionListSerializeCallback_V1);
                    mResourceManager.ReadWriteVersionListSerializer.RegisterSerializeCallback(2, BuiltinVersionListSerializer.LocalVersionListSerializeCallback_V2);

                    mResourceManager.ReadWriteVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V0);
                    mResourceManager.ReadWriteVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V1);
                    mResourceManager.ReadWriteVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.LocalVersionListDeserializeCallback_V2);

                    mResourceManager.ResourcePackVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.ResourcePackVersionListDeserializeCallback_V0);
                    break;
            }
        }

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant">当前变体</param>
        public void SetCurrentVariant(string currentVariant)
        {
            mResourceManager.SetCurrentVariant(string.IsNullOrEmpty(currentVariant) ? currentVariant : null);
        }

        /// <summary>
        /// 设置解密资源回调函数
        /// </summary>
        /// <param name="decryptResourceCallback">解密资源回调函数</param>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            mResourceManager.SetDecryptResourceCallback(decryptResourceCallback);
        }

        /// <summary>
        /// 预订执行释放未被使用的资源
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收</param>
        public void UnloadUnusedAssets(bool performGCCollect)
        {
            mPreorderUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                mPerformGCCollect = true;
            }
        }

        /// <summary>
        /// 强制执行释放未被使用的资源
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收</param>
        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            mForceUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                mPerformGCCollect = true;
            }
        }

        /// <summary>
        /// 使用单机模式并初始化资源
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时回调</param>
        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            mResourceManager.InitResources(initResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并检查版本资源列表
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            return mResourceManager.CheckVersionList(latestInternalResourceVersion);
        }

        /// <summary>
        /// 使用可更新模式并更新版本资源列表
        /// </summary>
        /// <param name="versionListInfo">版本资源列表信息</param>
        /// <param name="updateVersionListCallbacks">更新版本资源列表回调函数集</param>
        public void UpdateVersionList(VersionListInfo versionListInfo, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            mResourceManager.UpdateVersionList(versionListInfo, updateVersionListCallbacks);
        }

        /// <summary>
        /// 使用可更新模式并校验资源
        /// </summary>
        /// <param name="verifyResourcesCompleteCallback">校验资源完成回调函数</param>
        public void VerifyResource(VerifyResourcesCompleteCallback verifyResourcesCompleteCallback)
        {
            mResourceManager.VerifyResource(0, verifyResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并校验资源
        /// </summary>
        /// <param name="verifyResourceLengthPerFrame">每帧校验资源大小，以字节为单位</param>
        /// <param name="verifyResourcesCompleteCallback">校验资源完成回调函数</param>
        public void VerifyResource(int verifyResourceLengthPerFrame, VerifyResourcesCompleteCallback verifyResourcesCompleteCallback)
        {
            mResourceManager.VerifyResource(verifyResourceLengthPerFrame, verifyResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数</param>
        public void CheckResource(CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            mResourceManager.CheckResource(false, checkResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若否则移除其它变体的资源</param>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数</param>
        public void CheckResource(bool ignoreOtherVariant, CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            mResourceManager.CheckResource(ignoreOtherVariant, checkResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并应用资源
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="applyResourcesCompleteCallback">使用可更新模式并应用资源完成时的回调函数</param>
        public void ApplyResource(string resourcePackPath, ApplyResourcesCompleteCallback applyResourcesCompleteCallback)
        {
            mResourceManager.ApplyResource(resourcePackPath, applyResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            mResourceManager.UpdateResources(updateResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并更新指定资源组的资源
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(string resourceGroupName, UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            mResourceManager.UpdateResources(resourceGroupName, updateResourcesCompleteCallback);
        }

        /// <summary>
        /// 停止更新资源
        /// </summary>
        public void StopUpdateResource()
        {
            mResourceManager.StopUpdateResource();
        }

        /// <summary>
        /// 校验资源包
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <returns>是否成功校验资源包</returns>
        public bool VerifyResourcePack(string resourcePackPath)
        {
            return mResourceManager.VerifyResourcePack(resourcePackPath);
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <returns>所有加载资源任务的信息</returns>
        public TaskInfo[] GetAllLoadAssetInfos()
        {
            return mResourceManager.GetAllLoadAssetInfos();
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <param name="results">所有加载资源任务的信息</param>
        public void GetAllLoadAssetInfos(List<TaskInfo> results)
        {
            mResourceManager.GetAllLoadAssetInfos(results);
        }

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns>检查资源是否存在的结果</returns>
        public HasAssetResult HasAsset(string assetName)
        {
            return mResourceManager.HasAsset(assetName);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="loadAssetInfo">加载资源的信息</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        public void LoadAsset(LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks)
        {
            mResourceManager.LoadAsset(loadAssetInfo, loadAssetCallbacks);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源</param>
        public void UnloadAsset(object asset)
        {
            mResourceManager.UnloadAsset(asset);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="loadSceneInfo">加载场景信息</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        public void LoadScene(LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks)
        {
            mResourceManager.LoadScene(loadSceneInfo, loadSceneCallbacks);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null)
        {
            mResourceManager.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
        }

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的实际路径</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（非文件系统）中，否则为空</remarks>
        public string GetBinaryPath(string binaryAssetName)
        {
            return mResourceManager.GetBinaryPath(binaryAssetName);
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
            return mResourceManager.GetBinaryPath(binaryAssetName, out storageInReadOnly, out storageInFileSystem, out relativePath, out fileName);
        }

        /// <summary>
        /// 获取二进制资源的长度
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的长度</returns>
        public int GetBinaryLength(string binaryAssetName)
        {
            return mResourceManager.GetBinaryLength(binaryAssetName);
        }

        /// <summary>
        /// 加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                Log.Error("Binary asset name is invalid.");
                return;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                Log.Error($"Binary  asset name ({binaryAssetName}) is invalid.");
                return;
            }

            mResourceManager.LoadBinary(binaryAssetName, loadBinaryCallbacks, userData);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>存储加载二进制资源的二进制流</returns>
        public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                Log.Error("Binary asset name is invalid.");
                return null;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                Log.Error($"Binary  asset name ({binaryAssetName}) is invalid.");
                return null;
            }

            return mResourceManager.LoadBinaryFromFileSystem(binaryAssetName);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return LoadBinaryFromFileSystem(binaryAssetName, buffer, 0, buffer.Length);
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
            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return LoadBinaryFromFileSystem(binaryAssetName, buffer, startIndex, buffer.Length - startIndex);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                Log.Error("Binary asset name is invalid.");
                return 0;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                Log.Error($"Binary asset name ({binaryAssetName}) is invalid.");
                return 0;
            }

            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return mResourceManager.LoadBinaryFromFileSystem(binaryAssetName, buffer, startIndex, length);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length)
        {
            return LoadBinarySegmentFromFileSystem(binaryAssetName, 0, length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                Log.Error("Binary asset name is invalid.");
                return null;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                Log.Error($"Binary asset name ({binaryAssetName}) is invalid.");
                return null;
            }

            return mResourceManager.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, length);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, 0, buffer.Length);
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
            return LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, 0, length);
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
            return LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, startIndex, length);
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
            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, 0, buffer.Length);
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
            return LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, 0, length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                Log.Error("Binary asset name is invalid.");
                return 0;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                Log.Error($"Binary asset name ({binaryAssetName}) is invalid.");
                return 0;
            }

            if (buffer == null)
            {
                Log.Error("Buffer is invalid.");
                return 0;
            }

            return mResourceManager.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, startIndex, length);
        }

        /// <summary>
        /// 检查资源组是否存在
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>资源组是否存在</returns>
        public bool HasResourceGroup(string resourceGroupName)
        {
            return mResourceManager.HasResourceGroup(resourceGroupName);
        }

        /// <summary>
        /// 获取指定资源组
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>指定资源组</returns>
        public IResourceGroup GetResourceGroup(string resourceGroupName = null)
        {
            return mResourceManager.GetResourceGroup(resourceGroupName);
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <returns>所有资源组</returns>
        public IResourceGroup[] GetAllResourceGroups()
        {
            return mResourceManager.GetAllResourceGroups();
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <param name="results">所有资源组</param>
        public void GetAllResourceGroups(List<IResourceGroup> results)
        {
            mResourceManager.GetAllResourceGroups(results);
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(params string[] resourceGroupNames)
        {
            return mResourceManager.GetResourceGroupCollection(resourceGroupNames);
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(List<string> resourceGroupNames)
        {
            return mResourceManager.GetResourceGroupCollection(resourceGroupNames);
        }

        private void AddLoadResourceAgentHelper(int index)
        {
            var loadResourceAgentHelper = Helper.CreateHelper(mLoadResourceAgentHelperTypeName, mCustomLoadResourceAgentHelper);
            if (loadResourceAgentHelper == null)
            {
                Log.Error("Load resource agent helper is invalid.");
                return;
            }

            loadResourceAgentHelper.name = $"Load Resource Agent Helper - {index}";
            var trans = loadResourceAgentHelper.transform;
            trans.SetParent(mInstanceRoot);
            trans.localScale = Vector3.one;

            mResourceManager.AddLoadResourceAgentHelper(loadResourceAgentHelper);
        }


        private void OnResourceVerifyStart(object sender, Framework.ResourceVerifyStartEventArgs e)
        {
            mEventComponent.Fire(this, ResourceVerifyStartEventArgs.Create(e));
        }

        private void OnResourceVerifySuccess(object sender, Framework.ResourceVerifySuccessEventArgs e)
        {
            mEventComponent.Fire(this, ResourceVerifySuccessEventArgs.Create(e));
        }

        private void OnResourceVerifyFailure(object sender, Framework.ResourceVerifyFailureEventArgs e)
        {
            mEventComponent.Fire(this, ResourceVerifyFailureEventArgs.Create(e));
        }

        private void OnResourceApplyStart(object sender, Framework.ResourceApplyStartEventArgs e)
        {
            mEventComponent.Fire(this, ResourceApplyStartEventArgs.Create(e));
        }

        private void OnResourceApplySuccess(object sender, Framework.ResourceApplySuccessEventArgs e)
        {
            mEventComponent.Fire(this, ResourceApplySuccessEventArgs.Create(e));
        }

        private void OnResourceApplyFailure(object sender, Framework.ResourceApplyFailureEventArgs e)
        {
            mEventComponent.Fire(this, ResourceApplyFailureEventArgs.Create(e));
        }

        private void OnResourceUpdateStart(object sender, Framework.ResourceUpdateStartEventArgs e)
        {
            mEventComponent.Fire(this, ResourceUpdateStartEventArgs.Create(e));
        }

        private void OnResourceUpdateChanged(object sender, Framework.ResourceUpdateChangedEventArgs e)
        {
            mEventComponent.Fire(this, ResourceUpdateChangedEventArgs.Create(e));
        }

        private void OnResourceUpdateSuccess(object sender, Framework.ResourceUpdateSuccessEventArgs e)
        {
            mEventComponent.Fire(this, ResourceUpdateSuccessEventArgs.Create(e));
        }

        private void OnResourceUpdateFailure(object sender, Framework.ResourceUpdateFailureEventArgs e)
        {
            mEventComponent.Fire(this, ResourceUpdateFailureEventArgs.Create(e));
        }

        private void OnResourceUpdateAllComplete(object sender, Framework.ResourceUpdateAllCompleteEventArgs e)
        {
            mEventComponent.Fire(this, ResourceUpdateAllCompleteEventArgs.Create(e));
        }
    }
}