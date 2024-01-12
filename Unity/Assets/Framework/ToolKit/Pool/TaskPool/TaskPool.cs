/************************************************************
* Unity Version: 2022.3.15f1c1
* Author:        bear
* CreateTime:    2024/01/09 14:39:14
* Description:   
* Modify Record: 
*************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 任务池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TaskPool<T> where T : TaskBase
    {
        private readonly Stack<ITaskAgent<T>> mAvailableAgents;
        private readonly LinkedList<ITaskAgent<T>> mWorkingAgents;
        private readonly LinkedList<T> mWaitingTasks;
        private bool mPaused;

        public TaskPool()
        {
            mAvailableAgents = new Stack<ITaskAgent<T>>();
            mWorkingAgents = new LinkedList<ITaskAgent<T>>();
            mWaitingTasks = new LinkedList<T>();
            mPaused = false;
        }

        /// <summary>
        /// 任务池是否被暂停
        /// </summary>
        public bool Paused
        {
            get => mPaused;
            set => mPaused = value;
        }

        /// <summary>
        /// 可用的任务代理数量
        /// </summary>
        public int AvailableAgentsCount => mAvailableAgents.Count;

        /// <summary>
        /// 工作中的任务代理数量
        /// </summary>
        public int WorkingAgentsCount => mWorkingAgents.Count;

        /// <summary>
        /// 总的任务代理数量
        /// </summary>
        public int TotalAgentsCount => AvailableAgentsCount + WorkingAgentsCount;

        /// <summary>
        /// 等待中的任务代理数量
        /// </summary>
        public int WaitingTasksCount => mWaitingTasks.Count;

        /// <summary>
        /// 任务池轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (mPaused) return;

            ProcessWorkingTasks(elapseSeconds, realElapseSeconds);
            ProcessWaitingTasks(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理任务池
        /// </summary>
        public void Shutdown()
        {
            RemoveAllTasks();
            while (AvailableAgentsCount > 0)
            {
                mAvailableAgents.Pop().Shutdown();
            }
        }

        /// <summary>
        /// 增加任务代理
        /// </summary>
        /// <param name="agent">任务代理</param>
        /// <exception cref="Exception"></exception>
        public void AddAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                throw new Exception("Task agent is invalid.");
            }

            agent.Initialize();
            mAvailableAgents.Push(agent);
        }

        /// <summary>
        /// 根据序列编号获取任务信息
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>任务信息</returns>
        public TaskInfo GetTaskInfo(int serialId)
        {
            foreach (var workingAgent in mWorkingAgents)
            {
                var workingTask = workingAgent.Task;
                if (workingTask.SerialId == serialId)
                {
                    return new TaskInfo(workingTask.SerialId, workingTask.Tag, workingTask.Priority,
                        workingTask.UserData, workingTask.Done ? TaskStatus.Done : TaskStatus.Doing,
                        workingTask.Description);
                }
            }

            foreach (var waitingTask in mWaitingTasks)
            {
                if (waitingTask.SerialId == serialId)
                {
                    return new TaskInfo(waitingTask.SerialId, waitingTask.Tag, waitingTask.Priority,
                        waitingTask.UserData,
                        TaskStatus.Todo, waitingTask.Description);
                }
            }

            return default(TaskInfo);
        }

        /// <summary>
        /// 根据标签获取任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>任务信息</returns>
        public TaskInfo[] GetTaskInfos(string tag)
        {
            var results = new List<TaskInfo>();
            foreach (var workingAgent in mWorkingAgents)
            {
                var workingTask = workingAgent.Task;
                if (workingTask.Tag == tag)
                {
                    results.Add(new TaskInfo(workingTask.SerialId, workingTask.Tag, workingTask.Priority,
                        workingTask.UserData, workingTask.Done ? TaskStatus.Done : TaskStatus.Doing,
                        workingTask.Description));
                }
            }

            foreach (var waitingTask in mWaitingTasks)
            {
                if (waitingTask.Tag == tag)
                {
                    results.Add(new TaskInfo(waitingTask.SerialId, waitingTask.Tag, waitingTask.Priority,
                        waitingTask.UserData,
                        TaskStatus.Todo, waitingTask.Description));
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有任务任务信息
        /// </summary>
        /// <returns>任务信息</returns>
        public TaskInfo[] GetAllTaskInfos()
        {
            var results = new TaskInfo[mWorkingAgents.Count + mWaitingTasks.Count];
            var index = 0;
            foreach (var workingAgent in mWorkingAgents)
            {
                var workingTask = workingAgent.Task;
                results[index++] = new TaskInfo(workingTask.SerialId, workingTask.Tag, workingTask.Priority,
                    workingTask.UserData,
                    workingTask.Done ? TaskStatus.Done : TaskStatus.Doing, workingTask.Description);
            }

            foreach (var waitingTask in mWaitingTasks)
            {
                results[index++] = new TaskInfo(waitingTask.SerialId, waitingTask.Tag, waitingTask.Priority,
                    waitingTask.UserData,
                    TaskStatus.Todo, waitingTask.Description);
            }

            return results;
        }

        /// <summary>
        /// 获取所有任务信息
        /// </summary>
        /// <param name="results">任务信息</param>
        /// <exception cref="Exception"></exception>
        public void GetAllTaskInfos(List<TaskInfo> results)
        {
            if (results == null)
            {
                throw new Exception("Get all task info results is invalid.");
            }

            results.Clear();
            foreach (var workingAgent in mWorkingAgents)
            {
                var workingTask = workingAgent.Task;
                results.Add(new TaskInfo(workingTask.SerialId, workingTask.Tag, workingTask.Priority,
                    workingTask.UserData, workingTask.Done ? TaskStatus.Done : TaskStatus.Doing,
                    workingTask.Description));
            }

            foreach (var waitingTask in mWaitingTasks)
            {
                results.Add(new TaskInfo(waitingTask.SerialId, waitingTask.Tag, waitingTask.Priority,
                    waitingTask.UserData,
                    TaskStatus.Todo, waitingTask.Description));
            }
        }

        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="task">任务</param>
        public void AddTask(T task)
        {
            var current = mWaitingTasks.Last;
            while (current != null)
            {
                if (task.Priority <= current.Value.Priority)
                {
                    break;
                }

                current = current.Previous;
            }

            if (current != null)
            {
                mWaitingTasks.AddAfter(current, task);
            }
            else
            {
                mWaitingTasks.AddFirst(task);
            }
        }

        /// <summary>
        /// 根据任务序列编号移除任务
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveTask(int serialId)
        {
            foreach (var waitingTask in mWaitingTasks)
            {
                if (waitingTask.SerialId == serialId)
                {
                    mWaitingTasks.Remove(waitingTask);
                    ReferencePool.Release(waitingTask);
                    return true;
                }
            }

            var currentWorkingAgent = mWorkingAgents.First;
            while (currentWorkingAgent != null)
            {
                var agent = currentWorkingAgent.Value;
                var task = agent.Task;
                if (task.SerialId == serialId)
                {
                    agent.StopAndReset();
                    mAvailableAgents.Push(agent);
                    mWorkingAgents.Remove(agent);
                    ReferencePool.Release(task);
                    return true;
                }

                currentWorkingAgent = currentWorkingAgent.Next;
            }

            return false;
        }

        /// <summary>
        /// 根据任务标签移除任务
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除的任务数量</returns>
        public int RemoveTask(string tag)
        {
            var count = 0;
            var currentWaitingTask = mWaitingTasks.First;
            while (currentWaitingTask != null)
            {
                var task = currentWaitingTask.Value;
                if (task.Tag == tag)
                {
                    mWaitingTasks.Remove(task);
                    ReferencePool.Release(task);
                    count++;
                }

                currentWaitingTask = currentWaitingTask.Next;
            }

            var currentWorkingAgent = mWorkingAgents.First;
            while (currentWorkingAgent != null)
            {
                var agent = currentWorkingAgent.Value;
                var task = agent.Task;
                if (task.Tag == tag)
                {
                    agent.StopAndReset();
                    mAvailableAgents.Push(agent);
                    mWorkingAgents.Remove(agent);
                    ReferencePool.Release(task);
                    count++;
                }

                currentWorkingAgent = currentWorkingAgent.Next;
            }

            return count;
        }

        /// <summary>
        /// 移除任务池中所有任务
        /// </summary>
        /// <returns>移除的任务数量</returns>
        public int RemoveAllTasks()
        {
            var count = 0;
            foreach (var task in mWaitingTasks)
            {
                ReferencePool.Release(task);
                count++;
            }

            mWaitingTasks.Clear();

            foreach (var workingAgent in mWorkingAgents)
            {
                var task = workingAgent.Task;
                workingAgent.StopAndReset();
                mAvailableAgents.Push(workingAgent);
                ReferencePool.Release(task);
                count++;
            }

            mWorkingAgents.Clear();

            return count;
        }

        /// <summary>
        /// 轮询工作中的任务
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        private void ProcessWorkingTasks(float elapseSeconds, float realElapseSeconds)
        {
            var current = mWorkingAgents.First;
            while (current != null)
            {
                var agent = current.Value;
                var task = agent.Task;
                if (!task.Done)
                {
                    agent.Update(elapseSeconds, realElapseSeconds);
                    current = current.Next;
                    continue;
                }

                agent.StopAndReset();
                mAvailableAgents.Push(agent);
                mWorkingAgents.Remove(agent);
                ReferencePool.Release(task);
                current = current.Next;
            }
        }

        /// <summary>
        /// 轮询等待中的任务
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        private void ProcessWaitingTasks(float elapseSeconds, float realElapseSeconds)
        {
            var current = mWaitingTasks.First;
            while (current != null && AvailableAgentsCount > 0)
            {
                var agent = mAvailableAgents.Pop();
                var agentNode = mWorkingAgents.AddLast(agent);
                var task = agent.Task;
                var status = agent.Start(task);
                if (status == StartTaskStatus.Done || status == StartTaskStatus.HasToWait ||
                    status == StartTaskStatus.UnknownError)
                {
                    agent.StopAndReset();
                    mAvailableAgents.Push(agent);
                    mWorkingAgents.Remove(agentNode);
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.CanResume ||
                    status == StartTaskStatus.UnknownError)
                {
                    mWaitingTasks.Remove(task);
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.UnknownError)
                {
                    ReferencePool.Release(task);
                }
            }
        }
    }
}