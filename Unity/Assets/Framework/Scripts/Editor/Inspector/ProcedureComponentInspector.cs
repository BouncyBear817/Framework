// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/28 15:16:57
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using System.Linq;
using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(ProcedureComponent))]
    public sealed class ProcedureComponentInspector : FrameworkInspector
    {
        private SerializedProperty mAvailableProcedureTypeNames = null;
        private SerializedProperty mEntranceProcedureTypeName = null;

        private string[] mProcedureTypeNames = null;
        private List<string> mCurrentAvailableProcedureTypeNames = null;
        private int mEntranceProcedureIndex = -1;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as ProcedureComponent;
            if (t == null) return;

            if (string.IsNullOrEmpty(mEntranceProcedureTypeName.stringValue))
            {
                EditorGUILayout.HelpBox("Entrance procedure is invalid.", MessageType.Error);
            }
            else if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Current Procedure", t.FsmManager == null ? "None" : t.CurrentProcedure.GetType().ToString());
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                GUILayout.Label("Available Procedures", EditorStyles.boldLabel);
                if (mProcedureTypeNames.Length > 0)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        foreach (var procedureTypeName in mProcedureTypeNames)
                        {
                            var selected = mCurrentAvailableProcedureTypeNames.Contains(procedureTypeName);
                            if (selected != EditorGUILayout.ToggleLeft(procedureTypeName, selected))
                            {
                                if (!selected)
                                {
                                    mCurrentAvailableProcedureTypeNames.Add(procedureTypeName);
                                    WriteAvailableProcedureTypeNames();
                                }
                                else if (procedureTypeName != mEntranceProcedureTypeName.stringValue)
                                {
                                    mCurrentAvailableProcedureTypeNames.Remove(procedureTypeName);
                                    WriteAvailableProcedureTypeNames();
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.HelpBox("There is no available procedure.", MessageType.Warning);
                }

                if (mCurrentAvailableProcedureTypeNames.Count > 0)
                {
                    EditorGUILayout.Separator();

                    var selectedIndex = EditorGUILayout.Popup("Entrance Procedure", mEntranceProcedureIndex, mCurrentAvailableProcedureTypeNames.ToArray());
                    if (selectedIndex != mEntranceProcedureIndex)
                    {
                        mEntranceProcedureIndex = selectedIndex;
                        mEntranceProcedureTypeName.stringValue = mCurrentAvailableProcedureTypeNames[selectedIndex];
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Select available procedures first.", MessageType.Info);
                }
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mProcedureTypeNames = HelperType.GetRuntimeTypeNames(typeof(ProcedureBase));
            ReadAvailableProcedureTypeNames();
            var oldCount = mCurrentAvailableProcedureTypeNames.Count;
            mCurrentAvailableProcedureTypeNames = mCurrentAvailableProcedureTypeNames.Where(x => mProcedureTypeNames.Contains(x)).ToList();
            if (mCurrentAvailableProcedureTypeNames.Count != oldCount)
            {
                WriteAvailableProcedureTypeNames();
            }
            else if (!string.IsNullOrEmpty(mEntranceProcedureTypeName.stringValue))
            {
                mEntranceProcedureIndex = mCurrentAvailableProcedureTypeNames.IndexOf(mEntranceProcedureTypeName.stringValue);
                if (mEntranceProcedureIndex < 0)
                {
                    mEntranceProcedureTypeName.stringValue = null;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            mAvailableProcedureTypeNames = serializedObject.FindProperty("mAvailableProcedureTypeNames");
            mEntranceProcedureTypeName = serializedObject.FindProperty("mEntranceProcedureTypeName");

            OnRefreshTypeNames();
        }

        private void ReadAvailableProcedureTypeNames()
        {
            mCurrentAvailableProcedureTypeNames = new List<string>();
            var count = mAvailableProcedureTypeNames.arraySize;
            for (int i = 0; i < count; i++)
            {
                mCurrentAvailableProcedureTypeNames.Add(mAvailableProcedureTypeNames.GetArrayElementAtIndex(i).stringValue);
            }
        }

        private void WriteAvailableProcedureTypeNames()
        {
            mAvailableProcedureTypeNames.ClearArray();
            if (mCurrentAvailableProcedureTypeNames == null)
            {
                return;
            }

            mCurrentAvailableProcedureTypeNames.Sort();
            var count = mCurrentAvailableProcedureTypeNames.Count;
            for (int i = 0; i < count; i++)
            {
                mAvailableProcedureTypeNames.InsertArrayElementAtIndex(i);
                mAvailableProcedureTypeNames.GetArrayElementAtIndex(i).stringValue = mCurrentAvailableProcedureTypeNames[i];
            }

            if (!string.IsNullOrEmpty(mEntranceProcedureTypeName.stringValue))
            {
                mEntranceProcedureIndex = mCurrentAvailableProcedureTypeNames.IndexOf(mEntranceProcedureTypeName.stringValue);
                if (mEntranceProcedureIndex < 0)
                {
                    mEntranceProcedureTypeName.stringValue = null;
                }
            }
        }
    }
}