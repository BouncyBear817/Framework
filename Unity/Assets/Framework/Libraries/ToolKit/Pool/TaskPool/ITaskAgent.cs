/************************************************************
* Unity Version: 2022.3.15f1c1
* Author:        bear
* CreateTime:    2024/01/09 14:39:14
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 任务代理接口
    /// </summary>
    public interface ITaskAgent<T> where T : TaskBase
    {
        /// <summary>
        /// 任务
        /// </summary>
        T Task { get; }

        /// <summary>
        /// 初始化任务代理
        /// </summary>
        void Initialize();

        /// <summary>
        /// 任务代理轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理任务代理
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 开始处理任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>开始处理任务的状态</returns>
        StartTaskStatus Start(T task);

        /// <summary>
        /// 停止正在处理的任务并重置任务代理
        /// </summary>
        void StopAndReset();
    }
}