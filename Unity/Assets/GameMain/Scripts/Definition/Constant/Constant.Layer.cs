// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/29 15:36:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEngine;

namespace GameMain
{
    public static partial class Constant
    {
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);
        }
    }
}