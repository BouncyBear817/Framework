// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 16:4:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            /// <summary>
            /// 加载依赖资源任务
            /// </summary>
            private sealed class LoadDependencyAssetTask : LoadResourceTaskBase
            {
                private LoadResourceTaskBase mLoadDependencyTask;

                public LoadDependencyAssetTask()
                {
                    mLoadDependencyTask = null;
                }

                /// <summary>
                /// 资源是否为场景
                /// </summary>
                public override bool IsScene => false;

                /// <summary>
                /// 创建加载依赖资源任务
                /// </summary>
                /// <param name="assetName">资源名称</param>
                /// <param name="priority">资源优先级</param>
                /// <param name="resourceInfo">资源信息</param>
                /// <param name="dependencyAssetNames">依赖资源名称集合</param>
                /// <param name="loadDependencyTask">加载依赖资源任务</param>
                /// <param name="userData">用户自定义数据</param>
                /// <returns>加载依赖资源任务</returns>
                public static LoadDependencyAssetTask Create(string assetName, int priority, ResourceInfo resourceInfo,
                    string[] dependencyAssetNames, LoadResourceTaskBase loadDependencyTask, object userData)
                {
                    var task = ReferencePool.Acquire<LoadDependencyAssetTask>();
                    task.Initialize(assetName, null, priority, resourceInfo, dependencyAssetNames, userData);
                    task.mLoadDependencyTask = loadDependencyTask;
                    task.mLoadDependencyTask.TotalDependencyCount++;
                    return task;
                }

                public override void Clear()
                {
                    base.Clear();
                    mLoadDependencyTask = null;
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    mLoadDependencyTask.OnLoadDependencyAsset(agent, AssetName, asset);
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status,
                    string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    var appendErrorMessage =
                        $"Can not load dependency asset ({AssetName}), status ({status}), error message ({errorMessage}).";
                    mLoadDependencyTask.OnLoadAssetFailure(agent, LoadResourceStatus.DependencyError,
                        appendErrorMessage);
                }
            }
        }
    }
}