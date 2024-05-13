// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/11 10:37:16
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 默认Json辅助器
    /// </summary>
    public class DefaultJsonHelper : Utility.Json.IJsonHelper
    {
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json字符串</returns>
        public string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        /// <summary>
        /// 将json字符串反序列化为对象
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>对象</returns>
        public T ToObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// 将json字符串反序列化为对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="json">json字符串</param>
        /// <returns>对象</returns>
        public object ToObject(Type objectType, string json)
        {
            return JsonUtility.FromJson(json, objectType);
        }
    }
}