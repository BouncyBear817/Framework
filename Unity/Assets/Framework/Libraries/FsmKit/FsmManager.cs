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
    public sealed class FsmManager : FrameworkModule, IFsmManager
    {
        private readonly Dictionary<TypeNamePair, FsmBase> mFsms;
        private readonly List<FsmBase> mTempFsms;

        public FsmManager()
        {
            mFsms = new Dictionary<TypeNamePair, FsmBase>();
            mTempFsms = new List<FsmBase>();
        }

        public override int Priority => 1;

        public int Count => mFsms.Count;

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mTempFsms.Clear();
            if (mFsms.Count <= 0)
            {
                return;
            }

            foreach (var (_, fsm) in mFsms)
            {
                mTempFsms.Add(fsm);
            }

            foreach (var fsm in mTempFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理有限状态机
        /// </summary>
        public override void Shutdown()
        {
            foreach (var (_, fsm) in mFsms)
            {
                fsm.Shutdown();
            }

            mFsms.Clear();
            mTempFsms.Clear();
        }


        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>有限状态机</returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return InternalGetFsm(new TypeNamePair(typeof(T))) as IFsm<T>;
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>有限状态机</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>有限状态机</returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return InternalGetFsm(new TypeNamePair(typeof(T), name)) as IFsm<T>;
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机</returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <returns>所有有限状态机</returns>
        public FsmBase[] GetAllFsms()
        {
            int index = 0;
            var results = new FsmBase[mFsms.Count];
            foreach (var fsm in mFsms)
            {
                results[index++] = fsm.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <param name="results">所有有限状态机</param>
        public void GetAllFsms(List<FsmBase> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var fsm in results)
            {
                results.Add(fsm);
            }
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
            return CreateFsm(string.Empty, owner, states);
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
            var typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasFsm<T>(name))
            {
                throw new Exception($"Already exist fsm ({typeNamePair}).");
            }

            var fsm = Fsm<T>.Create(name, owner, states);
            mFsms.Add(typeNamePair, fsm);
            return fsm;
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>创建的有限状态机</returns>;
        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
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
            var typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasFsm<T>(name))
            {
                throw new Exception($"Already exist fsm ({typeNamePair}).");
            }

            var fsm = Fsm<T>.Create(name, owner, states);
            mFsms.Add(typeNamePair, fsm);
            return fsm;
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机类型</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            if (fsm == null)
            {
                throw new Exception("Fsm is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(fsm.OwnerType, fsm.Name));
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <typeparam name="T">有限状态机类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                throw new Exception("Fsm is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(typeof(T), fsm.Name));
        }

        private bool InternalHasFsm(TypeNamePair typeNamePair)
        {
            return mFsms.ContainsKey(typeNamePair);
        }

        private FsmBase InternalGetFsm(TypeNamePair typeNamePair)
        {
            if (mFsms.TryGetValue(typeNamePair, out var fsm))
            {
                return fsm;
            }

            return null;
        }

        private bool InternalDestroyFsm(TypeNamePair typeNamePair)
        {
            if (mFsms.TryGetValue(typeNamePair, out var fsm))
            {
                fsm.Shutdown();
                return mFsms.Remove(typeNamePair);
            }

            return false;
        }
    }
}