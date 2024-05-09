/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/9 11:17:13
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    public sealed partial class SoundManager : FrameworkModule, ISoundManager
    {
        /// <summary>
        /// 播放声音信息
        /// </summary>
        private sealed class PlaySoundInfo : IReference
        {
            private int mSerialId;
            private SoundGroup mSoundGroup;
            private SoundParams mSoundParams;
            private object mUserData;

            public PlaySoundInfo()
            {
                mSerialId = 0;
                mSoundGroup = null;
                mSoundParams = null;
                mUserData = null;
            }

            /// <summary>
            /// 声音序列编号
            /// </summary>
            public int SerialId => mSerialId;

            /// <summary>
            /// 声音组
            /// </summary>
            public SoundGroup SoundGroup => mSoundGroup;

            /// <summary>
            /// 声音参数
            /// </summary>
            public SoundParams SoundParams => mSoundParams;

            /// <summary>
            /// 用户自定义数据
            /// </summary>
            public object UserData => mUserData;

            /// <summary>
            /// 创建播放声音信息
            /// </summary>
            /// <param name="serialId">声音序列编号</param>
            /// <param name="soundGroup">声音组</param>
            /// <param name="soundParams">声音参数</param>
            /// <param name="userData">用户自定义数据</param>
            /// <returns>播放声音信息</returns>
            public static PlaySoundInfo Create(int serialId, SoundGroup soundGroup, SoundParams soundParams,
                object userData)
            {
                var playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
                playSoundInfo.mSerialId = serialId;
                playSoundInfo.mSoundGroup = soundGroup;
                playSoundInfo.mSoundParams = soundParams;
                playSoundInfo.mUserData = userData;
                return playSoundInfo;
            }

            /// <summary>
            /// 清理播放声音信息
            /// </summary>
            public void Clear()
            {
                mSerialId = 0;
                mSoundGroup = null;
                mSoundParams = null;
                mUserData = null;
            }
        }
    }
}