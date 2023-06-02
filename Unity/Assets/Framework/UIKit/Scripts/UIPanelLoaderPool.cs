using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public abstract class AbstractUIPanelLoaderPool : IUIPanelLoaderPool
    {
        private const int LoaderStackCount = 10;
        
        private Stack<IUIPanelLoader> mPoolStack = new Stack<IUIPanelLoader>(LoaderStackCount);

        protected abstract IUIPanelLoader CreateUIPanelLoader();
        
        public IUIPanelLoader Allocate()
        {
            return mPoolStack.Count > 0 ? mPoolStack.Pop() : CreateUIPanelLoader();
        }

        public void Recycle(IUIPanelLoader panelLoader)
        {
            panelLoader.OnRecycle();
            mPoolStack.Push(panelLoader);
        }
    }
    
    public class DefaultUIPanelLoaderPool : AbstractUIPanelLoaderPool
    {
        public class DefaultUIPanelLoader : IUIPanelLoader
        {
            private GameObject mPanel;
            public GameObject LoadPanel(UIPanelInfo panelInfo)
            {
                mPanel = Resources.Load<GameObject>(panelInfo.AssetName);
                return mPanel;
            }

            public void LoadPanelAsync(UIPanelInfo panelInfo, Action<GameObject> onUIPanelLoaded)
            {
                var request = Resources.LoadAsync<GameObject>(panelInfo.AssetName);

                request.completed += operation => onUIPanelLoaded(request.asset as GameObject);
            }

            public void OnRecycle()
            {
                mPanel = null;
            }
        }
        
        protected override IUIPanelLoader CreateUIPanelLoader()
        {
            return new DefaultUIPanelLoader();
        }
    }
}