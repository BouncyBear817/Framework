/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/8 10:40:23
 * Description:
 * Modify Record:
 *************************************************************/

using System.Collections.Generic;

namespace Framework
{
    public sealed partial class UIManager : FrameworkModule, IUIManager
    {
        private sealed partial class UIGroup : IUIGroup
        {
            /// <summary>
            /// 界面组内界面信息
            /// </summary>
            private sealed class UIFormInfo : IReference
            {
                private IUIForm mUIForm;
                private bool mPaused;
                private bool mCovered;

                public UIFormInfo()
                {
                    mUIForm = null;
                    mPaused = false;
                    mCovered = false;
                }
                
                /// <summary>
                /// 界面
                /// </summary>
                public IUIForm UIForm => mUIForm;

                /// <summary>
                /// 是否暂停
                /// </summary>
                public bool Paused
                {
                    get => mPaused;
                    set => mPaused = value;
                }

                /// <summary>
                /// 是否覆盖
                /// </summary>
                public bool Covered
                {
                    get => mCovered;
                    set => mCovered = value;
                }

                /// <summary>
                /// 创建界面信息
                /// </summary>
                /// <param name="uiForm">界面</param>
                /// <returns>界面信息</returns>
                public static UIFormInfo Create(IUIForm uiForm)
                {
                    var uiFormInfo = ReferencePool.Acquire<UIFormInfo>();
                    uiFormInfo.mUIForm = uiForm;
                    uiFormInfo.mPaused = true;
                    uiFormInfo.mCovered = true;
                    return uiFormInfo;
                }

                /// <summary>
                /// 清理界面信息
                /// </summary>
                public void Clear()
                {
                    mUIForm = null;
                    mPaused = false;
                    mCovered = false;
                }
            }
        }
    }
}