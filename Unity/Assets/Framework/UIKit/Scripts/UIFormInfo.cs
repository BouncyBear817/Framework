using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UIKit
{
    public class UIFormInfo
    {
        public int ID;

        public string AssetName;

        public UILevel Level = UILevel.Common;

        public object UIData;

        public static UIFormInfo Allocate(int id, string assetName, UILevel level, object data)
        {
            return null;
        }
    }
}

