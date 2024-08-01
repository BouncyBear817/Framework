// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/23 10:45:43
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public sealed class DataTableWindow : EditorWindow
    {
        private static readonly List<string> mDataTables = new List<string>();

        private readonly HashSet<string> mSelectedDataTables = new HashSet<string>();

        private Vector2 mDataTablesViewScroll = Vector2.zero;
        
        [MenuItem("Tools/Data Table/Open Window")]
        private static void Open()
        {
            var window = GetWindow<DataTableWindow>("Data Table Generator");
            window.minSize = new Vector2(800f, 600f);
        }

        private void OnEnable()
        {
            SetDataTables();
            mDataTablesViewScroll = Vector2.zero;
        }

        private void OnDisable()
        {
            mDataTables.Clear();
            mSelectedDataTables.Clear();
            mDataTablesViewScroll = Vector2.zero;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Text Asset List", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                {
                    mDataTablesViewScroll = EditorGUILayout.BeginScrollView(mDataTablesViewScroll);
                    {
                        foreach (var dataTable in mDataTables)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                var isSelect = mSelectedDataTables.Contains(dataTable);
                                if (isSelect != EditorGUILayout.Toggle(isSelect, GUILayout.Width(18f)))
                                {
                                    isSelect = !isSelect;
                                    SetSelectedDataTables(dataTable, isSelect);
                                }

                                EditorGUILayout.LabelField(dataTable.Substring(dataTable.IndexOf("DataTables", StringComparison.Ordinal)));
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(5f);
                    if (GUILayout.Button("Select All", GUILayout.Width(120)))
                    {
                        foreach (var dataTable in mDataTables)
                        {
                            mSelectedDataTables.Add(dataTable);
                        }
                    }

                    if (GUILayout.Button("Clear All", GUILayout.Width(120)))
                    {
                        mSelectedDataTables.Clear();
                    }

                    if (GUILayout.Button("Generator", GUILayout.Width(120)))
                    {
                        GeneratorDataTable();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void SetSelectedDataTables(string dataTable, bool isSelect)
        {
            if (isSelect)
            {
                mSelectedDataTables.Add(dataTable);
            }
            else
            {
                mSelectedDataTables.Remove(dataTable);
            }
        }

        private void SetDataTables()
        {
            var files = Directory.GetFiles(DataTableConstant.DataTablePath, DataTableConstant.TxtPattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.Contains(DataTableConstant.CSharpCodeTemplateFileName))
                {
                    continue;
                }

                mDataTables.Add(file);
            }

            files = Directory.GetFiles(DataTableConstant.DataTablePath, DataTableConstant.CsvPattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                mDataTables.Add(file);
            }

            mDataTables.Sort(StringComparer.Ordinal);
        }

        private void GeneratorDataTable()
        {
            foreach (var dataTable in mSelectedDataTables)
            {
                var strings = dataTable.Split('/');
                var dataTableName = strings[strings.Length - 1].Split('.')[0];
                var dataTableProcessor = DataTableGenerator.Create(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError($"Check raw data failure, DataTableName = ({dataTableName})");
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }
    }
}