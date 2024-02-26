/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/28 15:00:44
* Description:   读取数据时事件集合
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 读取数据时加载依赖事件参数
    /// </summary>
    public sealed class ReadDataDependencyAssetEventArgs : FrameworkEventArgs
    {
        public ReadDataDependencyAssetEventArgs()
        {
            DataAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = 0;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string DataAssetName { get; private set; }

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
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建读取数据依赖资源事件
        /// </summary>
        /// <param name="dataAssetName">资源名称</param>
        /// <param name="dependencyAssetName">依赖资源名称</param>
        /// <param name="loadedCount">已加载依赖资源数量</param>
        /// <param name="totalCount">总共依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>读取数据依赖资源事件</returns>
        public static ReadDataDependencyAssetEventArgs Create(string dataAssetName, string dependencyAssetName,
            int loadedCount, int totalCount, object userData)
        {
            var eventArgs = ReferencePool.Acquire<ReadDataDependencyAssetEventArgs>();
            eventArgs.DataAssetName = dataAssetName;
            eventArgs.DependencyAssetName = dependencyAssetName;
            eventArgs.LoadedCount = loadedCount;
            eventArgs.TotalCount = totalCount;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理读取数据时加载依赖资源事件
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = 0;
        }
    }

    /// <summary>
    /// 读取数据成功参数
    /// </summary>
    public sealed class ReadDataSuccessEventArgs : FrameworkEventArgs
    {
        public ReadDataSuccessEventArgs()
        {
            DataAssetName = null;
            Duration = 0.0f;
            UserData = null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string DataAssetName { get; private set; }

        /// <summary>
        /// 加载持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建读取数据成功事件
        /// </summary>
        /// <param name="dataAssetName"></param>
        /// <param name="duration"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static ReadDataSuccessEventArgs Create(string dataAssetName, float duration, object userData)
        {
            var eventArgs = ReferencePool.Acquire<ReadDataSuccessEventArgs>();
            eventArgs.DataAssetName = dataAssetName;
            eventArgs.Duration = duration;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理读取数据成功事件
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            Duration = 0.0f;
            UserData = null;
        }
    }

    /// <summary>
    /// 读取数据失败事件
    /// </summary>
    public sealed class ReadDataFailureEventArgs : FrameworkEventArgs
    {
        public ReadDataFailureEventArgs()
        {
            DataAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string DataAssetName { get; private set; }

        /// <summary>
        /// 失败信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建读取数据失败事件
        /// </summary>
        /// <param name="dataAssetName">资源名称</param>
        /// <param name="errorMessage">失败信息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>读取数据失败事件</returns>
        public static ReadDataFailureEventArgs Create(string dataAssetName, string errorMessage, object userData)
        {
            var eventArgs = ReferencePool.Acquire<ReadDataFailureEventArgs>();
            eventArgs.DataAssetName = dataAssetName;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理读取数据失败事件
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 读取数据更新事件
    /// </summary>
    public sealed class ReadDataUpdateEventArgs : FrameworkEventArgs
    {
        public ReadDataUpdateEventArgs()
        {
            DataAssetName = null;
            Progress = 0.0f;
            UserData = null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string DataAssetName { get; private set; }

        /// <summary>
        /// 读取数据进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建读取数据更新事件
        /// </summary>
        /// <param name="dataAssetName">资源名称</param>
        /// <param name="progress">读取数据进度</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        public static ReadDataUpdateEventArgs Create(string dataAssetName, float progress, object userData)
        {
            var eventArgs = ReferencePool.Acquire<ReadDataUpdateEventArgs>();
            eventArgs.DataAssetName = dataAssetName;
            eventArgs.Progress = progress;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理读取数据更新事件
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            Progress = 0.0f;
            UserData = null;
        }
    }
}