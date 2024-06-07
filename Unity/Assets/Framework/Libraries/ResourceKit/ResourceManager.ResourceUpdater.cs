// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/27 15:36:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源更新器
        /// </summary>
        private sealed partial class ResourceUpdater
        {
            private const int CachedHashBytesLength = 4;
            private const int CachedBytesLength = 0x1000;

            private readonly ResourceManager mResourceManager;
            private readonly Queue<ApplyInfo> mApplyWaitingInfo;
            private readonly List<UpdateInfo> mUpdateWaitingInfo;
            private readonly HashSet<UpdateInfo> mUpdateWaitingInfoWhilePlaying;
            private readonly Dictionary<ResourceName, UpdateInfo> mUpdateCandidateInfo;
            private readonly SortedDictionary<string, List<int>> mCachedFileSystemsForGenerateReadWriteVersionList;
            private readonly List<ResourceName> mCachedResourceNames;
            private readonly byte[] mCachedHashBytes;
            private readonly byte[] mCachedBytes;

            private IDownloadManager mDownloadManager;
            private bool mCheckResourcesComplete;
            private string mApplyingResourcePackPath;
            private FileStream mApplyingResourcePackStream;
            private ResourceGroup mUpdatingResourceGroup;
            private int mGenerateReadWriteVersionListLength;
            private int mCurrentGenerateReadWriteVersionListLength;
            private int mUpdateRetryCount;
            private bool mFailureFlag;
            private string mReadWriteVersionListFileName;
            private string mReadWriteVersionListTempFileName;

            public Action<string, int, long> ResourceApplyStart;
            public Action<ResourceName, string, string, int, int> ResourceApplySuccess;
            public Action<ResourceName, string, string> ResourceApplyFailure;
            public Action<string, bool> ResourceApplyComplete;
            public Action<ResourceName, string, string, int, int, int> ResourceUpdateStart;
            public Action<ResourceName, string, string, int, int> ResourceUpdateChanged;
            public Action<ResourceName, string, string, int, int> ResourceUpdateSuccess;
            public Action<ResourceName, string, int, int, string> ResourceUpdateFailure;
            public Action<ResourceGroup, bool> ResourceUpdateComplete;
            public Action ResourceUpdateAllComplete;

            public ResourceUpdater(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;
                mApplyWaitingInfo = new Queue<ApplyInfo>();
                mUpdateWaitingInfo = new List<UpdateInfo>();
                mUpdateWaitingInfoWhilePlaying = new HashSet<UpdateInfo>();
                mUpdateCandidateInfo = new Dictionary<ResourceName, UpdateInfo>();
                mCachedFileSystemsForGenerateReadWriteVersionList = new SortedDictionary<string, List<int>>();
                mCachedResourceNames = new List<ResourceName>();
                mCachedHashBytes = new byte[CachedHashBytesLength];
                mCachedBytes = new byte[CachedBytesLength];

                mDownloadManager = null;
                mCheckResourcesComplete = false;
                mApplyingResourcePackPath = null;
                mApplyingResourcePackStream = null;
                mUpdatingResourceGroup = null;
                mGenerateReadWriteVersionListLength = 0;
                mCurrentGenerateReadWriteVersionListLength = 0;
                mUpdateRetryCount = 3;
                mFailureFlag = false;
                mReadWriteVersionListFileName = Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath, LocalVersionListFileName));
                mReadWriteVersionListTempFileName = $"{mReadWriteVersionListFileName}.{TempExtension}";

                ResourceApplyStart = null;
                ResourceApplySuccess = null;
                ResourceApplyFailure = null;
                ResourceApplyComplete = null;
                ResourceUpdateStart = null;
                ResourceUpdateChanged = null;
                ResourceUpdateSuccess = null;
                ResourceUpdateFailure = null;
                ResourceUpdateComplete = null;
                ResourceUpdateAllComplete = null;
            }

            /// <summary>
            /// 每更新多少字节的资源，重新生成一次版本资源列表
            /// </summary>
            public int GenerateReadWriteVersionListLength
            {
                get => mGenerateReadWriteVersionListLength;
                set => mGenerateReadWriteVersionListLength = value;
            }

            /// <summary>
            /// 正在应用的资源包路径
            /// </summary>
            public string ApplyingResourcePackPath => mApplyingResourcePackPath;

            /// <summary>
            /// 等待应用的资源包数量
            /// </summary>
            public int ApplyWaitingCount => mApplyWaitingInfo.Count;

            /// <summary>
            /// 资源更新重试次数
            /// </summary>
            public int UpdateRetryCount
            {
                get => mUpdateRetryCount;
                set => mUpdateRetryCount = value;
            }

            /// <summary>
            /// 正在更新的资源组
            /// </summary>
            public IResourceGroup UpdatingResourceGroup => mUpdatingResourceGroup;

            /// <summary>
            /// 等待更新资源数量
            /// </summary>
            public int UpdateWaitingCount => mUpdateWaitingInfo.Count;

            /// <summary>
            /// 使用时下载的等待更新资源数量
            /// </summary>
            public int UpdateWaitingWhilePlayingCount => mUpdateWaitingInfoWhilePlaying.Count;

            /// <summary>
            /// 候选更新资源数量
            /// </summary>
            public int UpdateCandidateCount => mUpdateCandidateInfo.Count;

            /// <summary>
            /// 资源更新器轮询
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (mApplyingResourcePackStream != null)
                {
                    while (mApplyWaitingInfo.Count > 0)
                    {
                        var applyInfo = mApplyWaitingInfo.Dequeue();
                        if (ApplyResource(applyInfo))
                        {
                            return;
                        }
                    }

                    Array.Clear(mCachedBytes, 0, CachedBytesLength);
                    mApplyingResourcePackPath = null;
                    mApplyingResourcePackStream.Dispose();
                    mApplyingResourcePackStream = null;

                    ResourceApplyComplete?.Invoke(mApplyingResourcePackPath, !mFailureFlag);

                    if (mUpdateCandidateInfo.Count <= 0)
                    {
                        ResourceUpdateAllComplete?.Invoke();
                    }

                    return;
                }

                if (mUpdateWaitingInfo.Count > 0)
                {
                    var freeCount = mDownloadManager.AvailableAgentCount - mDownloadManager.WaitingTaskCount;
                    if (freeCount > 0)
                    {
                        for (int i = 0, count = 0; i < mUpdateWaitingInfo.Count && count < freeCount; i++)
                        {
                            if (DownloadResource(mUpdateWaitingInfo[i]))
                            {
                                count++;
                            }
                        }
                    }

                    return;
                }
            }

            /// <summary>
            /// 关闭并清理资源更新器
            /// </summary>
            public void Shutdown()
            {
                if (mDownloadManager != null)
                {
                    mDownloadManager.DownloadStart -= OnDownloadStart;
                    mDownloadManager.DownloadSuccess -= OnDownloadSuccess;
                    mDownloadManager.DownloadFailure -= OnDownloadFailure;
                    mDownloadManager.DownloadUpdate -= OnDownloadUpdate;
                }

                mUpdateWaitingInfo.Clear();
                mUpdateCandidateInfo.Clear();
                mCachedFileSystemsForGenerateReadWriteVersionList.Clear();
            }

            /// <summary>
            /// 设置下载管理器
            /// </summary>
            /// <param name="downloadManager">下载管理器</param>
            /// <exception cref="Exception"></exception>
            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                if (downloadManager == null)
                {
                    throw new Exception("Download manager is invalid.");
                }

                mDownloadManager = downloadManager;
                mDownloadManager.DownloadStart += OnDownloadStart;
                mDownloadManager.DownloadSuccess += OnDownloadSuccess;
                mDownloadManager.DownloadFailure += OnDownloadFailure;
                mDownloadManager.DownloadUpdate += OnDownloadUpdate;
            }

            /// <summary>
            /// 增加资源更新
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <param name="fileSystemName">资源所在的文件系统名称</param>
            /// <param name="loadType">资源加载方式</param>
            /// <param name="length">资源大小</param>
            /// <param name="hashCode">资源哈希值</param>
            /// <param name="compressedLength">压缩后资源大小</param>
            /// <param name="compressedHashCode">压缩后资源哈希值</param>
            /// <param name="resourcePath">资源路径</param>
            public void AddResourceUpdate(ResourceName resourceName, string fileSystemName, LoadType loadType,
                int length, int hashCode, int compressedLength, int compressedHashCode, string resourcePath)
            {
                mUpdateCandidateInfo.Add(resourceName,
                    new UpdateInfo(resourceName, fileSystemName, loadType, length, hashCode, compressedLength,
                        compressedHashCode, resourcePath));
            }

            /// <summary>
            /// 检查资源完成
            /// </summary>
            /// <param name="needGenerateReadWriteVersionList">是否需要重新生成读写区版本资源列表</param>
            public void CheckResourceComplete(bool needGenerateReadWriteVersionList)
            {
                mCheckResourcesComplete = true;
                if (needGenerateReadWriteVersionList)
                {
                    GenerateReadWriteVersionList();
                }
            }

            /// <summary>
            /// 应用指定资源包的资源
            /// </summary>
            /// <param name="resourcePackPath">资源包路径</param>
            /// <exception cref="Exception"></exception>
            public void ApplyResources(string resourcePackPath)
            {
                if (!mCheckResourcesComplete)
                {
                    throw new Exception("You must check resource complete first.");
                }

                if (mApplyingResourcePackStream != null)
                {
                    throw new Exception(
                        $"There is already a resource pack ({mApplyingResourcePackPath}) being applied.");
                }

                if (mUpdatingResourceGroup != null)
                {
                    throw new Exception(
                        $"There is already a resource group ({mUpdatingResourceGroup.Name}) being applied.");
                }

                if (mUpdateWaitingInfoWhilePlaying.Count > 0)
                {
                    throw new Exception("There is already some resources being updated while playing");
                }

                try
                {
                    var length = 0L;
                    var versionList = default(ResourcePackVersionList);
                    using (var fileStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read))
                    {
                        length = fileStream.Length;
                        versionList = mResourceManager.mResourcePackVersionListSerializer.Deserialize(fileStream);
                    }

                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize resource pack version list failure.");
                    }

                    if (versionList.Offset + versionList.Length != length)
                    {
                        throw new Exception("Resource pack length is invalid.");
                    }

                    mApplyingResourcePackPath = resourcePackPath;
                    mApplyingResourcePackStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read);
                    mApplyingResourcePackStream.Position = versionList.Offset;
                    mFailureFlag = false;

                    var totalLength = 0L;
                    var resources = versionList.Resources;
                    foreach (var resource in resources)
                    {
                        var resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        if (!mUpdateCandidateInfo.TryGetValue(resourceName, out var updateInfo))
                        {
                            continue;
                        }

                        if (updateInfo.LoadType == (LoadType)resource.LoadType &&
                            updateInfo.Length == resource.Length &&
                            updateInfo.HashCode == resource.HashCode)
                        {
                            totalLength += resource.Length;
                            mApplyWaitingInfo.Enqueue(new ApplyInfo(resourceName, updateInfo.FileSystemName,
                                (LoadType)resource.LoadType, resource.Offset, resource.Length, resource.HashCode,
                                resource.CompressedLength, resource.CompressedHashCode, updateInfo.ResourcePath));
                        }
                    }

                    ResourceApplyStart?.Invoke(mApplyingResourcePackPath, mApplyWaitingInfo.Count, totalLength);
                }
                catch (Exception e)
                {
                    if (mApplyingResourcePackStream != null)
                    {
                        mApplyingResourcePackStream.Dispose();
                        mApplyingResourcePackStream = null;
                    }

                    throw new Exception($"Apply resources ({resourcePackPath}) with exception ({e}).");
                }
            }

            /// <summary>
            /// 更新指定资源组的资源
            /// </summary>
            /// <param name="resourceGroup">资源组</param>
            /// <exception cref="Exception"></exception>
            public void UpdateResources(ResourceGroup resourceGroup)
            {
                if (mDownloadManager == null)
                {
                    throw new Exception("Download manager is invalid.");
                }

                if (!mCheckResourcesComplete)
                {
                    throw new Exception("You must check resources complete first.");
                }

                if (mApplyingResourcePackStream != null)
                {
                    throw new Exception(
                        $"There is already a resource pack ({mApplyingResourcePackPath}) being applied.");
                }

                if (mUpdatingResourceGroup != null)
                {
                    throw new Exception(
                        $"There is already a resource group ({mUpdatingResourceGroup.Name}) being applied.");
                }

                if (string.IsNullOrEmpty(resourceGroup.Name))
                {
                    foreach (var (_, updateInfo) in mUpdateCandidateInfo)
                    {
                        mUpdateWaitingInfo.Add(updateInfo);
                    }
                }
                else
                {
                    resourceGroup.InternalGetResourceNames(mCachedResourceNames);
                    foreach (var resourceName in mCachedResourceNames)
                    {
                        if (!mUpdateCandidateInfo.TryGetValue(resourceName, out var updateInfo))
                        {
                            continue;
                        }

                        mUpdateWaitingInfo.Add(updateInfo);
                    }

                    mCachedResourceNames.Clear();
                }

                mUpdatingResourceGroup = resourceGroup;
                mFailureFlag = false;
            }

            /// <summary>
            /// 更新指定资源
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <exception cref="Exception"></exception>
            public void UpdateResource(ResourceName resourceName)
            {
                if (mDownloadManager == null)
                {
                    throw new Exception("Download manager is invalid.");
                }

                if (!mCheckResourcesComplete)
                {
                    throw new Exception("You must check resources complete first.");
                }

                if (mApplyingResourcePackStream != null)
                {
                    throw new Exception(
                        $"There is already a resource pack ({mApplyingResourcePackPath}) being applied.");
                }

                if (mUpdateCandidateInfo.TryGetValue(resourceName, out var updateInfo) &&
                    mUpdateWaitingInfoWhilePlaying.Add(updateInfo))
                {
                    DownloadResource(updateInfo);
                }
            }

            /// <summary>
            /// 停止更新资源
            /// </summary>
            /// <exception cref="Exception"></exception>
            public void StopUpdateResources()
            {
                if (mDownloadManager == null)
                {
                    throw new Exception("Download manager is invalid.");
                }

                if (!mCheckResourcesComplete)
                {
                    throw new Exception("You must check resources complete first.");
                }

                if (mApplyingResourcePackStream != null)
                {
                    throw new Exception(
                        $"There is already a resource pack ({mApplyingResourcePackPath}) being applied.");
                }

                if (mUpdatingResourceGroup == null)
                {
                    throw new Exception("There is no resource group being updated.");
                }

                mUpdateWaitingInfo.Clear();
                mUpdatingResourceGroup = null;
            }

            private bool ApplyResource(ApplyInfo applyInfo)
            {
                var position = mApplyingResourcePackStream.Position;
                try
                {
                    var compressed = applyInfo.Length != applyInfo.CompressedLength ||
                                     applyInfo.HashCode != applyInfo.CompressedHashCode;
                    var bytesRead = 0;
                    var bytesLeft = applyInfo.CompressedLength;
                    var directory = Path.GetDirectoryName(applyInfo.ResourcePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    mApplyingResourcePackStream.Position += applyInfo.Offset;
                    using (var fileStream =
                           new FileStream(applyInfo.ResourcePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        while ((bytesRead = mApplyingResourcePackStream.Read(mCachedBytes, 0,
                                   bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
                        {
                            bytesLeft -= bytesRead;
                            fileStream.Write(mCachedBytes, 0, bytesRead);
                        }

                        if (compressed)
                        {
                            fileStream.Position = 0L;
                            var hashCode = Utility.Verifier.GetCrc32(fileStream);
                            if (hashCode != applyInfo.CompressedHashCode)
                            {
                                var errorMessage =
                                    $"Resource compressed hash code error, need ({applyInfo.CompressedHashCode}), applied ({hashCode}).";
                                ResourceApplyFailure?.Invoke(applyInfo.ResourceName, mApplyingResourcePackPath,
                                    errorMessage);

                                mFailureFlag = true;
                                return false;
                            }

                            fileStream.Position = 0L;
                            mResourceManager.PrepareCachedStream();
                            if (!Utility.Compression.Decompress(fileStream, mResourceManager.mCachedSteam))
                            {
                                var errorMessage =
                                    $"Unable to decompress resource ({applyInfo.ResourcePath}).";
                                ResourceApplyFailure?.Invoke(applyInfo.ResourceName, mApplyingResourcePackPath,
                                    errorMessage);

                                mFailureFlag = true;
                                return false;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(mResourceManager.mCachedSteam.GetBuffer(), 0,
                                (int)mResourceManager.mCachedSteam.Length);
                        }
                        else
                        {
                            var hashCode = 0;
                            fileStream.Position = 0L;
                            if (applyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                applyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                applyInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                                applyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                            {
                                Utility.Converter.GetBytes(applyInfo.HashCode, mCachedHashBytes);
                                if (applyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                    applyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, mCachedHashBytes,
                                        Utility.Encryption.QuickEncryptLength);
                                }
                                else if (applyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                         applyInfo.LoadType == LoadType.LoadFromBinaryDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, mCachedHashBytes,
                                        applyInfo.Length);
                                }

                                Array.Clear(mCachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(fileStream);
                            }

                            if (hashCode != applyInfo.HashCode)
                            {
                                var errorMessage =
                                    $"Resource compressed hash code error, need ({applyInfo.HashCode}), applied ({hashCode}).";
                                ResourceApplyFailure?.Invoke(applyInfo.ResourceName, mApplyingResourcePackPath,
                                    errorMessage);

                                mFailureFlag = true;
                                return false;
                            }
                        }
                    }

                    if (applyInfo.UseFileSystem)
                    {
                        var fileSystem = mResourceManager.GetFileSystem(applyInfo.FileSystemName, false);
                        var retVal = fileSystem.WriteFile(applyInfo.ResourceName.FullName, applyInfo.ResourcePath);
                        if (File.Exists(applyInfo.ResourcePath))
                        {
                            File.Delete(applyInfo.ResourcePath);
                        }

                        if (!retVal)
                        {
                            var errorMessage =
                                $"Unable to write resource ({applyInfo.ResourcePath}) to file system ({applyInfo.FileSystemName}).";
                            ResourceApplyFailure?.Invoke(applyInfo.ResourceName, mApplyingResourcePackPath,
                                errorMessage);

                            mFailureFlag = true;
                            return false;
                        }
                    }

                    var downloadingResource = $"{applyInfo.ResourcePath}.download";
                    if (File.Exists(downloadingResource))
                    {
                        File.Delete(downloadingResource);
                    }

                    mUpdateCandidateInfo.Remove(applyInfo.ResourceName);
                    mResourceManager.mResourceInfos[applyInfo.ResourceName].MarkReady();
                    mResourceManager.mReadWriteResourceInfos.Add(applyInfo.ResourceName,
                        new ReadWriteResourceInfo(applyInfo.FileSystemName, applyInfo.LoadType, applyInfo.Length,
                            applyInfo.HashCode));
                    ResourceApplySuccess?.Invoke(applyInfo.ResourceName, applyInfo.ResourcePath,
                        mApplyingResourcePackPath, applyInfo.Length, applyInfo.HashCode);

                    mCurrentGenerateReadWriteVersionListLength += applyInfo.CompressedLength;
                    if (mApplyWaitingInfo.Count <= 0 ||
                        mCurrentGenerateReadWriteVersionListLength >= mGenerateReadWriteVersionListLength)
                    {
                        GenerateReadWriteVersionList();
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    ResourceApplyFailure?.Invoke(applyInfo.ResourceName, mApplyingResourcePackPath, e.ToString());
                    mFailureFlag = true;
                    return false;
                }
                finally
                {
                    mApplyingResourcePackStream.Position = position;
                }
            }

            private bool DownloadResource(UpdateInfo updateInfo)
            {
                if (updateInfo.Downloading)
                {
                    return false;
                }

                updateInfo.Downloading = true;
                var resourceFullNameWithCrc32 = updateInfo.ResourceName.Variant != null
                    ? $"{updateInfo.ResourceName.Name}.{updateInfo.ResourceName.Variant}.{updateInfo.HashCode:x8}.{DefaultExtension}"
                    : $"{updateInfo.ResourceName.Name}.{updateInfo.HashCode:x8}.{DefaultExtension}";
                mDownloadManager.AddDownload(new DownloadInfo(updateInfo.ResourcePath,
                    Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mUpdatePrefixUri,
                        resourceFullNameWithCrc32)), updateInfo));
                return true;
            }

            private void GenerateReadWriteVersionList()
            {
                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(mReadWriteVersionListTempFileName, FileMode.Create, FileAccess.Write);
                    var resources = mResourceManager.mReadWriteResourceInfos.Count > 0
                        ? new LocalVersionList.Resource[mResourceManager.mReadWriteResourceInfos.Count]
                        : null;
                    if (resources != null)
                    {
                        var index = 0;
                        foreach (var (resourceName, readWriteResourceInfo) in mResourceManager.mReadWriteResourceInfos)
                        {
                            resources[index] = new LocalVersionList.Resource(resourceName.Name, resourceName.Variant,
                                resourceName.Extension, (byte)readWriteResourceInfo.LoadType,
                                readWriteResourceInfo.Length,
                                readWriteResourceInfo.HashCode);
                            if (readWriteResourceInfo.UseFileSystem)
                            {
                                if (!mCachedFileSystemsForGenerateReadWriteVersionList.TryGetValue(
                                        readWriteResourceInfo.FileSystemName, out var resourceIndexes))
                                {
                                    resourceIndexes = new List<int>();
                                    mCachedFileSystemsForGenerateReadWriteVersionList.Add(
                                        readWriteResourceInfo.FileSystemName, resourceIndexes);
                                }

                                resourceIndexes.Add(index);
                            }

                            index++;
                        }
                    }

                    var fileSystems = mCachedFileSystemsForGenerateReadWriteVersionList.Count > 0
                        ? new LocalVersionList.FileSystem[mCachedFileSystemsForGenerateReadWriteVersionList.Count]
                        : null;
                    if (fileSystems != null)
                    {
                        var index = 0;
                        foreach (var (key, value) in mCachedFileSystemsForGenerateReadWriteVersionList)
                        {
                            fileSystems[index++] = new LocalVersionList.FileSystem(key, value.ToArray());
                            value.Clear();
                        }
                    }

                    var versionList = new LocalVersionList(resources, fileSystems);
                    if (!mResourceManager.mReadWriteVersionListSerializer.Serialize(fileStream, versionList))
                    {
                        throw new Exception("Serialize read-write version list failure.");
                    }

                    fileStream.Dispose();
                    fileStream = null;
                }
                catch (Exception e)
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }

                    if (File.Exists(mReadWriteVersionListTempFileName))
                    {
                        File.Delete(mReadWriteVersionListTempFileName);
                    }

                    throw new Exception($"Generate read-write version list exception ({e})");
                }

                if (File.Exists(mReadWriteVersionListFileName))
                {
                    File.Delete(mReadWriteVersionListFileName);
                }

                File.Move(mReadWriteVersionListTempFileName, mReadWriteVersionListFileName);
                mCurrentGenerateReadWriteVersionListLength = 0;
            }

            private void OnDownloadStart(object sender, DownloadStartEventArgs e)
            {
                var updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (mDownloadManager == null)
                {
                    throw new Exception("You must set download manager first.");
                }

                if (e.CurrentLength > int.MaxValue)
                {
                    throw new Exception($"File ({e.DownloadPath}) is too large.");
                }

                ResourceUpdateStart?.Invoke(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri,
                    (int)e.CurrentLength, updateInfo.CompressedLength, updateInfo.RetryCount);
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                var updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                try
                {
                    using (var fileStream = new FileStream(e.DownloadPath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        var compressed = updateInfo.Length != updateInfo.CompressedLength ||
                                         updateInfo.HashCode != updateInfo.CompressedHashCode;
                        var length = (int)fileStream.Length;
                        if (length != updateInfo.CompressedLength)
                        {
                            fileStream.Close();
                            var errorMessage =
                                $"Resource compressed length error, need ({updateInfo.CompressedLength}), downloaded ({length}).";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }

                        if (compressed)
                        {
                            fileStream.Position = 0L;
                            var hashCode = Utility.Verifier.GetCrc32(fileStream);
                            if (hashCode != updateInfo.CompressedHashCode)
                            {
                                fileStream.Close();
                                var errorMessage =
                                    $"Resource compressed hash code error, need ({updateInfo.CompressedHashCode}), downloaded ({hashCode}).";
                                var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath,
                                    e.DownloadUri,
                                    errorMessage, e.UserData);
                                OnDownloadFailure(this, eventArgs);
                                ReferencePool.Release(eventArgs);
                                return;
                            }

                            fileStream.Position = 0L;
                            mResourceManager.PrepareCachedStream();
                            if (!Utility.Compression.Decompress(fileStream, mResourceManager.mCachedSteam))
                            {
                                fileStream.Close();
                                var errorMessage = $"Unable to decompress resource ({e.DownloadPath}).";
                                var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath,
                                    e.DownloadUri,
                                    errorMessage, e.UserData);
                                OnDownloadFailure(this, eventArgs);
                                ReferencePool.Release(eventArgs);
                                return;
                            }

                            var uncompressedLength = (int)mResourceManager.mCachedSteam.Length;
                            if (uncompressedLength != updateInfo.Length)
                            {
                                fileStream.Close();
                                var errorMessage =
                                    $"Resource length error, need ({updateInfo.Length}), downloaded ({uncompressedLength}).";
                                var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath,
                                    e.DownloadUri,
                                    errorMessage, e.UserData);
                                OnDownloadFailure(this, eventArgs);
                                ReferencePool.Release(eventArgs);
                                return;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(mResourceManager.mCachedSteam.GetBuffer(), 0, uncompressedLength);
                        }
                        else
                        {
                            var hashCode = 0;
                            fileStream.Position = 0L;
                            if (updateInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                updateInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                updateInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                                updateInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                            {
                                Utility.Converter.GetBytes(updateInfo.HashCode, mCachedHashBytes);
                                if (updateInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                    updateInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, mCachedHashBytes,
                                        Utility.Encryption.QuickEncryptLength);
                                }
                                else if (updateInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                         updateInfo.LoadType == LoadType.LoadFromBinaryDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, mCachedHashBytes, length);
                                }

                                Array.Clear(mCachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(fileStream);
                            }

                            if (hashCode != updateInfo.HashCode)
                            {
                                fileStream.Close();
                                var errorMessage =
                                    $"Resource hash code error, need ({updateInfo.HashCode}), downloaded ({hashCode}).";
                                var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath,
                                    e.DownloadUri,
                                    errorMessage, e.UserData);
                                OnDownloadFailure(this, eventArgs);
                                ReferencePool.Release(eventArgs);
                                return;
                            }
                        }
                    }

                    if (updateInfo.UseFileSystem)
                    {
                        var fileSystem = mResourceManager.GetFileSystem(updateInfo.FileSystemName, false);
                        var retVal = fileSystem.WriteFile(updateInfo.ResourceName.FullName, updateInfo.ResourcePath);
                        if (File.Exists(updateInfo.ResourcePath))
                        {
                            File.Delete(updateInfo.ResourcePath);
                        }

                        if (!retVal)
                        {
                            var errorMessage = $"Write resource to file system ({fileSystem.FullPath}) error.";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }
                    }

                    mUpdateCandidateInfo.Remove(updateInfo.ResourceName);
                    mUpdateWaitingInfo.Remove(updateInfo);
                    mUpdateWaitingInfoWhilePlaying.Remove(updateInfo);
                    mResourceManager.mResourceInfos[updateInfo.ResourceName].MarkReady();
                    mResourceManager.mReadWriteResourceInfos.Add(updateInfo.ResourceName,
                        new ReadWriteResourceInfo(updateInfo.FileSystemName, updateInfo.LoadType, updateInfo.Length,
                            updateInfo.HashCode));
                    ResourceApplySuccess?.Invoke(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri,
                        updateInfo.Length, updateInfo.CompressedLength);

                    mCurrentGenerateReadWriteVersionListLength += updateInfo.CompressedLength;
                    if (mUpdateCandidateInfo.Count <= 0 ||
                        mUpdateWaitingInfo.Count + mUpdateWaitingInfoWhilePlaying.Count <= 0 ||
                        mCurrentGenerateReadWriteVersionListLength >= mGenerateReadWriteVersionListLength)
                    {
                        GenerateReadWriteVersionList();
                    }

                    if (mUpdatingResourceGroup != null && mUpdateWaitingInfo.Count <= 0)
                    {
                        var updatingResourceGroup = mUpdatingResourceGroup;
                        mUpdatingResourceGroup = null;
                        ResourceUpdateComplete?.Invoke(updatingResourceGroup, !mFailureFlag);
                    }

                    if (mUpdateCandidateInfo.Count <= 0)
                    {
                        ResourceUpdateAllComplete?.Invoke();
                    }
                }
                catch (Exception exception)
                {
                    var errorMessage =
                        $"Update resource ({e.DownloadPath}) with error message ({exception}).";
                    var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                        errorMessage, e.UserData);
                    OnDownloadFailure(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }

            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                var updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                ResourceUpdateFailure?.Invoke(updateInfo.ResourceName, e.DownloadUri, updateInfo.RetryCount,
                    mUpdateRetryCount, e.ErrorMessage);

                if (updateInfo.RetryCount < mUpdateRetryCount)
                {
                    updateInfo.Downloading = false;
                    updateInfo.RetryCount++;
                    if (mUpdateWaitingInfoWhilePlaying.Contains(updateInfo))
                    {
                        DownloadResource(updateInfo);
                    }
                }
                else
                {
                    mFailureFlag = true;
                    updateInfo.Downloading = false;
                    updateInfo.RetryCount = 0;
                    mUpdateWaitingInfo.Remove(updateInfo);
                    mUpdateWaitingInfoWhilePlaying.Remove(updateInfo);
                }
            }

            private void OnDownloadUpdate(object sender, DownloadUpdateEventArgs e)
            {
                var updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (mDownloadManager == null)
                {
                    throw new Exception("You must set download manager first.");
                }

                if (e.CurrentLength > updateInfo.CompressedLength)
                {
                    mDownloadManager.RemoveDownload(e.SerialId);
                    var downloadFile = $"{e.DownloadPath}.download";
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    var errorMessage =
                        $"When download update, downloaded length is larger than compressed length, need ({updateInfo.CompressedLength}), downloaded ({e.CurrentLength}).";
                    var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                        errorMessage, e.UserData);
                    OnDownloadFailure(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                    return;
                }

                ResourceUpdateChanged?.Invoke(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri,
                    (int)e.CurrentLength, updateInfo.CompressedLength);
            }
        }
    }
}