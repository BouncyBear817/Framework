// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 15:47:3
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    /// <summary>
    /// 文件系统辅助器接口
    /// </summary>
    public interface IFileSystemHelper
    {
        /// <summary>
        /// 创建文件系统流
        /// </summary>
        /// <param name="fullPath">文件系统完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="createNew">是否创建新的文件系统流</param>
        /// <returns>文件系统流</returns>
        FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew);
    }
}