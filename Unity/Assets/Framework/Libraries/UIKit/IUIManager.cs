/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 15:34:36
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 界面管理器接口
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 获取界面组数量
        /// </summary>
        int UIGroupCount { get; }

        /// <summary>
        /// 界面实例对象自动释放的间隔秒数
        /// </summary>
        float InstanceAutoReleaseInterval { get; set; }

        /// <summary>
        /// 界面实例对象池的容量
        /// </summary>
        int InstanceCapacity { get; set; }

        /// <summary>
        /// 界面实例对象池优先级
        /// </summary>
        int InstancePriority { get; set; }

        /// <summary>
        /// 打开界面成功事件
        /// </summary>
        event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess;

        /// <summary>
        /// 打开界面失败事件
        /// </summary>
        event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure;

        /// <summary>
        /// 打开界面更新事件
        /// </summary>
        event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate;

        /// <summary>
        /// 打开界面依赖资源事件
        /// </summary>
        event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset;

        /// <summary>
        /// 关闭界面完成事件
        /// </summary>
        event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete;

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置界面辅助器
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器</param>
        void SetUIFormHelper(IUIFormHelper uiFormHelper);

        /// <summary>
        /// 是否存在界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>是否存在界面组</returns>
        bool HasUIGroup(string uiGroupName);

        /// <summary>
        /// 获取指定界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>指定界面组</returns>
        IUIGroup GetUIGroup(string uiGroupName);

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <returns>所有界面组</returns>
        IUIGroup[] GetAllUIGroups();

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <param name="results">所有界面组</param>
        void GetAllUIGroups(List<IUIGroup> results);

        /// <summary>
        /// 增加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="uiGroupHelper">界面组辅助器</param>
        /// <returns>是否增加成功</returns>
        bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 增加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="uiGroupHelper">界面组辅助器</param>
        /// <returns>是否增加成功</returns>
        bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>是否存在界面</returns>
        bool HasUIForm(int serialId);

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否存在界面</returns>
        bool HasUIForm(string uiFormAssetName);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>界面</returns>
        IUIForm GetUIForm(int serialId);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        IUIForm GetUIForm(string uiFormAssetName);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        IUIForm[] GetUIForms(string uiFormAssetName);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="results">界面</param>
        void GetUIForms(string uiFormAssetName, List<IUIForm> results);

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <returns>所有已加载的界面</returns>
        IUIForm[] GetAllLoadedUIForms();

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <param name="results">所有已加载的界面</param>
        void GetAllLoadedUIForms(List<IUIForm> results);

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <returns>所有正在加载界面的序列号</returns>
        int[] GetAllLoadingUIFormSerialIds();

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <param name="results">所有正在加载界面的序列号</param>
        void GetAllLoadingUIFormSerialIds(List<int> results);

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="serialId">界面 序列号</param>
        /// <returns>是否正在加载界面</returns>
        bool IsLoadingUIForm(int serialId);

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否正在加载界面</returns>
        bool IsLoadingUIForm(string uiFormAssetName);

        /// <summary>
        /// 是否是有效的界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <returns>是否是有效的界面</returns>
        bool IsValidUIForm(IUIForm uiForm);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData = null);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="priority">界面组优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, object userData = null);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="PauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        int OpenUIForm(string uiFormAssetName, string uiGroupName, bool PauseCoveredUIForm, object userData = null);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="priority">界面组优先级</param>
        /// <param name="PauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, bool PauseCoveredUIForm,
            object userData);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <param name="userData">用户自定义数据</param>
        void CloseUIForm(int serialId, object userData = null);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        void CloseUIForm(IUIForm uiForm, object userData = null);

        /// <summary>
        /// 关闭所有已加载界面
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        void CloseAllLoadedUIForms(object userData = null);

        /// <summary>
        /// 关闭所有正在加载界面
        /// </summary>
        void CloseAllLoadingUIForms();

        /// <summary>
        /// 激活界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        void RefocusUIForm(IUIForm uiForm, object userData = null);

        /// <summary>
        /// 设置界面实例是否加锁
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="locked">是否加锁</param>
        void SetUIFormInstanceLocked(object uiFormInstance, bool locked);

        /// <summary>
        /// 设置界面实例优先级
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="priority">界面优先级</param>
        void SetUIFormInstancePriority(object uiFormInstance, int priority);
    }
}