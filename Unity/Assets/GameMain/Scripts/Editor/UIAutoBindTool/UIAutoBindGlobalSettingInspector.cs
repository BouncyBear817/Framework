/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/04/23 16:09:47
 * Description:
 * Modify Record:
 *************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIAutoBindGlobalSetting))]
public class UIAutoBindGlobalSettingInspector : Editor
{
    private SerializedProperty mNamespace;
    private SerializedProperty mComponentCodePath;
    private SerializedProperty mMountCodePath;
    private SerializedProperty mMountScriptAssemblyList;
    private SerializedProperty mPrefixRuleList;

    private void OnEnable()
    {
        mNamespace = serializedObject.FindProperty("mNamespace");
        mComponentCodePath = serializedObject.FindProperty("mComponentCodePath");
        mMountCodePath = serializedObject.FindProperty("mMountCodePath");
        mMountScriptAssemblyList = serializedObject.FindProperty("mMountScriptAssemblyList");
        mPrefixRuleList = serializedObject.FindProperty("mPrefixRuleList");

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        mNamespace.stringValue = EditorGUILayout.TextField(new GUIContent("Default Namespace"), mNamespace.stringValue);

        EditorGUILayout.LabelField("Default Component Code Saved Path:");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(mComponentCodePath.stringValue);
        if (GUILayout.Button("Select", GUILayout.Width(50)))
        {
            var folder = Path.Combine(Application.dataPath, mComponentCodePath.stringValue);
            if (!Directory.Exists(folder))
            {
                folder = Application.dataPath;
            }

            var path = EditorUtility.OpenFolderPanel("Select Component Code Saved Path", folder, "");
            if (!string.IsNullOrEmpty(path))
            {
                mComponentCodePath.stringValue = path.Replace(Application.dataPath + "/", "");
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Default Mount Code Saved Path:");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(mMountCodePath.stringValue);
        if (GUILayout.Button("Select", GUILayout.Width(50)))
        {
            var folder = Path.Combine(Application.dataPath, mMountCodePath.stringValue);
            if (!Directory.Exists(folder))
            {
                folder = Application.dataPath;
            }

            var path = EditorUtility.OpenFolderPanel("Select Mount Code Saved Path", folder, "");
            if (!string.IsNullOrEmpty(path))
            {
                mMountCodePath.stringValue = path.Replace(Application.dataPath + "/", "");
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Default Mount Code Search Assembly:");
        EditorGUILayout.PropertyField(mMountScriptAssemblyList);

        EditorGUILayout.LabelField("Abbreviated Prefix name mapping for components:");
        EditorGUILayout.PropertyField(mPrefixRuleList);

        serializedObject.ApplyModifiedProperties();
    }
}