/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 10:38:32
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 界面接口
    /// </summary>
    public interface IUIForm
    {
        /// <summary>
        /// 界面序列号
        /// </summary>
        int SerialId { get; }

        /// <summary>
        /// 界面资源名称
        /// </summary>
        string UIFormAssetName { get; }

        /// <summary>
        /// 界面实例
        /// </summary>
        object Handle { get; }

        /// <summary>
        /// 界面所属界面组
        /// </summary>
        IUIGroup UIGroup { get; }

        /// <summary>
        /// 界面在界面组中的深度
        /// </summary>
        int DepthInUIGroup { get; }

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        bool PauseCoveredUIForm { get; }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroup">界面所属界面组</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="isNewInstance">是否为新实例</param>
        /// <param name="userData">用户自定义数据</param>
        void OnInit(int serialId, string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm, bool isNewInstance,
            object userData);

        /// <summary>
        /// 回收界面
        /// </summary>
        void OnRecycle();

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        void OnOpen(object userData);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发</param>
        /// <param name="userData">用户自定义数据</param>
        void OnClose(bool isShutdown, object userData);

        /// <summary>
        /// 暂停界面
        /// </summary>
        void OnPause();

        /// <summary>
        /// 恢复界面
        /// </summary>
        void OnResume();

        /// <summary>
        /// 遮挡界面
        /// </summary>
        void OnCover();

        /// <summary>
        /// 遮挡恢复界面
        /// </summary>
        void OnReveal();

        /// <summary>
        /// 激活界面
        /// </summary>
        /// <param name="userData"></param>
        void OnRefocus(object userData);

        /// <summary>
        /// 界面轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流失时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 界面深度改变
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度</param>
        void OnDepthChanged(int uiGroupDepth, int depthInUIGroup);
    }
}