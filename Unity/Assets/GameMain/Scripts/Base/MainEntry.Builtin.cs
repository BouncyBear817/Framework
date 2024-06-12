/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:
 * Modify Record:
 *************************************************************/

using Runtime;
using UnityEngine;

public sealed partial class MainEntry : MonoBehaviour
{
    public static BaseComponent Base { get; private set; }

    public static DataTableComponent DataTable { get; private set; }

    public static DownloadComponent Download { get; private set; }

    public static EventComponent Event { get; private set; }


    public static FileSystemComponent FileSystem { get; private set; }

    public static FsmComponent Fsm { get; private set; }

    public static NetworkComponent Network { get; private set; }

    public static ObjectPoolComponent ObjectPool { get; private set; }

    public static ProcedureComponent Procedure { get; private set; }

    public static ReferencePoolComponent ReferencePool { get; private set; }

    public static ResourceComponent Resource { get; private set; }

    public static SceneComponent Scene { get; private set; }

    public static SoundComponent Sound { get; private set; }

    public static UIComponent UI { get; private set; }

    public static WebRequestComponent WebRequest { get; private set; }

    private static void InitBuiltinComponents()
    {
        Base = MainEntryHelper.GetComponent<BaseComponent>();

        DataTable = MainEntryHelper.GetComponent<DataTableComponent>();

        Download = MainEntryHelper.GetComponent<DownloadComponent>();

        Event = MainEntryHelper.GetComponent<EventComponent>();

        FileSystem = MainEntryHelper.GetComponent<FileSystemComponent>();

        Fsm = MainEntryHelper.GetComponent<FsmComponent>();

        Network = MainEntryHelper.GetComponent<NetworkComponent>();

        ObjectPool = MainEntryHelper.GetComponent<ObjectPoolComponent>();

        Procedure = MainEntryHelper.GetComponent<ProcedureComponent>();

        ReferencePool = MainEntryHelper.GetComponent<ReferencePoolComponent>();

        Resource = MainEntryHelper.GetComponent<ResourceComponent>();

        Scene = MainEntryHelper.GetComponent<SceneComponent>();

        Sound = MainEntryHelper.GetComponent<SoundComponent>();

        UI = MainEntryHelper.GetComponent<UIComponent>();

        WebRequest = MainEntryHelper.GetComponent<WebRequestComponent>();
    }
}