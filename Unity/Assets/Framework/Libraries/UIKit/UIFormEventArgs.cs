/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 15:37:22
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 打开界面成功事件
    /// </summary>
    public class OpenUIFormSuccessEventArgs : FrameworkEventArgs
    {
        public OpenUIFormSuccessEventArgs()
        {
            UIForm = null;
            Duration = 0f;
            UserData = null;
        }

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
        /// <param name="uiForm">界面</param>
        /// <param name="duration">加载持续时间</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>打开界面成功事件</returns>
        public static OpenUIFormSuccessEventArgs Create(IUIForm uiForm, float duration, object userData)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormSuccessEventArgs>();
            eventArgs.UIForm = uiForm;
            eventArgs.Duration = duration;
            eventArgs.UserData = userData;
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
    public class OpenUIFormFailureEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>打开界面失败事件</returns>
        public static OpenUIFormFailureEventArgs Create(int serialId, string uiFormAssetName, string uiGroupName,
            bool pauseCoveredUIForm, string errorMessage, object userData)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormFailureEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.UIFormAssetName = uiFormAssetName;
            eventArgs.UIGroupName = uiGroupName;
            eventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
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
    public class OpenUIFormUpdateEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="progress">打开界面进度</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>打开界面更新事件</returns>
        public static OpenUIFormUpdateEventArgs Create(int serialId, string uiFormAssetName, string uiGroupName,
            bool pauseCoveredUIForm, float progress, object userData)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormUpdateEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.UIFormAssetName = uiFormAssetName;
            eventArgs.UIGroupName = uiGroupName;
            eventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
            eventArgs.Progress = progress;
            eventArgs.UserData = userData;
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
    public class OpenUIFormDependencyAssetEventArgs : FrameworkEventArgs
    {
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
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="loadedCount">当前已加载依赖资源数量</param>
        /// <param name="totalCount">总共加载依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>打开界面加载依赖事件</returns>
        public static OpenUIFormDependencyAssetEventArgs Create(int serialId, string uiFormAssetName,
            string uiGroupName,
            bool pauseCoveredUIForm, int loadedCount, int totalCount, object userData)
        {
            var eventArgs = ReferencePool.Acquire<OpenUIFormDependencyAssetEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.UIFormAssetName = uiFormAssetName;
            eventArgs.UIGroupName = uiGroupName;
            eventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
            eventArgs.LoadedCount = loadedCount;
            eventArgs.TotalCount = totalCount;
            eventArgs.UserData = userData;
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
    public class CloseUIFormCompleteEventArgs : FrameworkEventArgs
    {
        public CloseUIFormCompleteEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroup = null;
            UserData = null;
        }

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
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroup">界面所属界面组</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>关闭界面完成事件</returns>
        public static CloseUIFormCompleteEventArgs Create(int serialId, string uiFormAssetName,
            IUIGroup uiGroup, object userData)
        {
            var eventArgs = ReferencePool.Acquire<CloseUIFormCompleteEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.UIFormAssetName = uiFormAssetName;
            eventArgs.UIGroup = uiGroup;
            eventArgs.UserData = userData;
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