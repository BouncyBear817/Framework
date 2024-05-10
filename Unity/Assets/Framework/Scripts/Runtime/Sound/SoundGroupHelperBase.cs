/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 10:11:27
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace Runtime
{
    /// <summary>
    /// 声音组辅助器基类
    /// </summary>
    public abstract class SoundGroupHelperBase : MonoBehaviour, ISoundGroupHelper
    {
        [SerializeField] private AudioMixerGroup mAudioMixerGroup = null;

        /// <summary>
        /// 声音混音组
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get => mAudioMixerGroup;
            set => mAudioMixerGroup = value;
        }
    }
}