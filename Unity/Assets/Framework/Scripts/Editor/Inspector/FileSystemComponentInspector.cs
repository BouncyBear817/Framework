// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/28 11:7:53
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework.Runtime;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(FileSystemComponent))]
    public sealed class FileSystemComponentInspector : FrameworkInspector
    {
        private HelperInfo<FileSystemHelperBase> mFileSystemHelperInfo = new HelperInfo<FileSystemHelperBase>("FileSystem");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = target as FileSystemComponent;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                mFileSystemHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (t != null && EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("File System Count", t.Count.ToString());

                var fileSystems = t.GetAllFileSystems();
                foreach (var fileSystem in fileSystems)
                {
                    EditorGUILayout.LabelField(fileSystem.FullPath, $"{fileSystem.Access}, {fileSystem.FileCount} / {fileSystem.MaxFileCount} Files");
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mFileSystemHelperInfo.Refresh();
        }

        private void OnEnable()
        {
            mFileSystemHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }
    }
}