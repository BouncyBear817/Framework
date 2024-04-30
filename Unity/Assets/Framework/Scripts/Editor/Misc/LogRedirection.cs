/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/27 14:53:26
 * Description:
 * Modify Record:
 *************************************************************/

using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace Framework.Editor
{
    /// <summary>
    /// 日志重定向
    /// </summary>
    public static class LogRedirection
    {
        private static readonly Regex sLogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");
        private const string LogHelperName = "DefaultLogHelper.cs";

        [OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            var instance = EditorUtility.InstanceIDToObject(instanceId);
            var instancePath = AssetDatabase.GetAssetPath(instance);
            if (instancePath.EndsWith(".cs") && !instancePath.Contains(LogHelperName))
            {
                InternalEditorUtility.OpenFileAtLineExternal(
                    Path.Combine(Application.dataPath, instancePath.Substring(7)),
                    line);
                return true;
            }

            var selectedStackTrace = GetSelectedStackTrace();
            if (string.IsNullOrEmpty(selectedStackTrace))
            {
                return false;
            }

            if (!selectedStackTrace.Contains("Runtime.DefaultLogHelper:Log"))
            {
                return false;
            }

            var match = sLogRegex.Match(selectedStackTrace);
            if (!match.Success)
            {
                return false;
            }

            if (!match.Groups[1].Value.Contains(LogHelperName))
            {
                return false;
            }

            match = match.NextMatch();
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups[1].Value.Contains("Log.cs"))
            {
                match = match.NextMatch();
                if (!match.Success)
                {
                    return false;
                }
            }

            InternalEditorUtility.OpenFileAtLineExternal(
                Path.Combine(Application.dataPath, match.Groups[1].Value.Substring(7)),
                int.Parse(match.Groups[2].Value));
            return true;
        }

        private static string GetSelectedStackTrace()
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
            {
                return null;
            }

            var consoleWindowFieldInfo =
                consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            if (consoleWindowFieldInfo == null)
            {
                return null;
            }

            var consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
            if (consoleWindow == null)
            {
                return null;
            }

            if (consoleWindow != EditorWindow.focusedWindow)
            {
                return null;
            }

            var activeTextFiledInfo =
                consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (activeTextFiledInfo == null)
            {
                return null;
            }

            return activeTextFiledInfo.GetValue(consoleWindow) as string;
        }
    }
}