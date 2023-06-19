
using UnityEngine;

namespace Framework
{
    public interface IUIPanel
    {
        Transform Transform { get; }
        
        IUIPanelLoader Loader { get; set; }
        
        UIPanelInfo PanelInfo { get; set; }
        
        UIPanelState State { get; set; }

        void Init();

        void Open(IUIPanelData data);

        void Show();

        void Hide();

        void Recycle();
    }
}

