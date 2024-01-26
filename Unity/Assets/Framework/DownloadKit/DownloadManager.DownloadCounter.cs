using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class DownloadManager : FrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载计数器
        /// </summary>
        private sealed class DownloadCounter
        {
            /// <summary>
            /// 下载计数器节点
            /// </summary>
            private sealed class DownloadCounterNode : IReference
            {
                private long mDeltaLength;
                private float mElapseSeconds;

                public DownloadCounterNode()
                {
                    mDeltaLength = 0L;
                    mElapseSeconds = 0f;
                }

                /// <summary>
                /// 增量数据大小
                /// </summary>
                public long DeltaLength => mDeltaLength;

                /// <summary>
                /// 逻辑流逝事件
                /// </summary>
                public float ElapseSeconds => mElapseSeconds;

                /// <summary>
                /// 创建下载计数器节点
                /// </summary>
                /// <returns>下载计数器节点</returns>
                public static DownloadCounterNode Create()
                {
                    return ReferencePool.Acquire<DownloadCounterNode>();
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    mElapseSeconds += realElapseSeconds;
                }

                /// <summary>
                /// 增加增量数据大小
                /// </summary>
                /// <param name="deltaLength">增量数据大小</param>
                public void AddDeltaLength(int deltaLength)
                {
                    mDeltaLength += deltaLength;
                }

                /// <summary>
                /// 清理下载计数器节点
                /// </summary>
                public void Clear()
                {
                    mDeltaLength = 0L;
                    mElapseSeconds = 0f;
                }
            }

            private readonly LinkedList<DownloadCounterNode> mDownloadCounterNodes;
            private float mUpdateInterval;
            private float mRecordInterval;
            private float mCurrentSpeed;
            private float mAccumulator; //累加器
            private float mTimeLeft;

            public DownloadCounter(float updateInterval, float recordInterval)
            {
                if (updateInterval <= 0f || recordInterval <= 0f)
                {
                    throw new Exception(
                        $"DownloadCount update ({updateInterval}) or record ({recordInterval}) interval is invalid.");
                }

                mUpdateInterval = updateInterval;
                mRecordInterval = recordInterval;
                mDownloadCounterNodes = new LinkedList<DownloadCounterNode>();
                Reset();
            }

            /// <summary>
            /// 更新间隔
            /// </summary>
            /// <exception cref="Exception"></exception>
            public float UpdateInterval
            {
                get => mUpdateInterval;
                set
                {
                    if (value <= 0f)
                    {
                        throw new Exception("update interval is invalid.");
                    }

                    mUpdateInterval = value;
                    Reset();
                }
            }

            /// <summary>
            /// 记录间隔
            /// </summary>
            /// <exception cref="Exception"></exception>
            public float RecordInterval
            {
                get => mRecordInterval;
                set
                {
                    if (value <= 0f)
                    {
                        throw new Exception("Record interval is invalid.");
                    }

                    mRecordInterval = value;
                    Reset();
                }
            }

            /// <summary>
            /// 当前下载速度
            /// </summary>
            public float CurrentSpeed => mCurrentSpeed;

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (mDownloadCounterNodes.Count <= 0)
                {
                    return;
                }

                mAccumulator += realElapseSeconds;
                if (mAccumulator > mRecordInterval)
                {
                    mAccumulator = mRecordInterval;
                }

                mTimeLeft -= realElapseSeconds;
                foreach (var node in mDownloadCounterNodes)
                {
                    node.Update(elapseSeconds, realElapseSeconds);
                }

                while (mDownloadCounterNodes.Count > 0)
                {
                    var node = mDownloadCounterNodes.First.Value;
                    if (node.ElapseSeconds < mRecordInterval)
                    {
                        break;
                    }

                    ReferencePool.Release(node);
                    mDownloadCounterNodes.RemoveFirst();
                }

                if (mDownloadCounterNodes.Count <= 0)
                {
                    Reset();
                    return;
                }

                if (mTimeLeft <= 0)
                {
                    var totalDeltaLength = 0L;
                    foreach (var node in mDownloadCounterNodes)
                    {
                        totalDeltaLength += node.DeltaLength;
                    }

                    mCurrentSpeed = mAccumulator > 0f ? totalDeltaLength / mAccumulator : 0f;
                    mTimeLeft += mUpdateInterval;
                }
            }

            public void Shutdown()
            {
                Reset();
            }

            /// <summary>
            /// 记录增量数据大小
            /// </summary>
            /// <param name="deltaLength">增量数据大小</param>
            public void RecordDeltaLength(int deltaLength)
            {
                if (deltaLength <= 0)
                {
                    return;
                }

                DownloadCounterNode downloadCounterNode = null;
                if (mDownloadCounterNodes.Count > 0)
                {
                    downloadCounterNode = mDownloadCounterNodes.Last.Value;
                    if (downloadCounterNode.ElapseSeconds < mUpdateInterval)
                    {
                        downloadCounterNode.AddDeltaLength(deltaLength);
                        return;
                    }
                }

                downloadCounterNode = DownloadCounterNode.Create();
                downloadCounterNode.AddDeltaLength(deltaLength);
                mDownloadCounterNodes.AddLast(downloadCounterNode);
            }

            /// <summary>
            /// 清理计数器
            /// </summary>
            private void Reset()
            {
                mDownloadCounterNodes.Clear();
                mCurrentSpeed = 0f;
                mAccumulator = 0f;
                mTimeLeft = 0f;
            }
        }
    }
}