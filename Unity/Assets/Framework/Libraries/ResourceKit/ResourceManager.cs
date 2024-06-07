using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private const string RemoteVersionListFileName = "FrameworkVersion.dat";
        private const string LocalVersionListFileName = "FrameworkList.dat";
        private const string DefaultExtension = "dat";
        private const string TempExtension = "tmp";
        private const int FileSystemMaxFileCount = 1024 * 16;
        private const int FileSystemMaxBlockCount = 1024 * 256;

        private Dictionary<string, AssetInfo> mAssetInfos;
        private Dictionary<ResourceName, ResourceInfo> mResourceInfos;
        private SortedDictionary<ResourceName, ReadWriteResourceInfo> mReadWriteResourceInfos;
        private readonly Dictionary<string, IFileSystem> mReadOnlyFileSystems;
        private readonly Dictionary<string, IFileSystem> mReadWriteFileSystems;
        private readonly Dictionary<string, ResourceGroup> mResourceGroups;

        private PackageVersionListSerializer mPackageVersionListSerializer;
        private UpdatableVersionListSerializer mUpdatableVersionListSerializer;
        private ReadOnlyVersionListSerializer mReadOnlyVersionListSerializer;
        private ReadWriteVersionListSerializer mReadWriteVersionListSerializer;
        private ResourcePackVersionListSerializer mResourcePackVersionListSerializer;

        private IFileSystemManager mFileSystemManager;
        private ResourceInitializer mResourceInitializer;
        private VersionListProcessor mVersionListProcessor;
        private ResourceVerifier mResourceVerifier;
        private ResourceChecker mResourceChecker;
        private ResourceUpdater mResourceUpdater;
        private ResourceLoader mResourceLoader;
        private IResourceHelper mResourceHelper;

        private string mReadOnlyPath;
        private string mReadWritePath;
        private ResourceMode mResourceMode;
        private bool mRefuseSetFlag;
        private string mCurrentVariant;
        private string mUpdatePrefixUri;
        private string mApplicableVersion;
        private int mInternalResourceVersion;
        private MemoryStream mCachedSteam;

        private DecryptResourceCallback mDecryptResourceCallback;
        private InitResourcesCompleteCallback mInitResourcesCompleteCallback;
        private UpdateVersionListCallbacks mUpdateVersionListCallbacks;
        private VerifyResourcesCompleteCallback mVerifyResourcesCompleteCallback;
        private CheckResourcesCompleteCallback mCheckResourcesCompleteCallback;
        private ApplyResourcesCompleteCallback mApplyResourcesCompleteCallback;
        private UpdateResourcesCompleteCallback mUpdateResourcesCompleteCallback;

        private EventHandler<ResourceVerifyStartEventArgs> mResourceVerifyStartEventHandler;
        private EventHandler<ResourceVerifySuccessEventArgs> mResourceVerifySuccessEventHandler;
        private EventHandler<ResourceVerifyFailureEventArgs> mResourceVerifyFailureEventHandler;
        private EventHandler<ResourceApplyStartEventArgs> mResourceApplyStartEventHandler;
        private EventHandler<ResourceApplySuccessEventArgs> mResourceApplySuccessEventHandler;
        private EventHandler<ResourceApplyFailureEventArgs> mResourceApplyFailureEventHandler;
        private EventHandler<ResourceUpdateStartEventArgs> mResourceUpdateStartEventHandler;
        private EventHandler<ResourceUpdateChangedEventArgs> mResourceUpdateChangedEventHandler;
        private EventHandler<ResourceUpdateSuccessEventArgs> mResourceUpdateSuccessEventHandler;
        private EventHandler<ResourceUpdateFailureEventArgs> mResourceUpdateFailureEventHandler;
        private EventHandler<ResourceUpdateAllCompleteEventArgs> mResourceUpdateAllCompleteEventHandler;

        public ResourceManager()
        {
            mAssetInfos = null;
            mResourceInfos = null;
            mReadWriteResourceInfos = null;
            mReadOnlyFileSystems = new Dictionary<string, IFileSystem>(StringComparer.Ordinal);
            mReadWriteFileSystems = new Dictionary<string, IFileSystem>(StringComparer.Ordinal);
            mResourceGroups = new Dictionary<string, ResourceGroup>(StringComparer.Ordinal);

            mPackageVersionListSerializer = null;
            mUpdatableVersionListSerializer = null;
            mReadOnlyVersionListSerializer = null;
            mReadWriteVersionListSerializer = null;
            mResourcePackVersionListSerializer = null;

            mFileSystemManager = null;
            mResourceInitializer = null;
            mVersionListProcessor = null;
            mResourceVerifier = null;
            mResourceChecker = null;
            mResourceUpdater = null;
            mResourceLoader = new ResourceLoader(this);
            mResourceHelper = null;

            mReadOnlyPath = null;
            mReadWritePath = null;
            mResourceMode = ResourceMode.Unspecified;
            mRefuseSetFlag = false;
            mCurrentVariant = null;
            mUpdatePrefixUri = null;
            mApplicableVersion = null;
            mInternalResourceVersion = 0;
            mCachedSteam = null;

            mDecryptResourceCallback = null;
            mInitResourcesCompleteCallback = null;
            mUpdateVersionListCallbacks = null;
            mVerifyResourcesCompleteCallback = null;
            mCheckResourcesCompleteCallback = null;
            mApplyResourcesCompleteCallback = null;
            mUpdateResourcesCompleteCallback = null;

            mResourceVerifyStartEventHandler = null;
            mResourceVerifySuccessEventHandler = null;
            mResourceVerifyFailureEventHandler = null;
            mResourceApplyStartEventHandler = null;
            mResourceApplySuccessEventHandler = null;
            mResourceApplyFailureEventHandler = null;
            mResourceUpdateStartEventHandler = null;
            mResourceUpdateChangedEventHandler = null;
            mResourceUpdateSuccessEventHandler = null;
            mResourceUpdateFailureEventHandler = null;
            mResourceUpdateAllCompleteEventHandler = null;
        }

        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public override int Priority => 3;

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
        public ResourceMode ResourceMode => mResourceMode;

        /// <summary>
        /// 当前变体
        /// </summary>
        public string CurrentVariant => mCurrentVariant;

        /// <summary>
        /// 当前资源适用的版号
        /// </summary>
        public string ApplicableVersion => mApplicableVersion;

        /// <summary>
        /// 当前内部资源版本号
        /// </summary>
        public int InternalResourceVersion => mInternalResourceVersion;

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        public string UpdatePrefixUri
        {
            get => mUpdatePrefixUri;
            set => mUpdatePrefixUri = value;
        }

        /// <summary>
        /// 每更新多少字节的资源，重新生成一次版本资源列表
        /// </summary>
        public int GenerateReadWriteVersionListLength
        {
            get => mResourceUpdater != null ? mResourceUpdater.GenerateReadWriteVersionListLength : 0;
            set
            {
                if (mResourceUpdater == null)
                {
                    throw new Exception("You can not use GenerateReadWriteVersionListLength at this time.");
                }

                mResourceUpdater.GenerateReadWriteVersionListLength = value;
            }
        }

        /// <summary>
        /// 正在应用资源包路径
        /// </summary>
        public string ApplyingResourcePackPath => mResourceUpdater != null ? mResourceUpdater.ApplyingResourcePackPath : null;

        /// <summary>
        /// 等待应用资源的数量
        /// </summary>
        public int ApplyingWaitingCount => mResourceUpdater != null ? mResourceUpdater.ApplyWaitingCount : 0;

        /// <summary>
        /// 资源更新重试次数
        /// </summary>
        public int UpdateRetryCount
        {
            get => mResourceUpdater != null ? mResourceUpdater.UpdateRetryCount : 0;
            set
            {
                if (mResourceUpdater == null)
                {
                    throw new Exception("You can not use GenerateReadWriteVersionListLength at this time.");
                }

                mResourceUpdater.UpdateRetryCount = value;
            }
        }

        /// <summary>
        /// 正在更新的资源组
        /// </summary>
        public IResourceGroup UpdatingResourceGroup => mResourceUpdater != null ? mResourceUpdater.UpdatingResourceGroup : null;

        /// <summary>
        /// 等待更新资源的数量
        /// </summary>
        public int UpdateWaitingCount => mResourceUpdater != null ? mResourceUpdater.UpdateWaitingCount : 0;

        /// <summary>
        /// 使用时下载的等待更新资源的数量
        /// </summary>
        public int UpdateWaitingWhilePlayingCount => mResourceUpdater != null ? mResourceUpdater.UpdateWaitingWhilePlayingCount : 0;

        /// <summary>
        /// 候选更新资源的数量
        /// </summary>
        public int UpdateCandidateCount => mResourceUpdater != null ? mResourceUpdater.UpdateCandidateCount : 0;

        /// <summary>
        /// 加载资源代理总数量
        /// </summary>
        public int LoadTotalAgentCount => mResourceLoader.TotalAgentCount;

        /// <summary>
        /// 可用的加载资源代理的数量
        /// </summary>
        public int LoadAvailableAgentCount => mResourceLoader.AvailableAgentCount;

        /// <summary>
        /// 工作中加载资源代理的数量
        /// </summary>
        public int LoadWorkingAgentCount => mResourceLoader.WorkingAgentCount;

        /// <summary>
        /// 等待加载资源任务的数量
        /// </summary>
        public int LoadWaitingTaskCount => mResourceLoader.WaitingTaskCount;

        /// <summary>
        /// 资源数量
        /// </summary>
        public int AssetCount => mAssetInfos != null ? mAssetInfos.Count : 0;

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get => mResourceLoader.AssetAutoReleaseInterval;
            set => mResourceLoader.AssetAutoReleaseInterval = value;
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int AssetCapacity
        {
            get => mResourceLoader.AssetCapacity;
            set => mResourceLoader.AssetCapacity = value;
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int AssetPriority
        {
            get => mResourceLoader.AssetPriority;
            set => mResourceLoader.AssetPriority = value;
        }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int ResourceCount => mResourceInfos != null ? mResourceInfos.Count : 0;

        /// <summary>
        /// 资源组数量
        /// </summary>
        public int ResourceGroupCount => mResourceGroups.Count;

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get => mResourceLoader.ResourceAutoReleaseInterval;
            set => mResourceLoader.ResourceAutoReleaseInterval = value;
        }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int ResourceCapacity
        {
            get => mResourceLoader.ResourceCapacity;
            set => mResourceLoader.ResourceCapacity = value;
        }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int ResourcePriority
        {
            get => mResourceLoader.ResourcePriority;
            set => mResourceLoader.ResourcePriority = value;
        }

        /// <summary>
        /// 单机模式版本资源列表序列化器
        /// </summary>
        public PackageVersionListSerializer PackageVersionListSerializer => mPackageVersionListSerializer;

        /// <summary>
        /// 可更新模式版本资源列表序列化器
        /// </summary>
        public UpdatableVersionListSerializer UpdatableVersionListSerializer => mUpdatableVersionListSerializer;

        /// <summary>
        /// 本地只读区版本资源列表序列化器
        /// </summary>
        public ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer => mReadOnlyVersionListSerializer;

        /// <summary>
        /// 本地读写区版本资源序列化器
        /// </summary>
        public ReadWriteVersionListSerializer ReadWriteVersionListSerializer => mReadWriteVersionListSerializer;

        /// <summary>
        /// 资源包版本资源列表序列化器
        /// </summary>
        public ResourcePackVersionListSerializer ResourcePackVersionListSerializer => mResourcePackVersionListSerializer;

        /// <summary>
        /// 资源校验开始事件
        /// </summary>
        public event EventHandler<ResourceVerifyStartEventArgs> ResourceVerifyStart
        {
            add => mResourceVerifyStartEventHandler += value;
            remove => mResourceVerifyStartEventHandler -= value;
        }

        /// <summary>
        /// 资源校验成功事件
        /// </summary>
        public event EventHandler<ResourceVerifySuccessEventArgs> ResourceVerifySuccess
        {
            add => mResourceVerifySuccessEventHandler += value;
            remove => mResourceVerifySuccessEventHandler -= value;
        }

        /// <summary>
        /// 资源校验失败事件
        /// </summary>
        public event EventHandler<ResourceVerifyFailureEventArgs> ResourceVerifyFailure
        {
            add => mResourceVerifyFailureEventHandler += value;
            remove => mResourceVerifyFailureEventHandler -= value;
        }

        /// <summary>
        /// 资源更新开始事件
        /// </summary>
        public event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart
        {
            add => mResourceUpdateStartEventHandler += value;
            remove => mResourceUpdateStartEventHandler -= value;
        }

        /// <summary>
        /// 资源更新改变事件
        /// </summary>
        public event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged
        {
            add => mResourceUpdateChangedEventHandler += value;
            remove => mResourceUpdateChangedEventHandler -= value;
        }

        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        public event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess
        {
            add => mResourceUpdateSuccessEventHandler += value;
            remove => mResourceUpdateSuccessEventHandler -= value;
        }

        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        public event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure
        {
            add => mResourceUpdateFailureEventHandler += value;
            remove => mResourceUpdateFailureEventHandler -= value;
        }

        /// <summary>
        /// 资源更新全部完成事件
        /// </summary>
        public event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete
        {
            add => mResourceUpdateAllCompleteEventHandler += value;
            remove => mResourceUpdateAllCompleteEventHandler -= value;
        }

        /// <summary>
        /// 资源应用开始事件
        /// </summary>
        public event EventHandler<ResourceApplyStartEventArgs> ResourceApplyStart
        {
            add => mResourceApplyStartEventHandler += value;
            remove => mResourceApplyStartEventHandler -= value;
        }

        /// <summary>
        /// 资源应用成功事件
        /// </summary>
        public event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess
        {
            add => mResourceApplySuccessEventHandler += value;
            remove => mResourceApplySuccessEventHandler -= value;
        }

        /// <summary>
        /// 资源应用失败事件
        /// </summary>
        public event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure
        {
            add => mResourceApplyFailureEventHandler += value;
            remove => mResourceApplyFailureEventHandler -= value;
        }

        /// <summary>
        /// 资源管理器轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (mResourceVerifier != null)
            {
                mResourceVerifier.Update(elapseSeconds, realElapseSeconds);
                return;
            }

            mResourceUpdater?.Update(elapseSeconds, realElapseSeconds);

            mResourceLoader.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理资源管理器
        /// </summary>
        public override void Shutdown()
        {
            if (mResourceInitializer != null)
            {
                mResourceInitializer.Shutdown();
                mResourceInitializer = null;
            }

            if (mVersionListProcessor != null)
            {
                mVersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
                mVersionListProcessor.VersionListUpdateFailure -= OnVersionListProcessorUpdateFailure;
                mVersionListProcessor.Shutdown();
                mVersionListProcessor = null;
            }

            if (mResourceVerifier != null)
            {
                mResourceVerifier.ResourceVerifyStart -= OnVerifierResourceVerifyStart;
                mResourceVerifier.ResourceVerifySuccess -= OnVerifierResourceVerifySuccess;
                mResourceVerifier.ResourceVerifyFailure -= OnVerifierResourceVerifyFailure;
                mResourceVerifier.ResourceVerifyComplete -= OnVerifierResourceVerifyComplete;
                mResourceVerifier.Shutdown();
                mResourceVerifier = null;
            }

            if (mResourceChecker != null)
            {
                mResourceChecker.ResourceNeedUpdate -= OnCheckerResourceNeedUpdate;
                mResourceChecker.ResourceCheckComplete -= OnCheckerResourceCheckComplete;
                mResourceChecker.Shutdown();
                mResourceChecker = null;
            }

            if (mResourceUpdater != null)
            {
                mResourceUpdater.ResourceApplyStart -= OnUpdaterResourceApplyStart;
                mResourceUpdater.ResourceApplySuccess -= OnUpdaterResourceApplySuccess;
                mResourceUpdater.ResourceApplyFailure -= OnUpdaterResourceApplyFailure;
                mResourceUpdater.ResourceApplyComplete -= OnUpdaterResourceApplyComplete;
                mResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
                mResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
                mResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
                mResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
                mResourceUpdater.ResourceUpdateComplete -= OnUpdaterResourceUpdateComplete;
                mResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;

                if (mReadWriteResourceInfos != null)
                {
                    mReadWriteResourceInfos.Clear();
                    mReadWriteResourceInfos = null;
                }

                FreeCachedStream();
            }

            if (mResourceLoader != null)
            {
                mResourceLoader.Shutdown();
                mResourceLoader = null;
            }

            if (mAssetInfos != null)
            {
                mAssetInfos.Clear();
                mAssetInfos = null;
            }

            if (mResourceInfos != null)
            {
                mResourceInfos.Clear();
                mResourceInfos = null;
            }

            mReadOnlyFileSystems.Clear();
            mReadWriteFileSystems.Clear();
            mResourceGroups.Clear();
        }

        /// <summary>
        /// 设置只读区路径
        /// </summary>
        /// <param name="readOnlyPath">只读区路径</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                throw new Exception("Read-only path is invalid.");
            }

            if (mRefuseSetFlag)
            {
                throw new Exception("You can not set read-only path at this time.");
            }

            if (mResourceLoader.TotalAgentCount > 0)
            {
                throw new Exception("You must set read-only path before add load resource agent helper.");
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
                throw new Exception("Read-write path is invalid.");
            }

            if (mRefuseSetFlag)
            {
                throw new Exception("You can not set read-write path at this time.");
            }

            if (mResourceLoader.TotalAgentCount > 0)
            {
                throw new Exception("You must set read-only path before add load resource agent helper.");
            }

            mReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourceMode">资源模式</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            if (resourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("Resource mode is invalid.");
            }

            if (mRefuseSetFlag)
            {
                throw new Exception("You can not set resource mode at this time.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                mResourceMode = resourceMode;
                if (mResourceMode == ResourceMode.StandalonePackage)
                {
                    mPackageVersionListSerializer = new PackageVersionListSerializer();
                    mResourceInitializer = new ResourceInitializer(this);
                    mResourceInitializer.ResourceInitComplete += OnInitializerResourceInitComplete;
                }
                else if (mResourceMode == ResourceMode.Updatable || mResourceMode == ResourceMode.UpdatableWhilePlaying)
                {
                    mUpdatableVersionListSerializer = new UpdatableVersionListSerializer();
                    mReadOnlyVersionListSerializer = new ReadOnlyVersionListSerializer();
                    mReadWriteVersionListSerializer = new ReadWriteVersionListSerializer();
                    mResourcePackVersionListSerializer = new ResourcePackVersionListSerializer();

                    mVersionListProcessor = new VersionListProcessor(this);
                    mVersionListProcessor.VersionListUpdateSuccess += OnVersionListProcessorUpdateSuccess;
                    mVersionListProcessor.VersionListUpdateFailure += OnVersionListProcessorUpdateFailure;

                    mResourceChecker = new ResourceChecker(this);
                    mResourceChecker.ResourceNeedUpdate += OnCheckerResourceNeedUpdate;
                    mResourceChecker.ResourceCheckComplete += OnCheckerResourceCheckComplete;

                    mResourceUpdater = new ResourceUpdater(this);
                    mResourceUpdater.ResourceApplyStart += OnUpdaterResourceApplyStart;
                    mResourceUpdater.ResourceApplySuccess += OnUpdaterResourceApplySuccess;
                    mResourceUpdater.ResourceApplyFailure += OnUpdaterResourceApplyFailure;
                    mResourceUpdater.ResourceApplyComplete += OnUpdaterResourceApplyComplete;
                    mResourceUpdater.ResourceUpdateStart += OnUpdaterResourceUpdateStart;
                    mResourceUpdater.ResourceUpdateChanged += OnUpdaterResourceUpdateChanged;
                    mResourceUpdater.ResourceUpdateSuccess += OnUpdaterResourceUpdateSuccess;
                    mResourceUpdater.ResourceUpdateFailure += OnUpdaterResourceUpdateFailure;
                    mResourceUpdater.ResourceUpdateComplete += OnUpdaterResourceUpdateComplete;
                    mResourceUpdater.ResourceUpdateAllComplete += OnUpdaterResourceUpdateAllComplete;
                }
            }
            else if (mResourceMode != resourceMode)
            {
                throw new Exception("You can not change resource mode at this time.");
            }
        }

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant">当前变体</param>
        public void SetCurrentVariant(string currentVariant)
        {
            if (mRefuseSetFlag)
            {
                throw new Exception("You can not set current variant at this time.");
            }

            mCurrentVariant = currentVariant;
        }

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new Exception("Object pool manager is invalid.");
            }

            mResourceLoader.SetObjectPoolManager(objectPoolManager);
        }

        /// <summary>
        /// 设置文件系统管理器
        /// </summary>
        /// <param name="fileSystemManager">文件系统管理器</param>
        public void SetFileSystemManager(IFileSystemManager fileSystemManager)
        {
            if (fileSystemManager == null)
            {
                throw new Exception("File system manager is invalid.");
            }

            mFileSystemManager = fileSystemManager;
        }

        /// <summary>
        /// 设置下载管理器
        /// </summary>
        /// <param name="downloadManager">下载管理器</param>
        public void SetDownloadManager(IDownloadManager downloadManager)
        {
            if (downloadManager == null)
            {
                throw new Exception("Download manager is invalid.");
            }

            mVersionListProcessor?.SetDownloadManager(downloadManager);

            mResourceUpdater?.SetDownloadManager(downloadManager);
        }

        /// <summary>
        /// 设置解密资源回调函数
        /// </summary>
        /// <param name="decryptResourceCallback">解密资源回调函数</param>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            if (mResourceLoader.TotalAgentCount > 0)
            {
                throw new Exception("You must set decrypt resource callback before add load resource agent helper.");
            }

            mDecryptResourceCallback = decryptResourceCallback;
        }

        /// <summary>
        /// 加载资源辅助器
        /// </summary>
        /// <param name="resourceHelper">资源辅助器</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            if (resourceHelper == null)
            {
                throw new Exception("Resource helper is invalid.");
            }

            if (mResourceLoader.TotalAgentCount > 0)
            {
                throw new Exception("You must set resource helper before add load resource agent helper.");
            }

            mResourceHelper = resourceHelper;
        }

        /// <summary>
        /// 增加加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            if (loadResourceAgentHelper == null)
            {
                throw new Exception("Load resource agent helper is invalid.");
            }

            if (string.IsNullOrEmpty(mReadOnlyPath))
            {
                throw new Exception("Read-only path is invalid.");
            }

            if (string.IsNullOrEmpty(mReadWritePath))
            {
                throw new Exception("Read-write path is invalid.");
            }

            mResourceLoader.AddLoadResourceAgentHelper(loadResourceAgentHelper, mResourceHelper, mReadOnlyPath, mReadWritePath, mDecryptResourceCallback);
        }

        /// <summary>
        /// 使用单机模式并初始化资源
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时回调</param>
        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            if (initResourcesCompleteCallback == null)
            {
                throw new Exception("Init resources complete callback is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.StandalonePackage)
            {
                throw new Exception("You can not use InitResources without standalone package resource mode.");
            }

            if (mResourceInitializer == null)
            {
                throw new Exception("You can not use InitResources at this time.");
            }

            mRefuseSetFlag = true;
            mInitResourcesCompleteCallback = initResourcesCompleteCallback;
            mResourceInitializer.InitResource(mCurrentVariant);
        }

        /// <summary>
        /// 使用可更新模式并检查版本资源列表
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use CheckVersionList without updatable resource mode.");
            }

            if (mVersionListProcessor == null)
            {
                throw new Exception("You can not use CheckVersionList at this time.");
            }

            return mVersionListProcessor.CheckVersionList(latestInternalResourceVersion);
        }

        /// <summary>
        /// 使用可更新模式并更新版本资源列表
        /// </summary>
        /// <param name="versionListInfo">版本资源列表信息</param>
        /// <param name="updateVersionListCallbacks">更新版本资源列表回调函数集</param>
        public void UpdateVersionList(VersionListInfo versionListInfo, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            if (updateVersionListCallbacks == null)
            {
                throw new Exception("Update version list callbacks is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use UpdateVersionList without updatable resource mode.");
            }

            if (mVersionListProcessor == null)
            {
                throw new Exception("You can not use UpdateVersionList at this time.");
            }

            mUpdateVersionListCallbacks = updateVersionListCallbacks;
            mVersionListProcessor.UpdateVersionList(versionListInfo.Length, versionListInfo.HashCode, versionListInfo.CompressedLength, versionListInfo.CompressedHashCode);
        }

        /// <summary>
        /// 使用可更新模式并校验资源
        /// </summary>
        /// <param name="verifyResourceLengthPerFrame">每帧校验资源大小，以字节为单位</param>
        /// <param name="verifyResourcesCompleteCallback">校验资源完成回调函数</param>
        public void VerifyResource(int verifyResourceLengthPerFrame, VerifyResourcesCompleteCallback verifyResourcesCompleteCallback)
        {
            if (verifyResourcesCompleteCallback == null)
            {
                throw new Exception("Verify resource complete callback is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use VerifyResource without updatable resource mode.");
            }

            if (mRefuseSetFlag)
            {
                throw new Exception("You can not verify resources at this time.");
            }

            mResourceVerifier = new ResourceVerifier(this);
            mResourceVerifier.ResourceVerifyStart += OnVerifierResourceVerifyStart;
            mResourceVerifier.ResourceVerifySuccess += OnVerifierResourceVerifySuccess;
            mResourceVerifier.ResourceVerifyFailure += OnVerifierResourceVerifyFailure;
            mResourceVerifier.ResourceVerifyComplete += OnVerifierResourceVerifyComplete;
            mVerifyResourcesCompleteCallback = verifyResourcesCompleteCallback;
            mResourceVerifier.VerifyResource(verifyResourceLengthPerFrame);
        }

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若否则移除其它变体的资源</param>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数</param>
        public void CheckResource(bool ignoreOtherVariant, CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            if (checkResourcesCompleteCallback == null)
            {
                throw new Exception("Check resource complete callback is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use CheckResource without updatable resource mode.");
            }

            if (mResourceChecker == null)
            {
                throw new Exception("You can not use CheckResource at this time.");
            }

            mRefuseSetFlag = true;
            mCheckResourcesCompleteCallback = checkResourcesCompleteCallback;
            mResourceChecker.CheckResources(mCurrentVariant, ignoreOtherVariant);
        }

        /// <summary>
        /// 使用可更新模式并应用资源
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="applyResourcesCompleteCallback">使用可更新模式并应用资源完成时的回调函数</param>
        public void ApplyResource(string resourcePackPath, ApplyResourcesCompleteCallback applyResourcesCompleteCallback)
        {
            if (string.IsNullOrEmpty(resourcePackPath))
            {
                throw new Exception("Resource pack path is invalid.");
            }

            if (!File.Exists(resourcePackPath))
            {
                throw new Exception($"Resource pack ({resourcePackPath}) is not exist.");
            }

            if (applyResourcesCompleteCallback == null)
            {
                throw new Exception("Apply resource complete callback is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use ApplyResource without updatable resource mode.");
            }

            if (mResourceUpdater == null)
            {
                throw new Exception("You can not use ApplyResource at this time.");
            }

            mApplyResourcesCompleteCallback = applyResourcesCompleteCallback;
            mResourceUpdater.ApplyResources(resourcePackPath);
        }

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            UpdateResources(string.Empty, updateResourcesCompleteCallback);
        }

        /// <summary>
        /// 使用可更新模式并更新指定资源组的资源
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        public void UpdateResources(string resourceGroupName, UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            if (updateResourcesCompleteCallback == null)
            {
                throw new Exception("Update resource complete callback is invalid.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use UpdateResource without updatable resource mode.");
            }

            if (mResourceUpdater == null)
            {
                throw new Exception("You can not use UpdateResource at this time.");
            }

            var resourceGroup = GetResourceGroup(resourceGroupName) as ResourceGroup;
            if (resourceGroup == null)
            {
                throw new Exception($"Can not find resource group ({resourceGroupName}).");
            }

            mUpdateResourcesCompleteCallback = updateResourcesCompleteCallback;
            mResourceUpdater.UpdateResources(resourceGroup);
        }

        /// <summary>
        /// 停止更新资源
        /// </summary>
        public void StopUpdateResource()
        {
            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use StopUpdateResource without updatable resource mode.");
            }

            if (mResourceUpdater == null)
            {
                throw new Exception("You can not use StopUpdateResource at this time.");
            }

            mResourceUpdater.StopUpdateResources();
            mUpdateResourcesCompleteCallback = null;
        }

        /// <summary>
        /// 校验资源包
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <returns>是否成功校验资源包</returns>
        public bool VerifyResourcePack(string resourcePackPath)
        {
            if (string.IsNullOrEmpty(resourcePackPath))
            {
                throw new Exception("Resource pack path is invalid.");
            }

            if (!File.Exists(resourcePackPath))
            {
                throw new Exception($"Resource pack ({resourcePackPath}) is not exist.");
            }

            if (mResourceMode == ResourceMode.Unspecified)
            {
                throw new Exception("You must set resource mode first.");
            }

            if (mResourceMode != ResourceMode.Updatable && mResourceMode != ResourceMode.UpdatableWhilePlaying)
            {
                throw new Exception("You can not use VerifyResourcePack without updatable resource mode.");
            }

            if (mResourcePackVersionListSerializer == null)
            {
                throw new Exception("You can not use VerifyResourcePack at this time.");
            }

            try
            {
                var length = 0L;
                var versionList = default(ResourcePackVersionList);
                using (var fileStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read))
                {
                    length = fileStream.Length;
                    versionList = mResourcePackVersionListSerializer.Deserialize(fileStream);
                }

                if (!versionList.IsValid)
                {
                    return false;
                }

                if (versionList.Offset + versionList.Length != length)
                {
                    return false;
                }

                var hashCode = 0;
                using (var fileStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = versionList.Offset;
                    hashCode = Utility.Verifier.GetCrc32(fileStream);
                }

                if (versionList.HashCode != hashCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <returns>所有加载资源任务的信息</returns>
        public TaskInfo[] GetAllLoadAssetInfos()
        {
            return mResourceLoader.GetAllLoadAssetInfos();
        }

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <param name="results">所有加载资源任务的信息</param>
        public void GetAllLoadAssetInfos(List<TaskInfo> results)
        {
            mResourceLoader.GetAllLoadAssetInfos(results);
        }

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns>检查资源是否存在的结果</returns>
        public HasAssetResult HasAsset(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new Exception("Asset name is invalid.");
            }

            return mResourceLoader.HasAsset(assetName);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="loadAssetInfo">加载资源的信息</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        public void LoadAsset(LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(loadAssetInfo.AssetName))
            {
                throw new Exception("Asset name is invalid in load asset info.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new Exception("Load asset callbacks is invalid.");
            }

            mResourceLoader.LoadAsset(loadAssetInfo.AssetName, loadAssetInfo.AssetType, loadAssetInfo.Priority, loadAssetCallbacks, loadAssetInfo.UserData);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源</param>
        public void UnloadAsset(object asset)
        {
            if (asset == null)
            {
                throw new Exception("Asset is invalid.");
            }

            mResourceLoader?.UnloadAsset(asset);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="loadSceneInfo">加载场景信息</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        public void LoadScene(LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks)
        {
            if (string.IsNullOrEmpty(loadSceneInfo.SceneAssetName))
            {
                throw new Exception("Scene asset name is invalid in load scene info.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new Exception("Load scene callbacks is invalid.");
            }

            mResourceLoader?.LoadScene(loadSceneInfo.SceneAssetName, loadSceneInfo.Priority, loadSceneCallbacks, loadSceneInfo.UserData);
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
                throw new Exception("Scene asset name is invalid.");
            }

            if (unloadSceneCallbacks == null)
            {
                throw new Exception("Unload scene callbacks is invalid.");
            }

            mResourceLoader.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
        }

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的实际路径</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（非文件系统）中，否则为空</remarks>
        public string GetBinaryPath(string binaryAssetName)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            return mResourceLoader.GetBinaryPath(binaryAssetName);
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
            return mResourceLoader.GetBinaryPath(binaryAssetName, out storageInReadOnly, out storageInFileSystem, out relativePath, out fileName);
        }

        /// <summary>
        /// 获取二进制资源的长度
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的长度</returns>
        public int GetBinaryLength(string binaryAssetName)
        {
            return mResourceLoader.GetBinaryLength(binaryAssetName);
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
                throw new Exception("Binary asset name is invalid.");
            }

            if (loadBinaryCallbacks == null)
            {
                throw new Exception("Load binary callbacks is invalid.");
            }

            mResourceLoader.LoadBinary(binaryAssetName, loadBinaryCallbacks, userData);
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
                throw new Exception("Binary asset name is invalid.");
            }

            return mResourceLoader.LoadBinaryFromFileSystem(binaryAssetName);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinaryFromFileSystem(binaryAssetName, buffer, 0, buffer.Length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinaryFromFileSystem(binaryAssetName, buffer, startIndex, buffer.Length - startIndex);
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
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinaryFromFileSystem(binaryAssetName, buffer, startIndex, length);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, 0, length);
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
                throw new Exception("Binary asset name is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, length);
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, 0, buffer.Length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, 0, length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, 0, buffer, startIndex, length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, 0, buffer.Length);
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
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, 0, length);
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
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex,
            int length)
        {
            if (string.IsNullOrEmpty(binaryAssetName))
            {
                throw new Exception("Binary asset name is invalid.");
            }

            if (buffer == null)
            {
                throw new Exception("Buffer is invalid.");
            }

            return mResourceLoader.LoadBinarySegmentFromFileSystem(binaryAssetName, offset, buffer, startIndex, length);
        }

        /// <summary>
        /// 检查资源组是否存在
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>资源组是否存在</returns>
        public bool HasResourceGroup(string resourceGroupName)
        {
            return mResourceGroups.ContainsKey(resourceGroupName ?? string.Empty);
        }

        /// <summary>
        /// 获取指定资源组
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>指定资源组</returns>
        public IResourceGroup GetResourceGroup(string resourceGroupName = null)
        {
            return mResourceGroups.GetValueOrDefault(resourceGroupName ?? string.Empty);
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <returns>所有资源组</returns>
        public IResourceGroup[] GetAllResourceGroups()
        {
            var index = 0;
            var results = new IResourceGroup[mResourceGroups.Count];
            foreach (var (_, resourceGroup) in mResourceGroups)
            {
                results[index++] = resourceGroup;
            }

            return results;
        }

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <param name="results">所有资源组</param>
        public void GetAllResourceGroups(List<IResourceGroup> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, resourceGroup) in mResourceGroups)
            {
                results.Add(resourceGroup);
            }
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(params string[] resourceGroupNames)
        {
            if (resourceGroupNames == null || resourceGroupNames.Length < 1)
            {
                throw new Exception("Resource group names is invalid.");
            }

            var resourceGroups = new ResourceGroup[resourceGroupNames.Length];
            for (var i = 0; i < resourceGroupNames.Length; i++)
            {
                var resourceGroupName = resourceGroupNames[i];
                if (string.IsNullOrEmpty(resourceGroupName))
                {
                    throw new Exception("Resource group name is invalid.");
                }

                resourceGroups[i] = GetResourceGroup(resourceGroupName) as ResourceGroup;
                if (resourceGroups[i] == null)
                {
                    throw new Exception($"Resource group ({resourceGroupName}) is not exist.");
                }
            }

            return new ResourceGroupCollection(resourceGroups, mResourceInfos);
        }

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        public IResourceGroupCollection GetResourceGroupCollection(List<string> resourceGroupNames)
        {
            if (resourceGroupNames == null || resourceGroupNames.Count < 1)
            {
                throw new Exception("Resource group names is invalid.");
            }

            var resourceGroups = new ResourceGroup[resourceGroupNames.Count];
            for (var i = 0; i < resourceGroupNames.Count; i++)
            {
                var resourceGroupName = resourceGroupNames[i];
                if (string.IsNullOrEmpty(resourceGroupName))
                {
                    throw new Exception("Resource group name is invalid.");
                }

                resourceGroups[i] = GetResourceGroup(resourceGroupName) as ResourceGroup;
                if (resourceGroups[i] == null)
                {
                    throw new Exception($"Resource group ({resourceGroupName}) is not exist.");
                }
            }

            return new ResourceGroupCollection(resourceGroups, mResourceInfos);
        }

        private void UpdateResource(ResourceName resourceName)
        {
            mResourceUpdater.UpdateResource(resourceName);
        }

        private ResourceGroup GetOrAddResourceGroup(string resourceGroupName)
        {
            resourceGroupName ??= string.Empty;

            if (!mResourceGroups.TryGetValue(resourceGroupName, out var resourceGroup))
            {
                resourceGroup = new ResourceGroup(resourceGroupName, mResourceInfos);
                mResourceGroups.Add(resourceGroupName, resourceGroup);
            }

            return resourceGroup;
        }

        private AssetInfo GetAssetInfo(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new Exception("Asset name is invalid.");
            }

            return mAssetInfos?.GetValueOrDefault(assetName);
        }

        private ResourceInfo GetResourceInfo(ResourceName resourceName)
        {
            return mResourceInfos?.GetValueOrDefault(resourceName);
        }

        private IFileSystem GetFileSystem(string fileSystemName, bool storageInReadOnly)
        {
            if (string.IsNullOrEmpty(fileSystemName))
            {
                throw new Exception("File system name is invalid.");
            }

            IFileSystem fileSystem = null;
            if (storageInReadOnly)
            {
                if (!mReadOnlyFileSystems.TryGetValue(fileSystemName, out fileSystem))
                {
                    var fullPath = Utility.Path.GetRegularPath(Path.Combine(mReadOnlyPath, $"{fileSystemName}.{DefaultExtension}"));
                    fileSystem = mFileSystemManager.GetFileSystem(fullPath);
                    if (fileSystem == null)
                    {
                        fileSystem = mFileSystemManager.LoadFileSystem(fullPath, FileSystemAccess.Read);
                        mReadOnlyFileSystems.Add(fileSystemName, fileSystem);
                    }
                }
            }
            else
            {
                if (!mReadWriteFileSystems.TryGetValue(fileSystemName, out fileSystem))
                {
                    var fullPath = Utility.Path.GetRegularPath(Path.Combine(mReadOnlyPath, $"{fileSystemName}.{DefaultExtension}"));
                    fileSystem = mFileSystemManager.GetFileSystem(fullPath);
                    if (fileSystem == null)
                    {
                        if (File.Exists(fullPath))
                        {
                            fileSystem = mFileSystemManager.LoadFileSystem(fullPath, FileSystemAccess.ReadWrite);
                        }
                        else
                        {
                            var directory = Path.GetDirectoryName(fullPath);
                            if (!Directory.Exists(directory))
                            {
                                if (directory != null) Directory.CreateDirectory(directory);
                            }

                            fileSystem = mFileSystemManager.CreateFileSystem(fullPath, FileSystemAccess.ReadWrite, FileSystemMaxFileCount, FileSystemMaxBlockCount);
                        }

                        mReadOnlyFileSystems.Add(fileSystemName, fileSystem);
                    }
                }
            }

            return fileSystem;
        }

        private void PrepareCachedStream()
        {
            if (mCachedSteam == null)
            {
                mCachedSteam = new MemoryStream();
            }

            mCachedSteam.Position = 0L;
            mCachedSteam.SetLength(0L);
        }

        private void FreeCachedStream()
        {
            if (mCachedSteam != null)
            {
                mCachedSteam.Dispose();
                mCachedSteam = null;
            }
        }

        private void OnInitializerResourceInitComplete()
        {
            mResourceInitializer.ResourceInitComplete -= OnInitializerResourceInitComplete;
            mResourceInitializer.Shutdown();
            mResourceInitializer = null;

            mInitResourcesCompleteCallback();
            mInitResourcesCompleteCallback = null;
        }

        private void OnVersionListProcessorUpdateSuccess(string downloadPath, string downloadUri)
        {
            mUpdateVersionListCallbacks.UpdateVersionListSuccessCallback?.Invoke(downloadPath, downloadUri);
        }

        private void OnVersionListProcessorUpdateFailure(string downloadPath, string errorMessage)
        {
            mUpdateVersionListCallbacks.UpdateVersionListFailureCallback?.Invoke(downloadPath, errorMessage);
        }

        private void OnVerifierResourceVerifyStart(int count, long totalLength)
        {
            if (mResourceVerifyStartEventHandler != null)
            {
                var eventArgs = ResourceVerifyStartEventArgs.Create(count, totalLength);
                mResourceVerifyStartEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnVerifierResourceVerifySuccess(ResourceName resourceName, int length)
        {
            if (mResourceVerifySuccessEventHandler != null)
            {
                var eventArgs = ResourceVerifySuccessEventArgs.Create(resourceName.FullName, length);
                mResourceVerifySuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnVerifierResourceVerifyFailure(ResourceName resourceName)
        {
            if (mResourceVerifyFailureEventHandler != null)
            {
                var eventArgs = ResourceVerifyFailureEventArgs.Create(resourceName.FullName);
                mResourceVerifyFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnVerifierResourceVerifyComplete(bool result)
        {
            mVerifyResourcesCompleteCallback?.Invoke(result);
            mResourceVerifier.ResourceVerifyStart -= OnVerifierResourceVerifyStart;
            mResourceVerifier.ResourceVerifySuccess -= OnVerifierResourceVerifySuccess;
            mResourceVerifier.ResourceVerifyFailure -= OnVerifierResourceVerifyFailure;
            mResourceVerifier.ResourceVerifyComplete -= OnVerifierResourceVerifyComplete;
            mResourceVerifier.Shutdown();
            mResourceVerifier = null;
        }

        private void OnCheckerResourceNeedUpdate(ResourceName resourceName, string fileSystemName, LoadType loadType, int length, int hashCode, int compressedLength,
            int compressedHashCode)
        {
            var resourcePath = Utility.Path.GetRegularPath(Path.Combine(mReadWritePath, resourceName.FullName));
            mResourceUpdater.AddResourceUpdate(resourceName, fileSystemName, loadType, length, hashCode, compressedLength, compressedHashCode, resourcePath);
        }

        private void OnCheckerResourceCheckComplete(int movedCount, int removedCount, int updateCount, long updateTotalLength, long updateTotalCompressedLength)
        {
            mVersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
            mVersionListProcessor.VersionListUpdateFailure -= OnVersionListProcessorUpdateFailure;
            mVersionListProcessor.Shutdown();
            mVersionListProcessor = null;
            mUpdateVersionListCallbacks = null;

            mResourceChecker.ResourceNeedUpdate -= OnCheckerResourceNeedUpdate;
            mResourceChecker.ResourceCheckComplete -= OnCheckerResourceCheckComplete;
            mResourceChecker.Shutdown();
            mResourceChecker = null;

            mResourceUpdater.CheckResourceComplete(movedCount > 0 || removedCount > 0);

            if (updateCount > 0)
            {
                mResourceUpdater.ResourceApplyStart -= OnUpdaterResourceApplyStart;
                mResourceUpdater.ResourceApplySuccess -= OnUpdaterResourceApplySuccess;
                mResourceUpdater.ResourceApplyFailure -= OnUpdaterResourceApplyFailure;
                mResourceUpdater.ResourceApplyComplete -= OnUpdaterResourceApplyComplete;
                mResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
                mResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
                mResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
                mResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
                mResourceUpdater.ResourceUpdateComplete -= OnUpdaterResourceUpdateComplete;
                mResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;
                mResourceUpdater.Shutdown();
                mResourceUpdater = null;

                mReadWriteResourceInfos.Clear();
                mReadWriteResourceInfos = null;

                FreeCachedStream();
            }

            mCheckResourcesCompleteCallback(movedCount, removedCount, updateCount, updateTotalLength, updateTotalCompressedLength);
            mCheckResourcesCompleteCallback = null;
        }

        private void OnUpdaterResourceApplyStart(string resourcePackPath, int count, long totalLength)
        {
            if (mResourceApplyStartEventHandler != null)
            {
                var eventArgs = ResourceApplyStartEventArgs.Create(resourcePackPath, count, totalLength);
                mResourceApplyStartEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceApplySuccess(ResourceName resourceName, string applyPath, string resourcePackPath, int length, int compressedLength)
        {
            if (mResourceApplySuccessEventHandler != null)
            {
                var eventArgs = ResourceApplySuccessEventArgs.Create(resourceName.FullName, applyPath, resourcePackPath, length, compressedLength);
                mResourceApplySuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceApplyFailure(ResourceName resourceName, string resourcePackPath, string errorMessage)
        {
            if (mResourceApplyFailureEventHandler != null)
            {
                var eventArgs = ResourceApplyFailureEventArgs.Create(resourceName.FullName, resourcePackPath, errorMessage);
                mResourceApplyFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceApplyComplete(string resourcePackPath, bool result)
        {
            mApplyResourcesCompleteCallback?.Invoke(resourcePackPath, result);
            mApplyResourcesCompleteCallback = null;
        }

        private void OnUpdaterResourceUpdateStart(ResourceName resourceName, string downloadPath, string downloadUri, int currentLength, int compressedLength, int retryCount)
        {
            if (mResourceUpdateStartEventHandler != null)
            {
                var eventArgs = ResourceUpdateStartEventArgs.Create(resourceName.FullName, downloadPath, downloadUri, currentLength, compressedLength, retryCount);
                mResourceUpdateStartEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceUpdateChanged(ResourceName resourceName, string downloadPath, string downloadUri, int currentLength, int compressedLength)
        {
            if (mResourceUpdateChangedEventHandler != null)
            {
                var eventArgs = ResourceUpdateChangedEventArgs.Create(resourceName.FullName, downloadPath, downloadUri, currentLength, compressedLength);
                mResourceUpdateChangedEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceUpdateSuccess(ResourceName resourceName, string downloadPath, string downloadUri, int length, int compressedLength)
        {
            if (mResourceUpdateSuccessEventHandler != null)
            {
                var eventArgs = ResourceUpdateSuccessEventArgs.Create(resourceName.FullName, downloadPath, downloadUri, length, compressedLength);
                mResourceUpdateSuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceUpdateFailure(ResourceName resourceName, string downloadUri, int retryCount, int totalRetryCount, string errorMessage)
        {
            if (mResourceUpdateFailureEventHandler != null)
            {
                var eventArgs = ResourceUpdateFailureEventArgs.Create(resourceName.FullName, downloadUri, retryCount, totalRetryCount, errorMessage);
                mResourceUpdateFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnUpdaterResourceUpdateComplete(ResourceGroup resourceGroup, bool result)
        {
            Utility.Path.RemoveEmptyDirectory(mReadWritePath);
            mUpdateResourcesCompleteCallback?.Invoke(resourceGroup, result);
            mUpdateResourcesCompleteCallback = null;
        }

        private void OnUpdaterResourceUpdateAllComplete()
        {
            mResourceUpdater.ResourceApplyStart -= OnUpdaterResourceApplyStart;
            mResourceUpdater.ResourceApplySuccess -= OnUpdaterResourceApplySuccess;
            mResourceUpdater.ResourceApplyFailure -= OnUpdaterResourceApplyFailure;
            mResourceUpdater.ResourceApplyComplete -= OnUpdaterResourceApplyComplete;
            mResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
            mResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
            mResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
            mResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
            mResourceUpdater.ResourceUpdateComplete -= OnUpdaterResourceUpdateComplete;
            mResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;
            mResourceUpdater.Shutdown();
            mResourceUpdater = null;

            mReadWriteResourceInfos.Clear();
            mReadWriteResourceInfos = null;

            FreeCachedStream();
            Utility.Path.RemoveEmptyDirectory(mReadWritePath);

            if (mResourceUpdateAllCompleteEventHandler != null)
            {
                var eventArgs = ResourceUpdateAllCompleteEventArgs.Create();
                mResourceUpdateAllCompleteEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}