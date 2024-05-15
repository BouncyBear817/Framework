/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 14:18:59
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// Web请求管理器
    /// </summary>
    public interface IWebRequestManager
    {
        /// <summary>
        /// Web请求总代理数量
        /// </summary>
        int TotalAgentCount { get; }

        /// <summary>
        /// Web请求空闲代理数量
        /// </summary>
        int FreeAgentCount { get; }

        /// <summary>
        /// Web请求工作中代理数量
        /// </summary>
        int WorkingAgentCount { get; }

        /// <summary>
        /// 等待任务数量
        /// </summary>
        int WaitingTaskCount { get; }

        /// <summary>
        /// Web请求超时事件
        /// </summary>
        float Timeout { get; set; }

        /// <summary>
        /// Web请求开始事件
        /// </summary>
        event EventHandler<WebRequestStartEventArgs> WebRequestStart;

        /// <summary>
        /// Web请求成功事件
        /// </summary>
        event EventHandler<WebRequestSuccessEventArgs> WebRequestSuccess;

        /// <summary>
        /// Web请求失败事件
        /// </summary>
        event EventHandler<WebRequestFailureEventArgs> WebRequestFailure;

        /// <summary>
        /// 增加Web请求代理辅助器
        /// </summary>
        /// <param name="webRequestAgentHelper">Web请求代理辅助器</param>
        void AddWebRequestAgentHelper(IWebRequestAgentHelper webRequestAgentHelper);

        /// <summary>
        /// 根据任务序列编号获取Web请求的任务信息
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>Web请求的任务信息</returns>
        TaskInfo GetWebRequestInfo(int serialId);

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>Web请求的任务信息</returns>
        TaskInfo[] GetWebRequestInfos(string tag);

        /// <summary>
        /// 根据任务标签获取Web请求的任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <param name="results">Web请求的任务信息</param>
        void GetWebRequestInfos(string tag, List<TaskInfo> results);

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <returns>所有Web请求的任务信息</returns>
        TaskInfo[] GetAllWebRequestInfos();

        /// <summary>
        /// 获取所有Web请求的任务信息
        /// </summary>
        /// <param name="results">所有Web请求的任务信息</param>
        void GetAllWebRequestInfos(List<TaskInfo> results);

        /// <summary>
        /// 增加Web请求
        /// </summary>
        /// <param name="webRequestInfo">Web请求信息</param>
        /// <returns>任务序列编号</returns>
        int AddWebRequest(WebRequestInfo webRequestInfo);

        /// <summary>
        /// 根据任务序列编号获取Web请求
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>是否成功移除Web请求</returns>
        bool RemoveWebRequest(int serialId);

        /// <summary>
        /// 根据任务标签获取Web请求
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除Web请求的数量</returns>
        int RemoveWebRequests(string tag);

        /// <summary>
        /// 移除所有Web请求
        /// </summary>
        /// <returns>移除Web请求的数量</returns>
        int RemoveAllWebRequests();
    }
}