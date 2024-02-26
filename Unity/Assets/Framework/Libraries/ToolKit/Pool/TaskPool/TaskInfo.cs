/************************************************************
* Unity Version: 2022.3.15f1c1
* Author:        bear
* CreateTime:    2024/01/09 14:39:14
* Description:   
* Modify Record: 
*************************************************************/

using System.Runtime.InteropServices;

namespace Framework
{
    /// <summary>
    /// 任务信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct TaskInfo
    {
        private readonly bool mIsValid;
        private readonly int mSerialId;
        private readonly string mTag;
        private readonly int mPriority;
        private readonly object mUserData;
        private readonly TaskStatus mStatus;
        private readonly string mDescription;

        public TaskInfo(int mSerialId, string mTag, int mPriority, object mUserData, TaskStatus mStatus,
            string mDescription) : this()
        {
            mIsValid = true;
            this.mSerialId = mSerialId;
            this.mTag = mTag;
            this.mPriority = mPriority;
            this.mUserData = mUserData;
            this.mStatus = mStatus;
            this.mDescription = mDescription;
        }

        /// <summary>
        /// 任务信息
        /// </summary>
        public bool IsValid => mIsValid;

        /// <summary>
        /// 任务的序列编号
        /// </summary>
        public int SerialId => mSerialId;

        /// <summary>
        /// 任务的标签
        /// </summary>
        public string Tag => mTag;

        /// <summary>
        /// 任务的优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// 任务的用户数据
        /// </summary>
        public object UserData => mUserData;

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus Status => mStatus;

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Description => mDescription;
    }
}