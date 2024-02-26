/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:11:44
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 有限状态机基类
    /// </summary>
    public abstract class FsmBase
    {
        private string mName;

        protected FsmBase()
        {
            mName = string.Empty;
        }

        /// <summary>
        /// 有限状态机名称
        /// </summary>
        public string Name
        {
            get => mName;
            set => mName = value ?? string.Empty;
        }

        /// <summary>
        /// 有限状态机完整名称
        /// </summary>
        public string FullName => new TypeNamePair(OwnerType, mName).ToString();

        /// <summary>
        /// 有限状态机持有者类型
        /// </summary>
        public abstract Type OwnerType { get; }

        /// <summary>
        /// 有限状态机状态的数量
        /// </summary>
        public abstract int FsmStateCount { get; }

        /// <summary>
        /// 有限状态机是否在运行
        /// </summary>
        public abstract bool IsRunning { get; }

        /// <summary>
        /// 有限状态机是否被销毁
        /// </summary>
        public abstract bool IsDestroyed { get; }

        /// <summary>
        /// 有限状态机当前状态名称
        /// </summary>
        public abstract string CurrentStateName { get; }

        /// <summary>
        /// 有限状态机当前状态持续时间
        /// </summary>
        public abstract float CurrentStateTime { get; }

        /// <summary>
        /// 有限状态机轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理有限状态机
        /// </summary>
        public abstract void Shutdown();
    }
}