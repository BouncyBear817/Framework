using System;
using System.Collections.Generic;

namespace Framework
{
    public static class FrameworkEntry
    {
        private static readonly LinkedList<FrameworkModule> sFrameworkModules = new LinkedList<FrameworkModule>();

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var module in sFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public static void Shutdown()
        {
            for (var current = sFrameworkModules.Last; current != null; current = current.Next)
            {
                current.Value.Shutdown();
            }

            sFrameworkModules.Clear();
        }

        /// <summary>
        /// 获取框架模块
        /// </summary>
        /// <typeparam name="T">框架模块类型</typeparam>
        /// <returns>框架模块</returns>
        /// <remarks>如果获取的框架不存在，则自动创建框架模块</remarks>
        /// <exception cref="Exception"></exception>
        public static T GetModule<T>() where T : class
        {
            var interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must get module by interface, but ({interfaceType.FullName}) is not.");
            }

            if (interfaceType.FullName != null &&
                !interfaceType.FullName.StartsWith("Framework.", StringComparison.Ordinal))
            {
                throw new Exception($"You must get a framework module, but ({interfaceType.FullName}) is not.");
            }

            var moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            var moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new Exception($"Can not find framework module type ({moduleName})");
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取框架模块
        /// </summary>
        /// <param name="moduleType">框架模块类型</param>
        /// <remarks>如果获取的框架不存在，则自动创建框架模块</remarks>
        /// <returns>框架模块</returns>
        public static FrameworkModule GetModule(Type moduleType)
        {
            foreach (var module in sFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建框架模块
        /// </summary>
        /// <param name="moduleType">框架模块类型</param>
        /// <returns>框架模块</returns>
        /// <exception cref="Exception"></exception>
        private static FrameworkModule CreateModule(Type moduleType)
        {
            var module = Activator.CreateInstance(moduleType) as FrameworkModule;
            if (module == null)
            {
                throw new Exception($"Can not create module ({moduleType.FullName}).");
            }

            var current = sFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                sFrameworkModules.AddBefore(current, module);
            }
            else
            {
                sFrameworkModules.AddLast(module);
            }

            return module;
        }
    }
}