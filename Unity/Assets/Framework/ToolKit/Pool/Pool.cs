using System.Collections.Generic;

namespace Framework
{
    

    /// <summary>
    /// 对象池抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T>
    {
        protected readonly Stack<T> CacheStack = new Stack<T>();

        protected IObjectFactory<T> Factory;

        protected int MaxCount = 5;

        protected int CurrentCacheCount => CacheStack.Count;
        
        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        public virtual T Allocate()
        {
            return CacheStack.Count == 0 ? Factory.Create() : CacheStack.Pop();
        }
        public abstract bool Recycle(T t);
    }
}

