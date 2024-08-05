// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/5 10:38:36
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework.Runtime;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(UIComponent))]
    public sealed class UIComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnableOpenUIFormSuccessEvent = null;
        private SerializedProperty mEnableOpenUIFormFailureEvent = null;
        private SerializedProperty mEnableOpenUIFormUpdateEvent = null;
        private SerializedProperty mEnableOpenUIFormDependencyAssetEvent = null;
        private SerializedProperty mEnableCloseUIFormCompleteEvent = null;
        private SerializedProperty mInstanceAutoReleaseInterval = null;
        private SerializedProperty mInstanceCapacity = null;
        private SerializedProperty mInstancePriority = null;
        private SerializedProperty mInstanceRoot = null;
        private SerializedProperty mUIGroups = null;

        private HelperInfo<UIFormHelperBase> mUIFormHelperInfo = new HelperInfo<UIFormHelperBase>("UIForm");
        private HelperInfo<UIGroupHelperBase> mUIGroupHelperInfo = new HelperInfo<UIGroupHelperBase>("UIGroup");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(mEnableOpenUIFormSuccessEvent);
                EditorGUILayout.PropertyField(mEnableOpenUIFormFailureEvent);
                EditorGUILayout.PropertyField(mEnableOpenUIFormUpdateEvent);
                EditorGUILayout.PropertyField(mEnableOpenUIFormDependencyAssetEvent);
            }
            EditorGUI.EndDisabledGroup();

            var t = target as UIComponent;
            if (t != null)
            {
                var instanceAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Instance Auto Release Interval", mInstanceAutoReleaseInterval.floatValue);
                if (instanceAutoReleaseInterval != mInstanceAutoReleaseInterval.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.InstanceAutoReleaseInterval = instanceAutoReleaseInterval;
                    }
                    else
                    {
                        mInstanceAutoReleaseInterval.floatValue = instanceAutoReleaseInterval;
                    }
                }

                var instanceCapacity = EditorGUILayout.DelayedIntField("Instance Capacity", mInstanceCapacity.intValue);
                if (instanceCapacity != mInstanceCapacity.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.InstanceCapacity = instanceCapacity;
                    }
                    else
                    {
                        mInstanceCapacity.intValue = instanceCapacity;
                    }
                }

                var instancePriority = EditorGUILayout.DelayedIntField("Instance Priority", mInstancePriority.intValue);
                if (instancePriority != mInstancePriority.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.InstancePriority = instancePriority;
                    }
                    else
                    {
                        mInstancePriority.intValue = instancePriority;
                    }
                }

                EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
                {
                    EditorGUILayout.PropertyField(mInstanceRoot);
                    mUIFormHelperInfo.Draw();
                    mUIGroupHelperInfo.Draw();
                    EditorGUILayout.PropertyField(mUIGroups, true);
                }
                EditorGUI.EndDisabledGroup();

                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.LabelField("UI Group Count", t.UIGroupCount.ToString());
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mEnableOpenUIFormSuccessEvent = serializedObject.FindProperty("mEnableOpenUIFormSuccessEvent");
            mEnableOpenUIFormFailureEvent = serializedObject.FindProperty("mEnableOpenUIFormFailureEvent");
            mEnableOpenUIFormUpdateEvent = serializedObject.FindProperty("mEnableOpenUIFormUpdateEvent");
            mEnableOpenUIFormDependencyAssetEvent = serializedObject.FindProperty("mEnableOpenUIFormDependencyAssetEvent");
            mEnableCloseUIFormCompleteEvent = serializedObject.FindProperty("mEnableCloseUIFormCompleteEvent");
            mInstanceAutoReleaseInterval = serializedObject.FindProperty("mInstanceAutoReleaseInterval");
            mInstanceCapacity = serializedObject.FindProperty("mInstanceCapacity");
            mInstancePriority = serializedObject.FindProperty("mInstancePriority");
            mInstanceRoot = serializedObject.FindProperty("mInstanceRoot");
            mUIGroups = serializedObject.FindProperty("mUIGroups");

            mUIFormHelperInfo.Init(serializedObject);
            mUIGroupHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mUIFormHelperInfo.Refresh();
            mUIGroupHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
    }
}