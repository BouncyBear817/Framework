using System;

namespace Framework
{
    /// <summary>
    /// 类型和名称的组合值
    /// </summary>
    public struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type mType;
        private readonly string mName;

        public TypeNamePair(Type type) : this(type, string.Empty)
        {
        }

        public TypeNamePair(Type type, string name)
        {
            mType = type ?? throw new Exception("Type is invalid.");
            mName = name ?? string.Empty;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type => mType;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="obj">比较的对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return obj is TypeNamePair pair && Equals(pair);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return mType.GetHashCode() ^ mName.GetHashCode();
        }

        /// <summary>
        /// 获取类型和名称的组合值的字符串
        /// </summary>
        /// <returns>类型和名称的组合值的字符串</returns>
        /// <exception cref="Exception"></exception>
        public override string ToString()
        {
            if (mType == null)
            {
                throw new Exception("Type is invalid.");
            }

            string typeName = mType.FullName;
            return $"{typeName}.{mName}";
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="other">比较的对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(TypeNamePair other)
        {
            return mType == other.mType && mName == other.mName;
        }
        
        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="a">对象a</param>
        /// <param name="b">对象b</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }
        
        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="a">对象a</param>
        /// <param name="b">对象b</param>
        /// <returns>是否相等</returns>
        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }
}