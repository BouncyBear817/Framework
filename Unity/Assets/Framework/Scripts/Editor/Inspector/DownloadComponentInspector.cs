// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/26 15:26:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using System.Text;
using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(DownloadComponent))]
    public sealed class DownloadComponentInspector : FrameworkInspector
    {
        private SerializedProperty mInstanceRoot = null;
        private SerializedProperty mDownloadAgentHelperCount = null;
        private SerializedProperty mTimeout = null;
        private SerializedProperty mFlushSize = null;

        private HelperInfo<DownloadAgentHelperBase> mDownloadAgentHelperInfo = new HelperInfo<DownloadAgentHelperBase>("DownloadAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as DownloadComponent;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            {
                DrawPropertyField(mInstanceRoot);
                mDownloadAgentHelperInfo.Draw();
                mDownloadAgentHelperCount.intValue = EditorGUILayout.IntSlider("Download Agent Helper Count", mDownloadAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            var timeout = EditorGUILayout.Slider("Timeout", mTimeout.floatValue, 0f, 120f);
            if (t != null && timeout != mTimeout.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.Timeout = timeout;
                }
                else
                {
                    mTimeout.floatValue = timeout;
                }
            }

            var flushSize = EditorGUILayout.DelayedIntField("Flush Size", mFlushSize.intValue);
            if (t != null && flushSize != mFlushSize.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.FlushSize = flushSize;
                }
                else
                {
                    mFlushSize.intValue = flushSize;
                }
            }

            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Paused", t.Paused.ToString());
                EditorGUILayout.LabelField("Total Agent Count", t.TotalAgentCount.ToString());
                EditorGUILayout.LabelField("Available Agent Count", t.AvailableAgentCount.ToString());
                EditorGUILayout.LabelField("Working Agent Count", t.WorkingAgentCount.ToString());
                EditorGUILayout.LabelField("Waiting Task Count", t.WaitingTaskCount.ToString());
                EditorGUILayout.LabelField("Current Speed", ShowCurrentSpeed(t.CurrentSpeed));

                EditorGUILayout.BeginVertical("box");
                {
                    var downloadInfos = t.GetAllDownloadInfos();
                    if (downloadInfos.Length > 0)
                    {
                        foreach (var downloadInfo in downloadInfos)
                        {
                            EditorGUILayout.LabelField(downloadInfo.Description,
                                $"[SerialId]{downloadInfo.SerialId} [Tag]{downloadInfo.Tag} [Priority]{downloadInfo.Priority} [Status]{downloadInfo.Status}");
                        }

                        if (GUILayout.Button("Export CSV Data"))
                        {
                            var exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, "Download Task Data", "csv");
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    var index = 0;
                                    var data = new string[downloadInfos.Length + 1];
                                    data[index++] = "Id,Serial Id,Download Path,Tag,Priority,Status";
                                    foreach (var downloadInfo in downloadInfos)
                                    {
                                        data[index++] =
                                            $"{index - 1},{downloadInfo.SerialId},{downloadInfo.Description},{downloadInfo.Tag},{downloadInfo.Priority},{downloadInfo.Status}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Debug.Log($"Export download task CSV data to ({exportFileName}) success.");
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError($"Export download task csv data to ({exportFileName}) failure with exception is ({e})");
                                }
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Download Task is Empty...");
                    }
                }
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mDownloadAgentHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            mInstanceRoot = serializedObject.FindProperty("mInstanceRoot");
            mDownloadAgentHelperCount = serializedObject.FindProperty("mDownloadAgentHelperCount");
            mTimeout = serializedObject.FindProperty("mTimeout");
            mFlushSize = serializedObject.FindProperty("mFlushSize");

            mDownloadAgentHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        private string ShowCurrentSpeed(float currentSpeed)
        {
            if (currentSpeed > 0f)
            {
                var speed = currentSpeed / 1024;
                if (speed > 1)
                {
                    speed = currentSpeed / 1024 / 1024;
                    if (speed > 1)
                    {
                        speed = currentSpeed / 1024 / 1024 / 1024;
                        if (speed > 1)
                        {
                            return $"{speed:F2} Gb/s";
                        }
                        else
                        {
                            return $"{speed * 1024:F2} Mb/s";
                        }
                    }
                    else
                    {
                        return $"{speed * 1024:F2} Kb/s";
                    }
                }
                else
                {
                    return $"{speed:F2} b/s";
                }
            }
            else
            {
                return $"0.00 b/s";
            }
        }
    }
}