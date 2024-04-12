/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 16:14:49
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 界面管理器
    /// </summary>
    public sealed partial class UIManager : FrameworkModule, IUIManager
    {
        private readonly Dictionary<string, UIGroup> mUIGroups;
        private readonly Dictionary<int, string> mUIFormsBeingLoaded;
        private readonly HashSet<int> mUIFormToReleaseOnLoad;
        private readonly Queue<IUIForm> mRecycleUIFormQueue;
        private readonly LoadAssetCallbacks mLoadAssetCallbacks;

        private IObjectPoolManager mObjectPoolManager;
        private IResourceManager mResourceManager;
        private IObjectPool<UIFormInstanceObject> mInstanceObjectPool;
        private IUIFormHelper mUIFormHelper;

        private int mSerial;
        private bool mIsShutdown;

        private EventHandler<OpenUIFormSuccessEventArgs> mOpenUIFormSuccessEventHandler;
        private EventHandler<OpenUIFormFailureEventArgs> mOpenUIFormFailureEventHandler;
        private EventHandler<OpenUIFormUpdateEventArgs> mOpenUIFormUpdateEventHandler;
        private EventHandler<OpenUIFormDependencyAssetEventArgs> mOpenUIFormDependencyAssetEventHandler;
        private EventHandler<CloseUIFormCompleteEventArgs> mCloseUIFormCompleteEventHandler;

        public UIManager()
        {
            mUIGroups = new Dictionary<string, UIGroup>();
            mUIFormsBeingLoaded = new Dictionary<int, string>();
            mUIFormToReleaseOnLoad = new HashSet<int>();
            mRecycleUIFormQueue = new Queue<IUIForm>();
            mLoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback,
                LoadAssetUpdateCallback, LoadAssetDependencyCallback);

            mObjectPoolManager = null;
            mResourceManager = null;
            mInstanceObjectPool = null;
            mUIFormHelper = null;

            mSerial = 0;
            mIsShutdown = false;
            mOpenUIFormSuccessEventHandler = null;
            mOpenUIFormFailureEventHandler = null;
            mOpenUIFormUpdateEventHandler = null;
            mOpenUIFormDependencyAssetEventHandler = null;
            mCloseUIFormCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取界面组数量
        /// </summary>
        public int UIGroupCount => mUIGroups.Count;

        /// <summary>
        /// 界面实例对象自动释放的间隔秒数
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get => mInstanceObjectPool.AutoReleaseInterval;
            set => mInstanceObjectPool.AutoReleaseInterval = value;
        }

        /// <summary>
        /// 界面实例对象池的容量
        /// </summary>
        public int InstanceCapacity
        {
            get => mInstanceObjectPool.Capacity;
            set => mInstanceObjectPool.Capacity = value;
        }

        /// <summary>
        /// 界面实例对象池优先级
        /// </summary>
        public int InstancePriority
        {
            get => mInstanceObjectPool.Priority;
            set => mInstanceObjectPool.Priority = value;
        }

        /// <summary>
        /// 打开界面成功事件
        /// </summary>
        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add => mOpenUIFormSuccessEventHandler += value;
            remove => mOpenUIFormSuccessEventHandler -= value;
        }

        /// <summary>
        /// 打开界面失败事件
        /// </summary>
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add => mOpenUIFormFailureEventHandler += value;
            remove => mOpenUIFormFailureEventHandler -= value;
        }

        /// <summary>
        /// 打开界面更新事件
        /// </summary>
        public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
        {
            add => mOpenUIFormUpdateEventHandler += value;
            remove => mOpenUIFormUpdateEventHandler -= value;
        }

        /// <summary>
        /// 打开界面依赖资源事件
        /// </summary>
        public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
        {
            add => mOpenUIFormDependencyAssetEventHandler += value;
            remove => mOpenUIFormDependencyAssetEventHandler -= value;
        }

        /// <summary>
        /// 关闭界面完成事件
        /// </summary>
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add => mCloseUIFormCompleteEventHandler += value;
            remove => mCloseUIFormCompleteEventHandler -= value;
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (mRecycleUIFormQueue.Count > 0)
            {
                var uiForm = mRecycleUIFormQueue.Dequeue();
                uiForm.OnRecycle();
                mInstanceObjectPool.UnSpawn(uiForm.Handle);
            }

            foreach (var (_, uiGroup) in mUIGroups)
            {
                uiGroup.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            mIsShutdown = true;
            CloseAllLoadedUIForms();
            mUIGroups.Clear();
            mUIFormsBeingLoaded.Clear();
            mUIFormToReleaseOnLoad.Clear();
            mRecycleUIFormQueue.Clear();
        }

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new Exception("Object pool manager is invalid.");
            }

            mObjectPoolManager = objectPoolManager;
            mInstanceObjectPool =
                mObjectPoolManager.SpawnObjectPool<UIFormInstanceObject>(new ObjectPoolInfo("UI Instance Pool"));
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            mResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置界面辅助器
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            if (uiFormHelper == null)
            {
                throw new Exception("UI form helper is invalid.");
            }

            mUIFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 是否存在界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>是否存在界面组</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            return mUIGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取指定界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>指定界面组</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            return mUIGroups.GetValueOrDefault(uiGroupName);
        }

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <returns>所有界面组</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            var index = 0;
            var results = new IUIGroup[mUIGroups.Count];
            foreach (var (_, uiGroup) in mUIGroups)
            {
                results[index++] = uiGroup;
            }

            return results;
        }

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <param name="results">所有界面组</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, uiGroup) in mUIGroups)
            {
                results.Add(uiGroup);
            }
        }

        /// <summary>
        /// 增加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="uiGroupHelper">界面组辅助器</param>
        /// <returns>是否增加成功</returns>
        public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="uiGroupHelper">界面组辅助器</param>
        /// <returns>是否增加成功</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                throw new Exception("UI group helper is invalid.");
            }

            mUIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>是否存在界面</returns>
        public bool HasUIForm(int serialId)
        {
            foreach (var (_, uiGroup) in mUIGroups)
            {
                if (uiGroup.HasUIForm(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否存在界面</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            foreach (var (_, uiGroup) in mUIGroups)
            {
                if (uiGroup.HasUIForm(uiFormAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>界面</returns>
        public IUIForm GetUIForm(int serialId)
        {
            foreach (var (_, uiGroup) in mUIGroups)
            {
                var uiForm = uiGroup.GetUIForm(serialId);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        public IUIForm GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            foreach (var (_, uiGroup) in mUIGroups)
            {
                var uiForm = uiGroup.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        public IUIForm[] GetUIForms(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            var results = new List<IUIForm>();
            foreach (var (_, uiGroup) in mUIGroups)
            {
                results.AddRange(uiGroup.GetUIForms(uiFormAssetName));
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="results">界面</param>
        public void GetUIForms(string uiFormAssetName, List<IUIForm> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            results.Clear();
            foreach (var (_, uiGroup) in mUIGroups)
            {
                uiGroup.InternalGetUIForms(uiFormAssetName, results);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <returns>所有已加载的界面</returns>
        public IUIForm[] GetAllLoadedUIForms()
        {
            var results = new List<IUIForm>();
            foreach (var (_, uiGroup) in mUIGroups)
            {
                results.AddRange(uiGroup.GetAllUIForms());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <param name="results">所有已加载的界面</param>
        public void GetAllLoadedUIForms(List<IUIForm> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, uiGroup) in mUIGroups)
            {
                uiGroup.InternalGetAllUIForms(results);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <returns>所有正在加载界面的序列号</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            var index = 0;
            var results = new int[mUIFormsBeingLoaded.Count];
            foreach (var (id, _) in mUIFormsBeingLoaded)
            {
                results[index++] = id;
            }

            return results;
        }

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <param name="results">所有正在加载界面的序列号</param>
        public void GetAllLoadingUIFormSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (id, _) in mUIFormsBeingLoaded)
            {
                results.Add(id);
            }
        }

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="serialId">界面 序列号</param>
        /// <returns>是否正在加载界面</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return mUIFormsBeingLoaded.ContainsKey(serialId);
        }

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否正在加载界面</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            return mUIFormsBeingLoaded.ContainsValue(uiFormAssetName);
        }

        /// <summary>
        /// 是否是有效的界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <returns>是否是有效的界面</returns>
        public bool IsValidUIForm(IUIForm uiForm)
        {
            if (uiForm == null)
            {
                throw new Exception("UI form is invalid.");
            }

            return HasUIForm(uiForm.SerialId);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, 0, false, userData);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="priority">界面组优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, object userData = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, priority, false, userData);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="PauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool PauseCoveredUIForm,
            object userData = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, 0, PauseCoveredUIForm, userData);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="priority">界面组优先级</param>
        /// <param name="PauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列号</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, bool PauseCoveredUIForm,
            object userData)
        {
            if (mResourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            if (mUIFormHelper == null)
            {
                throw new Exception("UI form helper is invalid.");
            }

            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            var uiGroup = GetUIGroup(uiGroupName) as UIGroup;
            if (uiGroup == null)
            {
                throw new Exception($"UI group ({uiGroupName}) is not exist.");
            }

            var serialId = ++mSerial;
            var uiFormInstanceObject = mInstanceObjectPool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                mUIFormsBeingLoaded.Add(serialId, uiFormAssetName);
                mResourceManager.LoadAsset(
                    new LoadAssetInfo(uiFormAssetName, priority,
                        OpenUIFormInfo.Create(serialId, uiGroup, PauseCoveredUIForm, userData)), mLoadAssetCallbacks);
            }
            else
            {
                InternalOpenUIForm(serialId, uiFormAssetName, uiGroup, uiFormInstanceObject.Target, PauseCoveredUIForm,
                    false, 0f, userData);
            }

            return serialId;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <param name="userData">用户自定义数据</param>
        public void CloseUIForm(int serialId, object userData = null)
        {
            if (IsLoadingUIForm(serialId))
            {
                mUIFormToReleaseOnLoad.Add(serialId);
                mUIFormsBeingLoaded.Remove(serialId);
                return;
            }

            var uiForm = GetUIForm(serialId);
            CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        public void CloseUIForm(IUIForm uiForm, object userData = null)
        {
            if (uiForm == null)
            {
                throw new Exception("UI form is invalid.");
            }

            var uiGroup = uiForm.UIGroup as UIGroup;
            if (uiGroup == null)
            {
                throw new Exception("UI group is invalid.");
            }

            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(mIsShutdown, userData);
            uiGroup.Refresh();

            if (mCloseUIFormCompleteEventHandler != null)
            {
                var eventArgs =
                    CloseUIFormCompleteEventArgs.Create(uiForm.SerialId, uiForm.UIFormAssetName, uiGroup, userData);
                mCloseUIFormCompleteEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }

            mRecycleUIFormQueue.Enqueue(uiForm);
        }

        /// <summary>
        /// 关闭所有已加载界面
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        public void CloseAllLoadedUIForms(object userData = null)
        {
            var uiForms = GetAllLoadedUIForms();
            foreach (var uiForm in uiForms)
            {
                if (!HasUIForm(uiForm.SerialId))
                {
                    continue;
                }

                CloseUIForm(uiForm, userData);
            }
        }

        /// <summary>
        /// 关闭所有正在加载界面
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            foreach (var (serialId, _) in mUIFormsBeingLoaded)
            {
                mUIFormToReleaseOnLoad.Add(serialId);
            }

            mUIFormsBeingLoaded.Clear();
        }

        /// <summary>
        /// 激活界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        public void RefocusUIForm(IUIForm uiForm, object userData = null)
        {
            if (uiForm == null)
            {
                throw new Exception("UI form is invalid.");
            }

            var uiGroup = uiForm.UIGroup as UIGroup;
            if (uiGroup == null)
            {
                throw new Exception("UI group is invalid.");
            }

            uiGroup.RefocusUIForm(uiForm, userData);
            uiGroup.Refresh();
            uiForm.OnRefocus(userData);
        }

        /// <summary>
        /// 设置界面实例是否加锁
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="locked">是否加锁</param>
        public void SetUIFormInstanceLocked(object uiFormInstance, bool locked)
        {
            if (uiFormInstance == null)
            {
                throw new Exception("UI form instance is invalid.");
            }

            mInstanceObjectPool.SetLocked(uiFormInstance, locked);
        }

        /// <summary>
        /// 设置界面实例优先级
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="priority">界面优先级</param>
        public void SetUIFormInstancePriority(object uiFormInstance, int priority)
        {
            if (uiFormInstance == null)
            {
                throw new Exception("UI form instance is invalid.");
            }

            mInstanceObjectPool.SetPriority(uiFormInstance, priority);
        }

        private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance,
            bool pauseCoveredUIForm, bool isNewInstance, float duration, object userData)
        {
            try
            {
                var uiForm = mUIFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
                if (uiForm == null)
                {
                    throw new Exception("Can not create UI form in UI form helper.");
                }

                uiForm.OnInit(serialId, uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData);
                uiGroup.Refresh();

                if (mOpenUIFormSuccessEventHandler != null)
                {
                    var eventArgs =
                        OpenUIFormSuccessEventArgs.Create(uiForm, duration, userData);
                    mOpenUIFormSuccessEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
            catch (Exception e)
            {
                if (mOpenUIFormFailureEventHandler != null)
                {
                    var eventArgs =
                        OpenUIFormFailureEventArgs.Create(serialId, uiFormAssetName, uiGroup.Name, pauseCoveredUIForm,
                            e.ToString(), userData);
                    mOpenUIFormFailureEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }

                throw;
            }
        }

        private void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            var openUIFormInfo = userData as OpenUIFormInfo;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (mUIFormToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                mUIFormToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                ReferencePool.Release(openUIFormInfo);
                mUIFormHelper.ReleaseUIForm(asset, null);
                return;
            }

            mUIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            var uiFormInstanceObject = UIFormInstanceObject.Create(assetName, asset,
                mUIFormHelper.InstantiateUIForm(asset), mUIFormHelper);
            mInstanceObjectPool.Register(uiFormInstanceObject, true);

            InternalOpenUIForm(openUIFormInfo.SerialId, assetName, openUIFormInfo.UIGroup, uiFormInstanceObject.Target,
                openUIFormInfo.PauseCoveredUIForm, true, duration, userData);
            ReferencePool.Release(openUIFormInfo);
        }

        private void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage,
            object userData)
        {
            var openUIFormInfo = userData as OpenUIFormInfo;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (mUIFormToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                mUIFormToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                return;
            }

            mUIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            var AppendErrorMessage =
                $"Load UI form failure, asset name ({assetName}), status ({status}), error message ({errorMessage}).";
            if (mOpenUIFormFailureEventHandler != null)
            {
                var eventArgs =
                    OpenUIFormFailureEventArgs.Create(openUIFormInfo.SerialId, assetName, openUIFormInfo.UIGroup.Name,
                        openUIFormInfo.PauseCoveredUIForm,
                        AppendErrorMessage, userData);
                mOpenUIFormFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
                return;
            }

            throw new Exception(AppendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string assetName, float progress, object userData)
        {
            var openUIFormInfo = userData as OpenUIFormInfo;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (mOpenUIFormUpdateEventHandler != null)
            {
                var eventArgs =
                    OpenUIFormUpdateEventArgs.Create(openUIFormInfo.SerialId, assetName, openUIFormInfo.UIGroup.Name,
                        openUIFormInfo.PauseCoveredUIForm, progress, userData);
                mOpenUIFormUpdateEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadAssetDependencyCallback(string assetName, string dependencyAssetName, int loadedCount,
            int totalCount, object userData)
        {
            var openUIFormInfo = userData as OpenUIFormInfo;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (mOpenUIFormDependencyAssetEventHandler != null)
            {
                var eventArgs =
                    OpenUIFormDependencyAssetEventArgs.Create(openUIFormInfo.SerialId, assetName,
                        openUIFormInfo.UIGroup.Name,
                        openUIFormInfo.PauseCoveredUIForm, loadedCount, totalCount, userData);
                mOpenUIFormDependencyAssetEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}