using System;
using System.Collections.Generic;

namespace Framework
{
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> mReferenceCollections =
            new Dictionary<Type, ReferenceCollection>();
        private static bool mEnableStrictCheck = false;
        
        /// <summary>
        /// 是否开启强制检查
        /// </summary>
        public static bool EnableStrictCheck
        {
            get => mEnableStrictCheck;
            set => mEnableStrictCheck = value;
        }
        
        /// <summary>
        /// 获取引用池的数量
        /// </summary>
        public static int Count => mReferenceCollections.Count;
        
        /// <summary>
        /// 内部检查引用类型
        /// </summary>
        /// <param name="referenceType"></param>
        /// <exception cref="Exception"></exception>
        private static void InternalCheckReferenceType(Type referenceType)
        {
            if (!EnableStrictCheck)
            {
                return;
            }

            if (referenceType == null)
            {
                throw new Exception($"ReferenceType is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new Exception($"Reference type ({referenceType.FullName}) is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new Exception($"Reference type ({referenceType.FullName}） is invalid.");
            }
        }
        
        /// <summary>
        /// 获取指定类型的引用集合
        /// </summary>
        /// <param name="referenceType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new Exception($"ReferenceType is invalid.");
            }

            ReferenceCollection referenceCollection = null;
            lock (mReferenceCollections)
            {
                if (!mReferenceCollections.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    mReferenceCollections.Add(referenceType, referenceCollection);
                }
            }

            return referenceCollection;
        }
        
        /// <summary>
        /// 从引用池中清除指定类型的引用集合
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        private static void Clear<T>() where T : class, IReference, new()
        {
            lock (mReferenceCollections)
            {
                if (mReferenceCollections.ContainsKey(typeof(T)))
                {
                    mReferenceCollections.Remove(typeof(T));
                }
            }
        }
        
        /// <summary>
        /// 从引用池中清除指定类型的引用集合
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        private static void Clear(Type referenceType)
        {
            lock (mReferenceCollections)
            {
                if (mReferenceCollections.ContainsKey(referenceType))
                {
                    mReferenceCollections.Remove(referenceType);
                }
            }
        }
        
        /// <summary>
        /// 获取所有引用池信息
        /// </summary>
        /// <returns></returns>
        public static ReferencePoolInfo[] GetAllReferencePoolInfos()
        {
            var index = 0;
            ReferencePoolInfo[] poolInfos = null;

            lock (mReferenceCollections)
            {
                poolInfos = new ReferencePoolInfo[Count];
                foreach (var (type, collection) in mReferenceCollections)
                {
                    poolInfos[index++] = new ReferencePoolInfo(type, collection.UnusedReferenceCount,
                        collection.UsingReferenceCount, collection.AcquireReferenceCount,
                        collection.ReleaseReferenceCount, collection.AddReferenceCount,
                        collection.RemoveReferenceCount);
                }
            }

            return poolInfos;
        }
        
        /// <summary>
        /// 清除所有引用池
        /// </summary>
        public static void ClearAll()
        {
            lock (mReferenceCollections)
            {
                foreach (var (type, collection) in mReferenceCollections)
                {
                    collection.RemoveAll();
                }
                mReferenceCollections.Clear();
            }
        }
        
        /// <summary>
        /// 从引用池中获取指定类型的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <returns>引用</returns>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }
        
        /// <summary>
        /// 从引用池中获取指定类型的引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <returns>引用</returns>
        public static IReference Acquire(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            return GetReferenceCollection(referenceType).Acquire();
        }
        
        /// <summary>
        /// 将引用释放进引用池
        /// </summary>
        /// <param name="reference"></param>
        /// <exception cref="Exception"></exception>
        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new Exception($"ReferenceType is invalid.");
            }

            var type = reference.GetType();
            InternalCheckReferenceType(type);
            GetReferenceCollection(type).Release(reference);
        }
        
        /// <summary>
        /// 向引用池中增加一定数量指定类型的引用
        /// </summary>
        /// <param name="count">数量</param>
        /// <typeparam name="T">引用类型</typeparam>
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }
        
        /// <summary>
        /// 向引用池中增加一定数量指定类型的引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <param name="count">数量</param>
        public static void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }
        
        /// <summary>
        /// 从引用池中移除一定数量指定类型的引用
        /// </summary>
        /// <param name="count">数量</param>
        /// <typeparam name="T">引用类型</typeparam>
        public static void Remove<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }
        
        /// <summary>
        /// 从引用池中移除一定数量指定类型的引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <param name="count">数量</param>
        public static void Remove(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }
        
        /// <summary>
        /// 从引用池中移除指定类型的所有引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        public static void RemoveAll<T>() where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
            Clear<T>();
        }
        
        /// <summary>
        /// 从引用池中移除指定类型的所有引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
            Clear(referenceType);
        }
    }
}