/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/29 16:18:55
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 加载二进制资源成功回调函数
    /// </summary>
    public delegate void LoadBinarySuccessCallback(string binaryAssetName, byte[] binaryBytes, float duration,
        object userData);

    /// <summary>
    /// 加载二进制资源失败回调函数
    /// </summary>
    public delegate void LoadBinaryFailureCallback(string binaryAssetName, LoadResourceStatus status,
        string errorMessage, object userData);

    /// <summary>
    /// 加载二进制资源回调函数集
    /// </summary>
    public sealed class LoadBinaryCallbacks
    {
        private readonly LoadBinarySuccessCallback mLoadBinarySuccessCallback;
        private readonly LoadBinaryFailureCallback mLoadBinaryFailureCallback;

        public LoadBinaryCallbacks(LoadBinarySuccessCallback loadBinarySuccessCallback,
            LoadBinaryFailureCallback loadBinaryFailureCallback = null)
        {
            mLoadBinarySuccessCallback = loadBinarySuccessCallback ??
                                         throw new Exception("Load binary Asset callback is invalid.");
            mLoadBinaryFailureCallback = loadBinaryFailureCallback;
        }

        /// <summary>
        /// 加载二进制资源成功回调函数
        /// </summary>
        public LoadBinarySuccessCallback LoadBinarySuccessCallback => mLoadBinarySuccessCallback;

        /// <summary>
        /// 加载二进制资源失败回调函数
        /// </summary>
        public LoadBinaryFailureCallback LoadBinaryFailureCallback => mLoadBinaryFailureCallback;
    }
}