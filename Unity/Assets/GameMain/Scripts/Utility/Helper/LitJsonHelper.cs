// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/21 10:25:48
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using LitJson;

namespace GameMain.Utility.Helper
{
    /// <summary>
    /// LitJson函数集辅助器
    /// </summary>
    public class LitJsonHelper : Framework.Utility.Json.IJsonHelper
    {
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json字符串</returns>
        public string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        /// <summary>
        /// 将json字符串反序列化为对象
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>对象</returns>
        public T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        /// <summary>
        /// 将json字符串反序列化为对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="json">json字符串</param>
        /// <returns>对象</returns>
        public object ToObject(Type objectType, string json)
        {
            throw new NotSupportedException("'ToObject(Type objectType, string json)' is not supported, please use 'ToObject<T>(string json)'.");
        }
    }
}