/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 10:18:47
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 声音代理接口
    /// </summary>
    public interface ISoundAgent
    {
        /// <summary>
        /// 声音组
        /// </summary>
        ISoundGroup SoundGroup { get; }

        /// <summary>
        /// 声音的序列编号
        /// </summary>
        int SerialId { get; }

        /// <summary>
        /// 声音是否正在播放
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// 声音长度
        /// </summary>
        float Length { get; }

        /// <summary>
        /// 声音播放位置
        /// </summary>
        float Time { get; set; }

        /// <summary>
        /// 声音是否静音
        /// </summary>
        bool Mute { get; }

        /// <summary>
        /// 声音在声音组内是否静音
        /// </summary>
        bool MuteInSoundGroup { get; set; }

        /// <summary>
        /// 声音是否循环播放
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        /// 声音优先级
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 声音音量大小
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// 声音在声音组内的音量大小
        /// </summary>
        float VolumeInSoundGroup { get; set; }

        /// <summary>
        /// 声音音调
        /// </summary>
        float Pitch { get; set; }

        /// <summary>
        /// 声音立体声声相
        /// </summary>
        float PanStereo { get; set; }

        /// <summary>
        /// 声音空间混合量
        /// </summary>
        float SpatialBlend { get; set; }

        /// <summary>
        /// 声音最大距离
        /// </summary>
        float MaxDistance { get; set; }

        /// <summary>
        /// 声音多普勒等级
        /// </summary>
        float DopplerLevel { get; set; }

        /// <summary>
        /// 声音代理辅助器
        /// </summary>
        ISoundAgentHelper SoundAgentHelper { get; }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        void Play(float fadeInSeconds = 0f);

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        void Stop(float fadeOutSeconds = 0f);

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        void Pause(float fadeOutSeconds = 0f);

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        void Resume(float fadeInSeconds = 0f);

        /// <summary>
        /// 重置声音代理
        /// </summary>
        void Reset();
    }
}