using System;

namespace Framework
{
    public class UIPanelGenerator
    {
        private IUIPanelLoaderPool mPanelLoaderPool;

        public UIPanelGenerator(IUIPanelLoaderPool panelLoaderPool)
        {
            this.mPanelLoaderPool = panelLoaderPool;
        }

        public IUIPanel LoadUIPanel(UIPanelInfo panelInfo)
        {
            var panelLoader = mPanelLoaderPool.Allocate();
            var panel= panelLoader.LoadPanel(panelInfo);
            var objPanel = UnityEngine.Object.Instantiate(panel);
            var panelScript = objPanel.GetComponent<UIPanel>();
            panelScript.Loader = panelLoader;
            panelScript.State = UIPanelState.Loading;
            UIRoot.Instance.SetLevelOfPanel(panelInfo.Level, panelScript);

            return panelScript;
        }

        public void LoadUIPanelAsync(UIPanelInfo panelInfo, Action<IUIPanel> onPanelLoaded)
        {
            var panelLoader = mPanelLoaderPool.Allocate();
            panelLoader.LoadPanelAsync(panelInfo, panel =>
            {
                var objPanel = UnityEngine.Object.Instantiate(panel);
                var panelScript = objPanel.GetComponent<UIPanel>();
                panelScript.Loader = panelLoader;
                panelScript.State = UIPanelState.Loading;
                UIRoot.Instance.SetLevelOfPanel(panelInfo.Level, panelScript);
                
                onPanelLoaded?.Invoke(panelScript);
            });
        }
    }
}