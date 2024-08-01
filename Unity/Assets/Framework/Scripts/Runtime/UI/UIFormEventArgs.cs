/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 16:3:54
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 打开界面成功事件
    /// </summary>
    public class OpenUIFormSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(OpenUIFormSuccessEventArgs).GetHashCode();

        public OpenUIFormSuccessEventArgs()
        {
            UIForm = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 界面
        /// </summary>
        public IUIForm UIForm { get; private set; }

        /// <summary>
        /// 加载持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建打开界面成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>打开界面成功事件</returns>
        public static OpenUIFormSuccessEventArgs Create(Framework.OpenUIFormSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormSuccessEventArgs>();
            eventArgs.UIForm = e.UIForm;
            eventArgs.Duration = e.Duration;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理打开界面成功事件
        /// </summary>
        public override void Clear()
        {
            UIForm = null;
            Duration = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 打开界面失败事件
    /// </summary>
    public class OpenUIFormFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(OpenUIFormFailureEventArgs).GetHashCode();

        public OpenUIFormFailureEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 界面序列号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 界面资源名称
        /// </summary>
        public string UIFormAssetName { get; private set; }

        /// <summary>
        /// 界面组名称
        /// </summary>
        public string UIGroupName { get; private set; }

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建打开界面失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>打开界面失败事件</returns>
        public static OpenUIFormFailureEventArgs Create(Framework.OpenUIFormFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormFailureEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.UIFormAssetName = e.UIFormAssetName;
            eventArgs.UIGroupName = e.UIGroupName;
            eventArgs.PauseCoveredUIForm = e.PauseCoveredUIForm;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理打开界面失败事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 打开界面更新事件
    /// </summary>
    public class OpenUIFormUpdateEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(OpenUIFormUpdateEventArgs).GetHashCode();

        public OpenUIFormUpdateEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 界面序列号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 界面资源名称
        /// </summary>
        public string UIFormAssetName { get; private set; }

        /// <summary>
        /// 界面组名称
        /// </summary>
        public string UIGroupName { get; private set; }

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm { get; private set; }

        /// <summary>
        /// 打开界面进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建打开界面更新事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>打开界面更新事件</returns>
        public static OpenUIFormUpdateEventArgs Create(Framework.OpenUIFormUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormUpdateEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.UIFormAssetName = e.UIFormAssetName;
            eventArgs.UIGroupName = e.UIGroupName;
            eventArgs.PauseCoveredUIForm = e.PauseCoveredUIForm;
            eventArgs.Progress = e.Progress;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理打开界面更新事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            Progress = 0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 打开界面加载依赖事件
    /// </summary>
    public class OpenUIFormDependencyAssetEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(OpenUIFormDependencyAssetEventArgs).GetHashCode();

        public OpenUIFormDependencyAssetEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 界面序列号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 界面资源名称
        /// </summary>
        public string UIFormAssetName { get; private set; }

        /// <summary>
        /// 界面组名称
        /// </summary>
        public string UIGroupName { get; private set; }

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm { get; private set; }

        /// <summary>
        /// 当前已加载依赖资源数量
        /// </summary>
        public int LoadedCount { get; private set; }

        /// <summary>
        /// 总共加载依赖资源数量
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建打开界面加载依赖事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>打开界面加载依赖事件</returns>
        public static OpenUIFormDependencyAssetEventArgs Create(Framework.OpenUIFormDependencyAssetEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormDependencyAssetEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.UIFormAssetName = e.UIFormAssetName;
            eventArgs.UIGroupName = e.UIGroupName;
            eventArgs.PauseCoveredUIForm = e.PauseCoveredUIForm;
            eventArgs.LoadedCount = e.LoadedCount;
            eventArgs.TotalCount = e.TotalCount;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理打开界面加载依赖事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }

    /// <summary>
    /// 关闭界面完成事件
    /// </summary>
    public class CloseUIFormCompleteEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(CloseUIFormCompleteEventArgs).GetHashCode();

        public CloseUIFormCompleteEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroup = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 界面序列号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 界面资源名称
        /// </summary>
        public string UIFormAssetName { get; private set; }

        /// <summary>
        /// 界面所属界面组
        /// </summary>
        public IUIGroup UIGroup { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建关闭界面完成事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>关闭界面完成事件</returns>
        public static CloseUIFormCompleteEventArgs Create(Framework.CloseUIFormCompleteEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<CloseUIFormCompleteEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.UIFormAssetName = e.UIFormAssetName;
            eventArgs.UIGroup = e.UIGroup;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理关闭界面完成事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroup = null;
            UserData = null;
        }
    }
}