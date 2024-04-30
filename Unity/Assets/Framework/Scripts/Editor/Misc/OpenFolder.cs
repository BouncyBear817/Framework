/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/04/30 10:22:50
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    /// <summary>
    /// 打开文件夹相关的实用函数
    /// </summary>
    public static class OpenFolder
    {
        /// <summary>
        /// 打开 Data Path 文件夹
        /// </summary>
        [MenuItem("Framework/Open Folder/Data Path")]
        public static void OpenFolderDataPath()
        {
            Execute(Application.dataPath);
        }

        /// <summary>
        /// 打开 Persistent Data Path 文件夹
        /// </summary>
        [MenuItem("Framework/Open Folder/Persistent Data Path")]
        public static void OpenFolderPersistentDataPath()
        {
            Execute(Application.persistentDataPath);
        }

        /// <summary>
        /// 打开 Streaming Assets Path 文件夹
        /// </summary>
        [MenuItem("Framework/Open Folder/Streaming Assets Path")]
        public static void OpenFolderStreamingAssetsPath()
        {
            Execute(Application.streamingAssetsPath);
        }

        /// <summary>
        /// 打开 Temporary Cache Path 文件夹
        /// </summary>
        [MenuItem("Framework/Open Folder/Temporary Cache Path")]
        public static void OpenFolderTemporaryCachePath()
        {
            Execute(Application.temporaryCachePath);
        }

        /// <summary>
        /// 打开 Console Log Path 文件夹
        /// </summary>
        [MenuItem("Framework/Open Folder/Console Log Path")]
        public static void OpenFolderConsoleLogPath()
        {
            Execute(Application.consoleLogPath);
        }

        /// <summary>
        /// 打开指定路径的文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <exception cref="Exception"></exception>
        private static void Execute(string path)
        {
            path = $"\"{path}\"";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", path.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", path);
                    break;
                default:
                    throw new Exception($"Not support open folder on ({Application.platform}) platform.");
            }
        }
    }
}