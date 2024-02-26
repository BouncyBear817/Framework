/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 10:43:38
* Description:   
* Modify Record: 
*************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public static partial class ReferencePool
    {
        /// <summary>
        /// 引用集合
        /// </summary>
        private sealed class ReferenceCollection
        {
            private readonly Queue<IReference> mReferences;
            private readonly Type mReferenceType;
            private int mUsingReferenceCount;
            private int mAcquireReferenceCount;
            private int mReleaseReferenceCount;
            private int mAddReferenceCount;
            private int mRemoveReferenceCount;

            public ReferenceCollection(Type referenceType)
            {
                mReferences = new Queue<IReference>();
                mReferenceType = referenceType;
                mUsingReferenceCount = 0;
                mAcquireReferenceCount = 0;
                mReleaseReferenceCount = 0;
                mAddReferenceCount = 0;
                mRemoveReferenceCount = 0;
            }
            
            /// <summary>
            /// 引用类型
            /// </summary>
            public Type ReferenceType => mReferenceType;
            /// <summary>
            /// 未使用引用数量
            /// </summary>
            public int UnusedReferenceCount => mReferences.Count;
            /// <summary>
            /// 正在使用引用数量
            /// </summary>
            public int UsingReferenceCount => mUsingReferenceCount;
            /// <summary>
            /// 获取引用数量
            /// </summary>
            public int AcquireReferenceCount => mAcquireReferenceCount;
            /// <summary>
            /// 释放引用数量
            /// </summary>
            public int ReleaseReferenceCount => mReleaseReferenceCount;
            /// <summary>
            /// 增加引用数量
            /// </summary>
            public int AddReferenceCount => mAddReferenceCount;
            /// <summary>
            /// 移除引用数量
            /// </summary>
            public int RemoveReferenceCount => mRemoveReferenceCount;
            
            /// <summary>
            /// 获取引用实例，先从池内获取，无剩余则创建新的
            /// </summary>
            /// <typeparam name="T">引用类型</typeparam>
            /// <returns>引用</returns>
            /// <exception cref="Exception"></exception>
            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != mReferenceType)
                {
                    throw new Exception($"Acquire Reference ({nameof(T)}) is invalid.");
                }

                mUsingReferenceCount++;
                mAcquireReferenceCount++;
                lock (mReferences)
                {
                    if (mReferences.Count > 0)
                    {
                        return mReferences.Dequeue() as T;
                    }
                }

                mAddReferenceCount++;
                return new T();
            }
            
            /// <summary>
            /// 获取引用实例，先从池内获取，无剩余则创建新的
            /// </summary>
            /// <returns>引用</returns>
            public IReference Acquire()
            {
                mUsingReferenceCount++;
                mAcquireReferenceCount++;
                lock (mReferences)
                {
                    if (mReferences.Count > 0)
                    {
                        return mReferences.Dequeue();
                    }
                }

                mAddReferenceCount++;
                return Activator.CreateInstance(mReferenceType) as IReference;
            }
            
            /// <summary>
            /// 释放引用，将其放入引用池
            /// </summary>
            /// <param name="reference">引用类型</param>
            /// <exception cref="Exception"></exception>
            public void Release(IReference reference)
            {
                reference.Clear();
                lock (mReferences)
                {
                    if (mEnableStrictCheck && mReferences.Contains(reference))
                    {
                        throw new Exception($"The Reference ({nameof(reference)}) has been released.");
                    }
                    
                    mReferences.Enqueue(reference);
                }

                mReleaseReferenceCount++;
                mUsingReferenceCount--;
            }
            
            /// <summary>
            /// 在引用队列中增加一定数量的引用实例
            /// </summary>
            /// <param name="count">引用数量</param>
            /// <typeparam name="T">引用类型</typeparam>
            /// <exception cref="Exception"></exception>
            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != mReferenceType)
                {
                    throw new Exception($"Add Reference ({typeof(T).FullName}) is invalid.");
                }

                lock (mReferences)
                {
                    mAddReferenceCount += count;
                    while (count-- > 0)
                    {
                        mReferences.Enqueue(new T());
                    }
                }
            }
            
            /// <summary>
            /// 在引用队列中增加一定数量的引用
            /// </summary>
            /// <param name="count">引用数量</param>
            public void Add(int count)
            {
                lock (mReferences)
                {
                    mAddReferenceCount += count;
                    while (count-- > 0)
                    {
                        mReferences.Enqueue(Activator.CreateInstance(mReferenceType) as IReference);
                    }
                }
            }
            
            /// <summary>
            /// 在引用队列中移除一定数量的引用
            /// </summary>
            /// <param name="count">引用数量</param>
            public void Remove(int count)
            {
                lock (mReferences)
                {
                    count = mReferences.Count > count ? count : mReferences.Count;

                    mRemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        mReferences.Dequeue();
                    }
                }
            }
            
            /// <summary>
            /// 清空引用队列
            /// </summary>
            public void RemoveAll()
            {
                lock (mReferences)
                {
                    mRemoveReferenceCount += mReferences.Count;
                    mReferences.Clear();
                }
            }
        }
    }
}