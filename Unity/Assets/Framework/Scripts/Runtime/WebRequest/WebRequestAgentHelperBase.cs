/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 16:24:41
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// Web请求代理辅助器基类
    /// </summary>
    public abstract class WebRequestAgentHelperBase : MonoBehaviour, IWebRequestAgentHelper
    {
        /// <summary>
        /// Web请求代理辅助器完成事件
        /// </summary>
        public abstract event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete;

        /// <summary>
        /// Web请求代理辅助器错误事件
        /// </summary>
        public abstract event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError;

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        public abstract void Request(string webRequestUri, object userData);

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="postData">发送的数据流</param>
        /// <param name="userData">用户自定义数据</param>
        public abstract void Request(string webRequestUri, byte[] postData, object userData);

        /// <summary>
        /// 重置Web代理辅助器
        /// </summary>
        public abstract void Reset();
    }
}