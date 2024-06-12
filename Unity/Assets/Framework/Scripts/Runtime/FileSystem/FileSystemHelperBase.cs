// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 10:27:11
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 文件系统辅助器基类
    /// </summary>
    public abstract class FileSystemHelperBase : MonoBehaviour, IFileSystemHelper
    {
        /// <summary>
        /// 创建文件系统流
        /// </summary>
        /// <param name="fullPath">文件系统完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="createNew">是否创建新的文件系统流</param>
        /// <returns>文件系统流</returns>
        public abstract FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew);
    }
}