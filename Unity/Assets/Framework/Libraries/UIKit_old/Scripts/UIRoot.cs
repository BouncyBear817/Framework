using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class UIRoot : MonoBehaviour, ISingleton
    {
        public const string UIRootPath = "UIRoot";
        
        public Canvas UICanvas;
        public Camera UICamera;
        public CanvasScaler UICanvasScaler;
        public GraphicRaycaster UIGraphicRaycaster;

        public RectTransform Bg;
        public RectTransform Common;
        public RectTransform PopUI;
        public RectTransform DesignRoot;

        private static UIRoot mInstance;

        public static UIRoot Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<UIRoot>();
                }

                if (mInstance == null)
                {
                    Instantiate(Resources.Load<GameObject>(UIRootPath));
                    mInstance = MonoSingletonProperty<UIRoot>.Instance;
                    mInstance.name = "UIRoot";
                    DontDestroyOnLoad(mInstance);
                }

                return mInstance;
            }
        }

        private void Awake()
        {
            DesignRoot.SetActive(false);
        }

        public void SetResolution(int width, int height, float matchWidthOrHeight)
        {
            UICanvasScaler.referenceResolution = new Vector2(width, height);
            UICanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            UICanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        }

        public Vector2 GetResolution()
        {
            return UICanvasScaler.referenceResolution;
        }

        public void SetLevelOfPanel(UILevel level, IUIPanel panel)
        {
            switch (level)
            {
                case UILevel.Bg:
                    panel.Transform.SetParent(Bg);
                    break;
                case UILevel.Common:
                    panel.Transform.SetParent(Bg);
                    break;
                case UILevel.Pop:
                    panel.Transform.SetParent(Bg);
                    break;
            }
        }
        
        public void OnSingletonInit()
        {
            
        }
    }
}