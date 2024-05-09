/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/9 10:40:43
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 播放声音错误码
    /// </summary>
    public enum SoundErrorCode : byte
    {
        /// <summary>
        /// 位置错误
        /// </summary>
        UnKnown = 0,

        /// <summary>
        /// 声音组不存在
        /// </summary>
        SoundGroupNotExist,

        /// <summary>
        /// 声音组无声音代理
        /// </summary>
        SoundGroupHasNoAgent,

        /// <summary>
        /// 加载声音资源失败
        /// </summary>
        LoadAssetFailure,

        /// <summary>
        /// 播放声音因优先级低被忽略
        /// </summary>
        IgnoreDueToLowPriority,

        /// <summary>
        /// 设置声音资源失败
        /// </summary>
        SetSoundAssetFailure
    }
}