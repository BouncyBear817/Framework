/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 10:43:22
* Description:   对象池接口
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IObjectPool<T> where T : ObjectBase
    {
        /// <summary>
        /// 对象池名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 对象池对象类型
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// 对象池中对象的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 对象池中可被释放的对象数量
        /// </summary>
        int CanReleaseCount { get; }
        
        /// <summary>
        /// 是否允许对象多次生成
        /// </summary>
        bool AllowMultiSpawn { get; }

        /// <summary>
        /// 对象池中自动释放可释放对象的间隔秒数
        /// </summary>
        float AutoReleaseInterval { get; }

        /// <summary>
        /// 对象池容量
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// 对象池的优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="spawned">对象是否已被生成</param>
        void Register(T obj, bool spawned);

        /// <summary>
        /// 检查对象
        /// </summary>
        /// <returns></returns>
        bool CanSpawn();

        /// <summary>
        /// 检查对象
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns>检查的对象是否存在</returns>
        bool CanSpawn(string name);

        /// <summary>
        /// 生成对象
        /// </summary>
        /// <returns>对象</returns>
        T Spawn();

        /// <summary>
        /// 生成对象
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns>对象</returns>
        T Spawn(string name);

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj">对象</param>
        void UnSpawn(T obj);

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="target">对象</param>
        void UnSpawn(object target);
        
        /// <summary>
        /// 设置对象是否被加锁
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="locked">是否被加锁</param>
        void SetLocked(T obj, bool locked);
        
        /// <summary>
        /// 设置对象是否被加锁
        /// </summary>
        /// <param name="target">对象</param>
        /// <param name="locked">是否被加锁</param>
        void SetLocked(object target, bool locked);
        
        /// <summary>
        /// 设置对象的优先级
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="priority">优先级</param>
        void SetPriority(T obj, int priority);
        
        /// <summary>
        /// 设置对象的优先级
        /// </summary>
        /// <param name="target">对象</param>
        /// <param name="priority">优先级</param>
        void SetPriority(object target, int priority);
        
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">对象</param>
        bool ReleaseObject(T obj);
        
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="target">对象</param>
        bool ReleaseObject(object target);
        
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        void Release();
    }
}