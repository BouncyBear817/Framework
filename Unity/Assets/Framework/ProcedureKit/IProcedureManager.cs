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
    /// 流程管理器接口
    /// </summary>
    public interface IProcedureManager
    {
        /// <summary>
        /// 当前流程
        /// </summary>
        ProcedureBase CurrentProcedure { get; }

        /// <summary>
        /// 当前流程持续时间
        /// </summary>
        float CurrentProcedureTime { get; }

        /// <summary>
        /// 初始化流程管理器
        /// </summary>
        /// <param name="fsmManager"></param>
        /// <param name="procedures"></param>
        void Initialize(IFsmManager fsmManager, params ProcedureBase[] procedures);

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        void StartProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        void StartProcedure(Type procedureType);

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>是否存在流程</returns>
        bool HasProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>是否存在流程</returns>
        bool HasProcedure(Type procedureType);

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>流程</returns>
        ProcedureBase GetProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>流程</returns>
        ProcedureBase GetProcedure(Type procedureType);
    }
}