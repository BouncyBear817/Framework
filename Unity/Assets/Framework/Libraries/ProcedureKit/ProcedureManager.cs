/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:11:53
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 流程管理器
    /// </summary>
    public sealed class ProcedureManager : FrameworkModule, IProcedureManager
    {
        private IFsmManager mFsmManager;
        private IFsm<IProcedureManager> mProcedureFsm;

        public ProcedureManager()
        {
            mFsmManager = null;
            mProcedureFsm = null;
        }

        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public override int Priority => -2;

        /// <summary>
        /// 当前流程
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (mProcedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return mProcedureFsm.CurrentState as ProcedureBase;
            }
        }

        /// <summary>
        /// 当前流程持续时间
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (mProcedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return mProcedureFsm.CurrentStateTime;
            }
        }


        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理流程管理器
        /// </summary>
        public override void Shutdown()
        {
            if (mFsmManager != null)
            {
                if (mProcedureFsm != null)
                {
                    mFsmManager.DestroyFsm(mProcedureFsm);
                    mProcedureFsm = null;
                }

                mFsmManager = null;
            }
        }


        /// <summary>
        /// 初始化流程管理器
        /// </summary>
        /// <param name="fsmManager"></param>
        /// <param name="procedures"></param>
        public void Initialize(IFsmManager fsmManager, params ProcedureBase[] procedures)
        {
            if (fsmManager == null)
            {
                throw new Exception("Fsm manager is invalid.");
            }

            mFsmManager = fsmManager;
            mProcedureFsm = mFsmManager.CreateFsm(this, procedures);
        }

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            mProcedureFsm.Start<T>();
        }

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        public void StartProcedure(Type procedureType)
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            mProcedureFsm.Start(procedureType);
        }

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>是否存在流程</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return mProcedureFsm.HasState<T>();
        }

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>是否存在流程</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return mProcedureFsm.HasState(procedureType);
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>流程</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return mProcedureFsm.GetState<T>();
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>流程</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            if (mProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return mProcedureFsm.GetState(procedureType) as ProcedureBase;
        }
    }
}