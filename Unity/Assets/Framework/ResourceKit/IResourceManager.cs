/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 资源管理器接口
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 资源常量
        /// </summary>
        ResourceConstant ResourceConstant { get; }
        
        /// <summary>
        /// 资源数量
        /// </summary>
        int AssetCount { get; }
        
        string Variant { get; }
        
        /// <summary>
        /// 资源对象池信息
        /// </summary>
        ObjectPoolInfo AssetObjectPoolInfo { get; set; }
        
        /// <summary>
        /// 资源数量
        /// </summary>
        int ResourceCount { get; }
        
        /// <summary>
        /// 资源组数量
        /// </summary>
        int ResourceGroupCount { get; }
        
        /// <summary>
        /// 资源对象池信息
        /// </summary>
        ObjectPoolInfo ResourceObjectPoolInfo { get; set; }

        event EventHandler<ResourceVerifyStartEventArgs> ResourceVerifyStart;
        
        event EventHandler<ResourceVerifySuccessEventArgs> ResourceVerifySuccess;
        
        event EventHandler<ResourceVerifyFailureEventArgs> ResourceVerifyFailure;
        
        event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;
        
        event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;
        
        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;
        
        event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;
        
        event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;
        
        event EventHandler<ResourceApplyStartEventArgs> ResourceApplyStart;
        
        event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;
        
        event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager);
    }
}