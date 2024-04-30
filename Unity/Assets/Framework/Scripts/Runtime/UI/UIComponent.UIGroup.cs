/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/10 10:27:39
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using UnityEngine;

namespace Runtime
{
    public sealed partial class UIComponent : FrameworkComponent
    {
        [Serializable]
        private sealed class UIGroup
        {
            [SerializeField] private string mName = null;

            [SerializeField] private int mDepth = 0;

            public string Name => mName;

            public int Depth => mDepth;
        }
    }
}