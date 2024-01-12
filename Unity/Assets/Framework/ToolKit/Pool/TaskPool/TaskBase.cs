/************************************************************
* Unity Version: 2022.3.15f1c1
* Author:        bear
* CreateTime:    2024/01/09 14:39:14
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class TaskBase : IReference
    {
        public const int DefaultPriority = 0;

        private int mSerialId;
        private string mTag;
        private int mPriority;
        private object mUserData;
        private bool mDone;

        protected TaskBase()
        {
            mSerialId = 0;
            mTag = null;
            mPriority = DefaultPriority;
            mUserData = null;
            mDone = false;
        }

        /// <summary>
        /// 任务序列编号
        /// </summary>
        public int SerialId => mSerialId;

        /// <summary>
        /// 任务标签
        /// </summary>
        public string Tag => mTag;

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// 任务用户数据
        /// </summary>
        public object UserData => mUserData;

        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool Done => mDone;

        /// <summary>
        /// 任务描述
        /// </summary>
        public virtual string Description => null;

        /// <summary>
        /// 初始化任务基类
        /// </summary>
        /// <param name="serialId">任务序列编号</param>
        /// <param name="tag">任务标签</param>
        /// <param name="priority">任务优先级</param>
        /// <param name="userData">任务用户数据</param>
        public void Initialize(int serialId, string tag, int priority, object userData)
        {
            mSerialId = serialId;
            mTag = tag;
            mPriority = priority;
            mUserData = userData;
            mDone = false;
        }

        /// <summary>
        /// 清理任务基类
        /// </summary>
        public void Clear()
        {
            mSerialId = 0;
            mTag = null;
            mPriority = DefaultPriority;
            mUserData = null;
            mDone = false;
        }
    }
}