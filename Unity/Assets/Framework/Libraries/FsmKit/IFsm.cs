/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:11:44
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public interface IFsm<T> where T : class
    {
        /// <summary>
        /// 有限状态机名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 有限状态机完整名称
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 有限状态机持有者
        /// </summary>
        T Owner { get; }

        /// <summary>
        /// 有限状态机状态数量
        /// </summary>
        int FsmStateCount { get; }

        /// <summary>
        /// 有限状态机是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 有限状态机是否被销毁
        /// </summary>
        bool IsDestroyed { get; }

        /// <summary>
        /// 有限状态机当前状态
        /// </summary>
        FsmState<T> CurrentState { get; }

        /// <summary>
        /// 有限状态机当前状态持续时间
        /// </summary>
        float CurrentStateTime { get; }

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        void Start<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        void Start(Type stateType);

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        /// <returns>是否存在有限状态机状态</returns>
        bool HasState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        /// <returns>是否存在有限状态机状态</returns>
        bool HasState(Type stateType);

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        /// <returns>有限状态机状态</returns>
        TState GetState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        /// <returns>有限状态机状态</returns>
        FsmState<T> GetState(Type stateType);

        /// <summary>
        /// 获取所有有限状态机状态
        /// </summary>
        /// <returns>所有有限状态机状态</returns>
        FsmState<T>[] GetAllStates();

        /// <summary>
        /// 获取所有有限状态机状态
        /// </summary>
        /// <param name="results">所有有限状态机状态</param>
        void GetAllStates(List<FsmState<T>> results);

        /// <summary>
        /// 是否存在有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>是否存在有限状态机数据</returns>
        bool HasData(string name);

        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <typeparam name="TData">有限状态机数据类型</typeparam>
        /// <returns>有限状态机数据</returns>
        TData GetData<TData>(string name) where TData : Variable;

        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>有限状态机数据</returns>
        Variable GetData(string name);

        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <param name="data">有限状态机数据</param>
        /// <typeparam name="TData">有限状态机数据类型</typeparam>
        void SetData<TData>(string name, TData data) where TData : Variable;

        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <param name="data">有限状态机数据</param>
        void SetData(string name, Variable data);

        /// <summary>
        /// 移除有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>是否移除有限状态机数据</returns>
        bool RemoveData(string name);
    }
}