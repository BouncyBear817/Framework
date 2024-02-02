/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using UnityEngine;

namespace Runtime
{
    public sealed partial class MainEntry : MonoBehaviour
    {
        public static BaseComponent Base { get; private set; }
        public static DownloadComponent Download { get; private set; }

        public static EventComponent Event { get; private set; }

        public static ObjectPoolComponent ObjectPool { get; private set; }

        public static FsmComponent Fsm { get; private set; }

        public static ProcedureComponent Procedure { get; private set; }

        private static void InitBuiltinComponents()
        {
            Base = Helper.GetComponent<BaseComponent>();

            Download = Helper.GetComponent<DownloadComponent>();

            Event = Helper.GetComponent<EventComponent>();

            ObjectPool = Helper.GetComponent<ObjectPoolComponent>();

            Fsm = Helper.GetComponent<FsmComponent>();

            Procedure = Helper.GetComponent<ProcedureComponent>();
        }
    }
}