/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using UnityEngine;

namespace Framework.Runtime
{
    public abstract class FrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            MainEntryHelper.RegisterComponent(this);
        }
    }
}