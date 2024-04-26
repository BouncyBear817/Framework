// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/4/18 16:18:50
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 主入口辅助器
    /// </summary>
    public static class MainEntryHelper
    {
        private static readonly LinkedList<FrameworkComponent> sFrameworkComponents =
            new LinkedList<FrameworkComponent>();

        /// <summary>
        /// 框架所在的场景编号
        /// </summary>
        internal const int FrameworkSceneId = 0;

        /// <summary>
        /// 获取框架组件
        /// </summary>
        /// <typeparam name="T">框架组件类型</typeparam>
        /// <returns>框架组件</returns>
        public static T GetComponent<T>() where T : FrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取框架组件
        /// </summary>
        /// <param name="type">框架组件类型</param>
        /// <returns>框架组件</returns>
        public static FrameworkComponent GetComponent(Type type)
        {
            var current = sFrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// 注册框架组件
        /// </summary>
        /// <param name="component">框架组件类型</param>
        /// <exception cref="Exception"></exception>
        public static void RegisterComponent(FrameworkComponent component)
        {
            if (component == null)
            {
                throw new Exception("Framework component is invalid.");
            }

            var type = component.GetType();

            var current = sFrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    throw new Exception($"Framework component ({type.FullName}) is already exist.");
                }

                current = current.Next;
            }

            sFrameworkComponents.AddLast(component);
        }

        /// <summary>
        /// 关闭游戏框架
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型</param>
        public static void Shutdown(ShutdownType shutdownType)
        {
            Log.Info($"Shutdown framework ({shutdownType})......");
            var baseComponent = GetComponent<BaseComponent>();
            if (baseComponent != null)
            {
                baseComponent.Shutdown();
                baseComponent = null;
            }

            sFrameworkComponents.Clear();

            switch (shutdownType)
            {
                case ShutdownType.Restart:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(FrameworkSceneId);
                    break;
                case ShutdownType.Quit:
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;
            }
        }
    }
}