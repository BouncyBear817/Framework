/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 14:41:20
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 播放声音成功事件
    /// </summary>
    public sealed class PlaySoundSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(PlaySoundSuccessEventArgs).GetHashCode();

        public PlaySoundSuccessEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundAgent = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 声音序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 声音资源名称
        /// </summary>
        public string SoundAssetName { get; private set; }

        /// <summary>
        /// 声音代理
        /// </summary>
        public ISoundAgent SoundAgent { get; private set; }

        /// <summary>
        /// 加载持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建播放声音成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>播放声音成功事件</returns>
        public static PlaySoundSuccessEventArgs Create(Framework.PlaySoundSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundSuccessEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.SoundAssetName = e.SoundAssetName;
            eventArgs.SoundAgent = e.SoundAgent;
            eventArgs.Duration = e.Duration;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理播放声音成功事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundAgent = null;
            Duration = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 播放声音失败事件
    /// </summary>
    public sealed class PlaySoundFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(PlaySoundSuccessEventArgs).GetHashCode();

        public PlaySoundFailureEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
            SoundErrorCode = SoundErrorCode.UnKnown;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 声音序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 声音资源名称
        /// </summary>
        public string SoundAssetName { get; private set; }

        /// <summary>
        /// 声音组名称
        /// </summary>
        public string SoundGroupName { get; private set; }

        /// <summary>
        /// 声音参数
        /// </summary>
        public SoundParams SoundParams { get; private set; }

        /// <summary>
        /// 声音错误码
        /// </summary>
        public SoundErrorCode SoundErrorCode { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建播放声音失败事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns>播放声音失败事件</returns>
        public static PlaySoundFailureEventArgs Create(Framework.PlaySoundFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundFailureEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.SoundAssetName = e.SoundAssetName;
            eventArgs.SoundGroupName = e.SoundGroupName;
            eventArgs.SoundParams = e.SoundParams;
            eventArgs.SoundErrorCode = e.SoundErrorCode;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理播放声音失败事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
            SoundErrorCode = SoundErrorCode.UnKnown;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 播放声音更新事件
    /// </summary>
    public sealed class PlaySoundUpdateEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(PlaySoundSuccessEventArgs).GetHashCode();

        public PlaySoundUpdateEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 声音序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 声音资源名称
        /// </summary>
        public string SoundAssetName { get; private set; }

        /// <summary>
        /// 声音组名称
        /// </summary>
        public string SoundGroupName { get; private set; }

        /// <summary>
        /// 声音参数
        /// </summary>
        public SoundParams SoundParams { get; private set; }

        /// <summary>
        /// 加载声音进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建播放声音更新事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns>播放声音更新事件</returns>
        public static PlaySoundUpdateEventArgs Create(Framework.PlaySoundUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundUpdateEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.SoundAssetName = e.SoundAssetName;
            eventArgs.SoundGroupName = e.SoundGroupName;
            eventArgs.SoundParams = e.SoundParams;
            eventArgs.Progress = e.Progress;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理播放声音更新事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
            Progress = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 播放声音加载依赖资源事件
    /// </summary>
    public sealed class PlaySoundDependencyEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(PlaySoundSuccessEventArgs).GetHashCode();

        public PlaySoundDependencyEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
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
        /// 声音序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 声音资源名称
        /// </summary>
        public string SoundAssetName { get; private set; }

        /// <summary>
        /// 声音组名称
        /// </summary>
        public string SoundGroupName { get; private set; }

        /// <summary>
        /// 声音参数
        /// </summary>
        public SoundParams SoundParams { get; private set; }

        /// <summary>
        /// 加载依赖资源名称
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
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建播放声音加载依赖资源事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns>播放声音加载依赖资源事件</returns>
        public static PlaySoundDependencyEventArgs Create(Framework.PlaySoundDependencyEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundDependencyEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.SoundAssetName = e.SoundAssetName;
            eventArgs.SoundGroupName = e.SoundGroupName;
            eventArgs.SoundParams = e.SoundParams;
            eventArgs.DependencyAssetName = e.DependencyAssetName;
            eventArgs.LoadedCount = e.LoadedCount;
            eventArgs.TotalCount = e.TotalCount;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理播放声音加载依赖资源事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            SoundParams = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}