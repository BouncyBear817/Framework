// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/4/23 17:2:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIAutoBindTool))]
public class UIAutoBindToolInspector : Editor
{
    private const string DefaultNamespace = "GameMain.UI";
    private UIAutoBindTool mTarget;

    private SerializedProperty mBindDataList;
    private SerializedProperty mBindComponentList;
    private SerializedProperty mClassName;
    private SerializedProperty mNamespace;
    private SerializedProperty mComponentCodePath;
    private SerializedProperty mMountCodePath;

    private UIAutoBindGlobalSetting mUIAutoBindGlobalSetting;

    private List<UIAutoBindTool.BindData> mTempBindDataList = new List<UIAutoBindTool.BindData>();
    private List<string> mTempFiledNameList = new List<string>();
    private List<string> mTempTypeNameList = new List<string>();

    private void OnEnable()
    {
        mTarget = (UIAutoBindTool)target;

        mBindDataList = serializedObject.FindProperty("mBindDataList");
        mBindComponentList = serializedObject.FindProperty("mBindComponentList");
        mClassName = serializedObject.FindProperty("mClassName");
        mNamespace = serializedObject.FindProperty("mNamespace");
        mComponentCodePath = serializedObject.FindProperty("mComponentCodePath");
        mMountCodePath = serializedObject.FindProperty("mMountCodePath");

        mUIAutoBindGlobalSetting = UIAutoBindGlobalSetting.GetAutoBindGlobalSetting();

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawButton();

        DrawSetting();

        DrawBindData();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBindData()
    {
        var needDeleteIndex = -1;
        EditorGUILayout.LabelField("----------------------------------------------------------------------");
        EditorGUILayout.LabelField("Bind Data:");
        EditorGUILayout.BeginVertical();

        for (var i = 0; i < mBindDataList.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(25));
            var element = mBindDataList.GetArrayElementAtIndex(i);
            var name = element.FindPropertyRelative("Name");
            name.stringValue = EditorGUILayout.TextField(name.stringValue, GUILayout.Width(150));
            var component = element.FindPropertyRelative("BindComponent");
            component.objectReferenceValue =
                EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(Component), true);

            if (GUILayout.Button("X"))
            {
                needDeleteIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (needDeleteIndex != -1)
        {
            mBindDataList.DeleteArrayElementAtIndex(needDeleteIndex);
            SyncBindComponents();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawSetting()
    {
        EditorGUILayout.LabelField("----------------------------------------------------------------------");
        EditorGUILayout.BeginHorizontal();
        mNamespace.stringValue = EditorGUILayout.TextField(new GUIContent("Namespace"), mNamespace.stringValue);
        if (GUILayout.Button("Default"))
        {
            mNamespace.stringValue = mUIAutoBindGlobalSetting.Namespace;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("----------------------------------------------------------------------");
        EditorGUILayout.BeginHorizontal();
        mClassName.stringValue = EditorGUILayout.TextField(new GUIContent("Class Name"), mClassName.stringValue);
        if (GUILayout.Button("Object Name"))
        {
            mClassName.stringValue = mTarget.gameObject.name;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("----------------------------------------------------------------------");
        EditorGUILayout.LabelField("Default Component Code Saved Path:");
        EditorGUILayout.LabelField(mComponentCodePath.stringValue);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Select", GUILayout.Width(100)))
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

        if (GUILayout.Button("Default", GUILayout.Width(100)))
        {
            mComponentCodePath.stringValue = mUIAutoBindGlobalSetting.ComponentCodePath;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("----------------------------------------------------------------------");
        EditorGUILayout.LabelField("Default Mount Code Saved Path:");
        EditorGUILayout.LabelField(mMountCodePath.stringValue);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Select", GUILayout.Width(100)))
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

        if (GUILayout.Button("Default", GUILayout.Width(100)))
        {
            mMountCodePath.stringValue = mUIAutoBindGlobalSetting.MountCodePath;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawButton()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Sort"))
        {
            mTempBindDataList.Clear();
            foreach (var bindData in mTarget.mBindDataList)
            {
                mTempBindDataList.Add(new UIAutoBindTool.BindData(bindData.Name, bindData.BindComponent));
            }

            mTempBindDataList.Sort();

            mBindDataList.ClearArray();
            foreach (var bindData in mTempBindDataList)
            {
                AddBindData(bindData.Name, bindData.BindComponent);
            }

            SyncBindComponents();
        }

        if (GUILayout.Button("Remove All"))
        {
            mBindDataList.ClearArray();
            SyncBindComponents();
        }

        if (GUILayout.Button("Remove Null"))
        {
            for (int i = mBindDataList.arraySize - 1; i >= 0; i--)
            {
                var element = mBindDataList.GetArrayElementAtIndex(i);
                var property = element.FindPropertyRelative("BindComponent");
                if (property.objectReferenceValue == null)
                {
                    mBindDataList.DeleteArrayElementAtIndex(i);
                }
            }

            SyncBindComponents();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Auto Bind Component"))
        {
            AutoBindComponent();
        }

        if (GUILayout.Button("Generate Bind Code"))
        {
            if (string.IsNullOrEmpty(mClassName.stringValue) || mClassName.stringValue.Equals("UIForm"))
            {
                EditorUtility.DisplayDialog("Tips",
                    "Please first synchronize the object name to the class name and click the [Object Name] button.",
                    "OK");
                return;
            }

            GenerateAutoBindCode();
        }

        if (GUILayout.Button("Mount Code"))
        {
            var go = mTarget.gameObject;
            var className = string.IsNullOrEmpty(mTarget.ClassName) ? go.name : mTarget.ClassName;
            var ns = string.IsNullOrEmpty(mTarget.Namespace) ? DefaultNamespace : mTarget.Namespace;
            var fullName = $"{ns}.{className}";
            if (!go.GetComponent(fullName))
            {
                var type = GetTypeWithName(className, ns);
                go.AddComponent(type);
            }
            else
            {
                Debug.LogWarning($"Already exist this type ({fullName}).");
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private Type GetTypeWithName(string className, string targetNamespace)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.Name == className && type.Namespace == targetNamespace)
                {
                    return type;
                }
            }
        }

        return null;
    }

    private void GenerateAutoBindCode()
    {
        var go = mTarget.gameObject;
        var className = string.IsNullOrEmpty(mTarget.ClassName) ? go.name : mTarget.ClassName;
        var codePath = string.IsNullOrEmpty(mTarget.ComponentCodePath)
            ? mUIAutoBindGlobalSetting.ComponentCodePath
            : mTarget.ComponentCodePath;
        codePath = Path.Combine(Application.dataPath, codePath);
        if (!Directory.Exists(codePath))
        {
            Debug.LogError($"Component code path is invalid, path is ({codePath}).");
            return;
        }

        var filePath = $"{codePath}/{className}.BindComponent.cs";
        using (var sw = new StreamWriter(filePath))
        {
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using UnityEngine.UI;");
            sw.WriteLine("using TMPro;");

            var ns = string.IsNullOrEmpty(mTarget.Namespace) ? DefaultNamespace : mTarget.Namespace;
            sw.WriteLine($"\nnamespace {ns}" + "\n{");
            sw.WriteLine($"\tpublic partial class {className}" + "\n\t{");

            foreach (var bindData in mTarget.mBindDataList)
            {
                sw.WriteLine($"\t\tprivate {bindData.BindComponent.GetType().Name} m{bindData.Name};");
            }

            sw.WriteLine("\n\t\tprivate void GetBindComponents(GameObject go)\n\t\t{");
            sw.WriteLine("\t\t\tvar uiAutoBindTool = go.GetComponent<UIAutoBindTool>();\n");
            for (var i = 0; i < mTarget.mBindDataList.Count; i++)
            {
                var bindData = mTarget.mBindDataList[i];
                sw.WriteLine(
                    $"\t\t\tm{bindData.Name} = uiAutoBindTool.GetBindComponent<{bindData.BindComponent.GetType().Name}>({i});");
            }

            sw.WriteLine("\t\t}\n\t}\n}");
            sw.Close();
        }

        Debug.Log($"Generate bind code is success, class name is ({className})");
        GenerateMountCode(go, className);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void GenerateMountCode(GameObject go, string className)
    {
        var codePath = string.IsNullOrEmpty(mTarget.MountCodePath)
            ? mUIAutoBindGlobalSetting.MountCodePath
            : mTarget.MountCodePath;
        codePath = Path.Combine(Application.dataPath, codePath);
        if (!Directory.Exists(codePath))
        {
            Debug.LogError($"Mount code path is invalid, path is ({codePath}).");
            return;
        }

        var filePath = $"{codePath}/{className}.cs";
        var regionStart = "#region Auto Generate,Do not modify!";
        var regionEnd = "#endregion";
        var scriptEnd =
            "/*--------------------Auto generate footer.Do not add anything below the footer!------------*/";
        if (!File.Exists(filePath))
        {
            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine("using UnityEngine;");

                var ns = string.IsNullOrEmpty(mTarget.Namespace) ? DefaultNamespace : mTarget.Namespace;
                sw.WriteLine($"\nnamespace {ns}" + "\n{");
                sw.WriteLine($"\t/// <summary>\n\t/// Please modify the description.\n\t/// </summary>");
                sw.WriteLine($"\tpublic partial class {className}" + ": UGUIForm\n\t{");

                sw.WriteLine("\t\tprotected internal override void OnInit(object userData)\n\t\t{");
                sw.WriteLine("\t\t\tbase.OnInit(userData);");
                sw.WriteLine("\t\t\tGetBindComponents(gameObject);\n");
                sw.WriteLine($"\t\t\t{regionStart}\n");
                foreach (var bindData in mTarget.mBindDataList)
                {
                    var str = GetListener(bindData);
                    if (!string.IsNullOrEmpty(str))
                    {
                        sw.WriteLine(str);
                    }
                }

                sw.WriteLine($"\n\t\t\t{regionEnd}");
                sw.WriteLine("\t\t}\n");

                foreach (var bindData in mTarget.mBindDataList)
                {
                    var str = GetListenerFunc(bindData);
                    if (!string.IsNullOrEmpty(str))
                    {
                        sw.WriteLine(str);
                    }
                }

                sw.WriteLine(scriptEnd);
                sw.WriteLine("\t}\n}");
                sw.Close();
            }

            Debug.Log($"Generate mount code is success, class name is ({className})");
        }
        else
        {
            var strArray = File.ReadAllLines(filePath);
            var cachedStr = new List<string>();
            var isRecord = true;
            foreach (var str in strArray)
            {
                cachedStr.Add(str);

                if (str.Contains(regionStart))
                {
                    isRecord = false;
                    break;
                }
            }

            foreach (var bindData in mTarget.mBindDataList)
            {
                var str = GetListener(bindData);
                if (!string.IsNullOrEmpty(str))
                {
                    cachedStr.Add(str);
                }
            }

            foreach (var str in strArray)
            {
                if (str.Contains(regionEnd))
                {
                    isRecord = true;
                }

                if (str.Contains(scriptEnd))
                {
                    break;
                }

                if (isRecord)
                {
                    cachedStr.Add(str);
                }
            }

            foreach (var bindData in mTarget.mBindDataList)
            {
                var str = GetListenerFunc(bindData);
                if (!string.IsNullOrEmpty(str))
                {
                    var isRepeat = false;
                    foreach (var s in cachedStr)
                    {
                        if (s.Contains($"private void {bindData.Name}Event"))
                        {
                            isRepeat = true;
                        }
                    }

                    if (!isRepeat)
                    {
                        cachedStr.Add(str);
                    }
                }
            }

            cachedStr.Add(scriptEnd);
            cachedStr.Add("\t}\n}");

            File.WriteAllLines(filePath, cachedStr.ToArray());
            Debug.Log($"Modify mount code is success, class name is ({className})");
        }
    }

    /// <summary>
    /// 增加绑定数据
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="component">组件</param>
    private void AddBindData(string name, Component component)
    {
        for (var i = 0; i < mBindDataList.arraySize; i++)
        {
            var bindDataName = mBindDataList.GetArrayElementAtIndex(i).FindPropertyRelative("Name");
            if (bindDataName.stringValue == name)
            {
                Debug.LogError($"Repeated Bind! Please Check Name ({name}).");
                return;
            }
        }

        var index = mBindDataList.arraySize;
        mBindDataList.InsertArrayElementAtIndex(index);
        var element = mBindDataList.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("Name").stringValue = name;
        element.FindPropertyRelative("BindComponent").objectReferenceValue = component;
    }

    /// <summary>
    /// 同步绑定组建
    /// </summary>
    private void SyncBindComponents()
    {
        mBindComponentList.ClearArray();
        for (var i = 0; i < mBindDataList.arraySize; i++)
        {
            var property = mBindDataList.GetArrayElementAtIndex(i).FindPropertyRelative("BindComponent");
            mBindComponentList.InsertArrayElementAtIndex(i);
            mBindComponentList.GetArrayElementAtIndex(i).objectReferenceValue = property.objectReferenceValue;
        }
    }

    /// <summary>
    /// 自动绑定组件
    /// </summary>
    private void AutoBindComponent()
    {
        mBindDataList.ClearArray();
        var transforms = mTarget.gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var transform in transforms)
        {
            mTempFiledNameList.Clear();
            mTempTypeNameList.Clear();
            if (transform == mTarget.transform)
            {
                continue;
            }

            if (transform.GetComponent<UIAutoBindTool>() != null)
            {
                Debug.LogError($"Repeated UI Auto Bind Tool, Please Check name ({transform.name}).");
                continue;
            }

            if (UIAutoBindGlobalSetting.IsValidBind(transform, mTempFiledNameList, mTempTypeNameList))
            {
                for (var index = 0; index < mTempFiledNameList.Count; index++)
                {
                    var filedName = mTempFiledNameList[index];
                    var typeName = mTempTypeNameList[index];
                    var component = transform.GetComponent(typeName);
                    if (component == null)
                    {
                        Debug.LogError($"Bind failure, name is ({filedName}), type name is ({typeName}).");
                    }
                    else
                    {
                        AddBindData(filedName, component);
                    }
                }
            }
        }

        SyncBindComponents();
    }

    private string GetListener(UIAutoBindTool.BindData bindData)
    {
        if (bindData.BindComponent.GetType() == typeof(Button))
        {
            return $"\t\t\tm{bindData.Name}.onClick.AddListener({bindData.Name}Event);";
        }

        if (bindData.BindComponent.GetType() == typeof(TMP_InputField) ||
            bindData.BindComponent.GetType() == typeof(Toggle) ||
            bindData.BindComponent.GetType() == typeof(Slider) ||
            bindData.BindComponent.GetType() == typeof(Scrollbar) ||
            bindData.BindComponent.GetType() == typeof(TMP_Dropdown))
        {
            return $"\t\t\tm{bindData.Name}.onValueChanged.AddListener({bindData.Name}Event);";
        }

        return null;
    }

    private string GetListenerFunc(UIAutoBindTool.BindData bindData)
    {
        if (bindData.BindComponent.GetType() == typeof(Button))
        {
            return $"\t\tprivate void {bindData.Name}Event()" + "\n\t\t{\n\t\t}\n";
        }

        if (bindData.BindComponent.GetType() == typeof(TMP_InputField))
        {
            return $"\t\tprivate void {bindData.Name}Event(string value)" + "\n\t\t{\n\t\t}\n";
        }

        if (bindData.BindComponent.GetType() == typeof(Toggle))
        {
            return $"\t\tprivate void {bindData.Name}Event(bool isOn)" + "\n\t\t{\n\t\t}\n";
        }

        if (bindData.BindComponent.GetType() == typeof(Slider) ||
            bindData.BindComponent.GetType() == typeof(Scrollbar))
        {
            return $"\t\tprivate void {bindData.Name}Event(float value)" + "\n\t\t{\n\t\t}\n";
        }

        if (bindData.BindComponent.GetType() == typeof(TMP_Dropdown))
        {
            return $"\t\tprivate void {bindData.Name}Event(int index)" + "\n\t\t{\n\t\t}\n";
        }

        return null;
    }
}