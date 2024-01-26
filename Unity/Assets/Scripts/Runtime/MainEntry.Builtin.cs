using UnityEngine;

namespace Runtime
{
    public sealed partial class MainEntry : MonoBehaviour
    {
        public static BaseComponent Base { get; private set; }
        public static DownloadComponent Download { get; private set; }
        
        public static EventComponent Event { get; private set; }
        
        public static ObjectPoolComponent ObjectPool { get; private set; }

        private static void InitBuiltinComponents()
        {
            Base = MainEntry.Helper.GetComponent<BaseComponent>();
            
            Download = MainEntry.Helper.GetComponent<DownloadComponent>();

            Event = MainEntry.Helper.GetComponent<EventComponent>();

            ObjectPool = MainEntry.Helper.GetComponent<ObjectPoolComponent>();
        }
    }
}