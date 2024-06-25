/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:06
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Procedure")]
    public sealed class ProcedureComponent : FrameworkComponent
    {
        private IProcedureManager mProcedureManager;
        private ProcedureBase mEntranceProcedure; //入口流程
        private IFsmManager mFsmManager;

        [SerializeField] private string[] mAvailableProcedureTypeNames = null;

        [SerializeField] private string mEntranceProcedureTypeName = null;

        protected override void Awake()
        {
            base.Awake();

            mProcedureManager = FrameworkEntry.GetModule<IProcedureManager>();
            if (mProcedureManager == null)
            {
                Log.Fatal("Procedure manager is invalid.");
            }
        }

        private IEnumerator Start()
        {
            if (mAvailableProcedureTypeNames.Length <= 0)
            {
                Log.Error($"Can not find available procedure type name.");
                yield break;
            }

            var procedures = new ProcedureBase[mAvailableProcedureTypeNames.Length];
            for (int i = 0; i < mAvailableProcedureTypeNames.Length; i++)
            {
                var procedureType = Framework.Utility.Assembly.GetType(mAvailableProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Log.Error($"Can not find procedure type ({mAvailableProcedureTypeNames[i]}).");
                    yield break;
                }

                procedures[i] = Activator.CreateInstance(procedureType) as ProcedureBase;
                if (procedures[i] == null)
                {
                    Log.Error($"Can not create procedure instance ({mAvailableProcedureTypeNames[i]}).");
                    yield break;
                }

                if (mEntranceProcedureTypeName == mAvailableProcedureTypeNames[i])
                {
                    mEntranceProcedure = procedures[i];
                }
            }

            if (mEntranceProcedure == null)
            {
                Log.Error($"Entrance Procedure is invalid.");
                yield break;
            }

            mFsmManager = FrameworkEntry.GetModule<IFsmManager>();
            if (mFsmManager == null)
            {
                Log.Fatal("Fsm manager is invalid.");
                yield break;
            }

            mProcedureManager.Initialize(mFsmManager, procedures);

            yield return new WaitForEndOfFrame();

            mProcedureManager.StartProcedure(mEntranceProcedure.GetType());
        }

        /// <summary>
        /// 当前流程
        /// </summary>
        public ProcedureBase CurrentProcedure => mProcedureManager.CurrentProcedure;

        /// <summary>
        /// 当前流程持续时间
        /// </summary>
        public float CurrentProcedureTime => mProcedureManager.CurrentProcedureTime;

        /// <summary>
        /// 初始化流程管理器
        /// </summary>
        /// <param name="fsmManager"></param>
        /// <param name="procedures"></param>
        public void Initialize(IFsmManager fsmManager, params ProcedureBase[] procedures)
        {
            mProcedureManager.Initialize(fsmManager, procedures);
        }

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            mProcedureManager.StartProcedure<T>();
        }

        /// <summary>
        /// 开始流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        public void StartProcedure(Type procedureType)
        {
            mProcedureManager.StartProcedure(procedureType);
        }

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>是否存在流程</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            return mProcedureManager.HasProcedure<T>();
        }

        /// <summary>
        /// 是否存在流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>是否存在流程</returns>
        public bool HasProcedure(Type procedureType)
        {
            return mProcedureManager.HasProcedure(procedureType);
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <typeparam name="T">流程类型</typeparam>
        /// <returns>流程</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            return mProcedureManager.GetProcedure<T>();
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        /// <returns>流程</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            return mProcedureManager.GetProcedure(procedureType);
        }

#if UNITY_EDITOR
        [ContextMenu("Insert Type Names")]
        private void InsertTypeNames()
        {
            mAvailableProcedureTypeNames = null;
            var results = new List<string>();
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ProcedureBase)) && !type.IsAbstract)
                    {
                        results.Add(type.FullName);
                    }
                }
            }

            mAvailableProcedureTypeNames = results.ToArray();
            
        }

        [ContextMenu("Clear Type Names")]
        private void ClearTypeNames()
        {
            mAvailableProcedureTypeNames = null;
        }
#endif
    }
}