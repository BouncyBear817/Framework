/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/10 10:3:13
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 默认界面辅助器
    /// </summary>
    public class DefaultUIFormHelper : UIFormHelperBase
    {
        //TODO: need resource component

        /// <summary>
        /// 实例化界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <returns>实例化后的界面</returns>
        public override object InstantiateUIForm(object uiFormAsset)
        {
            return Instantiate(uiFormAsset as Object);
        }

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="uiFormInstance">界面实例</param>
        /// <param name="uiGroup">界面所属的界面组</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>创建后的界面</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            var gameObj = uiFormInstance as GameObject;
            if (gameObj == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            var trans = gameObj.transform;
            trans.SetParent(((MonoBehaviour)uiGroup.GroupHelper).transform);
            trans.localScale = Vector3.one;

            return gameObj.GetOrAddComponent<UIForm>();
        }

        /// <summary>
        /// 释放界面
        /// </summary>
        /// <param name="uiFormAsset">界面资源</param>
        /// <param name="uiFormInstance">界面实例</param>
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            //TODO need resource manager

            Destroy(uiFormInstance as Object);
        }
    }
}