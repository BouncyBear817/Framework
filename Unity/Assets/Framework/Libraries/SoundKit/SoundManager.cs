/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/9 11:17:13
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 声音管理器
    /// </summary>
    public sealed partial class SoundManager : FrameworkModule, ISoundManager
    {
        private readonly Dictionary<string, SoundGroup> mSoundGroups;
        private readonly List<int> mSoundsBeingLoaded;
        private readonly HashSet<int> mSoundsToReleaseOnLoad;
        private readonly LoadAssetCallbacks mLoadAssetCallbacks;

        private IResourceManager mResourceManager;
        private ISoundHelper mSoundHelper;
        private int mSerial;
        private EventHandler<PlaySoundSuccessEventArgs> mPlaySoundSuccessEventHandler;
        private EventHandler<PlaySoundFailureEventArgs> mPlaySoundFailureEventHandler;
        private EventHandler<PlaySoundUpdateEventArgs> mPlaySoundUpdateEventHandler;
        private EventHandler<PlaySoundDependencyEventArgs> mPlaySoundDependencyEventHandler;

        public SoundManager()
        {
            mSoundGroups = new Dictionary<string, SoundGroup>();
            mSoundsBeingLoaded = new List<int>();
            mSoundsToReleaseOnLoad = new HashSet<int>();
            mLoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback,
                LoadAssetUpdateCallback, LoadAssetDependencyCallback);

            mResourceManager = null;
            mSoundHelper = null;
            mSerial = 0;
            mPlaySoundSuccessEventHandler = null;
            mPlaySoundFailureEventHandler = null;
            mPlaySoundUpdateEventHandler = null;
            mPlaySoundDependencyEventHandler = null;
        }

        /// <summary>
        /// 获取声音组数量
        /// </summary>
        public int SoundGroupCount => mSoundGroups.Count;

        /// <summary>
        /// 播放声音成功事件
        /// </summary>
        public event EventHandler<PlaySoundSuccessEventArgs> PlaySoundSuccess
        {
            add => mPlaySoundSuccessEventHandler += value;
            remove => mPlaySoundSuccessEventHandler -= value;
        }

        /// <summary>
        /// 播放声音失败事件
        /// </summary>
        public event EventHandler<PlaySoundFailureEventArgs> PlaySoundFailure
        {
            add => mPlaySoundFailureEventHandler += value;
            remove => mPlaySoundFailureEventHandler -= value;
        }

        /// <summary>
        /// 播放声音更新事件
        /// </summary>
        public event EventHandler<PlaySoundUpdateEventArgs> PlaySoundUpdate
        {
            add => mPlaySoundUpdateEventHandler += value;
            remove => mPlaySoundUpdateEventHandler -= value;
        }

        /// <summary>
        /// 播放声音加载依赖资源事件
        /// </summary>
        public event EventHandler<PlaySoundDependencyEventArgs> PlaySoundDependency
        {
            add => mPlaySoundDependencyEventHandler += value;
            remove => mPlaySoundDependencyEventHandler -= value;
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            StopAllLoadedSounds();
            mSoundGroups.Clear();
            mSoundsBeingLoaded.Clear();
            mSoundsToReleaseOnLoad.Clear();
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            mResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置声音辅助器
        /// </summary>
        /// <param name="soundHelper">声音辅助器</param>
        public void SetSoundHelper(ISoundHelper soundHelper)
        {
            if (soundHelper == null)
            {
                throw new Exception("Sound helper is invalid.");
            }

            mSoundHelper = soundHelper;
        }

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns>是否存在指定声音组</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new Exception("Sound group name is invalid.");
            }

            return mSoundGroups.ContainsKey(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns>指定声音组</returns>
        public ISoundGroup GetSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new Exception("Sound group name is invalid.");
            }

            return mSoundGroups.GetValueOrDefault(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <returns>所有声音组</returns>
        public ISoundGroup[] GetAllSoundGroups()
        {
            var index = 0;
            var soundGroups = new ISoundGroup[mSoundGroups.Count];
            foreach (var (_, soundGroup) in mSoundGroups)
            {
                soundGroups[index++] = soundGroup;
            }

            return soundGroups;
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <param name="results">所有声音组</param>
        public void GetAllSoundGroups(List<ISoundGroup> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, soundGroup) in mSoundGroups)
            {
                results.Add(soundGroup);
            }
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupHelper">声音组辅助器</param>
        /// <returns>声音组是否增加成功</returns>
        public bool AddSoundGroup(string soundGroupName, ISoundGroupHelper soundGroupHelper)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume,
                soundGroupHelper);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组内的声音是否避免被同优先级声音替换</param>
        /// <param name="soundGroupMute">声音组是否静音</param>
        /// <param name="soundGroupVolume">声音组音量大小</param>
        /// <param name="soundGroupHelper">声音组辅助器</param>
        /// <returns>声音组是否增加成功</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority,
            bool soundGroupMute,
            float soundGroupVolume, ISoundGroupHelper soundGroupHelper)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new Exception("Sound group name is invalid.");
            }

            if (soundGroupHelper == null)
            {
                throw new Exception("Sound group helper is invalid.");
            }

            if (HasSoundGroup(soundGroupName))
            {
                return false;
            }

            var soundGroup = new SoundGroup(soundGroupName, soundGroupHelper)
            {
                AvoidBeingReplacedBySamePriority = soundGroupAvoidBeingReplacedBySamePriority,
                Mute = soundGroupMute,
                Volume = soundGroupVolume
            };
            mSoundGroups.Add(soundGroupName, soundGroup);
            return true;
        }

        /// <summary>
        /// 增加声音代理辅助器
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundAgentHelper">声音代理辅助器</param>
        public void AddSoundAgentHelper(string soundGroupName, ISoundAgentHelper soundAgentHelper)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new Exception("Sound group name is invalid.");
            }

            if (soundAgentHelper == null)
            {
                throw new Exception("Sound agent helper is invalid.");
            }

            var soundGroup = GetSoundGroup(soundGroupName) as SoundGroup;
            if (soundGroup == null)
            {
                throw new Exception("Sound group is invalid.");
            }

            soundGroup.AddSoundAgentHelper(mSoundHelper, soundAgentHelper);
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <returns>所有正在加载声音的序列编号</returns>
        public int[] GetAllLoadingSoundSerialIds()
        {
            return mSoundsBeingLoaded.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号</param>
        public void GetAllLoadingSoundSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            results.AddRange(mSoundsBeingLoaded);
        }

        /// <summary>
        /// 是否正在加载声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <returns>是否正在加载声音</returns>
        public bool IsLoadingSound(int serialId)
        {
            return mSoundsBeingLoaded.Contains(serialId);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="priority">加载声音资源优先级</param>
        /// <param name="soundParams">声音参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>声音序列编号</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority = 0,
            SoundParams soundParams = null,
            object userData = null)
        {
            if (mResourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            if (mSoundHelper == null)
            {
                throw new Exception("Sound helper is invalid.");
            }

            if (soundParams == null)
            {
                throw new Exception("Sound params is invalid.");
            }

            var serialId = ++mSerial;
            SoundErrorCode? errorCode = null;
            string errorMessage = null;
            var soundGroup = GetSoundGroup(soundGroupName) as SoundGroup;
            if (soundGroup == null)
            {
                errorCode = SoundErrorCode.SoundGroupNotExist;
                errorMessage = $"Sound group ({soundGroupName}) is not exist.";
            }
            else if (soundGroup.SoundAgentCount == 0)
            {
                errorCode = SoundErrorCode.SoundGroupHasNoAgent;
                errorMessage = $"Sound group ({soundGroupName})  have no sound agent.";
            }

            if (errorCode.HasValue)
            {
                if (mPlaySoundFailureEventHandler != null)
                {
                    var eventArgs = PlaySoundFailureEventArgs.Create(serialId, soundAssetName, soundGroupName,
                        soundParams, errorCode.Value, errorMessage, userData);
                    mPlaySoundFailureEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);

                    if (soundParams.Referenced)
                    {
                        ReferencePool.Release(soundParams);
                    }

                    return serialId;
                }

                throw new Exception(errorMessage);
            }

            mSoundsBeingLoaded.Add(serialId);
            mResourceManager.LoadAsset(
                new LoadAssetInfo(soundAssetName, priority,
                    PlaySoundInfo.Create(serialId, soundGroup, soundParams, userData)), mLoadAssetCallbacks);
            return serialId;
        }


        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        /// <returns>是否成功停止播放声音</returns>
        public bool StopSound(int serialId, float fadeOutSeconds = 0)
        {
            if (IsLoadingSound(serialId))
            {
                mSoundsToReleaseOnLoad.Add(serialId);
                mSoundsBeingLoaded.Remove(serialId);
                return true;
            }

            foreach (var (_, soundGroup) in mSoundGroups)
            {
                if (soundGroup.StopSound(serialId, fadeOutSeconds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 停止播放所有已加载声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public void StopAllLoadedSounds(float fadeOutSeconds = 0)
        {
            foreach (var (_, soundGroup) in mSoundGroups)
            {
                soundGroup.StopAllLoadedSounds(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public void PauseSound(int serialId, float fadeOutSeconds = 0)
        {
            foreach (var (_, soundGroup) in mSoundGroups)
            {
                if (soundGroup.PauseSound(serialId, fadeOutSeconds))
                {
                    return;
                }
            }

            throw new Exception($"Can not find sound ({serialId})");
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public void ResumeSound(int serialId, float fadeInSeconds = 0)
        {
            foreach (var (_, soundGroup) in mSoundGroups)
            {
                if (soundGroup.PauseSound(serialId, fadeInSeconds))
                {
                    return;
                }
            }

            throw new Exception($"Can not find sound ({serialId})");
        }

        private void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            var playSoundInfo = userData as PlaySoundInfo;
            if (playSoundInfo == null)
            {
                throw new Exception("Play sound info is invalid.");
            }

            if (mSoundsToReleaseOnLoad.Contains(playSoundInfo.SerialId))
            {
                mSoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
                if (playSoundInfo.SoundParams.Referenced)
                {
                    ReferencePool.Release(playSoundInfo.SoundParams);
                }

                ReferencePool.Release(playSoundInfo);
                mSoundHelper.ReleaseSoundAsset(asset);
            }

            mSoundsBeingLoaded.Remove(playSoundInfo.SerialId);
            SoundErrorCode? errorCode = null;
            ISoundAgent soundAgent = playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialId, asset,
                playSoundInfo.SoundParams, out errorCode);
            if (soundAgent != null)
            {
                if (mPlaySoundSuccessEventHandler != null)
                {
                    var eventArgs = PlaySoundSuccessEventArgs.Create(playSoundInfo.SerialId, assetName, soundAgent,
                        duration, userData);
                    mPlaySoundSuccessEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }

                if (playSoundInfo.SoundParams.Referenced)
                {
                    ReferencePool.Release(playSoundInfo.SoundParams);
                }

                ReferencePool.Release(playSoundInfo);
                return;
            }

            mSoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
            mSoundHelper.ReleaseSoundAsset(asset);
            var errorMessage = $"Sound group ({playSoundInfo.SoundGroup.Name}) play sound ({assetName}) failure.";
            if (mPlaySoundFailureEventHandler != null)
            {
                var eventArgs = PlaySoundFailureEventArgs.Create(playSoundInfo.SerialId, assetName,
                    playSoundInfo.SoundGroup.Name,
                    playSoundInfo.SoundParams, errorCode == null ? SoundErrorCode.UnKnown : errorCode.Value,
                    errorMessage, playSoundInfo.UserData);
                mPlaySoundFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);

                if (playSoundInfo.SoundParams.Referenced)
                {
                    ReferencePool.Release(playSoundInfo.SoundParams);
                }

                ReferencePool.Release(playSoundInfo);
                return;
            }

            if (playSoundInfo.SoundParams.Referenced)
            {
                ReferencePool.Release(playSoundInfo.SoundParams);
            }

            ReferencePool.Release(playSoundInfo);
            throw new Exception(errorMessage);
        }

        private void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage,
            object userData)
        {
            var playSoundInfo = userData as PlaySoundInfo;
            if (playSoundInfo == null)
            {
                throw new Exception("Play sound info is invalid.");
            }

            if (mSoundsToReleaseOnLoad.Contains(playSoundInfo.SerialId))
            {
                mSoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
                if (playSoundInfo.SoundParams.Referenced)
                {
                    ReferencePool.Release(playSoundInfo.SoundParams);
                }

                return;
            }

            mSoundsBeingLoaded.Remove(playSoundInfo.SerialId);
            string appendErrorMessage =
                $"Load sound failure, asset name ({assetName}), status ({status}), error message ({errorMessage})";
            if (mPlaySoundFailureEventHandler != null)
            {
                var eventArgs = PlaySoundFailureEventArgs.Create(playSoundInfo.SerialId, assetName,
                    playSoundInfo.SoundGroup.Name, playSoundInfo.SoundParams, SoundErrorCode.LoadAssetFailure,
                    appendErrorMessage, playSoundInfo.UserData);
                mPlaySoundFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);

                if (playSoundInfo.SoundParams.Referenced)
                {
                    ReferencePool.Release(playSoundInfo.SoundParams);
                }

                return;
            }

            throw new Exception(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string assetName, float progress, object userData)
        {
            var playSoundInfo = userData as PlaySoundInfo;
            if (playSoundInfo == null)
            {
                throw new Exception("Play sound info is invalid.");
            }

            if (mPlaySoundUpdateEventHandler != null)
            {
                var eventArgs = PlaySoundUpdateEventArgs.Create(playSoundInfo.SerialId, assetName,
                    playSoundInfo.SoundGroup.Name, playSoundInfo.SoundParams, progress, playSoundInfo.UserData);
                mPlaySoundUpdateEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadAssetDependencyCallback(string assetName, string dependencyAssetName, int loadedCount,
            int totalCount, object userData)
        {
            var playSoundInfo = userData as PlaySoundInfo;
            if (playSoundInfo == null)
            {
                throw new Exception("Play sound info is invalid.");
            }

            if (mPlaySoundDependencyEventHandler != null)
            {
                var eventArgs = PlaySoundDependencyEventArgs.Create(playSoundInfo.SerialId, assetName,
                    playSoundInfo.SoundGroup.Name, playSoundInfo.SoundParams, dependencyAssetName, loadedCount,
                    totalCount, playSoundInfo.UserData);
                mPlaySoundDependencyEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}