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
        /// 资源校验器
        /// </summary>
        private sealed partial class ResourceVerifier
        {
            private const int CachedHashBytesLength = 4;

            private readonly ResourceManager mResourceManager;
            private readonly List<VerifyInfo> mVerifyInfos;
            private readonly byte[] mCachedHashBytes;
            private bool mLoadReadWriteVersionListComplete;
            private int mVerifyResourceLengthPerFrame;
            private int mVerifyResourceIndex;
            private bool mFailureFlag;

            public Action<int, long> ResourceVerifyStart;
            public Action<ResourceName, int> ResourceVerifySuccess;
            public Action<ResourceName> ResourceVerifyFailure;
            public Action<bool> ResourceVerifyComplete;

            public ResourceVerifier(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;
                mVerifyInfos = new List<VerifyInfo>();
                mCachedHashBytes = new byte[CachedHashBytesLength];
                mLoadReadWriteVersionListComplete = false;
                mVerifyResourceLengthPerFrame = 0;
                mVerifyResourceIndex = 0;
                mFailureFlag = false;

                ResourceVerifyStart = null;
                ResourceVerifySuccess = null;
                ResourceVerifyFailure = null;
                ResourceVerifyComplete = null;
            }

            /// <summary>
            /// 资源校验器轮询
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (!mLoadReadWriteVersionListComplete)
                {
                    return;
                }

                var length = 0;
                while (mVerifyResourceIndex < mVerifyInfos.Count)
                {
                    var verifyInfo = mVerifyInfos[mVerifyResourceIndex];
                    length += verifyInfo.Length;
                    if (VerifyResource(verifyInfo))
                    {
                        mVerifyResourceIndex++;
                        ResourceVerifySuccess?.Invoke(verifyInfo.ResourceName, verifyInfo.Length);
                    }
                    else
                    {
                        mFailureFlag = true;
                        mVerifyInfos.RemoveAt(mVerifyResourceIndex);
                        ResourceVerifyFailure?.Invoke(verifyInfo.ResourceName);
                    }

                    if (length >= mVerifyResourceLengthPerFrame)
                    {
                        return;
                    }
                }

                mLoadReadWriteVersionListComplete = false;
                if (mFailureFlag)
                {
                    GenerateReadWriteVersionList();
                }

                ResourceVerifyComplete?.Invoke(!mFailureFlag);
            }

            /// <summary>
            /// 关闭并清理资源校验器
            /// </summary>
            public void Shutdown()
            {
                mVerifyInfos.Clear();
                mLoadReadWriteVersionListComplete = false;
                mVerifyResourceLengthPerFrame = 0;
                mVerifyResourceIndex = 0;
                mFailureFlag = false;
            }

            /// <summary>
            /// 校验资源
            /// </summary>
            /// <param name="verifyResourceLengthPerFrame">每帧校验资源的大小</param>
            /// <exception cref="Exception"></exception>
            public void VerifyResource(int verifyResourceLengthPerFrame)
            {
                if (verifyResourceLengthPerFrame < 0)
                {
                    throw new Exception("Verify resource count per frame is invalid.");
                }

                if (mResourceManager.mResourceHelper == null)
                {
                    throw new Exception("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(mResourceManager.mReadWritePath))
                {
                    throw new Exception("Read-write path is invalid.");
                }

                mVerifyResourceLengthPerFrame = verifyResourceLengthPerFrame;
                var path = Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mReadWritePath,
                    LocalVersionListFileName));
                mResourceManager.mResourceHelper.LoadBytes(path,
                    new LoadBytesCallbacks(OnLoadReadWriteVersionListSuccess, OnLoadReadWriteVersionListFailure), null);
            }

            private bool VerifyResource(VerifyInfo verifyInfo)
            {
                if (verifyInfo.UseFileSystem)
                {
                    var fileSystem = mResourceManager.GetFileSystem(verifyInfo.FileSystemName, false);
                    var fileName = verifyInfo.ResourceName.FullName;
                    var fileInfo = fileSystem.GetFileInfo(fileName);
                    if (!fileInfo.IsValid)
                    {
                        return false;
                    }

                    var length = fileInfo.Length;
                    if (length == verifyInfo.Length)
                    {
                        mResourceManager.PrepareCachedStream();
                        fileSystem.ReadFile(fileName, mResourceManager.mCachedSteam);
                        mResourceManager.mCachedSteam.Position = 0L;
                        var hashCode = 0;
                        if (verifyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                            verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                            verifyInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                            verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                        {
                            Utility.Converter.GetBytes(verifyInfo.HashCode, mCachedHashBytes);
                            if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                            {
                                hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam, mCachedHashBytes,
                                    Utility.Encryption.QuickEncryptLength);
                            }
                            else if (verifyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                     verifyInfo.LoadType == LoadType.LoadFromBinaryDecrypt)
                            {
                                hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam, mCachedHashBytes,
                                    length);
                            }

                            Array.Clear(mCachedHashBytes, 0, CachedHashBytesLength);
                        }
                        else
                        {
                            hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam);
                        }

                        if (hashCode == verifyInfo.HashCode)
                        {
                            return true;
                        }
                    }

                    fileSystem.DeleteFile(fileName);
                    return false;
                }
                else
                {
                    var resourcePath = Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath,
                        verifyInfo.ResourceName.FullName));
                    if (!File.Exists(resourcePath))
                    {
                        return false;
                    }

                    using (var fileStream = new FileStream(resourcePath, FileMode.Open, FileAccess.Read))
                    {
                        var length = (int)fileStream.Length;
                        if (length == verifyInfo.Length)
                        {
                            var hashCode = 0;
                            if (verifyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                verifyInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                                verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                            {
                                Utility.Converter.GetBytes(verifyInfo.HashCode, mCachedHashBytes);
                                if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                                    verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam,
                                        mCachedHashBytes,
                                        Utility.Encryption.QuickEncryptLength);
                                }
                                else if (verifyInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                                         verifyInfo.LoadType == LoadType.LoadFromBinaryDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam,
                                        mCachedHashBytes,
                                        length);
                                }

                                Array.Clear(mCachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(mResourceManager.mCachedSteam);
                            }

                            if (hashCode == verifyInfo.HashCode)
                            {
                                return true;
                            }
                        }
                    }

                    File.Delete(resourcePath);
                    return false;
                }
            }

            private void GenerateReadWriteVersionList()
            {
                var readWriteVersionListFileName =
                    Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath,
                        LocalVersionListFileName));
                var readWriteVersionListTempFileName = $"{readWriteVersionListFileName}.{TempExtension}";
                var cachedFileSystemsForGenerateReadWriteVersionList =
                    new SortedDictionary<string, List<int>>(StringComparer.Ordinal);

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(readWriteVersionListTempFileName, FileMode.Create, FileAccess.Read);
                    var resources = mVerifyInfos.Count > 0
                        ? new LocalVersionList.Resource[mVerifyInfos.Count]
                        : null;
                    if (resources != null)
                    {
                        var index = 0;
                        foreach (var verifyInfo in mVerifyInfos)
                        {
                            resources[index] = new LocalVersionList.Resource(verifyInfo.ResourceName.Name,
                                verifyInfo.ResourceName.Variant, verifyInfo.ResourceName.Extension,
                                (byte)verifyInfo.LoadType,
                                verifyInfo.Length, verifyInfo.HashCode);
                            if (verifyInfo.UseFileSystem)
                            {
                                if (!cachedFileSystemsForGenerateReadWriteVersionList.TryGetValue(
                                        verifyInfo.FileSystemName, out var resourceIndexes))
                                {
                                    resourceIndexes = new List<int>();
                                    cachedFileSystemsForGenerateReadWriteVersionList.Add(verifyInfo.FileSystemName,
                                        resourceIndexes);
                                }

                                resourceIndexes.Add(index);
                            }

                            index++;
                        }
                    }

                    var fileSystems = cachedFileSystemsForGenerateReadWriteVersionList.Count > 0
                        ? new LocalVersionList.FileSystem[cachedFileSystemsForGenerateReadWriteVersionList.Count]
                        : null;
                    if (fileSystems != null)
                    {
                        var index = 0;
                        foreach (var (key, value) in cachedFileSystemsForGenerateReadWriteVersionList)
                        {
                            fileSystems[index] = new LocalVersionList.FileSystem(key, value.ToArray());
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

                    if (File.Exists(readWriteVersionListTempFileName))
                    {
                        File.Delete(readWriteVersionListTempFileName);
                    }

                    throw new Exception($"Generate read-write version list exception ({e})");
                }

                if (File.Exists(readWriteVersionListFileName))
                {
                    File.Delete(readWriteVersionListFileName);
                }

                File.Move(readWriteVersionListTempFileName, readWriteVersionListFileName);
            }

            private void OnLoadReadWriteVersionListSuccess(string fileUri, byte[] bytes, float duration,
                object userData)
            {
                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    var versionList = mResourceManager.mReadWriteVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize read-write version list failure.");
                    }

                    var resources = versionList.Resources;
                    var fileSystems = versionList.FileSystems;
                    var resourceInFileSystemNames = new Dictionary<ResourceName, string>();
                    foreach (var fileSystem in fileSystems)
                    {
                        var resourceIndexes = fileSystem.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            resourceInFileSystemNames.Add(
                                new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    var totalLength = 0L;
                    foreach (var resource in resources)
                    {
                        var resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        resourceInFileSystemNames.TryGetValue(resourceName, out var fileSystemName);
                        totalLength += resource.Length;
                        mVerifyInfos.Add(new VerifyInfo(resourceName, fileSystemName, (LoadType)resource.LoadType,
                            resource.Length, resource.HashCode));
                    }

                    mLoadReadWriteVersionListComplete = true;
                    ResourceVerifyStart?.Invoke(mVerifyInfos.Count, totalLength);
                }
                catch (Exception e)
                {
                    throw new Exception($"Parse read-write version list exception ({e})");
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadReadWriteVersionListFailure(string fileUri, string errorMessage, object userData)
            {
                ResourceVerifyComplete?.Invoke(true);
            }
        }
    }
}