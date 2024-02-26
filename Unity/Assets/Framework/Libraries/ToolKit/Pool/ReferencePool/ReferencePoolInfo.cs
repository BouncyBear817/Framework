/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 10:43:38
* Description:   
* Modify Record: 
*************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Framework
{
    /// <summary>
    /// 引用池信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ReferencePoolInfo
    {
        private readonly Type mType;
        private readonly int mUnusedReferenceCount;
        private readonly int mUsingReferenceCount;
        private readonly int mAcquireReferenceCount;
        private readonly int mReleaseReferenceCount;
        private readonly int mAddReferenceCount;
        private readonly int mRemoveReferenceCount;
        
        /// <summary>
        /// 初始化引用池信息的新实例。
        /// </summary>
        /// <param name="type">引用池类型</param>
        /// <param name="unusedReferenceCount">未使用引用数量</param>
        /// <param name="usingReferenceCount">正在使用引用数量</param>
        /// <param name="acquireReferenceCount">获取引用数量</param>
        /// <param name="releaseReferenceCount">释放引用数量</param>
        /// <param name="addReferenceCount">增加引用数量</param>
        /// <param name="removeReferenceCount">移除引用数量</param>
        public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            mType = type;
            mUnusedReferenceCount = unusedReferenceCount;
            mUsingReferenceCount = usingReferenceCount;
            mAcquireReferenceCount = acquireReferenceCount;
            mReleaseReferenceCount = releaseReferenceCount;
            mAddReferenceCount = addReferenceCount;
            mRemoveReferenceCount = removeReferenceCount;
        }
        
        /// <summary>
        /// 获取引用池类型
        /// </summary>
        public Type Type => mType;
        /// <summary>
        /// 未使用引用数量
        /// </summary>
        public int UnusedReferenceCount => mUnusedReferenceCount;
        /// <summary>
        /// 正在使用引用数量
        /// </summary>
        public int UsingReferenceCount => mUsingReferenceCount;
        /// <summary>
        /// 获取引用数量
        /// </summary>
        public int AcquireReferenceCount => mAcquireReferenceCount;
        /// <summary>
        /// 释放引用数量
        /// </summary>
        public int ReleaseReferenceCount => mReleaseReferenceCount;
        /// <summary>
        /// 增加引用数量
        /// </summary>
        public int AddReferenceCount => mAddReferenceCount;
        /// <summary>
        /// 移除引用数量
        /// </summary>
        public int RemoveReferenceCount => mRemoveReferenceCount;
    }
}