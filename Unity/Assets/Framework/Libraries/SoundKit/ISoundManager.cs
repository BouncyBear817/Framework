/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 10:21:34
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 声音管理器接口
    /// </summary>
    public interface ISoundManager
    {
        /// <summary>
        /// 获取声音组数量
        /// </summary>
        int SoundGroupCount { get; }

        /// <summary>
        /// 播放声音成功事件
        /// </summary>
        event EventHandler<PlaySoundSuccessEventArgs> PlaySoundSuccess;

        /// <summary>
        /// 播放声音失败事件
        /// </summary>
        event EventHandler<PlaySoundFailureEventArgs> PlaySoundFailure;

        /// <summary>
        /// 播放声音更新事件
        /// </summary>
        event EventHandler<PlaySoundUpdateEventArgs> PlaySoundUpdate;

        /// <summary>
        /// 播放声音加载依赖资源事件
        /// </summary>
        event EventHandler<PlaySoundDependencyEventArgs> PlaySoundDependency;

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置声音辅助器
        /// </summary>
        /// <param name="soundHelper">声音辅助器</param>
        void SetSoundHelper(ISoundHelper soundHelper);

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns></returns>
        bool HasSoundGroup(string soundGroupName);

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns>指定声音组</returns>
        ISoundGroup GetSoundGroup(string soundGroupName);

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <returns>所有声音组</returns>
        ISoundGroup[] GetAllSoundGroups();

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <param name="results">所有声音组</param>
        void GetAllSoundGroups(List<ISoundGroup> results);

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupHelper">声音组辅助器</param>
        /// <returns>声音组是否增加成功</returns>
        bool AddSoundGroup(string soundGroupName, ISoundGroupHelper soundGroupHelper);

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组内的声音是否避免被同优先级声音替换</param>
        /// <param name="soundGroupMute">声音组是否静音</param>
        /// <param name="soundGroupVolume">声音组音量大小</param>
        /// <param name="soundGroupHelper">声音组辅助器</param>
        /// <returns>声音组是否增加成功</returns>
        bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute,
            float soundGroupVolume, ISoundGroupHelper soundGroupHelper);

        /// <summary>
        /// 增加声音代理辅助器
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundAgentHelper">声音代理辅助器</param>
        void AddSoundAgentHelper(string soundGroupName, ISoundAgentHelper soundAgentHelper);

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <returns>所有正在加载声音的序列编号</returns>
        int[] GetAllLoadingSoundSerialIds();

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号</param>
        void GetAllLoadingSoundSerialIds(List<int> results);

        /// <summary>
        /// 是否正在加载声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <returns>是否正在加载声音</returns>
        bool IsLoadingSound(int serialId);

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="priority">加载声音资源优先级</param>
        /// <param name="soundParams">声音参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>声音序列编号</returns>
        int PlaySound(string soundAssetName, string soundGroupName, int priority = 0, SoundParams soundParams = null,
            object userData = null);

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        /// <returns>是否成功停止播放声音</returns>
        bool StopSound(int serialId, float fadeOutSeconds = 0f);

        /// <summary>
        /// 停止播放所有已加载声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        void StopAllLoadedSounds(float fadeOutSeconds = 0f);

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        void StopAllLoadingSounds();

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        void PauseSound(int serialId, float fadeOutSeconds = 0f);

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        void ResumeSound(int serialId, float fadeInSeconds = 0f);
    }
}