// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/11 10:23:33
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// Json实用函数
        /// </summary>
        public static partial class Json
        {
            private static IJsonHelper sJsonHelper = null;

            /// <summary>
            /// 设置json辅助器
            /// </summary>
            /// <param name="jsonHelper">json辅助器</param>
            public static void SetJsonHelper(IJsonHelper jsonHelper)
            {
                sJsonHelper = jsonHelper;
            }

            /// <summary>
            /// 将对象序列化为json字符串
            /// </summary>
            /// <param name="obj">对象</param>
            /// <returns>json字符串</returns>
            public static string ToJson(object obj)
            {
                if (sJsonHelper == null)
                {
                    throw new Exception("Json helper is invalid.");
                }

                try
                {
                    return sJsonHelper.ToJson(obj);
                }
                catch (Exception e)
                {
                    throw new Exception($"Object can not convert to json with exception ({e})");
                }
            }

            /// <summary>
            /// 将json字符串反序列化为对象
            /// </summary>
            /// <param name="json">json字符串</param>
            /// <typeparam name="T">对象</typeparam>
            /// <returns>对象</returns>
            public static T ToObject<T>(string json)
            {
                if (sJsonHelper == null)
                {
                    throw new Exception("Json helper is invalid.");
                }

                try
                {
                    return sJsonHelper.ToObject<T>(json);
                }
                catch (Exception e)
                {
                    throw new Exception($"Json can not convert to object with exception ({e})");
                }
            }

            /// <summary>
            /// 将json字符串反序列化为对象
            /// </summary>
            /// <param name="objectType">对象类型</param>
            /// <param name="json">json字符串</param>
            /// <returns>对象</returns>
            public static object ToObject(Type objectType, string json)
            {
                if (sJsonHelper == null)
                {
                    throw new Exception("Json helper is invalid.");
                }

                try
                {
                    return sJsonHelper.ToObject(objectType, json);
                }
                catch (Exception e)
                {
                    throw new Exception($"Json can not convert to object with exception ({e})");
                }
            }
        }
    }
}