using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IPool<T>
    {
        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        T Allocate();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Recycle(T t);
    }

    internal interface IPoolable
    {
        void OnRecycle();
    }

    /// <summary>
    /// 对象池抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class Pool<T> : IPool<T>
    {
        protected readonly Stack<T> mCacheStack = new Stack<T>();

        protected IObjectFactory<T> mFactory;

        protected int mMaxCount = 5;

        public int CurrentCacheCount
        {
            get { return mCacheStack.Count; }
        }

        public virtual T Allocate()
        {
            return mCacheStack.Count == 0 ? mFactory.Create() : mCacheStack.Pop();
        }
        public abstract bool Recycle(T t);
    }
}

