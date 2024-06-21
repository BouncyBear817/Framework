/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 10:5:47
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace Framework.Runtime
{
    /// <summary>
    /// 声音代理辅助器基类
    /// </summary>
    public abstract class SoundAgentHelperBase : MonoBehaviour, ISoundAgentHelper
    {
        /// <summary>
        /// 声音是否正在播放
        /// </summary>
        public abstract bool IsPlaying { get; }

        /// <summary>
        /// 声音长度
        /// </summary>
        public abstract float Length { get; }

        /// <summary>
        /// 声音播放位置
        /// </summary>
        public abstract float Time { get; set; }

        /// <summary>
        /// 声音是否静音
        /// </summary>
        public abstract bool Mute { get; set; }

        /// <summary>
        /// 声音是否循环播放
        /// </summary>
        public abstract bool Loop { get; set; }

        /// <summary>
        /// 声音优先级
        /// </summary>
        public abstract int Priority { get; set; }

        /// <summary>
        /// 声音音量大小
        /// </summary>
        public abstract float Volume { get; set; }

        /// <summary>
        /// 声音音调
        /// </summary>
        public abstract float Pitch { get; set; }

        /// <summary>
        /// 声音立体声声相
        /// </summary>
        public abstract float PanStereo { get; set; }

        /// <summary>
        /// 声音空间混合量
        /// </summary>
        public abstract float SpatialBlend { get; set; }

        /// <summary>
        /// 声音最大距离
        /// </summary>
        public abstract float MaxDistance { get; set; }

        /// <summary>
        /// 声音多普勒等级
        /// </summary>
        public abstract float DopplerLevel { get; set; }

        /// <summary>
        /// 声音混音组
        /// </summary>
        public abstract AudioMixerGroup AudioMixerGroup { get; set; }

        /// <summary>
        /// 重置声音代理事件
        /// </summary>
        public abstract event EventHandler<ResetSoundAgentEventArgs> ResetSoundAgent;

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public abstract void Play(float fadeInSeconds = 0);

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public abstract void Stop(float fadeOutSeconds = 0);

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public abstract void Pause(float fadeOutSeconds = 0);

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public abstract void Resume(float fadeInSeconds = 0);

        /// <summary>
        /// 重置声音代理辅助器
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 设置声音资源
        /// </summary>
        /// <param name="soundAsset">声音资源</param>
        public abstract bool SetSoundAsset(object soundAsset);

        /// <summary>
        /// 设置绑定的对象
        /// </summary>
        /// <param name="bindingObject">绑定对象</param>
        public abstract void SetBindingObject(GameObject bindingObject);

        /// <summary>
        /// 设置声音在世界的位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        public abstract void SetWorldPosition(Vector3 worldPosition);
    }
}