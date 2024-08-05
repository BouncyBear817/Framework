/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 16:48:55
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// Web请求组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Web Request")]
    public sealed class WebRequestComponent : FrameworkComponent
    {
        private IWebRequestManager mWebRequestManager = null;
        private EventComponent mEventComponent = null;

        [SerializeField] private Transform mInstanceRoot = null;
        [SerializeField] private string mWebRequestAgentHelperTypeName = "Framework.Runtime.UnityWebRequestAgentHelper";
        [SerializeField] private WebRequestAgentHelperBase mCustomWebRequestAgentHelper = null;
        [SerializeField] private int mWebRequestAgentHelperCount = 1;
        [SerializeField] private float mTimeout = 30f;


        /// <summary>
        /// Web请求总代理数量
        /// </summary>
        public int TotalAgentCount => mWebRequestManager.TotalAgentCount;

        /// <summary>
        /// Web请求空闲代理数量
        /// </summary>
        public int AvailableAgentCount => mWebRequestManager.AvailableAgentCount;

        /// <summary>
        /// Web请求工作中代理数量
        /// </summary>
        public int WorkingAgentCount => mWebRequestManager.WorkingAgentCount;

        /// <summary>
        /// 等待任务数量
        /// </summary>
        public int WaitingTaskCount => mWebRequestManager.WaitingTaskCount;

        /// <summary>
        /// Web请求超时事件
        /// </summary>
        public float Timeout
        {
            get => mWebRequestManager.Timeout;
            set => mWebRequestManager.Timeout = mTimeout = value;
        }

        protected override void Awake()
        {
            base.Awake();

            mWebRequestManager = FrameworkEntry.GetModule<IWebRequestManager>();
            if (mWebRequestManager == null)
            {
                Log.Fatal("Web request manager is invalid.");
                return;
            }

            mWebRequestManager.Timeout = mTimeout;
            mWebRequestManager.WebRequestStart += OnWebRequestStart;
            mWebRequestManager.WebRequestSuccess += OnWebRequestSuccess;
            mWebRequestManager.WebRequestFailure += OnWebRequestFailure;
        }

        private void Start()
        {
            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (mInstanceRoot == null)
            {
                mInstanceRoot = new GameObject("Web Request Agent Instances").transform;
                mInstanceRoot.SetParent(gameObject.transform);
                mInstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < mWebRequestAgentHelperCount; i++)
            {
                AddWebRequestAgentHelper(i);
            }
        }

        /// <summary>
        /// 根据任务序列编号获取Web请求的任务信息
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>Web请求的任务信息</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return mWebRequestManager.GetWebRequestInfo(serialId);
        }

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>Web请求的任务信息</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return mWebRequestManager.GetWebRequestInfos(tag);
        }

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <param name="results">Web请求的任务信息</param>
        public void GetWebRequestInfos(string tag, List<TaskInfo> results)
        {
            mWebRequestManager.GetWebRequestInfos(tag, results);
        }

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <returns>所有Web请求的任务信息</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return mWebRequestManager.GetAllWebRequestInfos();
        }

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <param name="results">所有Web请求的任务信息</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            mWebRequestManager.GetAllWebRequestInfos(results);
        }

        /// <summary>
        /// 增加Web请求
        /// </summary>
        /// <param name="webRequestInfo">Web请求信息</param>
        /// <param name="wwwForm">WWW 表单</param>
        /// <returns>任务序列编号</returns>
        public int AddWebRequest(WebRequestInfo webRequestInfo, WWWForm wwwForm = null)
        {
            if (webRequestInfo == null)
            {
                Log.Error("Web request info is invalid.");
                return -1;
            }

            if (wwwForm != null)
            {
                webRequestInfo.UserData = WWWFormInfo.Create(wwwForm, webRequestInfo.UserData);
            }

            return mWebRequestManager.AddWebRequest(webRequestInfo);
        }

        /// <summary>
        /// 根据任务序列编号获取Web请求
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>是否成功移除Web请求</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return mWebRequestManager.RemoveWebRequest(serialId);
        }

        /// <summary>
        /// 根据任务标签获取Web请求
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除Web请求的数量</returns>
        public int RemoveWebRequests(string tag)
        {
            return mWebRequestManager.RemoveWebRequests(tag);
        }

        /// <summary>
        /// 移除所有Web请求
        /// </summary>
        /// <returns>移除Web请求的数量</returns>
        public int RemoveAllWebRequests()
        {
            return mWebRequestManager.RemoveAllWebRequests();
        }

        /// <summary>
        /// 增加Web请求代理辅助器
        /// </summary>
        /// <param name="index">Web请求代理辅助器索引</param>
        private void AddWebRequestAgentHelper(int index)
        {
            var webRequestAgentHelper =
                Helper.CreateHelper(mWebRequestAgentHelperTypeName, mCustomWebRequestAgentHelper);
            if (webRequestAgentHelper == null)
            {
                Log.Error("Can not create web request agent helper.");
                return;
            }

            webRequestAgentHelper.name = $"Web Request Agent Helper - {index}";
            var trans = webRequestAgentHelper.transform;
            trans.SetParent(mInstanceRoot);
            trans.localScale = Vector3.one;

            mWebRequestManager.AddWebRequestAgentHelper(webRequestAgentHelper);
        }

        private void OnWebRequestStart(object sender, Framework.WebRequestStartEventArgs e)
        {
            mEventComponent.FireNow(sender, WebRequestStartEventArgs.Create(e));
        }

        private void OnWebRequestSuccess(object sender, Framework.WebRequestSuccessEventArgs e)
        {
            Log.Info($"Web request success, Response data : \n{Utility.Converter.GetString(e.WebResponseBytes)}");
            mEventComponent.FireNow(sender, WebRequestSuccessEventArgs.Create(e));
        }

        private void OnWebRequestFailure(object sender, Framework.WebRequestFailureEventArgs e)
        {
            Log.Warning(
                $"Web request failure, web request serial id ({e.SerialId}), web request uri ({e.WebRequestUri}), error message ({e.ErrorMessage})");
            mEventComponent.FireNow(sender, WebRequestFailureEventArgs.Create(e));
        }
    }
}