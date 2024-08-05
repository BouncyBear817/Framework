// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/5 10:25:31
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework.Runtime;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(SoundComponent))]
    public sealed class SoundComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnablePlaySoundUpdateEvent = null;
        private SerializedProperty mEnablePlaySoundDependencyEvent = null;
        private SerializedProperty mInstanceRoot = null;
        private SerializedProperty mAudioMixer = null;
        private SerializedProperty mSoundGroups = null;

        private HelperInfo<SoundHelperBase> mSoundHelperInfo = new HelperInfo<SoundHelperBase>("Sound");
        private HelperInfo<SoundGroupHelperBase> mSoundGroupHelperInfo = new HelperInfo<SoundGroupHelperBase>("SoundGroup");
        private HelperInfo<SoundAgentHelperBase> mSoundAgentHelperInfo = new HelperInfo<SoundAgentHelperBase>("SoundAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(mEnablePlaySoundUpdateEvent);
                EditorGUILayout.PropertyField(mEnablePlaySoundDependencyEvent);
                EditorGUILayout.PropertyField(mInstanceRoot);
                EditorGUILayout.PropertyField(mAudioMixer);

                mSoundHelperInfo.Draw();
                mSoundGroupHelperInfo.Draw();
                mSoundAgentHelperInfo.Draw();

                EditorGUILayout.PropertyField(mSoundGroups, true);
            }
            EditorGUI.EndDisabledGroup();

            var t = target as SoundComponent;
            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Sound Group Count", t.SoundGroupCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mEnablePlaySoundUpdateEvent = serializedObject.FindProperty("mEnablePlaySoundUpdateEvent");
            mEnablePlaySoundDependencyEvent = serializedObject.FindProperty("mEnablePlaySoundDependencyEvent");
            mInstanceRoot = serializedObject.FindProperty("mInstanceRoot");
            mAudioMixer = serializedObject.FindProperty("mAudioMixer");
            mSoundGroups = serializedObject.FindProperty("mSoundGroups");

            mSoundHelperInfo.Init(serializedObject);
            mSoundGroupHelperInfo.Init(serializedObject);
            mSoundAgentHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mSoundHelperInfo.Refresh();
            mSoundGroupHelperInfo.Refresh();
            mSoundAgentHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
    }
}