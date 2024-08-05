// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/5 10:17:9
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(SettingComponent))]
    public sealed class SettingComponentInspector : FrameworkInspector
    {
        private HelperInfo<SettingHelperBase> mSettingHelperInfo = new HelperInfo<SettingHelperBase>("Setting");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                mSettingHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            var t = target as SettingComponent;
            if (t != null)
            {
                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.LabelField("Setting Count", t.Count >= 0 ? t.Count.ToString() : Constant.UnknownOptionName);
                    if (t.Count > 0)
                    {
                        var settingNames = t.GetAllSettingNames();
                        foreach (var settingName in settingNames)
                        {
                            EditorGUILayout.LabelField(settingName, t.GetString(settingName));
                        }
                    }
                }

                if (EditorApplication.isPlaying)
                {
                    if (GUILayout.Button("Save Setting"))
                    {
                        t.Save();
                    }

                    if (GUILayout.Button("Remove All Setting"))
                    {
                        t.RemoveAllSettings();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mSettingHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mSettingHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
    }
}