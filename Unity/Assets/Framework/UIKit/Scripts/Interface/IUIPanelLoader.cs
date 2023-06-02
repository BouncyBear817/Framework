using System;
using UnityEngine;

namespace Framework
{
    public interface IUIPanelLoader : IPoolable
    {
        GameObject LoadPanel(UIPanelInfo panelInfo);
        void LoadPanelAsync(UIPanelInfo panelInfo, Action<GameObject> onUIPanelLoaded);
    }

    public interface IUIPanelLoaderPool
    {
        IUIPanelLoader Allocate();

        void Recycle(IUIPanelLoader panelLoader);
    }
}