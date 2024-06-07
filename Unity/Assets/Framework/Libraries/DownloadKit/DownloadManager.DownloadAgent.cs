using System;
using System.IO;

namespace Framework
{
    public sealed partial class DownloadManager : FrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载代理器
        /// </summary>
        private sealed class DownloadAgent : ITaskAgent<DownloadTask>, IDisposable
        {
            private readonly IDownloadAgentHelper mDownloadAgentHelper;
            private DownloadTask mTask;
            private FileStream mFileStream;
            private int mWaitFlushSize;
            private float mWaitTime;
            private long mStartLength;
            private long mDownloadedLength;
            private long mSavedLength;
            private bool mDisposed;

            public Action<DownloadAgent> DownloadAgentStart;
            public Action<DownloadAgent, int> DownloadAgentUpdate;
            public Action<DownloadAgent, long> DownloadAgentSuccess;
            public Action<DownloadAgent, string> DownloadAgentFailure;

            public DownloadAgent(IDownloadAgentHelper downloadAgentHelper)
            {
                mDownloadAgentHelper = downloadAgentHelper ?? throw new Exception("Download agent helper is invalid.");

                mTask = null;
                mFileStream = null;
                mWaitFlushSize = 0;
                mWaitTime = 0f;
                mStartLength = 0L;
                mDownloadedLength = 0L;
                mSavedLength = 0L;
                mDisposed = false;

                DownloadAgentStart = null;
                DownloadAgentUpdate = null;
                DownloadAgentSuccess = null;
                DownloadAgentFailure = null;
            }

            /// <summary>
            /// 下载任务
            /// </summary>
            public DownloadTask Task => mTask;

            /// <summary>
            /// 下载等待时间
            /// </summary>
            public float WaitTime => mWaitTime;

            /// <summary>
            /// 开始文件大小
            /// </summary>
            public long StartLength => mStartLength;

            /// <summary>
            /// 下载文件大小
            /// </summary>
            public long DownloadedLength => mDownloadedLength;

            /// <summary>
            /// 已经下载保存的大小
            /// </summary>
            public long SavedLength => mSavedLength;

            /// <summary>
            /// 当前下载文件大小
            /// </summary>
            public long CurrentLength => mStartLength + mDownloadedLength;

            /// <summary>
            /// 获取下载文件名称
            /// </summary>
            /// <returns></returns>
            private string GetDownloadFile()
            {
                return $"{mTask.DownloadPath}.download";
            }

            /// <summary>
            /// 初始化下载代理器
            /// </summary>
            public void Initialize()
            {
                mDownloadAgentHelper.DownloadAgentHelperComplete += OnDownloadAgentHelperComplete;
                mDownloadAgentHelper.DownloadAgentHelperError += OnDownloadAgentHelperError;
                mDownloadAgentHelper.DownloadAgentHelperUpdateBytes += OnDownloadAgentHelperUpdateBytes;
                mDownloadAgentHelper.DownloadAgentHelperUpdateLength += OnDownloadAgentHelperUpdateLength;
            }

            /// <summary>
            /// 任务代理轮询
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (mTask.Status == DownloadTaskStatus.Doing)
                {
                    mWaitTime += realElapseSeconds;
                    if (mWaitTime >= mTask.Timeout)
                    {
                        var eventArgs = DownloadAgentHelperErrorEventArgs.Create(false, "Timeout");
                        OnDownloadAgentHelperError(this, eventArgs);
                        ReferencePool.Release(eventArgs);
                    }
                }
            }

            /// <summary>
            /// 关闭并清理任务代理
            /// </summary>
            public void Shutdown()
            {
                Dispose();

                mDownloadAgentHelper.DownloadAgentHelperComplete -= OnDownloadAgentHelperComplete;
                mDownloadAgentHelper.DownloadAgentHelperError -= OnDownloadAgentHelperError;
                mDownloadAgentHelper.DownloadAgentHelperUpdateBytes -= OnDownloadAgentHelperUpdateBytes;
                mDownloadAgentHelper.DownloadAgentHelperUpdateLength -= OnDownloadAgentHelperUpdateLength;
            }


