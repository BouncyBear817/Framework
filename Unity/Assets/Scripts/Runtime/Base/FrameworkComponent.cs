/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using UnityEngine;

namespace Runtime
{
    public abstract class FrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            MainEntry.Helper.RegisterComponent(this);
        }
    }
}