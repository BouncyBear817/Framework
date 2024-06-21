// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 11:13:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 文件系统组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/File System")]
    public sealed class FileSystemComponent : FrameworkComponent
    {
        private IFileSystemManager mFileSystemManager = null;

        [SerializeField] private string mFileSystemHelperTypeName = "Framework.Runtime.DefaultFileSystemHelper";
        [SerializeField] private FileSystemHelperBase mCustomFileSystemHelper = null;

        /// <summary>
        /// 文件系统的数量
        /// </summary>
        public int Count => mFileSystemManager.Count;

        protected override void Awake()
        {
            base.Awake();

            mFileSystemManager = FrameworkEntry.GetModule<IFileSystemManager>();
            if (mFileSystemManager == null)
            {
                Log.Fatal("File system manager is invalid.");
                return;
            }

            var fileSystemHelper = Helper.CreateHelper(mFileSystemHelperTypeName, mCustomFileSystemHelper);
            if (fileSystemHelper == null)
            {
                Log.Error("Can  not create file system helper.");
                return;
            }

            fileSystemHelper.name = "FileSystem Helper";
            var trans = fileSystemHelper.transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;

            mFileSystemManager.SetFileSystemHelper(fileSystemHelper);
        }

        /// <summary>
        /// 检查是否存在文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>是否存在文件系统</returns>
        public bool HasFileSystem(string fullPath)
        {
            return mFileSystemManager.HasFileSystem(fullPath);
        }

        /// <summary>
        /// 获取指定的文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <returns>指定的文件系统</returns>
        public IFileSystem GetFileSystem(string fullPath)
        {
            return mFileSystemManager.GetFileSystem(fullPath);
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
            return mFileSystemManager.CreateFileSystem(fullPath, access, maxFileCount, maxBlockCount);
        }

        /// <summary>
        /// 加载文件系统
        /// </summary>
        /// <param name="fullPath">文件系统的完整路径</param>
        /// <param name="access">文件系统访问方式</param>
        /// <returns>文件系统</returns>
        public IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access)
        {
            return mFileSystemManager.LoadFileSystem(fullPath, access);
        }

        /// <summary>
        /// 销毁文件系统
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件</param>
        public void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile)
        {
            mFileSystemManager.DestroyFileSystem(fileSystem, deletePhysicalFile);
        }

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <returns>所有的文件系统</returns>
        public IFileSystem[] GetAllFileSystems()
        {
            return mFileSystemManager.GetAllFileSystems();
        }

        /// <summary>
        /// 获取所有的文件系统
        /// </summary>
        /// <param name="results">所有的文件系统</param>
        public void GetAllFileSystems(List<IFileSystem> results)
        {
            mFileSystemManager.GetAllFileSystems(results);
        }
    }
}