/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 10:20:0
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 声音组接口
    /// </summary>
    public interface ISoundGroup
    {
        /// <summary>
        /// 声音组名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 声音代理器数量
        /// </summary>
        int SoundAgentCount { get; }

        /// <summary>
        /// 声音组内的声音是否避免被同优先级声音替换
        /// </summary>
        bool AvoidBeingReplacedBySamePriority { get; set; }

        /// <summary>
        /// 是否声音组静音
        /// </summary>
        bool Mute { get; set; }

        /// <summary>
        /// 声音组音量大小
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 声音组辅助器
        /// </summary>
        ISoundGroupHelper SoundGroupHelper { get; }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="soundAsset">声音资源</param>
        /// <param name="soundParams">声音参数</param>
        /// <param name="errorCode">播放声音错误码</param>
        /// <returns>声音代理</returns>
        public ISoundAgent PlaySound(int serialId, object soundAsset, SoundParams soundParams,
            out SoundErrorCode? errorCode);

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        /// <returns>是否成功停止播放声音</returns>
        public bool StopSound(int serialId, float fadeOutSeconds = 0f);

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        /// <returns>是否成功暂停播放声音</returns>
        public bool PauseSound(int serialId, float fadeOutSeconds = 0f);

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        /// <returns>是否成功恢复播放声音</returns>
        public bool ResumeSound(int serialId, float fadeInSeconds = 0f);

        /// <summary>
        /// 停止加载已所有声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        void StopAllLoadedSounds(float fadeOutSeconds = 0f);
    }
}