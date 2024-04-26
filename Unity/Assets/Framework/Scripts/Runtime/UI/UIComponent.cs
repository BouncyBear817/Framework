/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/10 10:26:54
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/UI")]
    public sealed partial class UIComponent : FrameworkComponent
    {
        private IUIManager mUIManager = null;
        private EventComponent mEventComponent = null;

        [SerializeField] private bool mEnableOpenUIFormSuccessEvent = true;
        [SerializeField] private bool mEnableOpenUIFormFailureEvent = true;
        [SerializeField] private bool mEnableOpenUIFormUpdateEvent = true;
        [SerializeField] private bool mEnableOpenUIFormDependencyAssetEvent = true;
        [SerializeField] private bool mEnableCloseUIFormCompleteEvent = true;

        [SerializeField] private float mInstanceAutoReleaseInterval = 60f;
        [SerializeField] private int mInstanceCapacity = 16;
        [SerializeField] private int mInstancePriority = 0;
        [SerializeField] private Transform mInstanceRoot = null;

        [SerializeField] private string mUIFormHelperTypeName = "Runtime.DefaultUIFormHelper";
        [SerializeField] private UIFormHelperBase mCustomUIFormHelper = null;
        [SerializeField] private string mUIGroupHelperTypeName = "Runtime.DefaultUIGroupHelper";
        [SerializeField] private UIGroupHelperBase mCustomUIGroupHelper = null;
        [SerializeField] private UIGroup[] mUIGroups = null;

        /// <summary>
        /// 获取界面组数量
        /// </summary>
        public int UIGroupCount => mUIManager.UIGroupCount;

        /// <summary>
        /// 界面实例对象自动释放的间隔秒数
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get => mUIManager.InstanceAutoReleaseInterval;
            set => mUIManager.InstanceAutoReleaseInterval = mInstanceAutoReleaseInterval = value;
        }

        /// <summary>
        /// 界面实例对象池的容量
        /// </summary>
        public int InstanceCapacity
        {
            get => mUIManager.InstanceCapacity;
            set => mUIManager.InstanceCapacity = mInstanceCapacity = value;
        }

        /// <summary>
        /// 界面实例对象池优先级
        /// </summary>
        public int InstancePriority
        {
            get => mUIManager.InstancePriority;
            set => mUIManager.InstancePriority = mInstancePriority = value;
        }

        protected override void Awake()
        {
            base.Awake();

            mUIManager = FrameworkEntry.GetModule<IUIManager>();
            if (mUIManager == null)
            {
                Log.Error("UI manager is invalid.");
                return;
            }

            if (mEnableOpenUIFormSuccessEvent)
            {
                mUIManager.OpenUIFormSuccess += OnOpenUIFormSuccess;
            }

            if (mEnableOpenUIFormFailureEvent)
            {
                mUIManager.OpenUIFormFailure += OnOpenUIFormFailure;
            }

            if (mEnableOpenUIFormUpdateEvent)
            {
                mUIManager.OpenUIFormUpdate += OnOpenUIFormUpdate;
            }

            if (mEnableOpenUIFormDependencyAssetEvent)
            {
                mUIManager.OpenUIFormDependencyAsset += OnOpenUIFormDependencyAsset;
            }

            if (mEnableCloseUIFormCompleteEvent)
            {
                mUIManager.CloseUIFormComplete += OnCloseUIFormComplete;
            }
        }

        private void Start()
        {
            var baseComponent = MainEntryHelper.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Error("Base component is invalid.");
                return;
            }

            var eventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (eventComponent == null)
            {
                Log.Error("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
            }
            else
            {
                mUIManager.SetResourceManager(FrameworkEntry.GetModule<IResourceManager>());
            }

            mUIManager.SetObjectPoolManager(FrameworkEntry.GetModule<IObjectPoolManager>());
            mUIManager.InstanceAutoReleaseInterval = mInstanceAutoReleaseInterval;
            mUIManager.InstanceCapacity = mInstanceCapacity;
            mUIManager.InstancePriority = mInstancePriority;

            var uiFormHelper = Helper.CreateHelper(mUIFormHelperTypeName, mCustomUIFormHelper);
            if (uiFormHelper == null)
            {
                Log.Error("Can not create UI form helper.");
                return;
            }

            uiFormHelper.name = "UI Form Helper";
            var trans = uiFormHelper.transform;
            trans.SetParent(this.transform);
            trans.localScale = Vector3.one;
            mUIManager.SetUIFormHelper(uiFormHelper);

            if (mInstanceRoot == null)
            {
                mInstanceRoot = new GameObject("UI Form Instances").transform;
                mInstanceRoot.SetParent(gameObject.transform);
                mInstanceRoot.localScale = Vector3.one;
                mInstanceRoot.gameObject.layer = LayerMask.NameToLayer("UI");
            }

            foreach (var uiGroup in mUIGroups)
            {
                if (!AddUIGroup(uiGroup.Name, uiGroup.Depth))
                {
                    Log.Warning($"Add UI group ({uiGroup.Name}) failure.");
                    continue;
                }
            }
        }

        /// <summary>
        /// 是否存在界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>是否存在界面组</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            return mUIManager.HasUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取指定界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>指定界面组</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            return mUIManager.GetUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <returns>所有界面组</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            return mUIManager.GetAllUIGroups();
        }

        /// <summary>
        /// 获取所有界面组
        /// </summary>
        /// <param name="results">所有界面组</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            mUIManager.GetAllUIGroups(results);
        }

        /// <summary>
        /// 增加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <returns>是否增加成功</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth = 0)
        {
            if (mUIManager.HasUIGroup(uiGroupName))
            {
                return false;
            }

            var uiGroupHelper = Helper.CreateHelper(mUIGroupHelperTypeName, mCustomUIGroupHelper, UIGroupCount);
            if (uiGroupHelper == null)
            {
                Log.Error("Can not create UI group helper.");
                return false;
            }

            uiGroupHelper.name = $"UI Group - {uiGroupName}";
            uiGroupHelper.gameObject.layer = LayerMask.NameToLayer("UI");
            var trans = uiGroupHelper.transform;
            trans.SetParent(mInstanceRoot);
            transform.localScale = Vector3.one;

            return mUIManager.AddUIGroup(uiGroupName, uiGroupDepth, uiGroupHelper);
        }

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>是否存在界面</returns>
        public bool HasUIForm(int serialId)
        {
            return mUIManager.HasUIForm(serialId);
        }

        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否存在界面</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            return mUIManager.HasUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>界面</returns>
        public UIForm GetUIForm(int serialId)
        {
            return mUIManager.GetUIForm(serialId) as UIForm;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        public UIForm GetUIForm(string uiFormAssetName)
        {
            return mUIManager.GetUIForm(uiFormAssetName) as UIForm;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面</returns>
        public UIForm[] GetUIForms(string uiFormAssetName)
        {
            var uiForms = mUIManager.GetUIForms(uiFormAssetName);
            var uiFormImpls = new UIForm[uiForms.Length];
            for (var i = 0; i < uiForms.Length; i++)
            {
                uiFormImpls[i] = uiForms[i] as UIForm;
            }

            return uiFormImpls;
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="results">界面</param>
        public void GetUIForms(string uiFormAssetName, List<UIForm> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            var internalResults = new List<IUIForm>();
            mUIManager.GetUIForms(uiFormAssetName, internalResults);
            foreach (var uiForm in internalResults)
            {
                results.Add(uiForm as UIForm);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <returns>所有已加载的界面</returns>
        public UIForm[] GetAllLoadedUIForms()
        {
            var uiForms = mUIManager.GetAllLoadedUIForms();
            var uiFormImpls = new UIForm[uiForms.Length];
            for (var i = 0; i < uiForms.Length; i++)
            {
                uiFormImpls[i] = uiForms[i] as UIForm;
            }

            return uiFormImpls;
        }

        /// <summary>
        /// 获取所有已加载的界面
        /// </summary>
        /// <param name="results">所有已加载的界面</param>
        public void GetAllLoadedUIForms(List<UIForm> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            var internalResults = new List<IUIForm>();
            mUIManager.GetAllLoadedUIForms(internalResults);
            foreach (var uiForm in internalResults)
            {
                results.Add(uiForm as UIForm);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <returns>所有正在加载界面的序列号</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            return mUIManager.GetAllLoadingUIFormSerialIds();
        }

        /// <summary>
        /// 获取所有正在加载界面的序列号
        /// </summary>
        /// <param name="results">所有正在加载界面的序列号</param>
        public void GetAllLoadingUIFormSerialIds(List<int> results)
        {
            mUIManager.GetAllLoadingUIFormSerialIds(results);
        }

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="serialId">界面 序列号</param>
        /// <returns>是否正在加载界面</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return mUIManager.IsLoadingUIForm(serialId);
        }

        /// <summary>
        /// 是否正在加载界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>是否正在加载界面</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            return mUIManager.IsLoadingUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 是否是有效的界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <returns>是否是有效的界面</returns>
        public bool IsValidUIForm(UIForm uiForm)
        {
            return mUIManager.IsValidUIForm(uiForm);
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
            return mUIManager.OpenUIForm(uiFormAssetName, uiGroupName, priority, PauseCoveredUIForm, userData);
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <param name="userData">用户自定义数据</param>
        public void CloseUIForm(int serialId, object userData = null)
        {
            mUIManager.CloseUIForm(serialId, userData);
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        public void CloseUIForm(UIForm uiForm, object userData = null)
        {
            mUIManager.CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭所有已加载界面
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        public void CloseAllLoadedUIForms(object userData = null)
        {
            mUIManager.CloseAllLoadedUIForms(userData);
        }

        /// <summary>
        /// 关闭所有正在加载界面
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            mUIManager.CloseAllLoadingUIForms();
        }

        /// <summary>
        /// 激活界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        /// <param name="userData">用户自定义数据</param>
        public void RefocusUIForm(UIForm uiForm, object userData = null)
        {
            mUIManager.RefocusUIForm(uiForm, userData);
        }

        /// <summary>
        /// 设置界面实例是否加锁
        /// </summary>
        /// <param name="uiForm">界面实例</param>
        /// <param name="locked">是否加锁</param>
        public void SetUIFormInstanceLocked(UIForm uiForm, bool locked)
        {
            if (uiForm == null)
            {
                Log.Warning("UI form is invalid.");
                return;
            }

            mUIManager.SetUIFormInstanceLocked(uiForm, locked);
        }

        /// <summary>
        /// 设置界面实例优先级
        /// </summary>
        /// <param name="uiForm">界面实例</param>
        /// <param name="priority">界面优先级</param>
        public void SetUIFormInstancePriority(UIForm uiForm, int priority)
        {
            if (uiForm == null)
            {
                Log.Warning("UI form is invalid.");
                return;
            }

            mUIManager.SetUIFormInstancePriority(uiForm, priority);
        }

        private void OnOpenUIFormSuccess(object sender, Framework.OpenUIFormSuccessEventArgs e)
        {
            mEventComponent.Fire(this, OpenUIFormSuccessEventArgs.Create(e));
        }

        private void OnOpenUIFormFailure(object sender, Framework.OpenUIFormFailureEventArgs e)
        {
            Log.Warning(
                $"Open UI form failure, asset name is ({e.UIFormAssetName}), ui group name is ({e.UIGroupName}), error message is ({e.ErrorMessage}).");
            mEventComponent.Fire(this, OpenUIFormFailureEventArgs.Create(e));
        }

        private void OnOpenUIFormUpdate(object sender, Framework.OpenUIFormUpdateEventArgs e)
        {
            mEventComponent.Fire(this, OpenUIFormUpdateEventArgs.Create(e));
        }

        private void OnOpenUIFormDependencyAsset(object sender, Framework.OpenUIFormDependencyAssetEventArgs e)
        {
            mEventComponent.Fire(this, OpenUIFormDependencyAssetEventArgs.Create(e));
        }

        private void OnCloseUIFormComplete(object sender, Framework.CloseUIFormCompleteEventArgs e)
        {
            mEventComponent.Fire(this, CloseUIFormCompleteEventArgs.Create(e));
        }
    }
}