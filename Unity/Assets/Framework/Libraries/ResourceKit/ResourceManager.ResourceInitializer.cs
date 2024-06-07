/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/21 11:29:1
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源初始化器
        /// </summary>
        private sealed class ResourceInitializer
        {
            private readonly ResourceManager mResourceManager;
            private readonly Dictionary<ResourceName, string> mCachedFileSystemNames;
            private string mCurrentVariant;

            public Action ResourceInitComplete;

            public ResourceInitializer(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;

                mCachedFileSystemNames = new Dictionary<ResourceName, string>();
                mCurrentVariant = null;

                ResourceInitComplete = null;
            }

            public void Shutdown()
            {
            }

            /// <summary>
            /// 初始化资源
            /// </summary>
            /// <param name="currentVariant">当前变体</param>
            /// <exception cref="Exception"></exception>
            public void InitResource(string currentVariant)
            {
                mCurrentVariant = currentVariant;

                if (mResourceManager.mResourceHelper == null)
                {
                    throw new Exception("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(mResourceManager.mReadOnlyPath))
                {
                    throw new Exception("Read-only path is invalid.");
                }

                var path = Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mReadOnlyPath,
                    RemoteVersionListFileName));
                ;
                mResourceManager.mResourceHelper.LoadBytes(path,
                    new LoadBytesCallbacks(OnLoadPackageVersionListSuccess, OnLoadPackageVersionListFailure), null);
            }

            private void OnLoadPackageVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    var versionList = mResourceManager.mPackageVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize package version list failure.");
                    }

                    var assets = versionList.Assets;
                    var resources = versionList.Resources;
                    var fileSystems = versionList.FileSystems;
                    var resourceGroups = versionList.ResourceGroups;

                    mResourceManager.mApplicableVersion = versionList.ApplicableVersion;
                    mResourceManager.mInternalResourceVersion = versionList.InternalResourceVersion;
                    mResourceManager.mAssetInfos =
                        new Dictionary<string, AssetInfo>(assets.Length, StringComparer.Ordinal);
                    mResourceManager.mResourceInfos =
                        new Dictionary<ResourceName, ResourceInfo>(resources.Length, new ResourceNameComparer());

                    var defaultResourceGroup = mResourceManager.GetOrAddResourceGroup(string.Empty);

                    foreach (var fileSystem in fileSystems)
                    {
                        var resourceIndexes = fileSystem.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != mCurrentVariant)
                            {
                                continue;
                            }

                            mCachedFileSystemNames.Add(
                                new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (var resource in resources)
                    {
                        if (resource.Variant != null && resource.Variant != mCurrentVariant)
                        {
                            continue;
                        }

                        var resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        var assetIndexes = resource.AssetIndexes;
                        foreach (var assetIndex in assetIndexes)
                        {
                            var asset = assets[assetIndex];
                            var dependencyAssetIndexes = asset.DependencyAssetIndexes;
                            var index = 0;
                            var dependencyAssetNames = new string[dependencyAssetIndexes.Length];
                            foreach (var dependencyAssetIndex in dependencyAssetIndexes)
                            {
                                dependencyAssetNames[index++] = assets[dependencyAssetIndex].Name;
                            }

                            mResourceManager.mAssetInfos.Add(asset.Name,
                                new AssetInfo(asset.Name, resourceName, dependencyAssetNames));
                        }

                        var fileSystemName = mCachedFileSystemNames.GetValueOrDefault(resourceName);

                        mResourceManager.mResourceInfos.Add(resourceName,
                            new ResourceInfo(resourceName, fileSystemName, (LoadType)resource.LoadType, resource.Length,
                                resource.HashCode, resource.Length, true, true));
                        defaultResourceGroup.AddResource(resourceName, resource.Length, resource.Length);
                    }

                    foreach (var resourceGroup in resourceGroups)
                    {
                        var group = mResourceManager.GetOrAddResourceGroup(resourceGroup.Name);
                        var resourceIndexes = resourceGroup.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != mCurrentVariant)
                            {
                                continue;
                            }

                            group.AddResource(new ResourceName(resource.Name, resource.Variant, resource.Extension),
                                resource.Length, resource.Length);
                        }
                    }

                    ResourceInitComplete();
                }
                catch (Exception e)
                {
                    throw new Exception($"Parse package version list with exception ({e}).");
                }
                finally
                {
                    mCachedFileSystemNames.Clear();
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadPackageVersionListFailure(string fileUri, string errorMessage, object userData)
            {
                throw new Exception($"Package version list ({fileUri}) is invalid, error message is ({errorMessage}).");
            }
        }
    }
}