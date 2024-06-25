// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 16:6:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEditor;

namespace GameMain.Editor
{
    public static class SettingMenu
    {
        [MenuItem("Tools/Setting/UI Auto Bind Global Setting", priority = 100)]
        public static void OpenAutoBindGlobalSetting() => SettingsService.OpenProjectSettings("Setting/UIAutoBindGlobalSetting");
    }
}