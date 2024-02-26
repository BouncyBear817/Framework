using System;
using System.Collections.Generic;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// 程序集相关的实用函数
        /// </summary>
        public static class Assembly
        {
            private static readonly System.Reflection.Assembly[] sAssemblies = null;

            private static readonly Dictionary<string, Type> sCachedTypes =
                new Dictionary<string, Type>(StringComparer.Ordinal);

            static Assembly()
            {
                sAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            /// <summary>
            /// 获取已加载的程序集
            /// </summary>
            /// <returns>已加载的程序集</returns>
            public static System.Reflection.Assembly[] GetAssemblies()
            {
                return sAssemblies;
            }

            /// <summary>
            /// 获取已加载的程序集中的所有类型
            /// </summary>
            /// <returns>已加载的程序集中的所有类型</returns>
            public static Type[] GetTypes()
            {
                var results = new List<Type>();
                foreach (var assembly in sAssemblies)
                {
                    results.AddRange(assembly.GetTypes());
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取已加载的程序集中的所有类型
            /// </summary>
            /// <param name="results">已加载的程序集中的所有类型</param>
            /// <exception cref="Exception"></exception>
            public static void GetTypes(List<Type> results)
            {
                if (results == null)
                {
                    throw new Exception("Assembly get types results is invalid.");
                }

                results.Clear();
                foreach (var assembly in sAssemblies)
                {
                    results.AddRange(assembly.GetTypes());
                }
            }

            /// <summary>
            /// 获取已加载的程序集中的指定类型
            /// </summary>
            /// <param name="typeName">类型名称</param>
            /// <returns>已加载的程序集中的指定类型</returns>
            /// <exception cref="Exception"></exception>
            public static Type GetType(string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new Exception("Assembly type name is invalid.");
                }

                if (sCachedTypes.TryGetValue(typeName, out var type))
                {
                    return type;
                }

                type = Type.GetType(typeName);
                if (type != null)
                {
                    sCachedTypes.Add(typeName, type);
                    return type;
                }

                foreach (var assembly in sAssemblies)
                {
                    type = Type.GetType($"{typeName}, {assembly.FullName}");
                    if (type != null)
                    {
                        sCachedTypes.Add(typeName, type);
                        return type;
                    }
                }

                return null;
            }
        }
    }
}