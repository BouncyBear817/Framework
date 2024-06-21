/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:06
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 辅助器创建器相关的实用函数
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 创建辅助器
        /// </summary>
        /// <param name="helperTypeName">辅助器类型名</param>
        /// <param name="customHelper">若要创建的辅助器类型为空时，使用默认的自定义辅助器类型</param>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器</returns>
        public static T CreateHelper<T>(string helperTypeName, T customHelper) where T : MonoBehaviour
        {
            return CreateHelper(helperTypeName, customHelper, 0);
        }

        /// <summary>
        /// 创建辅助器
        /// </summary>
        /// <param name="helperTypeName">辅助器类型名</param>
        /// <param name="customHelper">若要创建的辅助器类型为空时，使用默认的自定义辅助器类型</param>
        /// <param name="index">创建的辅助器索引</param>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器</returns>
        public static T CreateHelper<T>(string helperTypeName, T customHelper, int index) where T : MonoBehaviour
        {
            T helper = null;
            if (!string.IsNullOrEmpty(helperTypeName))
            {
                var helperType = Framework.Utility.Assembly.GetType(helperTypeName);
                if (helperType == null)
                {
                    Log.Warning($"Can not find helper type ({helperTypeName}).");
                    return null;
                }
                if (!typeof(T).IsAssignableFrom(helperType))
                {
                    Log.Warning($"Type ({typeof(T).FullName}) is not assignable from ({helperType.FullName}).");
                    return null;
                }

                helper = new GameObject().AddComponent(helperType) as T;
            }
            else if (customHelper == null)
            {
                Log.Warning($"You must set base helper with ({typeof(T).FullName}) type first.");
                return null;
            }
            else if (customHelper.gameObject.scene.name != null)
            {
                helper = index > 0 ? Object.Instantiate(customHelper) : customHelper;
            }
            else
            {
                helper = Object.Instantiate(customHelper);
            }

            return helper;
        }
    }
}