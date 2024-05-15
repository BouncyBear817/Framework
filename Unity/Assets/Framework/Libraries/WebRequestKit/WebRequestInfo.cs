/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 14:27:25
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// Web请求信息
    /// </summary>
    public sealed class WebRequestInfo : IReference
    {
        private string mWebRequestUri;
        private string mTag;
        private int mPriority;
        private byte[] mPostData;
        private object mUserData;

        public WebRequestInfo()
        {
            mWebRequestUri = null;
            mTag = null;
            mPriority = 0;
            mPostData = null;
            mUserData = null;
        }

        /// <summary>
        /// Web请求地址
        /// </summary>
        public string WebRequestUri => mWebRequestUri;

        /// <summary>
        /// 任务标签
        /// </summary>
        public string Tag => mTag;

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// Web请求的数据流
        /// </summary>
        public byte[] PostData => mPostData;

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData
        {
            get => mUserData;
            set => mUserData = value;
        }

        /// <summary>
        /// 创建Web请求信息
        /// </summary>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="tag">任务标签</param>
        /// <param name="priority">任务优先级</param>
        /// <param name="postData">Web请求的数据流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>Web请求信息</returns>
        public static WebRequestInfo Create(string webRequestUri, string tag, int priority, byte[] postData,
            object userData)
        {
            var info = ReferencePool.Acquire<WebRequestInfo>();
            info.mWebRequestUri = webRequestUri;
            info.mTag = tag;
            info.mPriority = priority;
            info.mPostData = postData;
            info.mUserData = userData;
            return info;
        }

        /// <summary>
        /// 清理Web请求信息
        /// </summary>
        public void Clear()
        {
            mWebRequestUri = null;
            mTag = null;
            mPriority = 0;
            mPostData = null;
            mUserData = null;
        }
    }
}