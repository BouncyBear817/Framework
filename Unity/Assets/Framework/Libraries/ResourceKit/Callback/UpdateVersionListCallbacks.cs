/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 更新版本资源列表成功回调函数
    /// </summary>
    public delegate void UpdateVersionListSuccessCallback(string downloadPath, string downloadUri);

    /// <summary>
    /// 更新版本资源列表失败回调函数
    /// </summary>
    public delegate void UpdateVersionListFailureCallback(string downloadUrl, string errorMessage);

    /// <summary>
    /// 更新版本资源列表回调函数集
    /// </summary>
    public class UpdateVersionListCallbacks
    {
        private readonly UpdateVersionListSuccessCallback mUpdateVersionListSuccessCallback;
        private readonly UpdateVersionListFailureCallback mUpdateVersionListFailureCallback;

        public UpdateVersionListCallbacks(UpdateVersionListSuccessCallback updateVersionListSuccessCallback,
            UpdateVersionListFailureCallback updateVersionListFailureCallback = null)
        {
            mUpdateVersionListSuccessCallback = updateVersionListSuccessCallback ??
                                                throw new Exception("Update version list success callback is invalid.");
            mUpdateVersionListFailureCallback = updateVersionListFailureCallback;
        }

        /// <summary>
        /// 更新版本资源列表成功回调函数
        /// </summary>
        public UpdateVersionListSuccessCallback UpdateVersionListSuccessCallback => mUpdateVersionListSuccessCallback;

        /// <summary>
        /// 更新版本资源列表失败回调函数
        /// </summary>
        public UpdateVersionListFailureCallback UpdateVersionListFailureCallback => mUpdateVersionListFailureCallback;
    }
}