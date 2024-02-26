/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/04 14:46:35
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 加载数据表成功事件
    /// </summary>
    public class LoadDataTableSuccessEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadDataTableSuccessEventArgs).GetHashCode();

        public LoadDataTableSuccessEventArgs()
        {
            DataTableAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string DataTableAssetName { get; private set; }

        /// <summary>
        /// 加载持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载数据表成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>加载数据表成功事件</returns>
        public static LoadDataTableSuccessEventArgs Create(ReadDataSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadDataTableSuccessEventArgs>();
            eventArgs.DataTableAssetName = e.DataAssetName;
            eventArgs.Duration = e.Duration;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表成功事件
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表失败事件
    /// </summary>
    public class LoadDataTableFailureEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadDataTableFailureEventArgs).GetHashCode();

        public LoadDataTableFailureEventArgs()
        {
            DataTableAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string DataTableAssetName { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载数据表失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>加载数据表失败事件</returns>
        public static LoadDataTableFailureEventArgs Create(ReadDataFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadDataTableFailureEventArgs>();
            eventArgs.DataTableAssetName = e.DataAssetName;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表更新事件
    /// </summary>
    public class LoadDataTableUpdateEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadDataTableUpdateEventArgs).GetHashCode();

        public LoadDataTableUpdateEventArgs()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string DataTableAssetName { get; private set; }

        /// <summary>
        /// 加载进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载数据表更新事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>加载数据表更新事件</returns>
        public static LoadDataTableUpdateEventArgs Create(ReadDataUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadDataTableUpdateEventArgs>();
            eventArgs.DataTableAssetName = e.DataAssetName;
            eventArgs.Progress = e.Progress;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表更新事件
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表依赖资源事件
    /// </summary>
    public class LoadDataTableDependencyAssetEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(LoadDataTableDependencyAssetEventArgs).GetHashCode();

        public LoadDataTableDependencyAssetEventArgs()
        {
            DataTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string DataTableAssetName { get; private set; }

        /// <summary>
        /// 依赖资源名称
        /// </summary>
        public string DependencyAssetName { get; private set; }

        /// <summary>
        /// 已加载依赖资源数量
        /// </summary>
        public int LoadedCount { get; private set; }

        /// <summary>
        /// 总共依赖资源数量
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建加载数据表依赖资源事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>加载数据表依赖资源事件</returns>
        public static LoadDataTableDependencyAssetEventArgs Create(ReadDataDependencyAssetEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadDataTableDependencyAssetEventArgs>();
            eventArgs.DataTableAssetName = e.DataAssetName;
            eventArgs.DependencyAssetName = e.DependencyAssetName;
            eventArgs.LoadedCount = e.LoadedCount;
            eventArgs.TotalCount = e.TotalCount;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表依赖资源事件
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}