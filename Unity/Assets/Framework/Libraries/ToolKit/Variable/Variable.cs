using System;

namespace Framework
{
    /// <summary>
    /// 变量
    /// </summary>
    public abstract class Variable : IReference
    {
        public Variable()
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

    /// <summary>
    /// 变量
    /// </summary>
    /// <typeparam name="T">变量类型</typeparam>
    public abstract class Variable<T> : Variable
    {
        private T mValue;

        public Variable()
        {
            mValue = default(T);
        }

        /// <summary>
        /// 变量类型
        /// </summary>
        public override Type Type => typeof(T);

        /// <summary>
        /// 变量值
        /// </summary>
        public T Value
        {
            get => mValue;
            set => mValue = value;
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return mValue;
        }

        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
            mValue = (T)value;
        }

        /// <summary>
        /// 清理变量值
        /// </summary>
        public override void Clear()
        {
            mValue = default(T);
        }
    }
}