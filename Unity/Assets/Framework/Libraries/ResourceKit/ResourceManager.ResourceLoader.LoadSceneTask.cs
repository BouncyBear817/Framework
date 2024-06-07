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
            /// 加载场景任务
            /// </summary>
            private sealed class LoadSceneTask : LoadResourceTaskBase
            {
                private LoadSceneCallbacks mLoadSceneCallbacks;

                public LoadSceneTask()
                {
                    mLoadSceneCallbacks = null;
                }

                /// <summary>
                /// 资源是否为场景
                /// </summary>
                public override bool IsScene => true;

                /// <summary>
                /// 创建加载场景任务
                /// </summary>
                /// <param name="assetName">资源名称</param>
                /// <param name="priority">资源优先级</param>
                /// <param name="resourceInfo">资源信息</param>
                /// <param name="dependencyAssetNames">依赖资源名称集合</param>
                /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
                /// <param name="userData">用户自定义数据</param>
                /// <returns>加载场景任务</returns>
                public static LoadSceneTask Create(string assetName, int priority, ResourceInfo resourceInfo,
                    string[] dependencyAssetNames, LoadSceneCallbacks loadSceneCallbacks, object userData)
                {
                    var task = ReferencePool.Acquire<LoadSceneTask>();
                    task.Initialize(assetName, null, priority, resourceInfo, dependencyAssetNames, userData);
                    task.mLoadSceneCallbacks = loadSceneCallbacks;
                    return task;
                }

                /// <summary>
                /// 清理任务基类
                /// </summary>
                public override void Clear()
                {
                    base.Clear();
                    mLoadSceneCallbacks = null;
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    mLoadSceneCallbacks.LoadSceneSuccessCallback?.Invoke(AssetName, duration, UserData);
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status,
                    string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    mLoadSceneCallbacks.LoadSceneFailureCallback?.Invoke(AssetName, status, errorMessage, UserData);
                }

                public override void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type,
                    float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadScene)
                    {
                        mLoadSceneCallbacks.LoadSceneUpdateCallback?.Invoke(AssetName, progress, UserData);
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName,
                    object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    mLoadSceneCallbacks.LoadSceneDependencyAssetCallback?.Invoke(AssetName, dependencyAssetName,
                        LoadedDependencyAssetCount, TotalDependencyCount, UserData);
                }
            }
        }
    }
}