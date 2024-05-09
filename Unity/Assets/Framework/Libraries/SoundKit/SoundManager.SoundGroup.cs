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
    public sealed partial class SoundManager : FrameworkModule, ISoundManager
    {
        /// <summary>
        /// 声音组
        /// </summary>
        private sealed class SoundGroup : ISoundGroup
        {
            private readonly string mName;
            private readonly ISoundGroupHelper mSoundGroupHelper;
            private readonly List<SoundAgent> mSoundAgents;
            private bool mAvoidBeingReplacedBySamePriority;
            private bool mMute;
            private float mVolume;

            public SoundGroup(string name, ISoundGroupHelper soundGroupHelper)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Sound group name is invalid.");
                }

                if (soundGroupHelper == null)
                {
                    throw new Exception("Sound group helper is invalid.");
                }

                mName = name;
                mSoundGroupHelper = soundGroupHelper;
                mSoundAgents = new List<SoundAgent>();
            }

            /// <summary>
            /// 声音组名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 声音代理器数量
            /// </summary>
            public int SoundAgentCount => mSoundAgents.Count;

            /// <summary>
            /// 声音组内的声音是否避免被同优先级声音替换
            /// </summary>
            public bool AvoidBeingReplacedBySamePriority
            {
                get => mAvoidBeingReplacedBySamePriority;
                set => mAvoidBeingReplacedBySamePriority = value;
            }

            /// <summary>
            /// 是否声音组静音
            /// </summary>
            public bool Mute
            {
                get => mMute;
                set
                {
                    mMute = value;
                    foreach (var soundAgent in mSoundAgents)
                    {
                        soundAgent.RefreshMute();
                    }
                }
            }

            /// <summary>
            /// 声音组音量大小
            /// </summary>
            public float Volume
            {
                get => mVolume;
                set
                {
                    mVolume = value;
                    foreach (var soundAgent in mSoundAgents)
                    {
                        soundAgent.RefreshVolume();
                    }
                }
            }

            /// <summary>
            /// 声音组辅助器
            /// </summary>
            public ISoundGroupHelper SoundGroupHelper => mSoundGroupHelper;

            /// <summary>
            /// 增加声音代理辅助器
            /// </summary>
            /// <param name="soundHelper">声音辅助器</param>
            /// <param name="soundAgentHelper">声音代理辅助器</param>
            public void AddSoundAgentHelper(ISoundHelper soundHelper, ISoundAgentHelper soundAgentHelper)
            {
                mSoundAgents.Add(new SoundAgent(this, soundHelper, soundAgentHelper));
            }

            /// <summary>
            /// 播放声音
            /// </summary>
            /// <param name="serialId">声音序列编号</param>
            /// <param name="soundAsset">声音资源</param>
            /// <param name="soundParams">声音参数</param>
            /// <param name="errorCode">播放声音错误码</param>
            /// <returns>声音代理</returns>
            public ISoundAgent PlaySound(int serialId, object soundAsset, SoundParams soundParams,
                out SoundErrorCode? errorCode)
            {
                errorCode = null;
                SoundAgent candidateAgent = null;
                foreach (var soundAgent in mSoundAgents)
                {
                    if (!soundAgent.IsPlaying)
                    {
                        candidateAgent = soundAgent;
                        break;
                    }

                    if (soundAgent.Priority < soundParams.Priority)
                    {
                        if (candidateAgent == null || soundAgent.Priority < candidateAgent.Priority)
                        {
                            candidateAgent = soundAgent;
                        }
                    }
                    else if (!mAvoidBeingReplacedBySamePriority && soundAgent.Priority == soundParams.Priority)
                    {
                        if (candidateAgent == null || soundAgent.SetSoundAssetTime < candidateAgent.SetSoundAssetTime)
                        {
                            candidateAgent = soundAgent;
                        }
                    }
                }

                if (candidateAgent == null)
                {
                    errorCode = SoundErrorCode.IgnoreDueToLowPriority;
                    return null;
                }

                if (!candidateAgent.SetSoundAsset(soundAsset))
                {
                    errorCode = SoundErrorCode.SetSoundAssetFailure;
                    return null;
                }

                candidateAgent.SetSoundParam(serialId, soundParams);
                candidateAgent.Play(soundParams.FadeInSeconds);
                return candidateAgent;
            }

            /// <summary>
            /// 停止播放声音
            /// </summary>
            /// <param name="serialId">声音序列编号</param>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
            /// <returns>是否成功停止播放声音</returns>
            public bool StopSound(int serialId, float fadeOutSeconds = 0f)
            {
                foreach (var soundAgent in mSoundAgents)
                {
                    if (soundAgent.SerialId == serialId)
                    {
                        soundAgent.Stop(fadeOutSeconds);
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 暂停播放声音
            /// </summary>
            /// <param name="serialId">声音序列编号</param>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
            /// <returns>是否成功暂停播放声音</returns>
            public bool PauseSound(int serialId, float fadeOutSeconds = 0f)
            {
                foreach (var soundAgent in mSoundAgents)
                {
                    if (soundAgent.SerialId == serialId)
                    {
                        soundAgent.Pause(fadeOutSeconds);
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 恢复播放声音
            /// </summary>
            /// <param name="serialId">声音序列编号</param>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
            /// <returns>是否成功恢复播放声音</returns>
            public bool ResumeSound(int serialId, float fadeInSeconds = 0f)
            {
                foreach (var soundAgent in mSoundAgents)
                {
                    if (soundAgent.SerialId == serialId)
                    {
                        soundAgent.Resume(fadeInSeconds);
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 停止加载已所有声音
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
            public void StopAllLoadedSounds(float fadeOutSeconds = 0f)
            {
                foreach (var soundAgent in mSoundAgents)
                {
                    soundAgent.Stop(fadeOutSeconds);
                }
            }
        }
    }
}