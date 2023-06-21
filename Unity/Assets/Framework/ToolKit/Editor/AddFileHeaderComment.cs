using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AddFileHeaderComment : AssetModificationProcessor
{
    // 添加脚本注释模板
    private static readonly string mStr =
        "/************************************************************\r\n"
        + "* Unity Version: #VERSION#\r\n"
        + "* Author:        #AUTHOR#\r\n"
        + "* CreateTime:    #CreateTime#\r\n"
        + "* Description:   \r\n"
        + "* Modify Record: \r\n"
        + "*************************************************************/\r\n\r\n";

    /// <summary>
    /// 此函数在asset被创建完，文件已经生成到磁盘上，但是没有生成.meta文件和import之前被调用
    /// </summary>
    /// <param name="newFileMeta">newfilemeta 是由创建文件的path加上.meta组成的</param>
    static void OnWillCreateAsset(string newFileMeta)
    {
        // 只修改C#脚本
        var newFilePath = newFileMeta.Replace(".meta", "");
        var subIndex = Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal);
        var rootPath = Application.dataPath.Substring(0, subIndex);
        var filePath = string.Format("{0}{1}", rootPath, newFilePath);
        AddHeaderComment(filePath);
    }
    
    [MenuItem("Assets/Add Header Comment On Script")]
    static void AddHeaderComment()
    {
        var objs = Selection.GetFiltered<TextAsset>(SelectionMode.DeepAssets);
        foreach (var obj in objs)
        {
        }
    }

    static void AddHeaderComment(string filePath)
    {
        if (File.Exists(filePath))
        {
            if (filePath.EndsWith(".cs"))
            {
                var scriptContent = mStr;
                
                //这里实现自定义的一些规则
                scriptContent = scriptContent.Replace("#VERSION#", Application.unityVersion);
                scriptContent = scriptContent.Replace("#AUTHOR#", "bear");
                // 替换字符串为系统时间
                scriptContent =
                    scriptContent.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                
                scriptContent += File.ReadAllText(filePath);
                File.WriteAllText(filePath, scriptContent);
                AssetDatabase.Refresh();
                scriptContent = "";
            }
        }
    }
}