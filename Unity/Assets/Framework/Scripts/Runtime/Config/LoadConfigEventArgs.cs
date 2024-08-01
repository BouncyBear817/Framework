/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 14:08:41
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 加载数据表成功事件
    /// </summary>
    public class LoadConfigSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigSuccessEventArgs).GetHashCode();

        public LoadConfigSuccessEventArgs()
        {
            ConfigAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string ConfigAssetName { get; private set; }

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
        public static LoadConfigSuccessEventArgs Create(ReadDataSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadConfigSuccessEventArgs>();
            eventArgs.ConfigAssetName = e.DataAssetName;
            eventArgs.Duration = e.Duration;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表成功事件
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表失败事件
    /// </summary>
    public class LoadConfigFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigFailureEventArgs).GetHashCode();

        public LoadConfigFailureEventArgs()
        {
            ConfigAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string ConfigAssetName { get; private set; }

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
        public static LoadConfigFailureEventArgs Create(ReadDataFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadConfigFailureEventArgs>();
            eventArgs.ConfigAssetName = e.DataAssetName;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表更新事件
    /// </summary>
    public class LoadConfigUpdateEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigUpdateEventArgs).GetHashCode();

        public LoadConfigUpdateEventArgs()
        {
            ConfigAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string ConfigAssetName { get; private set; }

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
        public static LoadConfigUpdateEventArgs Create(ReadDataUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadConfigUpdateEventArgs>();
            eventArgs.ConfigAssetName = e.DataAssetName;
            eventArgs.Progress = e.Progress;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载数据表更新事件
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 加载数据表依赖资源事件
    /// </summary>
    public class LoadConfigDependencyAssetEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigDependencyAssetEventArgs).GetHashCode();

        public LoadConfigDependencyAssetEventArgs()
        {
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 数据表资源名称
        /// </summary>
        public string ConfigAssetName { get; private set; }

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
        public static LoadConfigDependencyAssetEventArgs Create(ReadDataDependencyAssetEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>();
            eventArgs.ConfigAssetName = e.DataAssetName;
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
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}