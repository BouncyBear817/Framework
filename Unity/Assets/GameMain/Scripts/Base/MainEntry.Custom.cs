/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:19
 * Description:
 * Modify Record:
 *************************************************************/

using UnityEngine;

namespace Runtime
{
    public sealed partial class MainEntry : MonoBehaviour
    {
        public static NetConnectorComponent NetConnector { get; private set; }

        private static void InitCustomComponents()
        {
            NetConnector = MainEntry.Helper.GetComponent<NetConnectorComponent>();
        }
    }
}