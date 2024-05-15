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
        /// Web请求任务
        /// </summary>
        private sealed class WebRequestTask : TaskBase
        {
            private static int sSerialId = 0;

            private WebRequestTaskStatus mTaskStatus;
            private string mWebRequestUri;
            private byte[] mPostData;
            private float mTimeout;

            public WebRequestTask()
            {
                mTaskStatus = WebRequestTaskStatus.Todo;
                mWebRequestUri = null;
                mPostData = null;
                mTimeout = 0f;
            }

            /// <summary>
            /// Web请求任务状态
            /// </summary>
            public WebRequestTaskStatus TaskStatus
            {
                get => mTaskStatus;
                set => mTaskStatus = value;
            }

            /// <summary>
            /// Web请求地址
            /// </summary>
            public string WebRequestUri => mWebRequestUri;

            /// <summary>
            /// Web请求的数据流
            /// </summary>
            public byte[] PostData => mPostData;

            /// <summary>
            /// Web请求的超时时间
            /// </summary>
            public float Timeout => mTimeout;

            /// <summary>
            /// 任务描述
            /// </summary>
            public override string Description => mWebRequestUri;

            /// <summary>
            /// 创建Web请求任务
            /// </summary>
            /// <param name="webRequestInfo">Web请求信息</param>
            /// <param name="timeout">Web请求的超时时间</param>
            /// <returns>Web请求任务</returns>
            public static WebRequestTask Create(WebRequestInfo webRequestInfo, float timeout)
            {
                var task = ReferencePool.Acquire<WebRequestTask>();
                task.Initialize(++sSerialId, webRequestInfo.Tag, webRequestInfo.Priority, webRequestInfo.UserData);
                task.mWebRequestUri = webRequestInfo.WebRequestUri;
                task.mPostData = webRequestInfo.PostData;
                task.mTimeout = timeout;

                ReferencePool.Release(webRequestInfo);
                return task;
            }

            /// <summary>
            /// 清理Web请求任务
            /// </summary>
            public override void Clear()
            {
                base.Clear();
                mTaskStatus = WebRequestTaskStatus.Todo;
                mWebRequestUri = null;
                mPostData = null;
                mTimeout = 0f;
            }
        }
    }
}