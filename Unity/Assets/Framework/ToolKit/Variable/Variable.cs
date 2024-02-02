using System;

namespace Framework
{
    /// <summary>
    /// 变量
    /// </summary>
    public abstract class Variable : IReference
    {
        protected Variable()
        {
        }

        /// <summary>
        /// 变量类型
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <returns></returns>
        public abstract object GetValue();

        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetValue(object value);

        /// <summary>
        /// 清理变量值
        /// </summary>
        public abstract void Clear();
    }
}