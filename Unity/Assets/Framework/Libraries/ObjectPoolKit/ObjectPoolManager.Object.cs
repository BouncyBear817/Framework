/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:27
* Description:   对象池管理（内部对象）
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    public sealed partial class ObjectPoolManager
    {
        /// <summary>
        /// 内部对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        private sealed class Object<T> : IReference where T : ObjectBase
        {
            private T mObject;
            private int mSpawnCount;

            public Object()
            {
                mObject = null;
                mSpawnCount = 0;
            }

            /// <summary>
            /// 对象名称
            /// </summary>
            public string Name => mObject.Name;

            /// <summary>
            /// 对象是否被加锁
            /// </summary>
            public bool Locked
            {
                get => mObject.Locked;
                set => mObject.Locked = value;
            }

            /// <summary>
            /// 对象优先级
            /// </summary>
            public int Priority
            {
                get => mObject.Priority;
                set => mObject.Priority = value;
            }

            /// <summary>
            /// 对象释放检查标记
            /// </summary>
            public bool CanReleaseFlag => mObject.CanReleaseFlag;

            /// <summary>
            /// 对象是否正在使用
            /// </summary>
            public bool IsInUse => mSpawnCount > 0;

            /// <summary>
            /// 对象生成次数
            /// </summary>
            public int SpawnCount => mSpawnCount;

            /// <summary>
            /// 创建内部对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="spawned">是否已被生成</param>
            /// <returns>内部对象</returns>
            /// <exception cref="Exception"></exception>
            public static Object<T> Create(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                Object<T> internalObject = ReferencePool.Acquire<Object<T>>();
                internalObject.mObject = obj;
                internalObject.mSpawnCount = spawned ? 1 : 0;
                if (spawned)
                {
                    obj.OnSpawn();
                }

                return internalObject;
            }

            /// <summary>
            /// 清理内部对象
            /// </summary>
            public void Clear()
            {
                mObject = null;
                mSpawnCount = 0;
            }

            /// <summary>
            /// 查看对象
            /// </summary>
            /// <returns>内部对象</returns>
            public T Peek()
            {
                return mObject;
            }

            /// <summary>
            /// 生成对象
            /// </summary>
            /// <returns>内部对象</returns>
            public T Spawn()
            {
                mSpawnCount++;
                mObject.OnSpawn();
                return mObject;
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            /// <exception cref="Exception"></exception>
            public void UnSpawn()
            {
                mObject.OnUnSpawn();
                mSpawnCount--;
                if (mSpawnCount < 0)
                {
                    throw new Exception($"Object ({Name}) spawn count is less than 0.");
                }
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="isShutdown"></param>
            public void Release(bool isShutdown)
            {
                mObject.Release(isShutdown);
                ReferencePool.Release(mObject);
            }
        }
    }
}