using UnityEngine;

namespace Framework
{
    public interface IUIPanelData
    {
    }

    public abstract class UIPanel : UIPanelBehaviour, IUIPanel
    {
        public Transform Transform => transform;
        public IUIPanelLoader Loader { get; set; }
        public UIPanelInfo PanelInfo { get; set; }
        public UIPanelState State { get; set; }
        
        public void Init()
        {
            OnInit();
        }

        public void Open(IUIPanelData data)
        {
            OnOpen(data);
            Show();
        }

        public void Show()
        {
            State = UIPanelState.Show;
            Transform.SetActive(true);
        }

        private void Update()
        {
            OnUpdate();
        }

        public void Hide()
        {
            State = UIPanelState.Hide;
            Transform.SetActive(false);
            OnHide();
        }

        public void Recycle()
        {
            State = UIPanelState.Recycle;
            OnRecycle();
        }

        protected virtual void OnInit() {}
        protected virtual void OnOpen(IUIPanelData data) {}
        protected virtual void OnUpdate() {}
        protected virtual void OnHide() {}
        protected virtual void OnRecycle() {}
       
    }
}