/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/8 10:39:40
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    public sealed partial class UIManager : FrameworkModule, IUIManager
    {
        /// <summary>
        /// 打开界面信息
        /// </summary>
        private sealed class OpenUIFormInfo : IReference
        {
            private int mSerialId;
            private UIGroup mUIGroup;
            private bool mPauseCoveredUIForm;
            private object mUserData;

            public OpenUIFormInfo()
            {
                mSerialId = 0;
                mUIGroup = null;
                mPauseCoveredUIForm = false;
                mUserData = null;
            }

            /// <summary>
            /// 界面序列号
            /// </summary>
            public int SerialId => mSerialId;

            /// <summary>
            /// 界面所属界面组
            /// </summary>
            public UIGroup UIGroup => mUIGroup;

            /// <summary>
            /// 是否暂停被覆盖的界面
            /// </summary>
            public bool PauseCoveredUIForm => mPauseCoveredUIForm;

            /// <summary>
            /// 用户自定义数据
            /// </summary>
            public object UserData => mUserData;

            /// <summary>
            /// 创建打开界面信息
            /// </summary>
            /// <param name="serialId">界面序列号</param>
            /// <param name="uiGroup">界面所属界面组</param>
            /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
            /// <param name="userData">用户自定义数据</param>
            /// <returns>打开界面信息</returns>
            public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                var info = ReferencePool.Acquire<OpenUIFormInfo>();
                info.mSerialId = serialId;
                info.mUIGroup = uiGroup;
                info.mPauseCoveredUIForm = pauseCoveredUIForm;
                info.mUserData = userData;
                return info;
            }

            /// <summary>
            /// 清理打开界面信息
            /// </summary>
            public void Clear()
            {
                mSerialId = 0;
                mUIGroup = null;
                mPauseCoveredUIForm = false;
                mUserData = null;
            }
        }
    }
}