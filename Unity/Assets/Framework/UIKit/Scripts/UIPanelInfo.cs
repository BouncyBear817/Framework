

namespace Framework
{
    public class UIPanelInfo : IPoolable
    {
        public int ID;

        public string AssetName;

        public UILevel Level = UILevel.Common;

        public object UIData;

        public static UIPanelInfo Allocate(int id, string assetName, UILevel level, object uiData)
        {
            var uiFormInfo = ObjectPool<UIPanelInfo>.Instance.Allocate();
            uiFormInfo.ID = id;
            uiFormInfo.AssetName = assetName;
            uiFormInfo.Level = level;
            uiFormInfo.UIData = uiData;
            return null;
        }

        public void OnRecycle()
        {
            ID = -1;
            AssetName = null;
            Level = UILevel.Common;
            UIData = null;
        }
    }
}

