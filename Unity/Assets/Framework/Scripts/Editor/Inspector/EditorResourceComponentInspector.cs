// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/1 10:35:18
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework.Runtime;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(EditorResourceComponent))]
    public sealed class EditorResourceComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnableCachedAssets = null;
        private SerializedProperty mLoadAssetCountPerFrame = null;
        private SerializedProperty mMinLoadAssetRandomDelaySeconds = null;
        private SerializedProperty mMaxLoadAssetRandomDelaySeconds = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(mEnableCachedAssets);
            EditorGUILayout.PropertyField(mLoadAssetCountPerFrame);
            EditorGUILayout.PropertyField(mMinLoadAssetRandomDelaySeconds);
            EditorGUILayout.PropertyField(mMaxLoadAssetRandomDelaySeconds);

            var t = target as EditorResourceComponent;
            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Load Waiting Asset Count", t.LoadWaitingTaskCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mEnableCachedAssets = serializedObject.FindProperty("mEnableCachedAssets");
            mLoadAssetCountPerFrame = serializedObject.FindProperty("mLoadAssetCountPerFrame");
            mMinLoadAssetRandomDelaySeconds = serializedObject.FindProperty("mMinLoadAssetRandomDelaySeconds");
            mMaxLoadAssetRandomDelaySeconds = serializedObject.FindProperty("mMaxLoadAssetRandomDelaySeconds");
        }
    }
}