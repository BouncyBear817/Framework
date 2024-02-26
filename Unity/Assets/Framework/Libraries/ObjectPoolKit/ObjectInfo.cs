/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/18 11:05:27
* Description:   对象信息
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 对象信息
    /// </summary>
    public struct ObjectInfo
    {
        private readonly string mName;
        private readonly bool mLocked;
        private readonly bool mCanReleaseFlag;
        private readonly int mPriority;
        private readonly int mSpawnCount;

        /// <summary>
        /// 初始化对象信息的实例
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="locked">对象是否被加锁</param>
        /// <param name="canReleaseFlag">对象释放标记</param>
        /// <param name="priority">对象优先级</param>
        /// <param name="spawnCount">对象的生成次数</param>
        public ObjectInfo(string name, bool locked, bool canReleaseFlag, int priority, int spawnCount)
        {
            mName = name;
            mLocked = locked;
            mCanReleaseFlag = canReleaseFlag;
            mPriority = priority;
            mSpawnCount = spawnCount;
        }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// 对象是否被加锁
        /// </summary>
        public bool Locked => mLocked;

        /// <summary>
        /// 对象释放标记
        /// </summary>
        public bool CanReleaseFlag => mCanReleaseFlag;

        /// <summary>
        /// 对象优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// 对象的生成次数
        /// </summary>
        public int SpawnCount => mSpawnCount;

        /// <summary>
        /// 对象是否正在使用
        /// </summary>
        public bool IsInUse => mSpawnCount > 0;
    }
}