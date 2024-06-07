// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 14:33:32
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// 文件系统的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 设置文件系统辅助器
        /// </summary>
        /// <param name="fileSystemHelper">文件系统辅助器</param>
        void SetFileSystemHelper(IFileSystemHelper fileSystemHelper);

        /// <summary>
        /// 检查是否存在文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>是否存在文件系统</returns>
        bool HasFileSystem(string fullPath);

        /// <summary>
        /// 获取指定的文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>指定的文件系统</returns>
        IFileSystem GetFileSystem(string fullPath);

        /// <summary>
        /// 创建文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="maxFileCount">文件系统的最大文件数量</param>
        /// <param name="maxBlockCount">文件系统的最大块数据数量</param>
        /// <returns>文件系统</returns>
        IFileSystem CreateFileSystem(string fullPath, FileSystemAccess access, int maxFileCount, int maxBlockCount);

        /// <summary>
        /// 加载文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <returns>文件系统</returns>
        IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access);

        /// <summary>
        /// 销毁文件系统
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件</param>
        void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile);

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <returns>所有的文件系统</returns>
        IFileSystem[] GetAllFileSystems();

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <param name="results">所有的文件系统</param>
        void GetAllFileSystems(List<IFileSystem> results);
    }
}