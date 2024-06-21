// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 14:30:51
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认资源辅助器
    /// </summary>
    public sealed class DefaultResourceHelper : ResourceHelperBase
    {
        /// <summary>
        /// 从指定路径加载数据流
        /// </summary>
        /// <param name="fileUri">文件路径</param>
        /// <param name="loadBytesCallbacks">加载数据流回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public override void LoadBytes(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            StartCoroutine(LoadBytesCo(fileUri, loadBytesCallbacks, userData));
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneAssetName));
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="objectToRelease">待释放的资源</param>
        public override void Release(object objectToRelease)
        {
            var assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
            }
        }

        private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            var startTime = DateTime.UtcNow;

            var unityWebRequest = UnityWebRequest.Get(fileUri);
            yield return unityWebRequest.SendWebRequest();

            var isError = unityWebRequest.result != UnityWebRequest.Result.Success;

            var bytes = unityWebRequest.downloadHandler.data;
            var errorMessage = isError ? unityWebRequest.error : null;
            unityWebRequest.Dispose();

            if (!isError)
            {
                var elapseSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                loadBytesCallbacks?.LoadBytesSuccessCallback?.Invoke(fileUri, bytes, elapseSeconds, userData);
            }
            else
            {
                loadBytesCallbacks?.LoadBytesFailureCallback?.Invoke(fileUri, errorMessage, userData);
            }
        }

        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            var asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
            {
                unloadSceneCallbacks?.UnloadSceneSuccessCallback?.Invoke(sceneAssetName, userData);
            }
            else
            {
                unloadSceneCallbacks?.UnloadSceneFailureCallback?.Invoke(sceneAssetName, userData);
            }
        }
    }
}