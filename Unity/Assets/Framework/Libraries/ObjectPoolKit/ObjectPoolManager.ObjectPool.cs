/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/18 11:05:27
 * Description:   对象池管理（内部对象池）
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public sealed partial class ObjectPoolManager
    {
        /// <summary>
        /// 内部对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly MultiDictionary<string, Object<T>> mObjects;
            private readonly Dictionary<object, Object<T>> mObjectMap;

            private readonly List<T> mCachedCanReleaseObjects;

            private bool mAllowMultiSpawn;
            private float mAutoReleaseInterval;
            private int mCapacity;
            private int mPriority;
            private float mAutoReleaseTime;

            /// <summary>
            /// 初始化对象池的实例
            /// </summary>
            /// <param name="name">对象池名称</param>
            /// <param name="allowMultiSpawn">是否允许对象多次生成</param>
            /// <param name="autoReleaseInterval">对象池中自动释放可释放对象的间隔秒数</param>
            /// <param name="capacity">对象池容量</param>
            /// <param name="priority">对象池优先级</param>
            public ObjectPool(string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity,
                int priority) : base(name)
            {
                mObjects = new MultiDictionary<string, Object<T>>();
                mObjectMap = new Dictionary<object, Object<T>>();

                mCachedCanReleaseObjects = new List<T>();

                mAllowMultiSpawn = allowMultiSpawn;
                mAutoReleaseInterval = autoReleaseInterval;
                mCapacity = capacity;
                mPriority = priority;
                mAutoReleaseTime = 0;
            }

            /// <summary>
            /// 初始化对象池的实例
            /// </summary>
            /// <param name="objectPoolInfo">对象池信息</param>
            public ObjectPool(ObjectPoolInfo objectPoolInfo) : base(objectPoolInfo.Name)
            {
                mObjects = new MultiDictionary<string, Object<T>>();
                mObjectMap = new Dictionary<object, Object<T>>();

                mCachedCanReleaseObjects = new List<T>();

                mAllowMultiSpawn = objectPoolInfo.AllowMultiSpawn;
                mAutoReleaseInterval = objectPoolInfo.AutoReleaseInterval;
                mCapacity = objectPoolInfo.Capacity;
                mPriority = objectPoolInfo.Priority;
                mAutoReleaseTime = 0;
            }

            /// <summary>
            /// 对象池中对象类型
            /// </summary>
            public override Type ObjectType => typeof(T);

            /// <summary>
            /// 对象池中的对象的数量
            /// </summary>
            public override int Count => mObjectMap.Count;

            /// <summary>
            /// 对象池中可被释放的对象数量
            /// </summary>
            public override int CanReleaseCount => mCachedCanReleaseObjects.Count;

            /// <summary>
            /// 是否允许对象被多次生成
            /// </summary>
            public override bool AllowMultiSpawn => mAllowMultiSpawn;

            /// <summary>
            /// 对象池自动释放可释放对象的间隔秒数
            /// </summary>
            public override float AutoReleaseInterval
            {
                get => mAutoReleaseInterval;
                set => mAutoReleaseInterval = value;
            }

            /// <summary>
            /// 对象池容量
            /// </summary>
            /// <exception cref="Exception"></exception>
            public override int Capacity
            {
                get => mCapacity;
                set
                {
                    if (value < 0)
                    {
                        throw new Exception("Capacity is invalid.");
                    }

                    if (mCapacity == value)
                    {
                        return;
                    }

                    mCapacity = value;
                    Release();
                }
            }

            /// <summary>
            /// 对象池优先级
            /// </summary>
            public override int Priority
            {
                get => mPriority;
                set => mPriority = value;
            }

            /// <summary>
            /// 创建对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="spawned">对象是否已生成</param>
            /// <exception cref="Exception"></exception>
            public void Register(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                Object<T> internalObject = Object<T>.Create(obj, spawned);
                mObjects.Add(obj.Name, internalObject);
                mObjectMap.Add(obj.Target, internalObject);

                if (Count > mCapacity)
                {
                    Release();
                }
            }

            /// <summary>
            /// 检查对象
            /// </summary>
            /// <returns>对象是否已经生成</returns>
            public bool CanSpawn()
            {
                return CanSpawn(string.Empty);
            }

            /// <summary>
            /// 检查对象
            /// </summary>
            /// <param name="name">对象名称</param>
            /// <returns>对象是否已经生成</returns>
            /// <exception cref="Exception"></exception>
            public bool CanSpawn(string name)
            {
                if (name == null)
                {
                    throw new Exception("Name is invalid.");
                }

                if (mObjects.TryGetValue(name, out var linkedList))
                {
                    foreach (var obj in linkedList)
                    {
                        return mAllowMultiSpawn || !obj.IsInUse;
                    }
                }

                return false;
            }

            /// <summary>
            /// 生成对象
            /// </summary>
            /// <returns>对象</returns>
            public T Spawn()
            {
                return Spawn(string.Empty);
            }

            /// <summary>
            /// 生成对象
            /// </summary>
            /// <param name="name">对象名称</param>
            /// <returns>对象</returns>
            /// <exception cref="Exception"></exception>
            public T Spawn(string name)
            {
                if (name == null)
                {
                    throw new Exception("Name is invalid");
                }

                if (mObjects.TryGetValue(name, out var linkedList))
                {
                    foreach (var obj in linkedList)
                    {
                        return obj.Spawn();
                    }
                }

                return null;
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <exception cref="Exception"></exception>
            public void UnSpawn(T obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                UnSpawn(obj.Target);
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="target">对象</param>
            /// <exception cref="Exception"></exception>
            public void UnSpawn(object target)
            {
                if (target == null)
                {
                    throw new Exception("Object target is invalid.");
                }

                var obj = GetObject(target);
                if (obj != null)
                {
                    obj.UnSpawn();
                    if (Count > mCapacity && obj.SpawnCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new Exception(
                        $"Can not find target in object pool ({Name}), target type is ({target.GetType().FullName}). ");
                }
            }

            /// <summary>
            /// 设置对象是否被加锁
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="locked">是否被加锁</param>
            /// <exception cref="Exception"></exception>
            public void SetLocked(T obj, bool locked)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                SetLocked(obj.Target, locked);
            }

            /// <summary>
            /// 设置对象是否被加锁
            /// </summary>
            /// <param name="target">对象</param>
            /// <param name="locked">是否被加锁</param>
            /// <exception cref="Exception"></exception>
            public void SetLocked(object target, bool locked)
            {
                if (target == null)
                {
                    throw new Exception("Object target is invalid.");
                }

                var obj = GetObject(target);
                if (obj != null)
                {
                    obj.Locked = locked;
                }
                else
                {
                    throw new Exception(
                        $"Can not find target in object pool ({Name}), target type is ({target.GetType().FullName}). ");
                }
            }

            /// <summary>
            /// 设置对象的优先级
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(T obj, int priority)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                SetPriority(obj.Target, priority);
            }

            /// <summary>
            /// 设置对象的优先级
            /// </summary>
            /// <param name="target">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(object target, int priority)
            {
                if (target == null)
                {
                    throw new Exception("Object target is invalid.");
                }

                var obj = GetObject(target);
                if (obj != null)
                {
                    obj.Priority = priority;
                }
                else
                {
                    throw new Exception(
                        $"Can not find target in object pool ({Name}), target type is ({target.GetType().FullName}). ");
                }
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="obj">对象</param>
            public bool ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                return ReleaseObject(obj.Target);
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="target">对象</param>
            public bool ReleaseObject(object target)
            {
                if (target == null)
                {
                    throw new Exception("Object target is invalid.");
                }

                var obj = GetObject(target);
                if (obj == null || obj.IsInUse | obj.Locked || !obj.CanReleaseFlag)
                {
                    return false;
                }

                mObjects.Remove(obj.Name, obj);
                mObjectMap.Remove(obj.Peek().Target);

                obj.Release(false);
                ReferencePool.Release(obj);
                return true;
            }

            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            public override void Release()
            {
                mAutoReleaseTime = 0f;
                GetCanReleaseObjects();
                if (mCachedCanReleaseObjects.Count > 0)
                {
                    foreach (var obj in mCachedCanReleaseObjects)
                    {
                        ReleaseObject(obj);
                    }
                }
            }

            /// <summary>
            /// 获取所有对象信息
            /// </summary>
            /// <returns>所有对象信息</returns>
            public override ObjectInfo[] GetAllObjectInfos()
            {
                var results = new List<ObjectInfo>();
                foreach (var (_, value) in mObjects)
                {
                    results.AddRange(value.Select(obj =>
                        new ObjectInfo(obj.Name, obj.Locked, obj.CanReleaseFlag, obj.Priority, obj.SpawnCount)));
                }

                return results.ToArray();
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                mAutoReleaseTime += realElapseSeconds;
                if (mAutoReleaseTime > mAutoReleaseInterval)
                {
                    Release();
                }
            }

            internal override void Shutdown()
            {
                foreach (var (_, value) in mObjectMap)
                {
                    value.Release(true);
                    ReferencePool.Release(value);
                }

                mObjects.Clear();
                mObjectMap.Clear();
                mCachedCanReleaseObjects.Clear();
            }
            
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <param name="target">对象</param>
            /// <returns>对象</returns>
            /// <exception cref="Exception"></exception>
            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new Exception("Object target is invalid.");
                }

                return mObjectMap.GetValueOrDefault(target);
            }
            
            /// <summary>
            /// 获取可释放对象集合
            /// </summary>
            private void GetCanReleaseObjects()
            {
                mCachedCanReleaseObjects.Clear();
                foreach (var (_, value) in mObjectMap)
                {
                    if (value.IsInUse || value.Locked || value.CanReleaseFlag)
                    {
                        continue;
                    }

                    mCachedCanReleaseObjects.Add(value.Peek());
                }
            }
        }
    }
}