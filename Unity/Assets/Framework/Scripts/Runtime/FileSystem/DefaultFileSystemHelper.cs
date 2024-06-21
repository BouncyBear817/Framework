// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 10:29:20
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认文件系统辅助器
    /// </summary>
    public class DefaultFileSystemHelper : FileSystemHelperBase
    {
        private const string AndroidFileSystemPrefixString = "jar:";

        /// <summary>
        /// 创建文件系统流
        /// </summary>
        /// <param name="fullPath">文件系统完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="createNew">是否创建新的文件系统流</param>
        /// <returns>文件系统流</returns>
        public override FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (fullPath.StartsWith(AndroidFileSystemPrefixString, StringComparison.Ordinal))
            {
                return new AndroidFileSystemStream(fullPath, access, createNew);
            }
            else
            {
                return new CommonFileSystemStream(fullPath, access, createNew);
            }
        }
    }
}