// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/5 10:53:56
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
    [CustomEditor(typeof(WebRequestComponent))]
    public sealed class WebRequestComponentInspector : FrameworkInspector
    {
        private SerializedProperty mInstanceRoot = null;
        private SerializedProperty mWebRequestAgentHelperCount = null;
        private SerializedProperty mTimeout = null;

        private HelperInfo<WebRequestAgentHelperBase> mWebRequestAgentHelperInfo = new HelperInfo<WebRequestAgentHelperBase>("WebRequestAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(mInstanceRoot);
                mWebRequestAgentHelperInfo.Draw();
                mWebRequestAgentHelperCount.intValue = EditorGUILayout.IntSlider("Web Request Agent Helper Count", mWebRequestAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            var t = target as WebRequestComponent;
            if (t != null)
            {
                var timeout = EditorGUILayout.Slider("Timeout", mTimeout.floatValue, 0f, 120f);
                if (timeout != mTimeout.floatValue)
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

                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.LabelField("Total Agent Count", t.TotalAgentCount.ToString());
                    EditorGUILayout.LabelField("Available Agent Count", t.AvailableAgentCount.ToString());
                    EditorGUILayout.LabelField("Working Agent Count", t.WorkingAgentCount.ToString());
                    EditorGUILayout.LabelField("Waiting Task Count", t.WaitingTaskCount.ToString());

                    EditorGUILayout.BeginVertical("box");
                    {
                        var webRequestInfos = t.GetAllWebRequestInfos();
                        if (webRequestInfos.Length > 0)
                        {
                            foreach (var webRequestInfo in webRequestInfos)
                            {
                                var tag = webRequestInfo.Tag ?? Constant.NoneOptionName;
                                EditorGUILayout.LabelField(webRequestInfo.Description,
                                    $"[SerialId]{webRequestInfo.SerialId} [Tag]{tag} [Priority]{webRequestInfo.Priority} [Status]{webRequestInfo.Status}");
                            }

                            if (GUILayout.Button("Export CSV Data"))
                            {
                                var exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, $"WebRequest Task Data", "csv");
                                if (!string.IsNullOrEmpty(exportFileName))
                                {
                                    try
                                    {
                                        var index = 0;
                                        var data = new string[webRequestInfos.Length + 1];
                                        data[index++] = "WebRequest Uri,Serial Id,Tag,Priority,Status";
                                        foreach (var info in webRequestInfos)
                                        {
                                            data[index++] = $"{info.Description},{info.SerialId},{info.Tag},{info.Priority},{info.Status}";
                                        }

                                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                        Debug.Log($"Export web request task CSV data to ({exportFileName}) success.");
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log($"Export web request task CSV data to ({exportFileName}) failure, exception is ({e}).");
                                    }
                                }
                            }
                        }
                        else
                        {
                            GUILayout.Label("WebRequest Task is Empty ...");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mInstanceRoot = serializedObject.FindProperty("mInstanceRoot");
            mWebRequestAgentHelperCount = serializedObject.FindProperty("mWebRequestAgentHelperCount");
            mTimeout = serializedObject.FindProperty("mTimeout");

            mWebRequestAgentHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mWebRequestAgentHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
    }
}