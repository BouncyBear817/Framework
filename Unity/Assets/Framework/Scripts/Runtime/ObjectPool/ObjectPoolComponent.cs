/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 对象池组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Object Pool")]
    public sealed class ObjectPoolComponent : FrameworkComponent
    {
        private IObjectPoolManager mObjectPoolManager;

        /// <summary>
        /// 对象池数量
        /// </summary>
        public int Count => mObjectPoolManager.Count;

        protected override void Awake()
        {
            base.Awake();

            mObjectPoolManager = FrameworkEntry.GetModule<IObjectPoolManager>();
            if (mObjectPoolManager == null)
            {
                Log.Error("Object pool manager is invalid.");
            }
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return mObjectPoolManager.HasObjectPool<T>();
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(Type objectType)
        {
            return mObjectPoolManager.HasObjectPool(objectType);
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return mObjectPoolManager.HasObjectPool<T>(name);
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool(Type objectType, string name)
        {
            return mObjectPoolManager.HasObjectPool(objectType, name);
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            return mObjectPoolManager.HasObjectPool(condition);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return mObjectPoolManager.GetObjectPool<T>();
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            return mObjectPoolManager.GetObjectPool(objectType);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return mObjectPoolManager.GetObjectPool<T>(name);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            return mObjectPoolManager.GetObjectPool(objectType, name);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            return mObjectPoolManager.GetObjectPool(condition);
        }

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            return mObjectPoolManager.GetObjectPools(condition);
        }

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <param name="results">获取对象池的结果</param>
        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            mObjectPoolManager.GetObjectPools(condition, results);
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <returns>所有对象池</returns>
        public ObjectPoolBase[] GetAllObjectPools()
        {
            return mObjectPoolManager.GetAllObjectPools();
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="results">所有对象池</param>
        public void GetAllObjectPools(List<ObjectPoolBase> results)
        {
            mObjectPoolManager.GetAllObjectPools(results);
        }

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>生成的对象池</returns>
        public IObjectPool<T> SpawnObjectPool<T>(ObjectPoolInfo objectPoolInfo) where T : ObjectBase
        {
            return mObjectPoolManager.SpawnObjectPool<T>(objectPoolInfo);
        }

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <returns>生成的对象池</returns>
        public ObjectPoolBase SpawnObjectPool(Type objectType, ObjectPoolInfo objectPoolInfo)
        {
            return mObjectPoolManager.SpawnObjectPool(objectType, objectPoolInfo);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return mObjectPoolManager.DestroyObjectPool<T>();
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            return mObjectPoolManager.DestroyObjectPool(objectType);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return mObjectPoolManager.DestroyObjectPool<T>(name);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            return mObjectPoolManager.DestroyObjectPool(objectType, name);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPool">对象池</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            return mObjectPoolManager.DestroyObjectPool<T>(objectPool);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPoolBase">对象池</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPoolBase)
        {
            return mObjectPoolManager.DestroyObjectPool(objectPoolBase);
        }

        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        public void Release()
        {
            mObjectPoolManager.Release();
        }
    }
}