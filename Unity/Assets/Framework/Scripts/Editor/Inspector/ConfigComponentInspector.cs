// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 10:44:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(ConfigComponent))]
    public sealed class ConfigComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnableLoadConfigUpdateEvent = null;
        private SerializedProperty mEnableLoadConfigDependencyAssetEvent = null;
        private SerializedProperty mCachedBytesSize = null;

        private HelperInfo<ConfigHelperBase> mConfigHelperInfo = new HelperInfo<ConfigHelperBase>("Config");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as ConfigComponent;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                DrawPropertyField(mEnableLoadConfigUpdateEvent);
                DrawPropertyField(mEnableLoadConfigDependencyAssetEvent);
                mConfigHelperInfo.Draw();
                DrawPropertyField(mCachedBytesSize);
            }
            EditorGUI.EndDisabledGroup();

            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Config Count", t.Count.ToString());
                EditorGUILayout.LabelField("Cached Bytes Size", t.CachedBytesSize.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            mEnableLoadConfigUpdateEvent = serializedObject.FindProperty("mEnableLoadConfigUpdateEvent");
            mEnableLoadConfigDependencyAssetEvent = serializedObject.FindProperty("mEnableLoadConfigDependencyAssetEvent");
            mCachedBytesSize = serializedObject.FindProperty("mCachedBytesSize");

            mConfigHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            mConfigHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}