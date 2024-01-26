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