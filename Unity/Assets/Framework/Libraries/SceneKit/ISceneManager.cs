/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:58:45
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 场景管理器接口
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// 加载场景成功事件
        /// </summary>
        event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess;

        /// <summary>
        /// 加载场景失败事件
        /// </summary>
        event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure;

        /// <summary>
        /// 加载场景更新事件
        /// </summary>
        event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate;

        /// <summary>
        /// 加载场景依赖资源事件
        /// </summary>
        event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset;

        /// <summary>
        /// 卸载场景成功事件
        /// </summary>
        event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess;

        /// <summary>
        /// 卸载场景失败事件
        /// </summary>
        event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure;

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 获取场景是否已经加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否已经加载</returns>
        bool SceneIsLoaded(string sceneAssetName);

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <returns>所有已加载场景名称</returns>
        string[] GetLoadedSceneAssetNames();

        /// <summary>
        /// 获取所有已加载场景名称
        /// </summary>
        /// <param name="results">所有已加载场景名称</param>
        void GetLoadedSceneAssetNames(List<string> results);

        /// <summary>
        /// 获取场景是否正在加载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在加载</returns>
        bool SceneIsLoading(string sceneAssetName);

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <returns>所有正在加载的场景名称</returns>
        string[] GetLoadingSceneAssetNames();

        /// <summary>
        /// 获取所有正在加载的场景名称
        /// </summary>
        /// <param name="results">所有正在加载的场景名称</param>
        void GetLoadingSceneAssetNames(List<string> results);

        /// <summary>
        /// 获取场景是否正在卸载
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景是否正在卸载</returns>
        bool SceneIsUnloading(string sceneAssetName);

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <returns>所有正在卸载场景的名称</returns>
        string[] GetUnloadingSceneAssetNames();

        /// <summary>
        /// 获取所有正在卸载场景的名称
        /// </summary>
        /// <param name="results">所有正在卸载场景的名称</param>
        void GetUnloadingSceneAssetNames(List<string> results);

        /// <summary>
        /// 检查场景资源是否存在
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <returns>场景资源是否存在</returns>
        bool HasScene(string sceneAssetName);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="priority">场景优先级</param>
        /// <param name="userData">自定义数据</param>
        void LoadScene(string sceneAssetName, int priority = 0, object userData = null);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="userData">自定义数据</param>
        void UnloadScene(string sceneAssetName, object userData = null);
    }
}