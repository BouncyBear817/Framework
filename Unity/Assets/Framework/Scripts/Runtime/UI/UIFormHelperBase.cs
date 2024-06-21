/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 17:24:2
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 界面辅助器基类
    /// </summary>
    public abstract class UIFormHelperBase : MonoBehaviour, IUIFormHelper
    {
        /// <summary>
        /// 实例化界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <returns>实例化后的界面</returns>
        public abstract object InstantiateUIForm(object uiFormAsset);

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="uiGroup">界面所属的界面组</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>创建后的界面</returns>
        public abstract IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData);

        /// <summary>
        /// 释放界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <param name="uiFormInstance">界面实例</param>
        public abstract void ReleaseUIForm(object uiFormAsset, object uiFormInstance);
    }
}