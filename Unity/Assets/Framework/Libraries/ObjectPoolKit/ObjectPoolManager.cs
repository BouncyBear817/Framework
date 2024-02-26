/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/18 11:05:26
 * Description:   对象池管理器
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public sealed partial class ObjectPoolManager : FrameworkModule, IObjectPoolManager
    {
        private readonly Dictionary<TypeNamePair, ObjectPoolBase> mObjectPools;
        private readonly List<ObjectPoolBase> mCachedObjectPools;

        public ObjectPoolManager()
        {
            mObjectPools = new Dictionary<TypeNamePair, ObjectPoolBase>();
            mCachedObjectPools = new List<ObjectPoolBase>();
        }


        public override int Priority => 0;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var (_, value) in mObjectPools)
            {
                value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        public override void Shutdown()
        {
            foreach (var (_, value) in mObjectPools)
            {
                value.Shutdown();
            }

            mObjectPools.Clear();
            mCachedObjectPools.Clear();
        }

        /// <summary>
        /// 对象池数量
        /// </summary>
        public int Count => mObjectPools.Count;

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return mObjectPools.ContainsKey(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return mObjectPools.ContainsKey(new TypeNamePair(objectType));
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return mObjectPools.ContainsKey(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return mObjectPools.ContainsKey(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>是否存在对象池</returns>
        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            foreach (var (_, value) in mObjectPools)
            {
                if (condition(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return InternalGetObjectPool(new TypeNamePair(typeof(T))) as IObjectPool<T>;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return InternalGetObjectPool(new TypeNamePair(objectType));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取的对象池</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalGetObjectPool(new TypeNamePair(typeof(T), name)) as IObjectPool<T>;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return InternalGetObjectPool(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            foreach (var (_, value) in mObjectPools)
            {
                if (condition(value))
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <returns>获取的对象池</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            var results = new List<ObjectPoolBase>();
            foreach (var (_, value) in mObjectPools)
            {
                if (condition(value))
                {
                    results.Add(value);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取对象池集合
        /// </summary>
        /// <param name="condition">获取对象池的条件</param>
        /// <param name="results">获取对象池的结果</param>
        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, value) in mObjectPools)
            {
                if (condition(value))
                {
                    results.Add(value);
                }
            }
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <returns>所有对象池</returns>
        public ObjectPoolBase[] GetAllObjectPools()
        {
            var results = new List<ObjectPoolBase>();
            foreach (var (_, value) in mObjectPools)
            {
                results.Add(value);
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="results">所有对象池</param>
        public void GetAllObjectPools(List<ObjectPoolBase> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, value) in mObjectPools)
            {
                results.Add(value);
            }
        }

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>生成的对象池</returns>
        public IObjectPool<T> SpawnObjectPool<T>(ObjectPoolInfo objectPoolInfo) where T : ObjectBase
        {
            var typeNamePair = new TypeNamePair(typeof(T), objectPoolInfo.Name);
            if (HasObjectPool<T>(objectPoolInfo.Name))
            {
                throw new Exception($"Already exist object pool ({typeNamePair})");
            }

            var objectPool = new ObjectPool<T>(objectPoolInfo);
            mObjectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectPoolInfo">对象池信息</param>
        /// <returns>生成的对象池</returns>
        public ObjectPoolBase SpawnObjectPool(Type objectType, ObjectPoolInfo objectPoolInfo)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            var typeNamePair = new TypeNamePair(objectType, objectPoolInfo.Name);
            if (HasObjectPool(objectType, objectPoolInfo.Name))
            {
                throw new Exception($"Already exist object pool ({typeNamePair})");
            }

            var objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);

            var objectPool = Activator.CreateInstance(objectPoolType, objectPoolInfo) as ObjectPoolBase;
            mObjectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("object type is invalid.");
            }

            if (typeof(ObjectPoolBase).IsAssignableFrom(objectType))
            {
                throw new Exception($"object type ({objectType.FullName}) is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPool">对象池</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            if (objectPool == null)
            {
                throw new Exception("object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), objectPool.Name));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPoolBase">对象池</param>
        /// <returns>是否销毁对象池</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPoolBase)
        {
            if (objectPoolBase == null)
            {
                throw new Exception("object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectPoolBase.ObjectType, objectPoolBase.Name));
        }

        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        public void Release()
        {
            GetAllObjectPools(mCachedObjectPools);
            foreach (var objectPool in mCachedObjectPools)
            {
                objectPool.Release();
            }
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="typeNamePair">类型和名称的组合值</param>
        /// <returns>对象池</returns>
        private ObjectPoolBase InternalGetObjectPool(TypeNamePair typeNamePair)
        {
            return mObjectPools.GetValueOrDefault(typeNamePair);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="typeNamePair">类型和名称的组合值</param>
        /// <returns>是否销毁成功</returns>
        private bool InternalDestroyObjectPool(TypeNamePair typeNamePair)
        {
            if (mObjectPools.TryGetValue(typeNamePair, out var value))
            {
                value.Shutdown();
                return mObjectPools.Remove(typeNamePair);
            }

            return false;
        }
    }
}