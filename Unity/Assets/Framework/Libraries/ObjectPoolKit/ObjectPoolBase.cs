/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:26
* Description:   对象池基类
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 对象池基类
    /// </summary>
    public abstract class ObjectPoolBase : IComparable<ObjectPoolBase>
    {
        private readonly string mName;
        
        /// <summary>
        /// 初始化对象池基类的实例
        /// </summary>
        public ObjectPoolBase() : this(null)
        {
        }
        
        /// <summary>
        /// 初始化对象池基类的实例
        /// </summary>
        /// <param name="name">对象池名称</param>
        public ObjectPoolBase(string name)
        {
            mName = name ?? string.Empty;
        }

        /// <summary>
        /// 对象池名称
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// 对象池中对象类型
        /// </summary>
        public abstract Type ObjectType { get; }

        /// <summary>
        /// 对象池中的对象的数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 对象池中可被释放的对象数量
        /// </summary>
        public abstract int CanReleaseCount { get; }
        
        /// <summary>
        /// 是否允许对象被多次生成
        /// </summary>
        public abstract bool AllowMultiSpawn { get; }

        /// <summary>
        /// 对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public abstract float AutoReleaseInterval { get; set; }

        /// <summary>
        /// 对象池容量
        /// </summary>
        public abstract int Capacity { get; set; }

        /// <summary>
        /// 对象池优先级
        /// </summary>
        public abstract int Priority { get; set; }

        /// <summary>
        /// 释放对象池中未使用的对象
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 获取所有对象信息
        /// </summary>
        /// <returns>所有对象信息</returns>
        public abstract ObjectInfo[] GetAllObjectInfos();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 
        /// </summary>
        internal abstract void Shutdown();

        public int CompareTo(ObjectPoolBase other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}