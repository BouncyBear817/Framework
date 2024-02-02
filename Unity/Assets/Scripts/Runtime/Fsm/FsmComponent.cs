/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:06
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Fsm")]
    public class FsmComponent : FrameworkComponent
    {
        private IFsmManager mFsmManager;

        public int Count => mFsmManager.Count;

        protected override void Awake()
        {
            base.Awake();

            mFsmManager = FrameworkEntry.GetModule<IFsmManager>();
            if (mFsmManager == null)
            {
                Log.Error("Fsm manager is invalid.");
            }
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>() where T : class
        {
            return mFsmManager.HasFsm<T>();
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType)
        {
            return mFsmManager.HasFsm(ownerType);
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return mFsmManager.HasFsm<T>(name);
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            return mFsmManager.HasFsm(ownerType, name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>有限状态机</returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return mFsmManager.GetFsm<T>();
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>有限状态机</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            return mFsmManager.GetFsm(ownerType);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>有限状态机</returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return mFsmManager.GetFsm<T>(name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机</returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            return mFsmManager.GetFsm(ownerType, name);
        }

        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <returns>所有有限状态机</returns>
        public FsmBase[] GetAllFsms()
        {
            return mFsmManager.GetAllFsms();
        }

        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <param name="results">所有有限状态机</param>
        public void GetAllFsms(List<FsmBase> results)
        {
            mFsmManager.GetAllFsms(results);
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>创建的有限状态机</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return mFsmManager.CreateFsm<T>(owner, states);
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>创建的有限状态机</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            return mFsmManager.CreateFsm<T>(name, owner, states);
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>创建的有限状态机</returns>
        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return mFsmManager.CreateFsm<T>(owner, states);
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>创建的有限状态机</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            return mFsmManager.CreateFsm<T>(name, owner, states);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return mFsmManager.DestroyFsm<T>();
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机类型</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(Type ownerType)
        {
            return mFsmManager.DestroyFsm(ownerType);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return mFsmManager.DestroyFsm<T>(name);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            return mFsmManager.DestroyFsm(ownerType, name);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            return mFsmManager.DestroyFsm(fsm);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <typeparam name="T">有限状态机类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            return mFsmManager.DestroyFsm<T>(fsm);
        }
    }
}