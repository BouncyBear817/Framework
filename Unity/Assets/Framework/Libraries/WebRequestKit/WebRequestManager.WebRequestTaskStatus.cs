// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/13 15:18:58
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class WebRequestManager : FrameworkModule, IWebRequestManager
    {
        /// <summary>
        /// Web请求任务状态
        /// </summary>
        private enum WebRequestTaskStatus : byte
        {
            /// <summary>
            /// 准备请求
            /// </summary>
            Todo = 0,

            /// <summary>
            /// 正在请求
            /// </summary>
            Doing,

            /// <summary>
            /// 请求完成
            /// </summary>
            Done,

            /// <summary>
            /// 请求错误
            /// </summary>
            Error
        }
    }
}