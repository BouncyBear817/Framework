/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:11:43
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 有限状态机状态基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FsmState<T> where T : class
    {
        protected FsmState()
        {
        }

        /// <summary>
        /// 有限状态机状态初始化调用
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        protected internal virtual void OnInit(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 有限状态机状态进入时调用
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        protected internal virtual void OnEnter(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 有限状态机状态轮询时调用
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        protected internal virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 有限状态机状态离开时调用
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <param name="isShutdown">是否关闭有限状态机</param>
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }

        /// <summary>
        /// 有限状态机状态销毁时调用
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 切换当前有限状态机状态
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <typeparam name="TState">有限状态机类型</typeparam>
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            var fsmImplement = fsm as Fsm<T>;
            if (fsmImplement == null)
            {
                throw new Exception("Fsm is invalid.");
            }

            fsmImplement.ChangeState<TState>();
        }

        /// <summary>
        /// 切换当前有限状态机状态
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <param name="stateType">有限状态机类型</param>
        /// <exception cref="Exception"></exception>
        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            var fsmImplement = fsm as Fsm<T>;
            if (fsmImplement == null)
            {
                throw new Exception("Fsm is invalid.");
            }

            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception($"State type ({stateType.FullName}) is invalid.");
            }

            fsmImplement.ChangeState(stateType);
        }
    }
}