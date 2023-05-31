using System.Collections.Generic;

namespace Framework
{
    

    /// <summary>
    /// 对象池抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T>
    {
        protected readonly Stack<T> mCacheStack = new Stack<T>();

        protected IObjectFactory<T> mFactory;

        protected int mMaxCount = 5;

        protected int CurrentCacheCount => mCacheStack.Count;

        public virtual T Allocate()
        {
            return mCacheStack.Count == 0 ? mFactory.Create() : mCacheStack.Pop();
        }
        public abstract bool Recycle(T t);
    }
}

