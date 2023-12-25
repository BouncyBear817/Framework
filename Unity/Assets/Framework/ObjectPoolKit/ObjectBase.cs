/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:27
* Description:   对象基类
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 对象基类
    /// </summary>
    public abstract class ObjectBase : IReference, IComparable<ObjectBase>
    {
        private string mName;
        private object mTarget;
        private bool mLocked;
        private int mPriority;

        protected ObjectBase()
        {
            mName = null;
            mTarget = null;
            mLocked = false;
            mPriority = 0;                                               
        }
        
        /// <summary>
        /// 获取对象名称
        /// </summary>
        public string Name => mName;
        
        /// <summary>
        /// 获取对象
        /// </summary>
        public object Target => mTarget;
        
        /// <summary>
        /// 对象是否加锁
        /// </summary>
        public bool Locked
        {
            get => mLocked;
            set => mLocked = value;
        }
        
        /// <summary>
        /// 对象的优先级
        /// </summary>
        public int Priority
        {
            get => mPriority;
            set => mPriority = value;
        }

        /// <summary>
        /// 获取释放检查标记
        /// </summary>
        public virtual bool CanReleaseFlag => true;
        
        /// <summary>
        /// 初始化对象基类
        /// </summary>
        /// <param name="target">对象</param>
        protected void Initialize(object target)
        {
            Initialize(null, target, false, 0);
        }
        
        /// <summary>
        /// 初始化对象基类
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="target">对象</param>
        /// <param name="locked">对象是否加锁</param>
        protected void Initialize(string name, object target, bool locked = false)
        {
            Initialize(name, target, locked, 0);
        }
        
        /// <summary>
        /// 初始化对象基类
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="target">对象</param>
        /// <param name="priority">对象优先级</param>
        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name, target, false, priority);
        }
        
        /// <summary>
        /// 初始化对象基类
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="target">对象</param>
        /// <param name="locked">对象是否加锁</param>
        /// <param name="priority">对象优先级</param>
        /// <exception cref="Exception"></exception>
        protected void Initialize(string name, object target, bool locked, int priority)
        {
            mName = name ?? string.Empty;
            mTarget = target ?? throw new Exception($"Target ({name}) is valid.");
            mLocked = locked;
            mPriority = priority;
        }
        
        /// <summary>
        /// 清理对象基类
        /// </summary>
        public virtual void Clear()
        {
            mName = null;
            mTarget = null;
            mLocked = false;
            mPriority = 0;
        }
        
        /// <summary>
        /// 获取对象时的事件
        /// </summary>
        protected internal virtual void OnSpawn() {}
        
        /// <summary>
        /// 回收对象时的事件
        /// </summary>
        protected internal virtual void OnUnSpawn() {}
        
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象池时触发</param>
        protected internal abstract void Release(bool isShutdown);
        

        public int CompareTo(ObjectBase other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return mPriority.CompareTo(other.mPriority);
        }
    }
}