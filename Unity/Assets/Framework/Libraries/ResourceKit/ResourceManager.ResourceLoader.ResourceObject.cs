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
            private sealed class ResourceObject : ObjectBase
            {
                private List<object> mDependencyResources;
                private IResourceHelper mResourceHelper;
                private ResourceLoader mResourceLoader;

                public ResourceObject()
                {
                    mDependencyResources = new List<object>();
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
                        mResourceLoader.mResourceDependencyCount.TryGetValue(Target, out var targetReferenceCount);
                        return base.CanReleaseFlag && targetReferenceCount > 0;
                    }
                }

                /// <summary>
                /// 创建资源对象
                /// </summary>
                /// <param name="name">资源对象名称</param>
                /// <param name="target">目标资源</param>
                /// <param name="resourceHelper">资源辅助器</param>
                /// <param name="resourceLoader">加载资源器</param>
                /// <returns>资源对象</returns>
                public static ResourceObject Create(string name, object target, IResourceHelper resourceHelper,
                    ResourceLoader resourceLoader)
                {
                    if (resourceHelper == null)
                    {
                        throw new Exception("Resource helper is invalid.");
                    }

                    if (resourceLoader == null)
                    {
                        throw new Exception("Resource loader is invalid.");
                    }

                    var resourceObject = ReferencePool.Acquire<ResourceObject>();
                    resourceObject.Initialize(name, target);
                    resourceObject.mResourceHelper = resourceHelper;
                    resourceObject.mResourceLoader = resourceLoader;
                    return resourceObject;
                }

                /// <summary>
                /// 清理对象基类
                /// </summary>
                public override void Clear()
                {
                    base.Clear();

                    mDependencyResources.Clear();
                    mResourceHelper = null;
                    mResourceLoader = null;
                }

                public void AddDependencyResource(object dependencyResource)
                {
                    if (Target == dependencyResource)
                    {
                        return;
                    }

                    if (mDependencyResources.Contains(dependencyResource))
                    {
                        return;
                    }

                    mDependencyResources.Add(dependencyResource);
                    if (mResourceLoader.mAssetDependencyCount.TryGetValue(dependencyResource,
                            out var referenceCount))
                    {
                        mResourceLoader.mAssetDependencyCount[dependencyResource] = referenceCount + 1;
                    }
                    else
                    {
                        mResourceLoader.mResourceDependencyCount.Add(dependencyResource, 1);
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
                        if (mResourceLoader.mResourceDependencyCount.TryGetValue(Target,
                                out var targetReferenceCount) &&
                            targetReferenceCount > 0)
                        {
                            throw new Exception(
                                $"Resource target ({Name}) reference count is ({targetReferenceCount}) larger than 0.");
                        }

                        foreach (var dependencyResource in mDependencyResources)
                        {
                            if (mResourceLoader.mResourceDependencyCount.TryGetValue(dependencyResource,
                                    out var referenceCount))
                            {
                                mResourceLoader.mResourceDependencyCount[dependencyResource] = referenceCount - 1;
                            }
                            else
                            {
                                throw new Exception(
                                    $"Resource target ({Name}) dependency asset reference count is invalid.");
                            }
                        }
                    }

                    mResourceLoader.mResourceDependencyCount.Remove(Target);
                    mResourceHelper.Release(Target);
                }
            }
        }
    }
}