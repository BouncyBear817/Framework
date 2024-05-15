/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 16:26:42
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;
using UnityEngine;
using UnityEngine.Networking;

namespace Runtime
{
    public class UnityWebRequestAgentHelper : WebRequestAgentHelperBase, IDisposable
    {
        private UnityWebRequest mUnityWebRequest = null;
        private bool mDisposed = false;

        private EventHandler<WebRequestAgentHelperCompleteEventArgs> mWebRequestAgentHelperCompleteEventHandler;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> mWebRequestAgentHelperErrorEventHandler;

        /// <summary>
        /// Web请求代理辅助器完成事件
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add => mWebRequestAgentHelperCompleteEventHandler += value;
            remove => mWebRequestAgentHelperCompleteEventHandler -= value;
        }

        /// <summary>
        /// Web请求代理辅助器错误事件
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add => mWebRequestAgentHelperErrorEventHandler += value;
            remove => mWebRequestAgentHelperErrorEventHandler -= value;
        }

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        public override void Request(string webRequestUri, object userData)
        {
            if (mWebRequestAgentHelperCompleteEventHandler == null || mWebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            var wwwFormInfo = userData as WWWFormInfo;
            if (wwwFormInfo == null || wwwFormInfo.WWWForm == null)
            {
                mUnityWebRequest = UnityWebRequest.Get(webRequestUri);
            }
            else
            {
                mUnityWebRequest = UnityWebRequest.Post(webRequestUri, wwwFormInfo.WWWForm);
            }

            mUnityWebRequest.SendWebRequest();
        }

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="postData">发送的数据流</param>
        /// <param name="userData">用户自定义数据</param>
        public override void Request(string webRequestUri, byte[] postData, object userData)
        {
            if (mWebRequestAgentHelperCompleteEventHandler == null || mWebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            mUnityWebRequest = UnityWebRequest.PostWwwForm(webRequestUri, Utility.Converter.GetString(postData));

            mUnityWebRequest.SendWebRequest();
        }

        /// <summary>
        /// 重置Web代理辅助器
        /// </summary>
        public override void Reset()
        {
            if (mUnityWebRequest != null)
            {
                mUnityWebRequest.Dispose();
                mUnityWebRequest = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放</param>
        private void Dispose(bool disposing)
        {
            if (mDisposed)
            {
                return;
            }

            if (disposing)
            {
                Reset();
            }

            mDisposed = true;
        }

        private void Update()
        {
            if (mUnityWebRequest == null || !mUnityWebRequest.isDone)
            {
                return;
            }

            var isError = mUnityWebRequest.result != UnityWebRequest.Result.Success;

            if (isError)
            {
                var eventArgs = WebRequestAgentHelperErrorEventArgs.Create(mUnityWebRequest.error);
                mWebRequestAgentHelperErrorEventHandler?.Invoke(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
            else if (mUnityWebRequest.downloadHandler.isDone)
            {
                var eventArgs = WebRequestAgentHelperCompleteEventArgs.Create(mUnityWebRequest.downloadHandler.data);
                mWebRequestAgentHelperCompleteEventHandler?.Invoke(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}