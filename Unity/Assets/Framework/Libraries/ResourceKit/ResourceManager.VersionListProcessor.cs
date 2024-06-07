// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/27 15:36:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 版本资源列表处理器
        /// </summary>
        private sealed class VersionListProcessor
        {
            private readonly ResourceManager mResourceManager;

            private IDownloadManager mDownloadManager;
            private int mVersionListLength;
            private int mVersionListHashCode;
            private int mVersionListCompressedLength;
            private int mVersionListCompressedHashCode;

            public Action<string, string> VersionListUpdateSuccess;
            public Action<string, string> VersionListUpdateFailure;

            public VersionListProcessor(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;
                mDownloadManager = null;
                mVersionListLength = 0;
                mVersionListHashCode = 0;
                mVersionListCompressedLength = 0;
                mVersionListCompressedHashCode = 0;

                VersionListUpdateSuccess = null;
                VersionListUpdateFailure = null;
            }

            /// <summary>
            /// 关闭并清理版本资源列表处理器
            /// </summary>
            public void Shutdown()
            {
                if (mDownloadManager != null)
                {
                    mDownloadManager.DownloadSuccess -= OnDownloadSuccess;
                    mDownloadManager.DownloadFailure -= OnDownloadFailure;
                }
            }

            /// <summary>
            /// 设置下载管理器
            /// </summary>
            /// <param name="downloadManager">下载管理器</param>
            /// <exception cref="Exception"></exception>
            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                if (downloadManager == null)
                {
                    throw new Exception("Download manager is invalid.");
                }

                mDownloadManager = downloadManager;
                mDownloadManager.DownloadSuccess += OnDownloadSuccess;
                mDownloadManager.DownloadFailure += OnDownloadFailure;
            }

            /// <summary>
            /// 检查版本资源列表
            /// </summary>
            /// <param name="latestInternalResourceVersion">最新的内部资源版本号</param>
            /// <returns>检查版本资源列表结果</returns>
            /// <exception cref="Exception"></exception>
            public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
            {
                if (string.IsNullOrEmpty(mResourceManager.mReadWritePath))
                {
                    throw new Exception("Read-write path is invalid.");
                }

                var versionListFileName =
                    Utility.Path.GetRegularPath(
                        Path.Combine(mResourceManager.mReadWritePath, RemoteVersionListFileName));
                if (!File.Exists(versionListFileName))
                {
                    return CheckVersionListResult.NeedUpdate;
                }

                var internalResourceVersion = 0;
                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(versionListFileName, FileMode.Open, FileAccess.Read);
                    if (!mResourceManager.mUpdatableVersionListSerializer.TryGetValue(fileStream,
                            "InternalResourceVersion", out var internalResourceVersionObject))
                    {
                        return CheckVersionListResult.NeedUpdate;
                    }

                    internalResourceVersion = (int)internalResourceVersionObject;
                }
                catch
                {
                    return CheckVersionListResult.NeedUpdate;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }

                if (internalResourceVersion != latestInternalResourceVersion)
                {
                    return CheckVersionListResult.NeedUpdate;
                }

                return CheckVersionListResult.Updated;
            }

            /// <summary>
            /// 更新版本资源列表
            /// </summary>
            /// <param name="versionListLength">版本资源列表大小</param>
            /// <param name="versionListHashCode">版本资源列表哈希值</param>
            /// <param name="versionListCompressedLength">版本资源列表压缩后大小</param>
            /// <param name="versionListCompressedHashCode">版本资源列表压缩后哈希值</param>
            /// <exception cref="Exception"></exception>
            public void UpdateVersionList(int versionListLength, int versionListHashCode,
                int versionListCompressedLength, int versionListCompressedHashCode)
            {
                if (mDownloadManager == null)
                {
                    throw new Exception("You must set download manager first.");
                }

                mVersionListLength = versionListLength;
                mVersionListHashCode = versionListHashCode;
                mVersionListCompressedLength = versionListCompressedLength;
                mVersionListCompressedHashCode = versionListCompressedHashCode;
                var localVersionListFilePath =
                    Utility.Path.GetRegularPath(
                        Path.Combine(mResourceManager.mReadWritePath, RemoteVersionListFileName));
                var dotPosition = RemoteVersionListFileName.LastIndexOf('.');
                var latestVersionListFullNameWithCrc32 = string.Format("{0}.{2:x8}.{1}",
                    RemoteVersionListFileName.Substring(0, dotPosition),
                    RemoteVersionListFileName.Substring(dotPosition + 1), mVersionListHashCode);
                var versionListUri = Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mUpdatePrefixUri,
                    latestVersionListFullNameWithCrc32));
                mDownloadManager.AddDownload(new DownloadInfo(localVersionListFilePath, versionListUri, this));
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                var versionListProcessor = e.UserData as VersionListProcessor;
                if (versionListProcessor == null || versionListProcessor != this)
                {
                    return;
                }

                try
                {
                    using (var fileStream = new FileStream(e.DownloadPath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        var length = (int)fileStream.Length;
                        if (length != mVersionListCompressedLength)
                        {
                            fileStream.Close();
                            var errorMessage =
                                $"Latest version list compressed length error, need ({mVersionListCompressedLength}), downloaded ({length}).";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }

                        fileStream.Position = 0L;
                        var hashCode = Utility.Verifier.GetCrc32(fileStream);
                        if (hashCode != mVersionListCompressedHashCode)
                        {
                            fileStream.Close();
                            var errorMessage =
                                $"Latest version list compressed hash code error, need ({mVersionListCompressedHashCode}), downloaded ({hashCode}).";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }

                        fileStream.Position = 0L;
                        mResourceManager.PrepareCachedStream();
                        if (!Utility.Compression.Decompress(fileStream, mResourceManager.mCachedSteam))
                        {
                            fileStream.Close();
                            var errorMessage =
                                $"Unable to decompress latest version list ({e.DownloadPath})";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }

                        var uncompressedLength = (int)mResourceManager.mCachedSteam.Length;
                        if (uncompressedLength != mVersionListLength)
                        {
                            fileStream.Close();
                            var errorMessage =
                                $"Latest version list length error, need ({mVersionListLength}), downloaded ({uncompressedLength}).";
                            var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                                errorMessage, e.UserData);
                            OnDownloadFailure(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                            return;
                        }

                        fileStream.Position = 0L;
                        fileStream.SetLength(0L);
                        fileStream.Write(mResourceManager.mCachedSteam.GetBuffer(), 0, uncompressedLength);
                    }

                    VersionListUpdateSuccess?.Invoke(e.DownloadPath, e.DownloadUri);
                }
                catch (Exception exception)
                {
                    var errorMessage =
                        $"Update latest version list ({e.DownloadPath}) with error message ({exception}).";
                    var eventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri,
                        errorMessage, e.UserData);
                    OnDownloadFailure(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }

            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                var versionListProcessor = e.UserData as VersionListProcessor;
                if (versionListProcessor == null || versionListProcessor != this)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                VersionListUpdateFailure?.Invoke(e.DownloadUri, e.ErrorMessage);
            }
        }
    }
}