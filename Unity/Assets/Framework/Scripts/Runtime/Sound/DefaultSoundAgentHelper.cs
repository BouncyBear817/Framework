/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 11:6:0
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections;
using Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace Runtime
{
    /// <summary>
    /// 默认声音代理辅助器
    /// </summary>
    public class DefaultSoundAgentHelper : SoundAgentHelperBase
    {
        private Transform mCachedTransform = null;
        private AudioSource mAudioSource = null;
        private GameObject mBindingObject = null;
        private float mVolumeWhenPause = 0f;
        private bool mApplicationPauseFlag = false;
        private EventHandler<ResetSoundAgentEventArgs> mResetSoundAgentEventHandler = null;

        /// <summary>
        /// 声音是否正在播放
        /// </summary>
        public override bool IsPlaying => mAudioSource.isPlaying;

        /// <summary>
        /// 声音长度
        /// </summary>
        public override float Length => mAudioSource.clip != null ? mAudioSource.clip.length : 0f;

        /// <summary>
        /// 声音播放位置
        /// </summary>
        public override float Time
        {
            get => mAudioSource.time;
            set => mAudioSource.time = value;
        }

        /// <summary>
        /// 声音是否静音
        /// </summary>
        public override bool Mute

        {
            get => mAudioSource.mute;
            set => mAudioSource.mute = value;
        }

        /// <summary>
        /// 声音是否循环播放
        /// </summary>
        public override bool Loop
        {
            get => mAudioSource.loop;
            set => mAudioSource.loop = value;
        }

        /// <summary>
        /// 声音优先级
        /// </summary>
        public override int Priority
        {
            get => 128 - mAudioSource.priority;
            set => mAudioSource.priority = 128 - value;
        }

        /// <summary>
        /// 声音音量大小
        /// </summary>
        public override float Volume
        {
            get => mAudioSource.volume;
            set => mAudioSource.volume = value;
        }

        /// <summary>
        /// 声音音调
        /// </summary>
        public override float Pitch
        {
            get => mAudioSource.pitch;
            set => mAudioSource.pitch = value;
        }

        /// <summary>
        /// 声音立体声声相
        /// </summary>
        public override float PanStereo
        {
            get => mAudioSource.panStereo;
            set => mAudioSource.panStereo = value;
        }

        /// <summary>
        /// 声音空间混合量
        /// </summary>
        public override float SpatialBlend
        {
            get => mAudioSource.spatialBlend;
            set => mAudioSource.spatialBlend = value;
        }

        /// <summary>
        /// 声音最大距离
        /// </summary>
        public override float MaxDistance
        {
            get => mAudioSource.maxDistance;
            set => mAudioSource.maxDistance = value;
        }

        /// <summary>
        /// 声音多普勒等级
        /// </summary>
        public override float DopplerLevel
        {
            get => mAudioSource.dopplerLevel;
            set => mAudioSource.dopplerLevel = value;
        }

        /// <summary>
        /// 声音混音组
        /// </summary>
        public override AudioMixerGroup AudioMixerGroup
        {
            get => mAudioSource.outputAudioMixerGroup;
            set => mAudioSource.outputAudioMixerGroup = value;
        }

        /// <summary>
        /// 重置声音代理事件
        /// </summary>
        public override event EventHandler<ResetSoundAgentEventArgs> ResetSoundAgent
        {
            add => mResetSoundAgentEventHandler += value;
            remove => mResetSoundAgentEventHandler -= value;
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public override void Play(float fadeInSeconds = 0f)
        {
            StopAllCoroutines();

            mAudioSource.Play();
            if (fadeInSeconds > 0f)
            {
                var volume = mAudioSource.volume;
                mAudioSource.volume = 0f;
                StartCoroutine(FadeToVolume(mAudioSource, volume, fadeInSeconds));
            }
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public override void Stop(float fadeOutSeconds = 0f)
        {
            StopAllCoroutines();
            if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeToStop(mAudioSource, fadeOutSeconds));
            }
            else
            {
                mAudioSource.Stop();
            }
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public override void Pause(float fadeOutSeconds = 0f)
        {
            StopAllCoroutines();

            mVolumeWhenPause = mAudioSource.volume;
            if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeToPause(mAudioSource, fadeOutSeconds));
            }
            else
            {
                mAudioSource.Pause();
            }
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public override void Resume(float fadeInSeconds = 0f)
        {
            StopAllCoroutines();

            mAudioSource.UnPause();
            if (fadeInSeconds > 0f)
            {
                StartCoroutine(FadeToVolume(mAudioSource, mVolumeWhenPause, fadeInSeconds));
            }
            else
            {
                mAudioSource.volume = mVolumeWhenPause;
            }
        }

        /// <summary>
        /// 重置声音代理辅助器
        /// </summary>
        public override void Reset()
        {
            mCachedTransform.localPosition = Vector3.zero;
            mAudioSource.clip = null;
            mBindingObject = null;
            mVolumeWhenPause = 0f;
        }

        /// <summary>
        /// 设置声音资源
        /// </summary>
        /// <param name="soundAsset">声音资源</param>
        public override bool SetSoundAsset(object soundAsset)
        {
            var audioClip = soundAsset as AudioClip;
            if (audioClip == null)
            {
                return false;
            }

            mAudioSource.clip = audioClip;
            return true;
        }

        /// <summary>
        /// 设置绑定的对象
        /// </summary>
        /// <param name="bindingObject">绑定对象</param>
        public override void SetBindingObject(GameObject bindingObject)
        {
            mBindingObject = bindingObject;
            if (mBindingObject != null)
            {
                UpdateAgentPosition();
                return;
            }

            ResetSoundAgentEvent();
        }

        /// <summary>
        /// 设置声音在世界的位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        public override void SetWorldPosition(Vector3 worldPosition)
        {
            mCachedTransform.position = worldPosition;
        }

        private void Awake()
        {
            mCachedTransform = transform;
            mAudioSource = gameObject.GetOrAddComponent<AudioSource>();
            mAudioSource.playOnAwake = false;
            mAudioSource.rolloffMode = AudioRolloffMode.Custom;
        }

        private void Update()
        {
            if (!mApplicationPauseFlag && !IsPlaying && mAudioSource.clip != null)
            {
                ResetSoundAgentEvent();
            }

            if (mBindingObject != null)
            {
                UpdateAgentPosition();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            mApplicationPauseFlag = pauseStatus;
        }

        private void UpdateAgentPosition()
        {
            if (mBindingObject.activeInHierarchy)
            {
                mCachedTransform.position = mBindingObject.transform.position;
            }
        }

        private void ResetSoundAgentEvent()
        {
            if (mResetSoundAgentEventHandler != null)
            {
                var eventArgs = ResetSoundAgentEventArgs.Create();
                mResetSoundAgentEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private IEnumerator FadeToVolume(AudioSource audioSource, float destVolume, float duration)
        {
            var time = 0f;
            var originalVolume = audioSource.volume;
            while (time < duration)
            {
                time += UnityEngine.Time.deltaTime;
                audioSource.volume = Mathf.Lerp(originalVolume, destVolume, time / duration);
                yield return new WaitForEndOfFrame();
            }

            audioSource.volume = destVolume;
        }

        private IEnumerator FadeToStop(AudioSource audioSource, float fadeOutSeconds)
        {
            yield return FadeToVolume(audioSource, 0f, fadeOutSeconds);
            audioSource.Stop();
        }

        private IEnumerator FadeToPause(AudioSource audioSource, float fadeOutSeconds)
        {
            yield return FadeToVolume(audioSource, 0f, fadeOutSeconds);
            audioSource.Pause();
        }
    }
}