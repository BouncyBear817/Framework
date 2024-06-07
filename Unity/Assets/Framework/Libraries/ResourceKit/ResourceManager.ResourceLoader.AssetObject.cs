// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 16:4:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            /// <summary>
            /// 资源对象
            /// </summary>
            private sealed class AssetObject : ObjectBase
            {
                private List<object> mDependencyAssets;
                private object mResource;
                private IResourceHelper mResourceHelper;
                private ResourceLoader mResourceLoader;

                public AssetObject()
                {
                    mDependencyAssets = new List<object>();
                    mResource = null;
                    mResourceHelper = null;
                    mResourceLoader = null;
                }

                /// <summary>
                /// 获取释放检查标记
                /// </summary>
                public override bool CanReleaseFlag
                {
                    get
                    {
                        mResourceLoader.mAssetDependencyCount.TryGetValue(Target, out var targetReferenceCount);
                        return base.CanReleaseFlag && targetReferenceCount > 0;
                    }
                }

                /// <summary>
                /// 创建资源对象
                /// </summary>
                /// <param name="name">资源对象名称</param>
                /// <param name="target">目标资源</param>
                /// <param name="dependencyAssets">依赖资源列表</param>
                /// <param name="resource">资源对象</param>
                /// <param name="resourceHelper">资源辅助器</param>
                /// <param name="resourceLoader">加载资源器</param>
                /// <returns>资源对象</returns>
                public static AssetObject Create(string name, object target, List<object> dependencyAssets,
                    object resource, IResourceHelper resourceHelper, ResourceLoader resourceLoader)
                {
                    if (dependencyAssets == null)
                    {
                        throw new Exception("Dependency assets is invalid.");
                    }

                    if (resource == null)
                    {
                        throw new Exception("Resource is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new Exception("Resource helper is invalid.");
                    }

                    if (resourceLoader == null)
                    {
                        throw new Exception("Resource loader is invalid.");
                    }

                    var assetObject = ReferencePool.Acquire<AssetObject>();
                    assetObject.Initialize(name, target);
                    assetObject.mDependencyAssets.AddRange(dependencyAssets);
                    assetObject.mResource = resource;
                    assetObject.mResourceHelper = resourceHelper;
                    assetObject.mResourceLoader = resourceLoader;

                    foreach (var dependencyAsset in dependencyAssets)
                    {
                        if (resourceLoader.mAssetDependencyCount.TryGetValue(dependencyAsset, out var referenceCount))
                        {
                            resourceLoader.mAssetDependencyCount[dependencyAsset] = referenceCount + 1;
                        }
                        else
                        {
                            resourceLoader.mAssetDependencyCount.Add(dependencyAsset, 1);
                        }
                    }

                    return assetObject;
                }

                /// <summary>
                /// 清理对象基类
                /// </summary>
                public override void Clear()
                {
                    base.Clear();

                    mDependencyAssets.Clear();
                    mResource = null;
                    mResourceHelper = null;
                    mResourceLoader = null;
                }

                /// <summary>
                /// 回收对象时的事件
                /// </summary>
                protected internal override void OnUnSpawn()
                {
                    base.OnUnSpawn();

                    foreach (var dependencyAsset in mDependencyAssets)
                    {
                        mResourceLoader.mAssetPool.UnSpawn(dependencyAsset);
                    }
                }

                /// <summary>
                /// 释放对象
                /// </summary>
                /// <param name="isShutdown">是否是关闭对象池时触发</param>
                protected internal override void Release(bool isShutdown)
                {
                    if (!isShutdown)
                    {
                        if (mResourceLoader.mAssetDependencyCount.TryGetValue(Target, out var targetReferenceCount) &&
                            targetReferenceCount > 0)
                        {
                            throw new Exception(
                                $"Asset target ({Name}) reference count is ({targetReferenceCount}) larger than 0.");
                        }

                        foreach (var dependencyAsset in mDependencyAssets)
                        {
                            if (mResourceLoader.mAssetDependencyCount.TryGetValue(dependencyAsset,
                                    out var referenceCount))
                            {
                                mResourceLoader.mAssetDependencyCount[dependencyAsset] = referenceCount - 1;
                            }
                            else
                            {
                                throw new Exception(
                                    $"Asset target ({Name}) dependency asset reference count is invalid.");
                            }
                        }

                        mResourceLoader.mResourcePool.UnSpawn(mResource);
                    }

                    mResourceLoader.mAssetDependencyCount.Remove(Target);
                    mResourceLoader.mAssetToResourceMap.Remove(Target);
                    mResourceHelper.Release(Target);
                }
            }
        }
    }
}