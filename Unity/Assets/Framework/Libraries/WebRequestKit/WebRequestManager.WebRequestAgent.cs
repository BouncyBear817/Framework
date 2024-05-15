// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/13 15:18:58
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public sealed partial class WebRequestManager : FrameworkModule, IWebRequestManager
    {
        /// <summary>
        /// Web请求代理
        /// </summary>
        private sealed class WebRequestAgent : ITaskAgent<WebRequestTask>
        {
            private readonly IWebRequestAgentHelper mWebRequestAgentHelper;
            private WebRequestTask mTask;
            private float mWaitTime;

            public Action<WebRequestAgent> WebRequestAgentStart;
            public Action<WebRequestAgent, byte[]> WebRequestAgentSuccess;
            public Action<WebRequestAgent, string> WebRequestAgentFailure;

            public WebRequestAgent(IWebRequestAgentHelper webRequestAgentHelper)
            {
                if (webRequestAgentHelper == null)
                {
                    throw new Exception("Web request agent helper is invalid.");
                }

                mWebRequestAgentHelper = webRequestAgentHelper;
                mTask = null;
                mWaitTime = 0f;

                WebRequestAgentStart = null;
                WebRequestAgentSuccess = null;
                WebRequestAgentFailure = null;
            }

            /// <summary>
            /// 任务
            /// </summary>
            public WebRequestTask Task => mTask;

            /// <summary>
            /// 任务等待时间
            /// </summary>
            public float WaitTime => mWaitTime;

            /// <summary>
            /// 初始化任务代理
            /// </summary>
            public void Initialize()
            {
                mWebRequestAgentHelper.WebRequestAgentHelperComplete += OnWebRequestAgentHelperComplete;
                mWebRequestAgentHelper.WebRequestAgentHelperError += OnWebRequestAgentHelperError;
            }

            /// <summary>
            /// 任务代理轮询
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (mTask.TaskStatus == WebRequestTaskStatus.Doing)
                {
                    mWaitTime += realElapseSeconds;
                    if (mWaitTime >= mTask.Timeout)
                    {
                        var eventArgs = WebRequestAgentHelperErrorEventArgs.Create("Timeout");
                        OnWebRequestAgentHelperError(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
            }

            /// <summary>
            /// 关闭并清理任务代理
            /// </summary>
            public void Shutdown()
            {
                StopAndReset();
                mWebRequestAgentHelper.WebRequestAgentHelperComplete -= OnWebRequestAgentHelperComplete;
                mWebRequestAgentHelper.WebRequestAgentHelperError -= OnWebRequestAgentHelperError;
            }

            /// <summary>
            /// 开始处理任务
            /// </summary>
            /// <param name="task">任务</param>
            /// <returns>开始处理任务的状态</returns>
            public StartTaskStatus Start(WebRequestTask task)
            {
                if (task == null)
                {
                    throw new Exception("Task is invalid.");
                }

                mTask = task;
                mTask.TaskStatus = WebRequestTaskStatus.Doing;

                WebRequestAgentStart?.Invoke(this);

                var postData = mTask.PostData;
                if (postData == null)
                {
                    mWebRequestAgentHelper.Request(mTask.WebRequestUri, mTask.UserData);
                }
                else
                {
                    mWebRequestAgentHelper.Request(mTask.WebRequestUri, postData, mTask.UserData);
                }

                mWaitTime = 0f;
                return StartTaskStatus.CanResume;
            }

            /// <summary>
            /// 停止正在处理的任务并重置任务代理
            /// </summary>
            public void StopAndReset()
            {
                mWebRequestAgentHelper.Reset();
                mTask = null;
                mWaitTime = 0f;
            }

            private void OnWebRequestAgentHelperComplete(object sender, WebRequestAgentHelperCompleteEventArgs e)
            {
                mWebRequestAgentHelper.Reset();
                mTask.TaskStatus = WebRequestTaskStatus.Done;
                WebRequestAgentSuccess?.Invoke(this, e.WebResponseBytes);
                mTask.Done = true;
            }

            private void OnWebRequestAgentHelperError(object sender, WebRequestAgentHelperErrorEventArgs e)
            {
                mWebRequestAgentHelper.Reset();
                mTask.TaskStatus = WebRequestTaskStatus.Error;
                WebRequestAgentFailure?.Invoke(this, e.ErrorMessage);
                mTask.Done = true;
            }
        }
    }
}