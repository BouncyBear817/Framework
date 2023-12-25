/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:27
* Description:   对象池管理器接口
* Modify Record: 
*************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 对象池管理器接口
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 对象池数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        bool HasObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Type objectType);

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        bool HasObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否存在对象池</returns>
        bool HasObjectPool(Type objectType, string name);

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>是否存在对象池</returns>
        bool HasObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>获取的对象池</returns>
        ObjectPoolBase GetObjectPool(Type objectType);

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>获取的对象池</returns>
        ObjectPoolBase GetObjectPool(Type objectType, string name);

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <param name="results">获取对象池的结果</param>
        void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results);

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <returns>所有对象池</returns>
        ObjectPoolBase[] GetAllObjectPools();

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="results">所有对象池</param>
        void GetAllObjectPools(List<ObjectPoolBase> results);

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>生成的对象池</returns>
        IObjectPool<T> SpawnObjectPool<T>(ObjectPoolInfo objectPoolInfo) where T : ObjectBase;

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <returns>生成的对象池</returns>
        ObjectPoolBase SpawnObjectPool(Type objectType, ObjectPoolInfo objectPoolInfo);

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool(Type objectType);

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool(Type objectType, string name);

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPool">对象池</param>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPoolBase">对象池</param>
        /// <returns>是否销毁对象池</returns>
        bool DestroyObjectPool(ObjectPoolBase objectPoolBase);

        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        void Release();
    }
}