// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/28 11:32:38
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
    [CustomEditor(typeof(ObjectPoolComponent))]
    public sealed class ObjectPoolComponentInspector : FrameworkInspector
    {
        private readonly HashSet<string> mOpenedItems = new HashSet<string>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            var t = target as ObjectPoolComponent;
            if (t != null && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Object Pool Count", t.Count.ToString());

                var objectPools = t.GetAllObjectPools();
                foreach (var objectPool in objectPools)
                {
                    DrawObjectPool(objectPool);
                }
            }

            Repaint();
        }

        private void DrawObjectPool(ObjectPoolBase objectPool)
        {
            var lastState = mOpenedItems.Contains(objectPool.FullName);
            var currentState = EditorGUILayout.Foldout(lastState, objectPool.FullName);
            if (currentState != lastState)
            {
                if (currentState)
                {
                    mOpenedItems.Add(objectPool.FullName);
                }
                else
                {
                    mOpenedItems.Remove(objectPool.FullName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Name", objectPool.Name);
                    EditorGUILayout.LabelField("Type", objectPool.ObjectType.FullName);
                    EditorGUILayout.LabelField("Auto Release Interval", objectPool.AutoReleaseInterval.ToString());
                    EditorGUILayout.LabelField("Capacity", objectPool.Capacity.ToString());
                    EditorGUILayout.LabelField("Used Count", objectPool.Count.ToString());
                    EditorGUILayout.LabelField("Can Release Count", objectPool.CanReleaseCount.ToString());
                    EditorGUILayout.LabelField("Priority", objectPool.Priority.ToString());

                    var objectInfos = objectPool.GetAllObjectInfos();
                    if (objectInfos.Length > 0)
                    {
                        EditorGUILayout.LabelField("Name",
                            objectPool.AllowMultiSpawn ? "Locked\tCount\tFlag\tPriority" : "Locked\tIn Use\tFlag\tPriority");
                        foreach (var objectInfo in objectInfos)
                        {
                            var label1 = string.IsNullOrEmpty(objectInfo.Name) ? Constant.NoneOptionName : objectInfo.Name;
                            var label2 = objectPool.AllowMultiSpawn
                                ? $"{objectInfo.Locked}\t{objectInfo.SpawnCount}\t{objectInfo.CanReleaseFlag}\t{objectInfo.Priority}"
                                : $"{objectInfo.Locked}\t{objectInfo.IsInUse}\t{objectInfo.CanReleaseFlag}\t{objectInfo.Priority}";
                            EditorGUILayout.LabelField(label1, label2);
                        }

                        if (GUILayout.Button("Release"))
                        {
                            objectPool.Release();
                        }

                        if (GUILayout.Button("Export CSV Data"))
                        {
                            var exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, $"Object Pool Data - {objectPool.Name}", "csv");
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    var index = 0;
                                    var data = new string[objectInfos.Length + 1];
                                    var str = objectPool.AllowMultiSpawn ? "Count" : "In Use";
                                    data[index++] = $"Name,Locked,{str},Custom Can Release Flag,Priority,Last Use Time";
                                    foreach (var objectInfo in objectInfos)
                                    {
                                        data[index] = objectPool.AllowMultiSpawn
                                            ? $"{objectInfo.Name},{objectInfo.Locked},{objectInfo.SpawnCount},{objectInfo.CanReleaseFlag},{objectPool.Priority}"
                                            : $"{objectInfo.Name},{objectInfo.Locked},{objectInfo.IsInUse},{objectInfo.CanReleaseFlag},{objectPool.Priority}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Debug.Log($"Export object pool CSV data to ({exportFileName}) success.");
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError($"Export object pool CSV data to ({exportFileName}) failure with exception ({e}).");
                                }
                            }
                        }
                    }
                    else
                    {
                        GUILayout.Label("Object Pool is Empty...");
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }
        }
    }
}