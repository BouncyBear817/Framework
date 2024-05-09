/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/9 11:17:13
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    public sealed partial class SoundManager : FrameworkModule, ISoundManager
    {
        /// <summary>
        /// 声音代理
        /// </summary>
        private sealed class SoundAgent : ISoundAgent
        {
            private readonly SoundGroup mSoundGroup;
            private readonly ISoundHelper mSoundHelper;
            private readonly ISoundAgentHelper mSoundAgentHelper;

            private int mSerialId;
            private object mSoundAsset;
            private DateTime mSetSoundAssetTime;
            private bool mMuteInSoundGroup;
            private float mVolumeInSoundGroup;

            public SoundAgent(SoundGroup soundGroup, ISoundHelper soundHelper, ISoundAgentHelper soundAgentHelper)
            {
                if (soundGroup == null)
                {
                    throw new Exception("Sound group is invalid.");
                }

                if (soundHelper == null)
                {
                    throw new Exception("Sound helper is invalid.");
                }

                if (soundAgentHelper == null)
                {
                    throw new Exception("Sound agent helper is invalid.");
                }

                mSoundGroup = soundGroup;
                mSoundHelper = soundHelper;
                mSoundAgentHelper = soundAgentHelper;
                mSoundAgentHelper.ResetSoundAgent += OnResetSoundAgent;
                mSerialId = 0;
                mSoundAsset = null;
                Reset();
            }


            /// <summary>
            /// 声音组
            /// </summary>
            public ISoundGroup SoundGroup => mSoundGroup;

            /// <summary>
            /// 声音的序列编号
            /// </summary>
            public int SerialId
            {
                get => mSerialId;
                set => mSerialId = value;
            }

            /// <summary>
            /// 声音是否正在播放
            /// </summary>
            public bool IsPlaying => mSoundAgentHelper.IsPlaying;

            /// <summary>
            /// 声音长度
            /// </summary>
            public float Length => mSoundAgentHelper.Length;

            /// <summary>
            /// 声音播放位置
            /// </summary>
            public float Time
            {
                get => mSoundAgentHelper.Time;
                set => mSoundAgentHelper.Time = value;
            }

            /// <summary>
            /// 声音是否静音
            /// </summary>
            public bool Mute => mSoundAgentHelper.Mute;

            /// <summary>
            /// 声音在声音组内是否静音
            /// </summary>
            public bool MuteInSoundGroup
            {
                get => mMuteInSoundGroup;
                set
                {
                    mMuteInSoundGroup = value;
                    RefreshMute();
                }
            }

            /// <summary>
            /// 声音是否循环播放
            /// </summary>
            public bool Loop
            {
                get => mSoundAgentHelper.Loop;
                set => mSoundAgentHelper.Loop = value;
            }

            /// <summary>
            /// 声音优先级
            /// </summary>
            public int Priority
            {
                get => mSoundAgentHelper.Priority;
                set => mSoundAgentHelper.Priority = value;
            }

            /// <summary>
            /// 声音音量大小
            /// </summary>
            public float Volume => mSoundAgentHelper.Volume;

            /// <summary>
            /// 声音在声音组内的音量大小
            /// </summary>
            public float VolumeInSoundGroup
            {
                get => mVolumeInSoundGroup;
                set
                {
                    mVolumeInSoundGroup = value;
                    RefreshVolume();
                }
            }


            /// <summary>
            /// 声音音调
            /// </summary>
            public float Pitch
            {
                get => mSoundAgentHelper.Pitch;
                set => mSoundAgentHelper.Pitch = value;
            }

            /// <summary>
            /// 声音立体声声相
            /// </summary>
            public float PanStereo
            {
                get => mSoundAgentHelper.PanStereo;
                set => mSoundAgentHelper.PanStereo = value;
            }

            /// <summary>
            /// 声音空间混合量
            /// </summary>
            public float SpatialBlend
            {
                get => mSoundAgentHelper.SpatialBlend;
                set => mSoundAgentHelper.SpatialBlend = value;
            }

            /// <summary>
            /// 声音最大距离
            /// </summary>
            public float MaxDistance
            {
                get => mSoundAgentHelper.MaxDistance;
                set => mSoundAgentHelper.MaxDistance = value;
            }

            /// <summary>
            /// 声音多普勒等级
            /// </summary>
            public float DopplerLevel
            {
                get => mSoundAgentHelper.DopplerLevel;
                set => mSoundAgentHelper.DopplerLevel = value;
            }

            /// <summary>
            /// 声音代理辅助器
            /// </summary>
            public ISoundAgentHelper SoundAgentHelper => mSoundAgentHelper;

            /// <summary>
            /// 声音创建时间
            /// </summary>
            public DateTime SetSoundAssetTime => mSetSoundAssetTime;

            /// <summary>
            /// 播放声音
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
            public void Play(float fadeInSeconds = 0)
            {
                mSoundAgentHelper.Play(fadeInSeconds);
            }

            /// <summary>
            /// 停止播放声音
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
            public void Stop(float fadeOutSeconds = 0f)
            {
                mSoundAgentHelper.Stop(fadeOutSeconds);
            }

            /// <summary>
            /// 暂停播放声音
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
            public void Pause(float fadeOutSeconds = 0f)
            {
                mSoundAgentHelper.Pause(fadeOutSeconds);
            }

            /// <summary>
            /// 恢复播放声音
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
            public void Resume(float fadeInSeconds = 0f)
            {
                mSoundAgentHelper.Resume(fadeInSeconds);
            }

            /// <summary>
            /// 重置声音代理
            /// </summary>
            public void Reset()
            {
                if (mSoundAsset != null)
                {
                    mSoundHelper.ReleaseSoundAsset(mSoundAsset);
                    mSoundAsset = null;
                }

                mSetSoundAssetTime = DateTime.MinValue;
                Time = SoundConstant.DefaultTime;
                MuteInSoundGroup = SoundConstant.DefaultMute;
                Loop = SoundConstant.DefaultLoop;
                Priority = SoundConstant.DefaultPriority;
                VolumeInSoundGroup = SoundConstant.DefaultVolume;
                Pitch = SoundConstant.DefaultPitch;
                PanStereo = SoundConstant.DefaultPanStereo;
                SpatialBlend = SoundConstant.DefaultSpatialBlend;
                MaxDistance = SoundConstant.DefaultMaxDistance;
                DopplerLevel = SoundConstant.DefaultDopplerLevel;
                mSoundAgentHelper.Reset();
            }

            /// <summary>
            /// 设置声音资源
            /// </summary>
            /// <param name="soundAsset"></param>
            /// <returns></returns>
            public bool SetSoundAsset(object soundAsset)
            {
                Reset();
                mSoundAsset = soundAsset;
                mSetSoundAssetTime = DateTime.UtcNow;
                return mSoundAgentHelper.SetSoundAsset(soundAsset);
            }

            /// <summary>
            /// 设置声音参数
            /// </summary>
            /// <param name="serialId"></param>
            /// <param name="soundParams"></param>
            public void SetSoundParam(int serialId, SoundParams soundParams)
            {
                mSerialId = serialId;
                Time = soundParams.Time;
                MuteInSoundGroup = soundParams.MuteInSoundGroup;
                Loop = soundParams.Loop;
                Priority = soundParams.Priority;
                VolumeInSoundGroup = soundParams.VolumeInSoundGroup;
                Pitch = soundParams.Pitch;
                PanStereo = soundParams.PanStereo;
                SpatialBlend = soundParams.SpatialBlend;
                MaxDistance = soundParams.MaxDistance;
                DopplerLevel = soundParams.DopplerLevel;
            }

            /// <summary>
            /// 刷新声音是否静音
            /// </summary>
            public void RefreshMute()
            {
                mSoundAgentHelper.Mute = mSoundGroup.Mute || mMuteInSoundGroup;
            }

            /// <summary>
            /// 刷新声音音量
            /// </summary>
            public void RefreshVolume()
            {
                mSoundAgentHelper.Volume = mSoundGroup.Volume * mVolumeInSoundGroup;
            }

            private void OnResetSoundAgent(object sender, ResetSoundAgentEventArgs e)
            {
                Reset();
            }
        }
    }
}