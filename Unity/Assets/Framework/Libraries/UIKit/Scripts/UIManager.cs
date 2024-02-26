using System;

namespace Framework
{
    [MonoSingletonPath("UIRoot/Manager")]
    public sealed class UIManager : MgrBehaviour, ISingleton
    {
        public static UIManager Instance => MonoSingletonProperty<UIManager>.Instance;
        
        public void OnSingletonInit() {}

        public override int ManagerId => MgrId.UI;
        public override void Init()
        {
            MsgCenter.Instance.RegisterManagerFactory(ManagerId, () => Instance);
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