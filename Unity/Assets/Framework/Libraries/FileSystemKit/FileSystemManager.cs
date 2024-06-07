// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/7 10:42:21
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public class FileSystemManager : FrameworkModule, IFileSystemManager
    {
        private readonly Dictionary<string, FileSystem> mFileSystems;

        private IFileSystemHelper mFileSystemHelper;

        public FileSystemManager()
        {
            mFileSystems = new Dictionary<string, FileSystem>(StringComparer.Ordinal);
            mFileSystemHelper = null;
        }

        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public override int Priority => 4;

        /// <summary>
        /// 文件系统的数量
        /// </summary>
        public int Count => mFileSystems.Count;

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            while (mFileSystems.Count > 0)
            {
                foreach (var (_, fileSystem) in mFileSystems)
                {
                    DestroyFileSystem(fileSystem, false);
                    break;
                }
            }
        }

        /// <summary>
        /// 设置文件系统辅助器
        /// </summary>
        /// <param name="fileSystemHelper">文件系统辅助器</param>
        public void SetFileSystemHelper(IFileSystemHelper fileSystemHelper)
        {
            if (fileSystemHelper == null)
            {
                throw new Exception("File system helper is invalid.");
            }

            mFileSystemHelper = fileSystemHelper;
        }

        /// <summary>
        /// 检查是否存在文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>是否存在文件系统</returns>
        public bool HasFileSystem(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            return mFileSystems.ContainsKey(Utility.Path.GetRegularPath(fullPath));
        }

        /// <summary>
        /// 获取指定的文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>指定的文件系统</returns>
        public IFileSystem GetFileSystem(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            return mFileSystems.GetValueOrDefault(Utility.Path.GetRegularPath(fullPath));
        }

        /// <summary>
        /// 创建文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <param name="maxFileCount">文件系统的最大文件数量</param>
        /// <param name="maxBlockCount">文件系统的最大块数据数量</param>
        /// <returns>文件系统</returns>
        public IFileSystem CreateFileSystem(string fullPath, FileSystemAccess access, int maxFileCount, int maxBlockCount)
        {
            if (mFileSystemHelper == null)
            {
                throw new Exception("File system helper is invalid.");
            }

            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified || access == FileSystemAccess.Read)
            {
                throw new Exception("File system access is invalid.");
            }

            fullPath = Utility.Path.GetRegularPath(fullPath);
            if (mFileSystems.ContainsKey(fullPath))
            {
                throw new Exception($"File system ({fullPath}) is already exist.");
            }

            var fileSystemStream = mFileSystemHelper.CreateFileSystemStream(fullPath, access, true);
            if (fileSystemStream == null)
            {
                throw new Exception($"Create file system stream ({fullPath}) failure.");
            }

            var fileSystem = FileSystem.Create(fullPath, access, fileSystemStream, maxFileCount, maxBlockCount);
            if (fileSystem == null)
            {
                throw new Exception($"Create file system ({fullPath}) failure.");
            }

            mFileSystems.Add(fullPath, fileSystem);
            return fileSystem;
        }

        /// <summary>
        /// 加载文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <returns>文件系统</returns>
        public IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access)
        {
            if (mFileSystemHelper == null)
            {
                throw new Exception("File system helper is invalid.");
            }

            if (string.IsNullOrEmpty(fullPath))
            {
                throw new Exception("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified)
            {
                throw new Exception("File system access is invalid.");
            }

            fullPath = Utility.Path.GetRegularPath(fullPath);
            if (mFileSystems.ContainsKey(fullPath))
            {
                throw new Exception($"File system ({fullPath}) is already exist.");
            }

            var fileSystemStream = mFileSystemHelper.CreateFileSystemStream(fullPath, access, true);
            if (fileSystemStream == null)
            {
                throw new Exception($"Create file system stream ({fullPath}) failure.");
            }

            var fileSystem = FileSystem.Load(fullPath, access, fileSystemStream);
            if (fileSystem == null)
            {
                fileSystemStream.Close();
                throw new Exception($"Load file system ({fullPath}) failure.");
            }

            mFileSystems.Add(fullPath, fileSystem);
            return fileSystem;
        }

        /// <summary>
        /// 销毁文件系统
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件</param>
        public void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile)
        {
            if (fileSystem == null)
            {
                throw new Exception("File system is invalid.");
            }

            var fullPath = fileSystem.FullPath;
            ((FileSystem)fileSystem).Shutdown();
            mFileSystems.Remove(fullPath);

            if (deletePhysicalFile && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <returns>所有的文件系统</returns>
        public IFileSystem[] GetAllFileSystems()
        {
            var index = 0;
            var results = new IFileSystem[mFileSystems.Count];
            foreach (var (_, fileSystem) in mFileSystems)
            {
                results[index++] = fileSystem;
            }

            return results;
        }

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <param name="results">所有的文件系统</param>
        public void GetAllFileSystems(List<IFileSystem> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, fileSystem) in mFileSystems)
            {
                results.Add(fileSystem);
            }
        }
    }
}