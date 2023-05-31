namespace Framework
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T>
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

    public interface IPoolable
    {
        void OnRecycle();
    }
}