            /// <summary>
            /// 开始处理任务
            /// </summary>
            /// <param name="task">任务</param>
            /// <returns>开始处理任务的状态</returns>
            public StartTaskStatus Start(DownloadTask task)
            {
                mTask = task ?? throw new Exception("Task is invalid.");
                mTask.Status = DownloadTaskStatus.Doing;

                var downloadFile = GetDownloadFile();
                try
                {
                    if (File.Exists(downloadFile))
                    {
                        mFileStream = File.OpenWrite(downloadFile);
                        mFileStream.Seek(0L, SeekOrigin.End);
                        mStartLength = mSavedLength = mFileStream.Length;
                        mDownloadedLength = 0L;
                    }
                    else
                    {
                        var directory = Path.GetDirectoryName(mTask.DownloadPath);
                        if (directory != null && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        mFileStream = new FileStream(downloadFile, FileMode.Create, FileAccess.Write);
                        mStartLength = mSavedLength = mDownloadedLength = 0L;
                    }

                    DownloadAgentStart?.Invoke(this);

                    if (mStartLength > 0L)
                    {
                        mDownloadAgentHelper.Download(mTask.DownloadUri, mTask.UserData, mStartLength);
                    }
                    else
                    {
                        mDownloadAgentHelper.Download(mTask.DownloadUri, mTask.UserData);
                    }

                    return StartTaskStatus.CanResume;
                }
                catch (Exception e)
                {
                    var eventArgs = DownloadAgentHelperErrorEventArgs.Create(false, e.ToString());
                    OnDownloadAgentHelperError(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                    return StartTaskStatus.UnknownError;
                }
            }

            /// <summary>
            /// 停止正在处理的任务并重置任务代理
            /// </summary>
            public void StopAndReset()
            {
                mDownloadAgentHelper.Reset();

                mFileStream?.Close();

                mTask = null;
                mFileStream = null;
                mWaitFlushSize = 0;
                mWaitTime = 0f;
                mStartLength = 0L;
                mDownloadedLength = 0L;
                mSavedLength = 0L;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// 释放资源
            /// </summary>
            /// <param name="disposing">是否释放</param>
            private void Dispose(bool disposing)
            {
                if (mDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    mFileStream?.Dispose();
                    mFileStream = null;
                }

                mDisposed = true;
            }

            private void OnDownloadAgentHelperComplete(object sender, DownloadAgentHelperCompleteEventArgs e)
            {
                mWaitTime = 0f;
                mDownloadedLength = e.Length;
                if (mSavedLength != CurrentLength)
                {
                    throw new Exception("Internal download error.");
                }

                mDownloadAgentHelper.Reset();
                mFileStream?.Close();
                mFileStream = null;

                mTask.Status = DownloadTaskStatus.Done;

                DownloadAgentSuccess?.Invoke(this, e.Length);

                mTask.Done = true;
            }

            private void OnDownloadAgentHelperError(object sender, DownloadAgentHelperErrorEventArgs e)
            {
                mDownloadAgentHelper.Reset();
                mFileStream?.Close();
                mFileStream = null;

                if (e.IsDeleteDownloading)
                {
                    File.Delete(GetDownloadFile());
                }

                mTask.Status = DownloadTaskStatus.Error;
                DownloadAgentFailure?.Invoke(this, e.ErrorMessage);
                mTask.Done = true;
            }

            private void OnDownloadAgentHelperUpdateBytes(object sender, DownloadAgentHelperUpdateBytesEventArgs e)
            {
                mWaitTime = 0f;
                try
                {
                    mFileStream.Write(e.Bytes, e.Offset, e.Length);
                    mWaitFlushSize += e.Length;
                    mSavedLength += e.Length;
                    if (mWaitFlushSize >= mTask.FlushSize)
                    {
                        mFileStream.Flush();
                        mWaitFlushSize = 0;
                    }
                }
                catch (Exception exception)
                {
                    var eventArgs = DownloadAgentHelperErrorEventArgs.Create(false, exception.ToString());
                    OnDownloadAgentHelperError(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }

            private void OnDownloadAgentHelperUpdateLength(object sender, DownloadAgentHelperUpdateLengthEventArgs e)
            {
                mWaitTime = 0f;
                mDownloadedLength += e.DeltaLength;
                DownloadAgentUpdate?.Invoke(this, e.DeltaLength);
            }
        }
    }
}