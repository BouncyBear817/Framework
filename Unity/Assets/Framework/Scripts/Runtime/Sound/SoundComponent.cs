/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 14:52:52
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Runtime
{
    /// <summary>
    /// 声音组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Sound")]
    public sealed partial class SoundComponent : FrameworkComponent
    {
        private ISoundManager mSoundManager = null;
        private EventComponent mEventComponent = null;
        private AudioListener mAudioListener = null;

        [SerializeField] private bool mEnablePlaySoundUpdateEvent = false;
        [SerializeField] private bool mEnablePlaySoundDependencyEvent = false;
        [SerializeField] private Transform mInstanceRoot = null;
        [SerializeField] private AudioMixer mAudioMixer = null;
        [SerializeField] private string mSoundHelperTypeName = "Runtime.DefaultSoundHelper";
        [SerializeField] private SoundHelperBase mCustomSoundHelper = null;
        [SerializeField] private string mSoundGroupHelperTypeName = "Runtime.DefaultSoundGroupHelper";
        [SerializeField] private SoundGroupHelperBase mCustomSoundGroupHelper = null;
        [SerializeField] private string mSoundAgentHelperTypeName = "Runtime.DefaultSoundAgentHelper";
        [SerializeField] private SoundAgentHelperBase mCustomSoundAgentHelper = null;
        [SerializeField] private SoundGroup[] mSoundGroups = null;

        /// <summary>
        /// 获取声音组数量
        /// </summary>
        public int SoundGroupCount => mSoundManager.SoundGroupCount;

        public AudioMixer AudioMixer => mAudioMixer;

        protected override void Awake()
        {
            base.Awake();

            mSoundManager = FrameworkEntry.GetModule<ISoundManager>();
            if (mSoundManager == null)
            {
                Log.Fatal("Sound manager is invalid.");
                return;
            }

            mSoundManager.PlaySoundSuccess += OnPlaySoundSuccess;
            mSoundManager.PlaySoundFailure += OnPlaySoundFailure;
            if (mEnablePlaySoundUpdateEvent)
            {
                mSoundManager.PlaySoundUpdate += OnPlaySoundUpdate;
            }

            if (mEnablePlaySoundDependencyEvent)
            {
                mSoundManager.PlaySoundDependency += OnPlaySoundDependency;
            }

            mAudioListener = gameObject.GetOrAddComponent<AudioListener>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Start()
        {
            var baseComponent = MainEntryHelper.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            var soundHelper = Helper.CreateHelper(mSoundHelperTypeName, mCustomSoundHelper);
            if (soundHelper == null)
            {
                Log.Error("Can not create sound helper.");
                return;
            }

            soundHelper.name = "Sound Helper";
            var soundHelperTrans = soundHelper.transform;
            soundHelperTrans.SetParent(transform);
            soundHelperTrans.localScale = Vector3.one;

            mSoundManager.SetSoundHelper(soundHelper);

            if (mInstanceRoot == null)
            {
                mInstanceRoot = new GameObject("Sound Instance").transform;
                mInstanceRoot.SetParent(transform);
                mInstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < mSoundGroups.Length; i++)
            {
                if (!AddSoundGroup(mSoundGroups[i].Name, mSoundGroups[i].AvoidBeingReplacedBySamePriority,
                        mSoundGroups[i].Mute, mSoundGroups[i].Volume, mSoundGroups[i].AgentHelperCount))
                {
                    Log.Warning("Add sound group ({mSoundGroups[i].Name}) failure.");
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns>是否存在指定声音组</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            return mSoundManager.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        /// <param name="soundGroupName">指定声音组名称</param>
        /// <returns>指定声音组</returns>
        public ISoundGroup GetSoundGroup(string soundGroupName)
        {
            return mSoundManager.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <returns>所有声音组</returns>
        public ISoundGroup[] GetAllSoundGroups()
        {
            return mSoundManager.GetAllSoundGroups();
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <param name="results">所有声音组</param>
        public void GetAllSoundGroups(List<ISoundGroup> results)
        {
            mSoundManager.GetAllSoundGroups(results);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量</param>
        /// <returns>声音组是否增加成功</returns>
        public bool AddSoundGroup(string soundGroupName, int soundAgentHelperCount)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume,
                soundAgentHelperCount);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组内的声音是否避免被同优先级声音替换</param>
        /// <param name="soundGroupMute">声音组是否静音</param>
        /// <param name="soundGroupVolume">声音组音量大小</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量</param>
        /// <returns>声音组是否增加成功</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority,
            bool soundGroupMute,
            float soundGroupVolume, int soundAgentHelperCount)
        {
            if (HasSoundGroup(soundGroupName))
            {
                return false;
            }

            var soundGroupHelper =
                Helper.CreateHelper(mSoundGroupHelperTypeName, mCustomSoundGroupHelper, SoundGroupCount);
            if (soundGroupHelper == null)
            {
                Log.Error("Can not create sound group helper.");
                return false;
            }

            soundGroupHelper.name = $"Sound Group Helper - {soundGroupName}";
            var soundGroupHelperTrans = soundGroupHelper.transform;
            soundGroupHelperTrans.SetParent(mInstanceRoot);
            soundGroupHelperTrans.localScale = Vector3.one;

            if (mAudioMixer != null)
            {
                var audioMixerGroups = mAudioMixer.FindMatchingGroups($"Master/{soundGroupName}");
                soundGroupHelper.AudioMixerGroup = audioMixerGroups.Length > 0
                    ? audioMixerGroups[0]
                    : mAudioMixer.FindMatchingGroups("Master")[0];
            }

            if (!mSoundManager.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority, soundGroupMute,
                    soundGroupVolume, soundGroupHelper))
            {
                return false;
            }

            for (int i = 0; i < soundAgentHelperCount; i++)
            {
                if (!AddSoundAgentHelper(soundGroupName, soundGroupHelper, i))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <returns>所有正在加载声音的序列编号</returns>
        public int[] GetAllLoadingSoundSerialIds()
        {
            return mSoundManager.GetAllLoadingSoundSerialIds();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号</param>
        public void GetAllLoadingSoundSerialIds(List<int> results)
        {
            mSoundManager.GetAllLoadingSoundSerialIds();
        }

        /// <summary>
        /// 是否正在加载声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <returns>是否正在加载声音</returns>
        public bool IsLoadingSound(int serialId)
        {
            return mSoundManager.IsLoadingSound(serialId);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundAssetName">声音资源名称</param>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="bindingObject">声音绑定的对象</param>
        /// <param name="priority">加载声音资源优先级</param>
        /// <param name="soundParams">声音参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>声音序列编号</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Vector3 worldPosition,
            GameObject bindingObject = null, int priority = 0, SoundParams soundParams = null,
            object userData = null)
        {
            return mSoundManager.PlaySound(soundAssetName, soundGroupName, priority, soundParams,
                PlaySoundInfo.Create(bindingObject, worldPosition, userData));
        }


        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        /// <returns>是否成功停止播放声音</returns>
        public bool StopSound(int serialId, float fadeOutSeconds = 0)
        {
            return mSoundManager.StopSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 停止播放所有已加载声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public void StopAllLoadedSounds(float fadeOutSeconds = 0)
        {
            mSoundManager.StopAllLoadedSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        public void StopAllLoadingSounds()
        {
            mSoundManager.StopAllLoadingSounds();
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位</param>
        public void PauseSound(int serialId, float fadeOutSeconds = 0)
        {
            mSoundManager.PauseSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialId">声音序列编号</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位</param>
        public void ResumeSound(int serialId, float fadeInSeconds = 0)
        {
            mSoundManager.ResumeSound(serialId, fadeInSeconds);
        }

        private void OnPlaySoundSuccess(object sender, Framework.PlaySoundSuccessEventArgs e)
        {
            var playSoundInfo = e.UserData as PlaySoundInfo;
            if (playSoundInfo != null)
            {
                var soundAgentHelper = e.SoundAgent.SoundAgentHelper as SoundAgentHelperBase;
                if (soundAgentHelper != null)
                {
                    if (playSoundInfo.BindingObject != null)
                    {
                        soundAgentHelper.SetSoundAsset(playSoundInfo.BindingObject);
                    }
                    else
                    {
                        soundAgentHelper.SetWorldPosition(playSoundInfo.WorldPosition);
                    }
                }
            }

            mEventComponent.Fire(this, PlaySoundSuccessEventArgs.Create(e));
        }

        private void OnPlaySoundFailure(object sender, Framework.PlaySoundFailureEventArgs e)
        {
            var errorMessage =
                $"Play sound failure, asset name ({e.SoundAssetName}), sound group name ({e.SoundGroupName}), error code ({e.SoundErrorCode}), error message({e.ErrorMessage})";
            if (e.SoundErrorCode == SoundErrorCode.IgnoreDueToLowPriority)
            {
                Log.Info(errorMessage);
            }
            else
            {
                Log.Warning(errorMessage);
            }

            mEventComponent.Fire(this, PlaySoundFailureEventArgs.Create(e));
        }

        private void OnPlaySoundUpdate(object sender, Framework.PlaySoundUpdateEventArgs e)
        {
            mEventComponent.Fire(this, PlaySoundUpdateEventArgs.Create(e));
        }

        private void OnPlaySoundDependency(object sender, Framework.PlaySoundDependencyEventArgs e)
        {
            mEventComponent.Fire(this, PlaySoundDependencyEventArgs.Create(e));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RefreshAudioListener(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RefreshAudioListener(scene);
        }

        private void RefreshAudioListener(Scene scene)
        {
            mAudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }

        /// <summary>
        /// 增加声音代理辅助器
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupHelper">声音组辅助器</param>
        /// <param name="index">索引</param>
        /// <returns>是否成功增加声音代理辅助器</returns>
        private bool AddSoundAgentHelper(string soundGroupName, SoundGroupHelperBase soundGroupHelper, int index)
        {
            var soundAgentHelper = Helper.CreateHelper(mSoundAgentHelperTypeName, mCustomSoundAgentHelper, index);
            if (soundAgentHelper == null)
            {
                Log.Error("Can not create sound agent helper.");
                return false;
            }

            soundAgentHelper.name = $"Sound Agent Helper - {soundGroupName} - {index}";
            var soundAgentHelperTrans = soundAgentHelper.transform;
            soundAgentHelperTrans.SetParent(soundGroupHelper.transform);
            soundAgentHelperTrans.localScale = Vector3.one;

            if (mAudioMixer != null)
            {
                var audioMixerGroups = mAudioMixer.FindMatchingGroups($"Master/{soundGroupName}");
                soundGroupHelper.AudioMixerGroup = audioMixerGroups.Length > 0
                    ? audioMixerGroups[0]
                    : mAudioMixer.FindMatchingGroups("Master")[0];
            }

            mSoundManager.AddSoundAgentHelper(soundGroupName, soundAgentHelper);
            return true;
        }
    }
}