namespace Framework
{
    public class UIPanelUtil
    {
        public static IUIPanelLoaderPool UIPanelLoaderPool;
        
        public static UIPanelGenerator UIPanelGenerator;

        static UIPanelUtil()
        {
            UIPanelLoaderPool = new DefaultUIPanelLoaderPool();
            UIPanelGenerator = new UIPanelGenerator(UIPanelLoaderPool);
        }
        
        
    }
}