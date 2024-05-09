/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 15:50:54
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 播放声音参数
    /// </summary>
    public class SoundParams : IReference
    {
        private bool mReferenced;
        private float mTime;
        private bool mMuteInSoundGroup;
        private bool mLoop;
        private int mPriority;
        private float mVolumeInSoundGroup;
        private float mFadeInSeconds;
        private float mFadeOutSeconds;
        private float mPitch;
        private float mPanStereo;
        private float mSpatialBlend;
        private float mMaxDistance;
        private float mDopplerLevel;

        public SoundParams()
        {
            mReferenced = false;
            mTime = SoundConstant.DefaultTime;
            mMuteInSoundGroup = SoundConstant.DefaultMute;
            mLoop = SoundConstant.DefaultLoop;
            mPriority = SoundConstant.DefaultPriority;
            mVolumeInSoundGroup = SoundConstant.DefaultVolume;
            mFadeInSeconds = SoundConstant.DefaultFadeInSeconds;
            mFadeOutSeconds = SoundConstant.DefaultFadeOutSeconds;
            mPitch = SoundConstant.DefaultPitch;
            mPanStereo = SoundConstant.DefaultPanStereo;
            mSpatialBlend = SoundConstant.DefaultSpatialBlend;
            mMaxDistance = SoundConstant.DefaultMaxDistance;
            mDopplerLevel = SoundConstant.DefaultDopplerLevel;
        }

        public bool Referenced => mReferenced;

        /// <summary>
        /// 声音播放位置
        /// </summary>
        public float Time
        {
            get => mTime;
            set => mTime = value;
        }

        /// <summary>
        /// 声音组内是否静音
        /// </summary>
        public bool MuteInSoundGroup
        {
            get => mMuteInSoundGroup;
            set => mMuteInSoundGroup = value;
        }

        /// <summary>
        /// 声音是否循环
        /// </summary>
        public bool Loop
        {
            get => mLoop;
            set => mLoop = value;
        }

        /// <summary>
        /// 声音优先级
        /// </summary>
        public int Priority
        {
            get => mPriority;
            set => mPriority = value;
        }

        /// <summary>
        /// 声音组内音量大小
        /// </summary>
        public float VolumeInSoundGroup
        {
            get => mVolumeInSoundGroup;
            set => mVolumeInSoundGroup = value;
        }

        /// <summary>
        /// 声音淡入时间，以秒为单位
        /// </summary>
        public float FadeInSeconds
        {
            get => mFadeInSeconds;
            set => mFadeInSeconds = value;
        }

        /// <summary>
        /// 声音淡出时间，以秒为单位
        /// </summary>
        public float FadeOutSeconds
        {
            get => mFadeOutSeconds;
            set => mFadeOutSeconds = value;
        }

        /// <summary>
        /// 声音音调
        /// </summary>
        public float Pitch
        {
            get => mPitch;
            set => mPitch = value;
        }

        /// <summary>
        /// 声音立体声声相
        /// </summary>
        public float PanStereo
        {
            get => mPanStereo;
            set => mPanStereo = value;
        }

        /// <summary>
        /// 声音空间混合量
        /// </summary>
        public float SpatialBlend
        {
            get => mSpatialBlend;
            set => mSpatialBlend = value;
        }

        /// <summary>
        /// 声音最大距离
        /// </summary>
        public float MaxDistance
        {
            get => mMaxDistance;
            set => mMaxDistance = value;
        }

        /// <summary>
        /// 声音多普勒等级
        /// </summary>
        public float DopplerLevel
        {
            get => mDopplerLevel;
            set => mDopplerLevel = value;
        }

        /// <summary>
        /// 创建声音参数
        /// </summary>
        /// <returns>声音参数</returns>
        public static SoundParams Create()
        {
            var soundParams = ReferencePool.Acquire<SoundParams>();
            soundParams.mReferenced = true;
            return soundParams;
        }

        /// <summary>
        /// 清理声音参数
        /// </summary>
        public void Clear()
        {
            mTime = SoundConstant.DefaultTime;
            mMuteInSoundGroup = SoundConstant.DefaultMute;
            mLoop = SoundConstant.DefaultLoop;
            mPriority = SoundConstant.DefaultPriority;
            mVolumeInSoundGroup = SoundConstant.DefaultVolume;
            mFadeInSeconds = SoundConstant.DefaultFadeInSeconds;
            mFadeOutSeconds = SoundConstant.DefaultFadeOutSeconds;
            mPitch = SoundConstant.DefaultPitch;
            mPanStereo = SoundConstant.DefaultPanStereo;
            mSpatialBlend = SoundConstant.DefaultSpatialBlend;
            mMaxDistance = SoundConstant.DefaultMaxDistance;
            mDopplerLevel = SoundConstant.DefaultDopplerLevel;
        }
    }
}