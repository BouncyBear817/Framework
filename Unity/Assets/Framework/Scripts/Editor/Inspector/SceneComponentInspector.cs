// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/2 15:18:22
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(SceneComponent))]
    public sealed class SceneComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnableLoadSceneUpdateEvent = null;
        private SerializedProperty mEnableLoadSceneDependencyAssetEvent = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(mEnableLoadSceneUpdateEvent);
                EditorGUILayout.PropertyField(mEnableLoadSceneDependencyAssetEvent);
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            var t = target as SceneComponent;
            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.ObjectField("Main Camera", t.MainCamera, typeof(Camera), true);
                EditorGUILayout.LabelField("Loaded Scene Asset Names", GetSceneNameString(t.GetLoadedSceneAssetNames()));
                EditorGUILayout.LabelField("Loading Scene Asset Names", GetSceneNameString(t.GetLoadingSceneAssetNames()));
                EditorGUILayout.LabelField("Unloading Scene Asset Names", GetSceneNameString(t.GetUnloadingSceneAssetNames()));

                Repaint();
            }
        }

        private void OnEnable()
        {
            mEnableLoadSceneUpdateEvent = serializedObject.FindProperty("mEnableLoadSceneUpdateEvent");
            mEnableLoadSceneDependencyAssetEvent = serializedObject.FindProperty("mEnableLoadSceneUpdateEvent");
        }

        private string GetSceneNameString(string[] sceneAssetNames)
        {
            if (sceneAssetNames == null | sceneAssetNames.Length <= 0)
            {
                return "<Empty>";
            }

            var sceneNameString = string.Empty;
            foreach (var assetName in sceneAssetNames)
            {
                if (!string.IsNullOrEmpty(sceneNameString))
                {
                    sceneNameString += ", ";
                }

                sceneNameString += SceneComponent.GetSceneName(assetName);
            }

            return sceneNameString;
        }
    }
}