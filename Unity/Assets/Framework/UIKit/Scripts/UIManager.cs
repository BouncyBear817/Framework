namespace Framework
{
    [MonoSingletonPath("UIRoot/Manager")]
    public sealed class UIManager : MgrBehaviour, ISingleton
    {
        public override int ManagerId => MgrId.UI;
        public static UIManager Instance => MonoSingletonProperty<UIManager>.Instance;
        public void OnSingletonInit()
        {
            
        }
        
        /// <summary>
        /// 应用程序退出：释放当前对象并销毁相关GameObject
        /// </summary>
        private void OnApplicationQuit()
        {
            MonoSingletonProperty<UIManager>.OnDispose();
        }
    }
}