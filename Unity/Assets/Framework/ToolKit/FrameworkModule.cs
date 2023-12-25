namespace Framework
{
    /// <summary>
    /// 框架模块抽象类
    /// </summary>
    public abstract class FrameworkModule
    {
        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public virtual int Priority => 0;
        
        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public abstract void Update(float elapseSeconds, float realElapseSeconds);
        
        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public abstract void Shutdown();
    }
}