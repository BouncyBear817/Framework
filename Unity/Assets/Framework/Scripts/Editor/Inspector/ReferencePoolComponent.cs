// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/1 10:48:27
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(ReferencePoolComponent))]
    public sealed class ReferencePoolComponentInspector : FrameworkInspector
    {
        private readonly Dictionary<string, List<ReferencePoolInfo>> mReferencePoolInfos = new Dictionary<string, List<ReferencePoolInfo>>();
        private readonly HashSet<string> mOpenedItems = new HashSet<string>();

        private SerializedProperty mEnableStrictCheck = null;

        private bool mShowFullClassName = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as ReferencePoolComponent;
            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                var enableStrictCheck = EditorGUILayout.Toggle("Enable Strict Check", t.EnableStrictCheck);
                if (enableStrictCheck != t.EnableStrictCheck)
                {
                    t.EnableStrictCheck = enableStrictCheck;
                }

                EditorGUILayout.LabelField("Reference Pool Count", ReferencePool.Count.ToString());
                mShowFullClassName = EditorGUILayout.Toggle("Show Full Class Name", mShowFullClassName);
                mReferencePoolInfos.Clear();
                var referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
                foreach (var info in referencePoolInfos)
                {
                    var assemblyName = info.Type.Assembly.GetName().Name;
                    if (!mReferencePoolInfos.TryGetValue(assemblyName, out var results))
                    {
                        results = new List<ReferencePoolInfo>();
                        mReferencePoolInfos.Add(assemblyName, results);
                    }

                    results.Add(info);
                }

                foreach (var referencePoolInfo in mReferencePoolInfos)
                {
                    var lastState = mOpenedItems.Contains(referencePoolInfo.Key);
                    var currentState = EditorGUILayout.Foldout(lastState, referencePoolInfo.Key);
                    if (currentState != lastState)
                    {
                        if (currentState)
                        {
                            mOpenedItems.Add(referencePoolInfo.Key);
                        }
                        else
                        {
                            mOpenedItems.Remove(referencePoolInfo.Key);
                        }
                    }

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            EditorGUILayout.LabelField(mShowFullClassName ? "Full Class Name" : "Class Name", "Unused\tUsing\tAcquire\tRelease\tAdd\tRemove");
                            referencePoolInfo.Value.Sort(Comparison);
                            foreach (var info in referencePoolInfo.Value)
                            {
                                DrawReferencePoolInfo(info);
                            }

                            if (GUILayout.Button("Export CSV Data"))
                            {
                                var exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, $"Reference Pool Data - ({referencePoolInfo.Key})", "csv");
                                if (!string.IsNullOrEmpty(exportFileName))
                                {
                                    try
                                    {
                                        var index = 0;
                                        var data = new string[referencePoolInfo.Value.Count + 1];
                                        data[index++] = "Class Name,Full Class Name,Unused,Using,Acquire,Release,Add,Remove";
                                        foreach (var info in referencePoolInfo.Value)
                                        {
                                            data[index++] =
                                                $"{info.Type.Name},{info.Type.FullName},{info.UnusedReferenceCount},{info.UsingReferenceCount},{info.AcquireReferenceCount},{info.ReleaseReferenceCount},{info.AddReferenceCount},{info.RemoveReferenceCount}";
                                        }

                                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                        Debug.Log($"Export reference pool CSV data to ({exportFileName}) success.");
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log($"Export reference pool CSV data to ({exportFileName}) failure, exception is ({e}).");
                                    }
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.Separator();
                    }
                }
            }
            else
            {
                EditorGUILayout.PropertyField(mEnableStrictCheck);
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mEnableStrictCheck = serializedObject.FindProperty("mEnableStrictCheck");
        }

        private void DrawReferencePoolInfo(ReferencePoolInfo info)
        {
            EditorGUILayout.LabelField(mShowFullClassName ? info.Type.FullName : info.Type.Name,
                $"{info.UnusedReferenceCount}\t{info.UsingReferenceCount}\t{info.AcquireReferenceCount}\t{info.ReleaseReferenceCount}\t{info.AddReferenceCount}\t{info.RemoveReferenceCount}");
        }

        private int Comparison(ReferencePoolInfo x, ReferencePoolInfo y)
        {
            return mShowFullClassName
                ? string.Compare(x.Type.FullName, y.Type.FullName, StringComparison.Ordinal)
                : string.Compare(x.Type.Name, y.Type.Name, StringComparison.Ordinal);
        }
    }
}