/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:11:53
 * Description:   
 * Modify Record: 
 *************************************************************/

using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace Framework
{
    /// <summary>
    /// 流程基类
    /// </summary>
    public abstract class ProcedureBase : FsmState<IProcedureManager>
    {
        /// <summary>
        /// 有限状态机状态初始化调用
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        /// <summary>
        /// 有限状态机状态进入时调用
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        /// <summary>
        /// 有限状态机状态轮询时调用
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 有限状态机状态离开时调用
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        /// <param name="isShutdown">是否关闭有限状态机</param>
        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 有限状态机状态销毁时调用
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        protected internal override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}