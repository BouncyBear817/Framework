/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/7 15:25:19
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 界面接口辅助器
    /// </summary>
    public interface IUIFormHelper
    {
        /// <summary>
        /// 实例化界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <returns>实例化后的界面</returns>
        object InstantiateUIForm(object uiFormAsset);

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="uiGroup">界面所属的界面组</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>创建后的界面</returns>
        IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData);

        /// <summary>
        /// 释放界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <param name="uiFormInstance">界面实例</param>
        void ReleaseUIForm(object uiFormAsset, object uiFormInstance);
    }
}