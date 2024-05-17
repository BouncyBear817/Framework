// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 14:46:5
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// Path相关的实用函数
        /// </summary>
        public static class Path
        {
            /// <summary>
            /// 获取规范的路径
            /// </summary>
            /// <param name="path">路径</param>
            /// <returns>规范的路径</returns>
            public static string GetRegularPath(string path)
            {
                if (path == null)
                {
                    return null;
                }

                return path.Replace("\\", "/");
            }

            /// <summary>
            /// 获取远程的路径（带有file://或http://前缀）
            /// </summary>
            /// <param name="path">路径</param>
            /// <returns>远程的路径</returns>
            public static string GetRemotePath(string path)
            {
                var regularPath = GetRegularPath(path);
                if (regularPath == null)
                {
                    return null;
                }

                return regularPath.Contains("://")
                    ? regularPath
                    : ("file:///" + regularPath).Replace("file:////", "file:///");
            }

            /// <summary>
            /// 移除空的文件夹
            /// </summary>
            /// <param name="directoryName">文件夹名称</param>
            /// <returns>是否成功移除空的文件夹</returns>
            public static bool RemoveEmptyDirectory(string directoryName)
            {
                if (string.IsNullOrEmpty(directoryName))
                {
                    return false;
                }

                try
                {
                    if (!Directory.Exists(directoryName))
                    {
                        return false;
                    }

                    var subDirectoryNames = Directory.GetDirectories(directoryName, "*");
                    int subDirectoryCount = subDirectoryNames.Length;
                    foreach (var subDirectoryName in subDirectoryNames)
                    {
                        if (RemoveEmptyDirectory(subDirectoryName))
                        {
                            subDirectoryCount--;
                        }
                    }

                    if (subDirectoryCount > 0)
                    {
                        return false;
                    }

                    if (Directory.GetFiles(directoryName, "*").Length > 0)
                    {
                        return false;
                    }

                    Directory.Delete(directoryName);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}