/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 10:40:8
 * Description:
 * Modify Record:
 *************************************************************/

using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 界面组接口
    /// </summary>
    public interface IUIGroup
    {
        /// <summary>
        /// 界面组名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 界面组深度
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// 界面组是否暂停
        /// </summary>
        bool Pause { get; set; }

        /// <summary>
        /// 界面组中界面数量
        /// </summary>
        int UIFormCount { get; }

        /// <summary>
        /// 界面组中当前界面
        /// </summary>
        IUIForm CurrentUIForm { get; }

        /// <summary>
        /// 界面组辅助器
        /// </summary>
        IUIGroupHelper GroupHelper { get; }

        /// <summary>
        /// 界面组中是否存在界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>界面组中是否存在界面</returns>
        bool HasUIForm(int serialId);

        /// <summary>
        /// 界面组中是否存在界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面组中是否存在界面</returns>
        bool HasUIForm(string uiFormAssetName);

        /// <summary>
        /// 获取界面组中的界面
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <returns>界面组中的界面</returns>
        IUIForm GetUIForm(int serialId);

        /// <summary>
        /// 获取界面组中的界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面组中的界面</returns>
        IUIForm GetUIForm(string uiFormAssetName);

        /// <summary>
        /// 获取界面组中的界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>界面组中的界面</returns>
        IUIForm[] GetUIForms(string uiFormAssetName);

        /// <summary>
        /// 获取界面组中的界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="results">界面组中的界面</param>
        void GetUIForms(string uiFormAssetName, List<IUIForm> results);

        /// <summary>
        /// 获取界面组中的所有界面
        /// </summary>
        /// <returns>界面组中的所有界面</returns>
        IUIForm[] GetAllUIForms();

        /// <summary>
        /// 获取界面组中的所有界面
        /// </summary>
        /// <param name="results">界面组中的所有界面</param>
        void GetAllUIForms(List<IUIForm> results);
    }
}