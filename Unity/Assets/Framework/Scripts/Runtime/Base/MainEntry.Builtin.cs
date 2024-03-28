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

        public static DataTableComponent DataTable { get; private set; }

        public static DownloadComponent Download { get; private set; }

        public static EventComponent Event { get; private set; }

        public static FsmComponent Fsm { get; private set; }
        
        public static NetworkComponent Network { get; private set; }

        public static ObjectPoolComponent ObjectPool { get; private set; }

        public static ProcedureComponent Procedure { get; private set; }

        public static ReferencePoolComponent ReferencePool { get; private set; }

        private static void InitBuiltinComponents()
        {
            Base = Helper.GetComponent<BaseComponent>();

            DataTable = Helper.GetComponent<DataTableComponent>();

            Download = Helper.GetComponent<DownloadComponent>();

            Event = Helper.GetComponent<EventComponent>();

            Fsm = Helper.GetComponent<FsmComponent>();

            Network = Helper.GetComponent<NetworkComponent>();

            ObjectPool = Helper.GetComponent<ObjectPoolComponent>();

            Procedure = Helper.GetComponent<ProcedureComponent>();

            ReferencePool = Helper.GetComponent<ReferencePoolComponent>();
        }
    }
}