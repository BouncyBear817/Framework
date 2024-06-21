// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/13 10:48:26
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEngine;

namespace Framework.Runtime
{
    public static class ObjectExtension
    {
        public static void SetHelperTransform(this GameObject helper, string name, Transform parent)
        {
            helper.name = name;
            helper.transform.SetParent(parent);
            helper.transform.localScale = Vector3.one;
        }
    }
}