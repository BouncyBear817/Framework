/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 16:58:37
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// Unity扩展方法
    /// </summary>
    public static class UnityExtension
    {
        private static readonly List<Transform> sCachedTransforms = new List<Transform>();

        /// <summary>
        /// 获取或增加组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <typeparam name="T">要获取或增加的组件</typeparam>
        /// <returns>获取或增加组件</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="type">要获取或增加的组件</param>
        /// <returns>获取或增加组件</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            if (typeof(Component).IsAssignableFrom(type))
            {
                throw new Exception("This component is not assignable from type.");
            }

            var component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 递归设置游戏对象的层级
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="layer">游戏对象的层级</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.GetComponentsInChildren(true, sCachedTransforms);
            foreach (var t in sCachedTransforms)
            {
                t.gameObject.layer = layer;
            }

            sCachedTransforms.Clear();
        }
    }
}