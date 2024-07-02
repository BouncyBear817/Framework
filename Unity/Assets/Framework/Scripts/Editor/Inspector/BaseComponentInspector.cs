// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 15:43:51
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(BaseComponent))]
    public sealed class BaseComponentInspector : FrameworkInspector
    {
        private static readonly float[] sGameSpeed = new[] { 0f, 0.01f, 0.1f, 0.5f, 1f, 2f, 4f, 8f };
        private static readonly string[] sGameSpeedForDisplay = new[] { "0x", "0.01x", "0.1x", "0,5x", "1x", "2x", "4x", "8x" };

        private SerializedProperty mEditorResourceMode = null;
        private SerializedProperty mFrameRate = null;
        private SerializedProperty mGameSpeed = null;
        private SerializedProperty mRunInBackground = null;
        private SerializedProperty mNeverSleep = null;

        private HelperInfo mVersionHelperInfo = new HelperInfo("Version", typeof(Version.IVersionHelper));
        private HelperInfo mLogHelperInfo = new HelperInfo("Log", typeof(ILogHelper));
        private HelperInfo mJsonHelperInfo = new HelperInfo("Json", typeof(Utility.Json.IJsonHelper));
        private HelperInfo mCompressionHelperInfo = new HelperInfo("Compression", typeof(Utility.Compression.ICompressionHelper));

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as BaseComponent;
            if (t == null) return;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                mEditorResourceMode.boolValue = EditorGUILayout.BeginToggleGroup("Editor Resource Mode", mEditorResourceMode.boolValue);
                {
                    EditorGUILayout.HelpBox("Editor resource mode option is only for editor mode. Framework will use editor resource files, which you should validate first.",
                        MessageType.Warning);
                }
                EditorGUILayout.EndToggleGroup();

                var frameRate = EditorGUILayout.IntSlider("Frame Rate", mFrameRate.intValue, 1, 120);
                if (frameRate != mFrameRate.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.FrameRate = frameRate;
                    }
                    else
                    {
                        mFrameRate.intValue = frameRate;
                    }
                }

                EditorGUILayout.BeginVertical("box");
                {
                    var gameSpeed = EditorGUILayout.Slider("Game Speed", mGameSpeed.floatValue, 0f, 8f);
                    int selectedGameSpeed = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), sGameSpeedForDisplay, 4);
                    if (selectedGameSpeed >= 0)
                    {
                        gameSpeed = GetGameSpeed(selectedGameSpeed);
                    }

                    if (gameSpeed != mGameSpeed.floatValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.GameSpeed = gameSpeed;
                        }
                        else
                        {
                            mGameSpeed.floatValue = gameSpeed;
                        }
                    }
                }
                EditorGUILayout.EndVertical();

                var runInBackground = EditorGUILayout.Toggle("Run In Background", mRunInBackground.boolValue);
                if (runInBackground != mRunInBackground.boolValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.RunInBackground = runInBackground;
                    }
                    else
                    {
                        mRunInBackground.boolValue = runInBackground;
                    }
                }

                var neverSleep = EditorGUILayout.Toggle("Never Sleep", mNeverSleep.boolValue);
                if (neverSleep != mNeverSleep.boolValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.NeverSleep = neverSleep;
                    }
                    else
                    {
                        mNeverSleep.boolValue = neverSleep;
                    }
                }

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Global Helpers", EditorStyles.boldLabel);
                    mVersionHelperInfo.Draw();
                    mLogHelperInfo.Draw();
                    mJsonHelperInfo.Draw();
                    mCompressionHelperInfo.Draw();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnRefreshTypeNames()
        {
            mVersionHelperInfo.Refresh();
            mLogHelperInfo.Refresh();
            mJsonHelperInfo.Refresh();
            mCompressionHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnEnable()
        {
            mEditorResourceMode = serializedObject.FindProperty("mEditorResourceMode");
            mFrameRate = serializedObject.FindProperty("mFrameRate");
            mGameSpeed = serializedObject.FindProperty("mGameSpeed");
            mRunInBackground = serializedObject.FindProperty("mRunInBackground");
            mNeverSleep = serializedObject.FindProperty("mNeverSleep");

            mVersionHelperInfo.Init(serializedObject);
            mLogHelperInfo.Init(serializedObject);
            mJsonHelperInfo.Init(serializedObject);
            mCompressionHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }
        
        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return sGameSpeed[0];
            }

            if (selectedGameSpeed >= sGameSpeed.Length)
            {
                return sGameSpeed[sGameSpeed.Length - 1];
            }

            return sGameSpeed[selectedGameSpeed];
        }

        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < sGameSpeed.Length; i++)
            {
                if (gameSpeed == sGameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}