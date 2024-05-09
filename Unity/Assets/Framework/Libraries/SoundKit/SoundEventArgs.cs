/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 10:22:6
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 播放声音成功事件
    /// </summary>
    public sealed class PlaySoundSuccessEventArgs : FrameworkEventArgs
    {
        public PlaySoundSuccessEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundAgent = null;
            Duration = 0f;
            UserData = null;
        }

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
        /// <param name="serialId">声音序列编号</param>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundAgent">声音代理</param>
        /// <param name="duration">加载持续时间</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>播放声音成功事件</returns>
        public static PlaySoundSuccessEventArgs Create(int serialId, string soundAssetName, ISoundAgent soundAgent,
            float duration, object userData)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundSuccessEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.SoundAssetName = soundAssetName;
            eventArgs.SoundAgent = soundAgent;
            eventArgs.Duration = duration;
            eventArgs.UserData = userData;
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
    public sealed class PlaySoundFailureEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">声音序列编号</param>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundParams">声音参数</param>
        /// <param name="soundErrorCode">声音错误码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>播放声音失败事件</returns>
        public static PlaySoundFailureEventArgs Create(int serialId, string soundAssetName, string soundGroupName,
            SoundParams soundParams, SoundErrorCode soundErrorCode, string errorMessage, object userData)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundFailureEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.SoundAssetName = soundAssetName;
            eventArgs.SoundGroupName = soundGroupName;
            eventArgs.SoundParams = soundParams;
            eventArgs.SoundErrorCode = soundErrorCode;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
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
    public sealed class PlaySoundUpdateEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">声音序列编号</param>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundParams">声音组名称</param>
        /// <param name="progress">加载声音进度</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>播放声音更新事件</returns>
        public static PlaySoundUpdateEventArgs Create(int serialId, string soundAssetName, string soundGroupName,
            SoundParams soundParams, float progress, object userData)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundUpdateEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.SoundAssetName = soundAssetName;
            eventArgs.SoundGroupName = soundGroupName;
            eventArgs.SoundParams = soundParams;
            eventArgs.Progress = progress;
            eventArgs.UserData = userData;
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
    public sealed class PlaySoundDependencyEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">声音序列编号</param>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundParams">声音组名称</param>
        /// <param name="dependencyAssetName">加载依赖资源名称</param>
        /// <param name="loadedCount">已加载依赖资源数量</param>
        /// <param name="totalCount">总共依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>播放声音加载依赖资源事件</returns>
        public static PlaySoundDependencyEventArgs Create(int serialId, string soundAssetName, string soundGroupName,
            SoundParams soundParams, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            var eventArgs = ReferencePool.Acquire<PlaySoundDependencyEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.SoundAssetName = soundAssetName;
            eventArgs.SoundGroupName = soundGroupName;
            eventArgs.SoundParams = soundParams;
            eventArgs.DependencyAssetName = dependencyAssetName;
            eventArgs.LoadedCount = loadedCount;
            eventArgs.TotalCount = totalCount;
            eventArgs.UserData = userData;
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

    /// <summary>
    /// 重置声音代理事件
    /// </summary>
    public sealed class ResetSoundAgentEventArgs : FrameworkEventArgs
    {
        public ResetSoundAgentEventArgs()
        {
        }

        /// <summary>
        /// 创建重置声音代理事件
        /// </summary>
        /// <returns>重置声音代理事件</returns>
        public static ResetSoundAgentEventArgs Create()
        {
            return ReferencePool.Acquire<ResetSoundAgentEventArgs>();
        }

        /// <summary>
        /// 清理重置声音代理事件
        /// </summary>
        public override void Clear()
        {
        }
    }
}