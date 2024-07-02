// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/26 15:7:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework.Runtime;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(DataTableComponent))]
    public sealed class DataTableComponentInspector : FrameworkInspector
    {
        private SerializedProperty mEnableLoadDataTableUpdateEvent = null;
        private SerializedProperty mEnableLoadDataTableDependencyAssetEvent = null;
        private SerializedProperty mCachedBytesSize = null;

        private HelperInfo<DataTableHelperBase> mDataTableHelperInfo = new HelperInfo<DataTableHelperBase>("DataTable");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            var t = target as DataTableComponent;
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                DrawPropertyField(mEnableLoadDataTableUpdateEvent);
                DrawPropertyField(mEnableLoadDataTableDependencyAssetEvent);
                mDataTableHelperInfo.Draw();
                DrawPropertyField(mCachedBytesSize);
            }
            EditorGUI.EndDisabledGroup();

            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Data Table Count", t.Count.ToString());
                EditorGUILayout.LabelField("Cached Bytes Size", t.CachedBytesSize.ToString());

                var dataTables = t.GetAllDataTables();
                foreach (var dataTable in dataTables)
                {
                    EditorGUILayout.LabelField(dataTable.FullName, $"{dataTable.Count} Rows");
                }
            }
            
            serializedObject.ApplyModifiedProperties();
            
            Repaint();
        }
        
        protected override void OnRefreshTypeNames()
        {
            mDataTableHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            mEnableLoadDataTableUpdateEvent = serializedObject.FindProperty("mEnableLoadDataTableUpdateEvent");
            mEnableLoadDataTableDependencyAssetEvent = serializedObject.FindProperty("mEnableLoadDataTableDependencyAssetEvent");
            mCachedBytesSize = serializedObject.FindProperty("mCachedBytesSize");
            
            mDataTableHelperInfo.Init(serializedObject);
            
            OnRefreshTypeNames();
        }

       
    }
}