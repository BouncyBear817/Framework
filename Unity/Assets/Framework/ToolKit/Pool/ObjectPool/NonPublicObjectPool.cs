/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:27
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    public class NonPublicObjectPool<T> : Pool<T>, ISingleton where T : class, IPoolable
    {
        public static NonPublicObjectPool<T> Instance => SingletonProperty<NonPublicObjectPool<T>>.Instance;

        private NonPublicObjectPool()
        {
            Factory = new NonPublicObjectFactory<T>();
        }

        public int MaxCacheCount
        {
            get => MaxCount;
            set
            {
                MaxCount = value;

                if (CacheStack == null) return;
                
                if (MaxCount <= 0 || MaxCount >= CacheStack.Count) return;
                
                var removeCount = CacheStack.Count - MaxCount;
                while (removeCount > 0)
                {
                    CacheStack.Pop();
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
                    Recycle(Factory.Create());
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

            if (MaxCount > 0)
            {
                if (CacheStack.Count >= MaxCount)
                {
                    t.OnRecycle();
                    return true;
                }
            }

            t.OnRecycle();
            CacheStack.Push(t);
            return true;
        }

        public void OnSingletonInit()
        {
            
        }

        public void OnDispose()
        {
            SingletonProperty<NonPublicObjectPool<T>>.Instance.OnDispose();
        }
    }

    public interface IPoolable
    {
        void OnRecycle();
    }
}