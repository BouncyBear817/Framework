using System;

namespace Framework
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">需实现IPoolable</typeparam>
    public class ObjectPool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {
        public ObjectPool()
        {
            mFactory = new DefaultObjectFactory<T>();
        }

        public static ObjectPool<T> Instance => SingletonProperty<ObjectPool<T>>.Instance;

        public int MaxCacheCount
        {
            get => mMaxCount;
            set
            {
                mMaxCount = value;

                if (mCacheStack == null) return;
                
                if (mMaxCount <= 0 || mMaxCount >= mCacheStack.Count) return;
                
                var removeCount = mCacheStack.Count - mMaxCount;
                while (removeCount > 0)
                {
                    mCacheStack.Pop();
                    --removeCount;
                }
            }
        }

        /// <summary>
        /// 设置最大的缓存数量及初始的数量
        /// </summary>
        /// <param name="maxCount">最大缓存数量</param>
        /// <param name="initCount">初始化数量</param>
        public void Init(int maxCount, int initCount)
        {
            MaxCacheCount = maxCount;

            if (maxCount > 0)
            {
                initCount = Math.Max(maxCount, initCount);
            }

            if (CurrentCacheCount < initCount)
            {
                for (var i = CurrentCacheCount; i < initCount; ++i)
                {
                    Recycle(new T());
                }
            }
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        public override T Allocate()
        {
            return base.Allocate();
        }

        /// <summary>
        /// 回收对象进池中
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override bool Recycle(T t)
        {
            if (t == null) return false;

            if (mMaxCount > 0)
            {
                if (mCacheStack.Count >= mMaxCount)
                {
                    t.OnRecycle();
                    return true;
                }
            }

            t.OnRecycle();
            mCacheStack.Push(t);
            return true;
        }

        public void OnSingletonInit()
        {
            
        }

        public void OnDispose()
        {
            SingletonProperty<ObjectPool<T>>.Instance.OnDispose();
        }
    }
}