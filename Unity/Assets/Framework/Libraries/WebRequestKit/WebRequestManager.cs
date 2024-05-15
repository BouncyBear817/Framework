// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/13 15:18:58
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class WebRequestManager : FrameworkModule, IWebRequestManager
    {
        private readonly TaskPool<WebRequestTask> mTaskPool;
        private float mTimeout;
        private EventHandler<WebRequestStartEventArgs> mWebRequestStartEventHandler;
        private EventHandler<WebRequestSuccessEventArgs> mWebRequestSuccessEventHandler;
        private EventHandler<WebRequestFailureEventArgs> mWebRequestFailureEventHandler;

        public WebRequestManager()
        {
            mTaskPool = new TaskPool<WebRequestTask>();
            mTimeout = 30f;
            mWebRequestStartEventHandler = null;
            mWebRequestSuccessEventHandler = null;
            mWebRequestFailureEventHandler = null;
        }


        /// <summary>
        /// Web请求总代理数量
        /// </summary>
        public int TotalAgentCount => mTaskPool.TotalAgentsCount;

        /// <summary>
        /// Web请求空闲代理数量
        /// </summary>
        public int FreeAgentCount => mTaskPool.AvailableAgentsCount;

        /// <summary>
        /// Web请求工作中代理数量
        /// </summary>
        public int WorkingAgentCount => mTaskPool.WorkingAgentsCount;

        /// <summary>
        /// 等待任务数量
        /// </summary>
        public int WaitingTaskCount => mTaskPool.WaitingTasksCount;

        /// <summary>
        /// Web请求超时事件
        /// </summary>
        public float Timeout
        {
            get => mTimeout;
            set => mTimeout = value;
        }

        /// <summary>
        /// Web请求开始事件
        /// </summary>
        public event EventHandler<WebRequestStartEventArgs> WebRequestStart
        {
            add => mWebRequestStartEventHandler += value;
            remove => mWebRequestStartEventHandler -= value;
        }

        /// <summary>
        /// Web请求成功事件
        /// </summary>
        public event EventHandler<WebRequestSuccessEventArgs> WebRequestSuccess
        {
            add => mWebRequestSuccessEventHandler += value;
            remove => mWebRequestSuccessEventHandler -= value;
        }

        /// <summary>
        /// Web请求失败事件
        /// </summary>
        public event EventHandler<WebRequestFailureEventArgs> WebRequestFailure
        {
            add => mWebRequestFailureEventHandler += value;
            remove => mWebRequestFailureEventHandler -= value;
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mTaskPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            mTaskPool.Shutdown();
        }

        /// <summary>
        /// 增加Web请求代理辅助器
        /// </summary>
        /// <param name="webRequestAgentHelper">Web请求代理辅助器</param>
        public void AddWebRequestAgentHelper(IWebRequestAgentHelper webRequestAgentHelper)
        {
            var agent = new WebRequestAgent(webRequestAgentHelper);
            agent.WebRequestAgentStart += OnWebRequestAgentStart;
            agent.WebRequestAgentSuccess += OnWebRequestAgentSuccess;
            agent.WebRequestAgentFailure += OnWebRequestAgentFailure;

            mTaskPool.AddAgent(agent);
        }

        /// <summary>
        /// 根据任务序列编号获取Web请求的任务信息
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>Web请求的任务信息</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return mTaskPool.GetTaskInfo(serialId);
        }

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>Web请求的任务信息</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return mTaskPool.GetTaskInfos(tag);
        }

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <param name="results">Web请求的任务信息</param>
        public void GetWebRequestInfos(string tag, List<TaskInfo> results)
        {
            mTaskPool.GetTaskInfos(tag, results);
        }

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <returns>所有Web请求的任务信息</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return mTaskPool.GetAllTaskInfos();
        }

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <param name="results">所有Web请求的任务信息</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            mTaskPool.GetAllTaskInfos(results);
        }

        /// <summary>
        /// 增加Web请求
        /// </summary>
        /// <param name="webRequestInfo">Web请求信息</param>
        /// <returns>任务序列编号</returns>
        public int AddWebRequest(WebRequestInfo webRequestInfo)
        {
            if (webRequestInfo == null)
            {
                throw new Exception("Web request info is invalid.");
            }

            if (string.IsNullOrEmpty(webRequestInfo.WebRequestUri))
            {
                throw new Exception("Web request uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new Exception("You must add web request agent first.");
            }

            var webRequestTask = WebRequestTask.Create(webRequestInfo, mTimeout);
            mTaskPool.AddTask(webRequestTask);
            return webRequestTask.SerialId;
        }

        /// <summary>
        /// 根据任务序列编号获取Web请求
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>是否成功移除Web请求</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return mTaskPool.RemoveTask(serialId);
        }

        /// <summary>
        /// 根据任务标签获取Web请求
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除Web请求的数量</returns>
        public int RemoveWebRequests(string tag)
        {
            return mTaskPool.RemoveTask(tag);
        }

        /// <summary>
        /// 移除所有Web请求
        /// </summary>
        /// <returns>移除Web请求的数量</returns>
        public int RemoveAllWebRequests()
        {
            return mTaskPool.RemoveAllTasks();
        }

        private void OnWebRequestAgentStart(WebRequestAgent webRequestAgent)
        {
            if (mWebRequestStartEventHandler != null)
            {
                var eventArgs = WebRequestStartEventArgs.Create(webRequestAgent.Task.SerialId,
                    webRequestAgent.Task.WebRequestUri, webRequestAgent.Task.UserData);
                mWebRequestStartEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnWebRequestAgentSuccess(WebRequestAgent webRequestAgent, byte[] postData)
        {
            if (mWebRequestSuccessEventHandler != null)
            {
                var eventArgs = WebRequestSuccessEventArgs.Create(webRequestAgent.Task.SerialId,
                    webRequestAgent.Task.WebRequestUri, postData, webRequestAgent.Task.UserData);
                mWebRequestSuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnWebRequestAgentFailure(WebRequestAgent webRequestAgent, string errorMessage)
        {
            if (mWebRequestFailureEventHandler != null)
            {
                var eventArgs = WebRequestFailureEventArgs.Create(webRequestAgent.Task.SerialId,
                    webRequestAgent.Task.WebRequestUri, errorMessage, webRequestAgent.Task.UserData);
                mWebRequestFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}