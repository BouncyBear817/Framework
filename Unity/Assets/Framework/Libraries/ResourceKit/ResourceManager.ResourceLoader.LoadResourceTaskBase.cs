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
            /// 加载资源任务基类
            /// </summary>
            private abstract class LoadResourceTaskBase : TaskBase
            {
                private static int sSerial = 0;

                private string mAssetName;
                private Type mAssetType;
                private ResourceInfo mResourceInfo;
                private string[] mDependencyAssetNames;
                private readonly List<object> mDependencyAssets;
                private ResourceObject mResourceObject;
                private DateTime mStartTime;
                private int mTotalDependencyCount;


                protected LoadResourceTaskBase()
                {
                    mAssetName = null;
                    mAssetType = null;
                    mResourceInfo = null;
                    mDependencyAssetNames = null;
                    mDependencyAssets = new List<object>();
                    mResourceObject = null;
                    mStartTime = default(DateTime);
                    mTotalDependencyCount = 0;
                }

                /// <summary>
                /// 资源名称
                /// </summary>
                public string AssetName => mAssetName;

                /// <summary>
                /// 资源类型
                /// </summary>
                public Type AssetType => mAssetType;

                /// <summary>
                /// 资源信息
                /// </summary>
                public ResourceInfo ResourceInfo => mResourceInfo;

                /// <summary>
                /// 资源对象
                /// </summary>
                public ResourceObject ResourceObject => mResourceObject;

                /// <summary>
                /// 资源开始加载时间
                /// </summary>
                public DateTime StartTime
                {
                    get => mStartTime;
                    set => mStartTime = value;
                }

                /// <summary>
                /// 已加载的依赖资源数量
                /// </summary>
                public int LoadedDependencyAssetCount => mDependencyAssets.Count;

                /// <summary>
                /// 依赖资源的总数
                /// </summary>
                public int TotalDependencyCount
                {
                    get => mTotalDependencyCount;
                    set => mTotalDependencyCount = value;
                }

                /// <summary>
                /// 资源是否为场景
                /// </summary>
                public abstract bool IsScene { get; }

                /// <summary>
                /// 任务描述
                /// </summary>
                public override string Description => mAssetName;

                /// <summary>
                /// 清理任务基类
                /// </summary>
                public override void Clear()
                {
                    base.Clear();

                    mAssetName = null;
                    mAssetType = null;
                    mResourceInfo = null;
                    mDependencyAssetNames = null;
                    mDependencyAssets.Clear();
                    mResourceObject = null;
                    mStartTime = default(DateTime);
                    mTotalDependencyCount = 0;
                }

                /// <summary>
                /// 获取依赖资源名称集合
                /// </summary>
                /// <returns></returns>
                public string[] GetDependencyAssetNames() => mDependencyAssetNames;

                /// <summary>
                /// 获取依赖资源集合
                /// </summary>
                /// <returns></returns>
                public List<object> GetDependencyAssets() => mDependencyAssets;

                /// <summary>
                /// 加载资源
                /// </summary>
                /// <param name="agent">加载资源代理</param>
                /// <param name="resourceObject">资源对象</param>
                public void LoadMain(LoadResourceAgent agent, ResourceObject resourceObject)
                {
                    mResourceObject = resourceObject;
                    agent.AgentHelper.LoadAsset(resourceObject.Target, mAssetName, mAssetType, IsScene);
                }

                public virtual void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                }

                public virtual void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status,
                    string errorMessage)
                {
                }

                public virtual void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type,
                    float progress)
                {
                }

                public virtual void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName,
                    object dependencyAsset)
                {
                    mDependencyAssets.Add(dependencyAsset);
                }

                protected void Initialize(string assetName, Type assetType, int priority, ResourceInfo resourceInfo,
                    string[] dependencyAssetName, object userData)
                {
                    Initialize(++sSerial, null, priority, userData);
                    mAssetName = assetName;
                    mAssetType = assetType;
                    mResourceInfo = resourceInfo;
                    mDependencyAssetNames = dependencyAssetName;
                }
            }
        }
    }
}