/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:19
 * Description:
 * Modify Record:
 *************************************************************/


using Runtime;
using UnityEngine;

public sealed partial class MainEntry : MonoBehaviour
{
    public static NetConnectorComponent NetConnector { get; private set; }

    private static void InitCustomComponents()
    {
        NetConnector = MainEntryHelper.GetComponent<NetConnectorComponent>();
    }
}