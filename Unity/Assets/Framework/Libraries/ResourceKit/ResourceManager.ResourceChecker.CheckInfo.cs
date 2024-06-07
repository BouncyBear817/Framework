using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceChecker
        {
            internal sealed partial class CheckInfo
            {
                private readonly ResourceName mResourceName;
                private CheckStatus mStatus;
                private bool mNeedRemove;
                private bool mNeedMoveToDisk;
                private bool mNeedMoveToFileSystem;
                private RemoteVersionInfo mRemoteVersionInfo;
                private LocalVersionInfo mReadOnlyInfo;
                private LocalVersionInfo mReadWriteInfo;
                private string mCachedFileSystemName;

                public CheckInfo(ResourceName resourceName)
                {
                    mResourceName = resourceName;
                    mStatus = CheckStatus.Unknown;
                    mNeedRemove = false;
                    mNeedMoveToDisk = false;
                    mNeedMoveToFileSystem = false;
                    mRemoteVersionInfo = default(RemoteVersionInfo);
                    mReadOnlyInfo = default(LocalVersionInfo);
                    mReadWriteInfo = default(LocalVersionInfo);
                    mCachedFileSystemName = null;
                }

                /// <summary>
                /// 资源名称
                /// </summary>
                public ResourceName ResourceName => mResourceName;

                /// <summary>
                /// 资源检查状态
                /// </summary>
                public CheckStatus Status => mStatus;

                /// <summary>
                /// 是否需要移除读写区资源
                /// </summary>
                public bool NeedRemove => mNeedRemove;

                /// <summary>
                /// 是否需要将读写区资源移至磁盘
                /// </summary>
                public bool NeedMoveToDisk => mNeedMoveToDisk;

                /// <summary>
                /// 是否需要将读写区资源移至文件系统
                /// </summary>
                public bool NeedMoveToFileSystem => mNeedMoveToFileSystem;

                /// <summary>
                /// 资源所在文件系统的名称
                /// </summary>
                public string FileSystemName => mRemoteVersionInfo.FileSystemName;

                /// <summary>
                /// 资源加载方式
                /// </summary>
                public LoadType LoadType => mRemoteVersionInfo.LoadType;

                /// <summary>
                /// 资源大小
                /// </summary>
                public int Length => mRemoteVersionInfo.Length;

                /// <summary>
                /// 资源哈希值
                /// </summary>
                public int HashCode => mRemoteVersionInfo.HashCode;

                /// <summary>
                /// 压缩后资源大小
                /// </summary>
                public int CompressedLength => mRemoteVersionInfo.CompressedLength;

                /// <summary>
                /// 压缩后资源哈希值
                /// </summary>
                public int CompressedHashCode => mRemoteVersionInfo.CompressHashCode;

                /// <summary>
                /// 读写区资源是否使用文件系统
                /// </summary>
                public bool ReadWriteUseFileSystem => mReadWriteInfo.UseFileSystem;

                /// <summary>
                /// 读写区资源所在的文件系统的名称
                /// </summary>
                public string ReadWriteFileSystemName => mReadWriteInfo.FileSystemName;

                /// <summary>
                /// 设置临时缓存资源所在的文件系统的名称
                /// </summary>
                /// <param name="fileSystemName">资源所在的文件系统的名称</param>
                public void SetCachedFileSystemName(string fileSystemName)
                {
                    mCachedFileSystemName = fileSystemName;
                }

                /// <summary>
                /// 设置资源在远程版本中的信息
                /// </summary>
                /// <param name="loadType">资源加载方式</param>
                /// <param name="length">资源大小</param>
                /// <param name="hashCode">资源哈希值</param>
                /// <param name="compressedLength">压缩后资源大小</param>
                /// <param name="compressedHashCode">压缩后资源哈希值</param>
                /// <exception cref="Exception"></exception>
                public void SetRemoteVersionInfo(LoadType loadType, int length, int hashCode, int compressedLength,
                    int compressedHashCode)
                {
                    if (mRemoteVersionInfo.Exist)
                    {
                        throw new Exception(
                            $"You must set remote version info of ({mResourceName.FullName}) only once.");
                    }

                    mRemoteVersionInfo = new RemoteVersionInfo(mCachedFileSystemName, loadType, length, hashCode,
                        compressedLength, compressedHashCode);
                    mCachedFileSystemName = null;
                }

                /// <summary>
                /// 设置资源在只读区的信息
                /// </summary>
                /// <param name="loadType">资源加载方式</param>
                /// <param name="length">资源大小</param>
                /// <param name="hashCode">资源哈希值</param>
                /// <exception cref="Exception"></exception>
                public void SetReadOnlyInfo(LoadType loadType, int length, int hashCode)
                {
                    if (mReadOnlyInfo.Exist)
                    {
                        throw new Exception($"You must set read-only info of ({mResourceName.FullName}) only once.");
                    }

                    mReadOnlyInfo = new LocalVersionInfo(mCachedFileSystemName, loadType, length, hashCode);
                    mCachedFileSystemName = null;
                }

                /// <summary>
                /// 设置资源在读写区的信息
                /// </summary>
                /// <param name="loadType">资源加载方式</param>
                /// <param name="length">资源大小</param>
                /// <param name="hashCode">资源哈希值</param>
                /// <exception cref="Exception"></exception>
                public void SetReadWriteInfo(LoadType loadType, int length, int hashCode)
                {
                    if (mReadWriteInfo.Exist)
                    {
                        throw new Exception($"You must set read-write info of ({mResourceName.FullName}) only once.");
                    }

                    mReadWriteInfo = new LocalVersionInfo(mCachedFileSystemName, loadType, length, hashCode);
                    mCachedFileSystemName = null;
                }

                /// <summary>
                /// 刷新资源信息状态
                /// </summary>
                /// <param name="currentVariant">当前变体</param>
                /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若不忽略则移除</param>
                public void RefreshStatus(string currentVariant, bool ignoreOtherVariant)
                {
                    if (!mRemoteVersionInfo.Exist)
                    {
                        mStatus = CheckStatus.Disuse;
                        mNeedRemove = mReadWriteInfo.Exist;
                        return;
                    }

                    if (mResourceName.Variant == null || mResourceName.Variant == currentVariant)
                    {
                        if (mReadOnlyInfo.Exist &&
                            mReadOnlyInfo.FileSystemName == mRemoteVersionInfo.FileSystemName &&
                            mReadOnlyInfo.LoadType == mRemoteVersionInfo.LoadType &&
                            mReadOnlyInfo.Length == mRemoteVersionInfo.Length &&
                            mReadOnlyInfo.HashCode == mRemoteVersionInfo.HashCode)
                        {
                            mStatus = CheckStatus.StorageInReadOnly;
                            mNeedRemove = mReadOnlyInfo.Exist;
                        }
                        else if (mReadWriteInfo.Exist &&
                                 mReadWriteInfo.LoadType == mRemoteVersionInfo.LoadType &&
                                 mReadWriteInfo.Length == mRemoteVersionInfo.Length &&
                                 mReadWriteInfo.HashCode == mRemoteVersionInfo.HashCode)
                        {
                            var differentFileSystem =
                                mReadWriteInfo.FileSystemName != mRemoteVersionInfo.FileSystemName;
                            mStatus = CheckStatus.StorageInReadWrite;
                            mNeedMoveToDisk = mReadWriteInfo.UseFileSystem && differentFileSystem;
                            mNeedMoveToFileSystem = mRemoteVersionInfo.UseFileSystem && differentFileSystem;
                        }
                        else
                        {
                            mStatus = CheckStatus.Update;
                            mNeedRemove = mReadWriteInfo.Exist;
                        }
                    }
                    else
                    {
                        mStatus = CheckStatus.Unavailable;
                        mNeedRemove = !ignoreOtherVariant && mReadWriteInfo.Exist;
                    }
                }
            }
        }
    }
}