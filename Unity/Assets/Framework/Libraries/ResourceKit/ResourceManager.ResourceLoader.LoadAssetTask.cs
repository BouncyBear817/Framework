// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 16:4:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            /// <summary>
            /// 加载资源任务
            /// </summary>
            private sealed class LoadAssetTask : LoadResourceTaskBase
            {
                private LoadAssetCallbacks mLoadAssetCallbacks;

                public LoadAssetTask()
                {
                    mLoadAssetCallbacks = null;
                }

                /// <summary>
                /// 资源是否为场景
                /// </summary>
                public override bool IsScene => false;

                /// <summary>
                /// 创建加载资源任务
                /// </summary>
                /// <param name="assetName">资源名称</param>
                /// <param name="assetType">资源类型</param>
                /// <param name="priority">资源优先级</param>
                /// <param name="resourceInfo">资源信息</param>
                /// <param name="dependencyAssetNames">依赖资源名称集合</param>
                /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
                /// <param name="userData">用户自定义数据</param>
                /// <returns>加载资源任务</returns>
                public static LoadAssetTask Create(string assetName, Type assetType, int priority,
                    ResourceInfo resourceInfo, string[] dependencyAssetNames, LoadAssetCallbacks loadAssetCallbacks,
                    object userData)
                {
                    var task = ReferencePool.Acquire<LoadAssetTask>();
                    task.Initialize(assetName, assetType, priority, resourceInfo, dependencyAssetNames, userData);
                    task.mLoadAssetCallbacks = loadAssetCallbacks;
                    return task;
                }

                /// <summary>
                /// 清理任务基类
                /// </summary>
                public override void Clear()
                {
                    base.Clear();
                    mLoadAssetCallbacks = null;
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    mLoadAssetCallbacks.LoadAssetSuccessCallback?.Invoke(AssetName, asset, duration, UserData);
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status,
                    string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    mLoadAssetCallbacks.LoadAssetFailureCallback?.Invoke(AssetName, status, errorMessage, UserData);
                }

                public override void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type,
                    float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadAsset)
                    {
                        mLoadAssetCallbacks.LoadAssetUpdateCallback?.Invoke(AssetName, progress, UserData);
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName,
                    object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    mLoadAssetCallbacks.LoadAssetDependencyCallback?.Invoke(AssetName, dependencyAssetName,
                        LoadedDependencyAssetCount, TotalDependencyCount, UserData);
                }
            }
        }
    }
}