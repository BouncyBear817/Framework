/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/11 10:23:33
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    public static partial class Utility
    {
        public static partial class Json
        {
            /// <summary>
            /// Json辅助器接口
            /// </summary>
            public interface IJsonHelper
            {
                /// <summary>
                /// 将对象序列化为json字符串
                /// </summary>
                /// <param name="obj">对象</param>
                /// <returns>json字符串</returns>
                string ToJson(object obj);

                /// <summary>
                /// 将json字符串反序列化为对象
                /// </summary>
                /// <param name="json">json字符串</param>
                /// <typeparam name="T">对象</typeparam>
                /// <returns>对象</returns>
                T ToObject<T>(string json);

                /// <summary>
                /// 将json字符串反序列化为对象
                /// </summary>
                /// <param name="objectType">对象类型</param>
                /// <param name="json">json字符串</param>
                /// <returns>对象</returns>
                object ToObject(Type objectType, string json);
            }
        }
    }
}