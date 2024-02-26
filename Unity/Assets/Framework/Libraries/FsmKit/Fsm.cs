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
    public sealed class Fsm<T> : FsmBase, IReference, IFsm<T> where T : class
    {
        private T mOwner;
        private Dictionary<Type, FsmState<T>> mStates;
        private Dictionary<string, Variable> mDatas;
        private FsmState<T> mCurrentState;
        private float mCurrentStateTime;
        private bool mIsDestroyed;

        public Fsm()
        {
            mOwner = null;
            mStates = new Dictionary<Type, FsmState<T>>();
            mDatas = new Dictionary<string, Variable>();
            mCurrentState = null;
            mCurrentStateTime = 0f;
            mIsDestroyed = false;
        }

        /// <summary>
        /// 有限状态机持有者类型
        /// </summary>
        public override Type OwnerType => typeof(T);

        /// <summary>
        /// 有限状态机持有者
        /// </summary>
        public T Owner => mOwner;

        /// <summary>
        /// 有限状态机状态的数量
        /// </summary>
        public override int FsmStateCount => mStates.Count;

        /// <summary>
        /// 有限状态机是否在运行
        /// </summary>
        public override bool IsRunning => mCurrentState != null;

        /// <summary>
        /// 有限状态机是否被销毁
        /// </summary>
        public override bool IsDestroyed => mIsDestroyed;

        /// <summary>
        /// 有限状态机当前状态
        /// </summary>
        public FsmState<T> CurrentState => mCurrentState;

        /// <summary>
        /// 有限状态机当前状态名称
        /// </summary>
        public override string CurrentStateName => mCurrentState != null ? mCurrentState.GetType().FullName : null;

        /// <summary>
        /// 有限状态机当前状态持续时间
        /// </summary>
        public override float CurrentStateTime => mCurrentStateTime;

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new Exception("Fsm owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new Exception("Fsm states is invalid.");
            }

            var fsm = ReferencePool.Acquire<Fsm<T>>();
            fsm.Name = name;
            fsm.mOwner = owner;
            fsm.mIsDestroyed = false;
            foreach (var state in states)
            {
                if (state == null)
                {
                    throw new Exception("Fsm state is invalid.");
                }

                var stateType = state.GetType();
                if (fsm.mStates.ContainsKey(stateType))
                {
                    throw new Exception(
                        $"Fsm ({new TypeNamePair(typeof(T), name)}) state ({stateType.FullName}) is already exist.");
                }

                fsm.mStates.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new Exception("Fsm owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new Exception("Fsm states is invalid.");
            }

            var fsm = ReferencePool.Acquire<Fsm<T>>();
            fsm.Name = name;
            fsm.mOwner = owner;
            fsm.mIsDestroyed = false;
            foreach (var state in states)
            {
                if (state == null)
                {
                    throw new Exception("Fsm state is invalid.");
                }

                var stateType = state.GetType();
                if (fsm.mStates.ContainsKey(stateType))
                {
                    throw new Exception(
                        $"Fsm ({new TypeNamePair(typeof(T), name)}) state ({stateType.FullName}) is already exist.");
                }

                fsm.mStates.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new Exception("Fsm is running, can not start again.");
            }

            var state = GetState<TState>();
            if (state == null)
            {
                throw new Exception(
                    $"Fsm ({new TypeNamePair(typeof(T), Name)}) can not start state ({typeof(TState).FullName}) which is not exist.");
            }

            mCurrentState = state;
            mCurrentStateTime = 0f;
            mCurrentState.OnEnter(this);
        }

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new Exception("Fsm is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception($"State type ({stateType.FullName}) is invalid.");
            }

            var state = GetState(stateType);
            if (state == null)
            {
                throw new Exception(
                    $"Fsm ({new TypeNamePair(typeof(T), Name)}) can not start state ({stateType.FullName}) which is not exist.");
            }

            mCurrentState = state;
            mCurrentStateTime = 0f;
            mCurrentState.OnEnter(this);
        }

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return mStates.ContainsKey(typeof(TState));
        }

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception($"State type ({stateType.FullName}) is invalid.");
            }

            return mStates.ContainsKey(stateType);
        }

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        /// <returns>有限状态机状态</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            if (mStates.TryGetValue(typeof(TState), out var state))
            {
                return state as TState;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        /// <returns>有限状态机状态</returns>
        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception($"State type ({stateType.FullName}) is invalid.");
            }

            if (mStates.TryGetValue(stateType, out var state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// 获取所有有限状态机状态
        /// </summary>
        /// <returns>所有有限状态机状态</returns>
        public FsmState<T>[] GetAllStates()
        {
            var index = 0;
            var results = new FsmState<T>[mStates.Count];
            foreach (var state in mStates)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有有限状态机状态
        /// </summary>
        /// <param name="results">所有有限状态机状态</param>
        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var state in mStates)
            {
                results.Add(state.Value);
            }
        }

        /// <summary>
        /// 是否存在有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>是否存在有限状态机数据</returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (mDatas == null)
            {
                return false;
            }

            return mDatas.ContainsKey(name);
        }

        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <typeparam name="TData">有限状态机数据类型</typeparam>
        /// <returns>有限状态机数据</returns>
        public TData GetData<TData>(string name) where TData : Variable
        {
            return GetData(name) as TData;
        }

        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>有限状态机数据</returns>
        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (mDatas == null)
            {
                return null;
            }

            if (mDatas.TryGetValue(name, out var data))
            {
                return data;
            }

            return null;
        }

        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <param name="data">有限状态机数据</param>
        /// <typeparam name="TData">有限状态机数据类型</typeparam>
        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, data as Variable);
        }

        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <param name="data">有限状态机数据</param>
        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (mDatas == null)
            {
                mDatas = new Dictionary<string, Variable>();
            }

            var oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            mDatas[name] = data;
        }

        /// <summary>
        /// 移除有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>是否移除有限状态机数据</returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (mDatas == null)
            {
                return false;
            }

            var oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            return mDatas.Remove(name);
        }

        /// <summary>
        /// 有限状态机轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (CurrentState == null)
            {
                return;
            }

            mCurrentStateTime += elapseSeconds;
            mCurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理有限状态机
        /// </summary>
        public override void Shutdown()
        {
            ReferencePool.Release(this);
        }

        /// <summary>
        /// 清理引用
        /// </summary>
        public void Clear()
        {
            if (mCurrentState != null)
            {
                mCurrentState.OnLeave(this, true);
            }

            foreach (var state in mStates)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            mOwner = null;
            mStates.Clear();

            if (mDatas != null)
            {
                foreach (var data in mDatas)
                {
                    if (data.Value == null)
                    {
                        continue;
                    }

                    ReferencePool.Release(data.Value);
                }

                mDatas.Clear();
            }

            mCurrentState = null;
            mCurrentStateTime = 0f;
            mIsDestroyed = true;
        }

        /// <summary>
        /// 切换有限状态机状态
        /// </summary>
        /// <typeparam name="TState">有限状态机状态类型</typeparam>
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// 切换有限状态机状态
        /// </summary>
        /// <param name="stateType">有限状态机状态类型</param>
        /// <exception cref="Exception"></exception>
        public void ChangeState(Type stateType)
        {
            if (mCurrentState == null)
            {
                throw new Exception("Current state is invalid.");
            }

            var state = GetState(stateType);
            if (state == null)
            {
                throw new Exception(
                    $"Fsm ({new TypeNamePair(typeof(T), Name)}) can not start state ({stateType.FullName}) which is not exist.");
            }

            mCurrentState.OnLeave(this, false);
            mCurrentStateTime = 0f;
            mCurrentState = state;
            mCurrentState.OnEnter(this);
        }
    }
}