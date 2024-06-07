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
    /// 加载数据流成功回调函数
    /// </summary>
    public delegate void LoadBytesSuccessCallback(string fileUri, byte[] bytes, float duration, object userData);

    /// <summary>
    /// 加载数据流失败回调函数
    /// </summary>
    public delegate void LoadBytesFailureCallback(string fileUri, string errorMessage, object userData);

    /// <summary>
    /// 加载数据流回调函数集
    /// </summary>
    public sealed class LoadBytesCallbacks
    {
        private readonly LoadBytesSuccessCallback mLoadBytesSuccessCallback;
        private readonly LoadBytesFailureCallback mLoadBytesFailureCallback;

        public LoadBytesCallbacks(LoadBytesSuccessCallback loadBytesSuccessCallback,
            LoadBytesFailureCallback loadBytesFailureCallback = null)
        {
            mLoadBytesSuccessCallback = loadBytesSuccessCallback ??
                                        throw new Exception("Load bytes success callback is invalid.");
            mLoadBytesFailureCallback = loadBytesFailureCallback;
        }

        /// <summary>
        /// 加载数据流成功回调函数
        /// </summary>
        public LoadBytesSuccessCallback LoadBytesSuccessCallback => mLoadBytesSuccessCallback;

        /// <summary>
        /// 加载数据流失败回调函数
        /// </summary>
        public LoadBytesFailureCallback LoadBytesFailureCallback => mLoadBytesFailureCallback;
    }
}