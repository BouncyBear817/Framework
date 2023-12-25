/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/18 11:05:26
 * Description:   对象池信息
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 对象池信息
    /// </summary>
    public struct ObjectPoolInfo
    {
        private const int DefaultCapacity = int.MaxValue;
        private const int DefaultPriority = 0;
        private const int DefaultAutoReleaseInterval = 0;
        private const bool DefaultAllowMultiSpawn = true;

        private string mName;
        private int mCapacity;
        private int mPriority;
        private float mAutoReleaseInterval;
        private bool mAllowMultiSpawn;

        /// <summary>
        /// 初始化对象信息实例
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="priority">对象池优先级</param>
        /// <param name="autoReleaseInterval">对象池自动释放对象的时间间隔</param>
        /// <param name="allowMultiSpawn">对象池中对象是否可被多次生成</param>
        public ObjectPoolInfo(string name, int capacity = DefaultCapacity,
            int priority = DefaultPriority, float autoReleaseInterval = DefaultAutoReleaseInterval,
            bool allowMultiSpawn = DefaultAllowMultiSpawn)
        {
            mName = name;
            mCapacity = capacity;
            mPriority = priority;
            mAutoReleaseInterval = autoReleaseInterval;
            mAllowMultiSpawn = allowMultiSpawn;
        }

        /// <summary>
        /// 对象池名称
        /// </summary>
        public string Name
        {
            get => mName;
            set => mName = value;
        }

        /// <summary>
        /// 对象池容量
        /// </summary>
        public int Capacity
        {
            get => mCapacity;
            set => mCapacity = value;
        }

        /// <summary>
        /// 对象池优先级
        /// </summary>
        public int Priority
        {
            get => mPriority;
            set => mPriority = value;
        }

        /// <summary>
        /// 对象池自动释放对象的时间间隔
        /// </summary>
        public float AutoReleaseInterval
        {
            get => mAutoReleaseInterval;
            set => mAutoReleaseInterval = value;
        }

        /// <summary>
        /// 对象池中对象是否可被多次生成
        /// </summary>
        public bool AllowMultiSpawn
        {
            get => mAllowMultiSpawn;
            set => mAllowMultiSpawn = value;
        }
    }
}