/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 17:25:32
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 界面组辅助器基类
    /// </summary>
    public abstract class UIGroupHelperBase : MonoBehaviour, IUIGroupHelper
    {
        /// <summary>
        /// 设置界面组深度
        /// </summary>
        /// <param name="depth">界面组深度</param>
        public abstract void SetDepth(int depth);
    }
}