/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/8 10:39:55
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    public sealed partial class UIManager : FrameworkModule, IUIManager
    {
        /// <summary>
        /// 界面实例对象
        /// </summary>
        private sealed class UIFormInstanceObject : ObjectBase
        {
            private object mUIFormAsset;
            private IUIFormHelper mUIFormHelper;

            public UIFormInstanceObject()
            {
                mUIFormAsset = null;
                mUIFormHelper = null;
            }

            /// <summary>
            /// 创建界面实例对象
            /// </summary>
            /// <param name="name">界面实例对象名称</param>
            /// <param name="uiFormAsset">界面资源</param>
            /// <param name="uiFormInstance">界面实例</param>
            /// <param name="uiFormHelper">界面辅助器</param>
            /// <returns>界面实例对象</returns>
            /// <exception cref="Exception"></exception>
            public static UIFormInstanceObject Create(string name, object uiFormAsset, object uiFormInstance,
                IUIFormHelper uiFormHelper)
            {
                if (uiFormAsset == null)
                {
                    throw new Exception("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new Exception("UI form helper is invalid.");
                }

                var instanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
                instanceObject.Initialize(name, uiFormInstance);
                instanceObject.mUIFormAsset = uiFormAsset;
                instanceObject.mUIFormHelper = uiFormHelper;
                return instanceObject;
            }

            /// <summary>
            /// 清理对象基类
            /// </summary>
            public override void Clear()
            {
                base.Clear();
                mUIFormAsset = null;
                mUIFormHelper = null;
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="isShutdown">是否是关闭对象池时触发</param>
            protected internal override void Release(bool isShutdown)
            {
                mUIFormHelper.ReleaseUIForm(mUIFormAsset, Target);
            }
        }
    }
}