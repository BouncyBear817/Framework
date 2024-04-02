/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:04
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Base")]
    public sealed class BaseComponent : FrameworkComponent
    {
        private float mGameSpeedBeforePause = 0f;
        
        [SerializeField] private int mFrameRate = 30;

        [SerializeField] private float mGameSpeed = 1f;

        [SerializeField] private bool mRunInBackground = true;

        [SerializeField] private bool mNeverSleep = true;

        [SerializeField] private string mVersionHelperTypeName = "Runtime.DefaultVersionHelper";

        [SerializeField] private string mLogHelperTypeName = "Runtime.DefaultLogHelper";

        public int FrameRate
        {
            get => mFrameRate;
            set => mFrameRate = value;
        }

        public float GameSpeed
        {
            get => mGameSpeed;
            set => Time.timeScale = mGameSpeed = value >= 0f ? value : 0f;
        }

        public bool IsGamePause => mGameSpeed <= 0f;

        public bool IsNormalGameSpeed => mGameSpeed == 1f;

        public bool RunInBackground
        {
            get => mRunInBackground;
            set => Application.runInBackground = mRunInBackground = value;
        }

        public bool NeverSleep
        {
            get => mNeverSleep;
            set
            {
                mNeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            InitLogHelper();
            InitVersionHelper();

            Application.targetFrameRate = mFrameRate;
            Time.timeScale = mGameSpeed;
            Application.runInBackground = mRunInBackground;
            Screen.sleepTimeout = mNeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            Application.lowMemory += OnLowMemory;
        }

        private void Update()
        {
            FrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnApplicationQuit()
        {
            Application.lowMemory -= OnLowMemory;
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            FrameworkEntry.Shutdown();
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePause)
            {
                return;
            }

            mGameSpeedBeforePause = mGameSpeed;
            mGameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePause)
            {
                return;
            }

            GameSpeed = mGameSpeedBeforePause;
        }

        /// <summary>
        /// 重置为正常游戏速度
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsGamePause)
            {
                return;
            }

            GameSpeed = 1f;
        }

        public void Shutdown()
        {
            Destroy(gameObject);
        }

        private void InitLogHelper()
        {
            if (string.IsNullOrEmpty(mLogHelperTypeName))
            {
                return;
            }

            var logHelperType = Framework.Utility.Assembly.GetType(mLogHelperTypeName);
            if (logHelperType == null)
            {
                throw new Exception($"Can not find log helper type ({mLogHelperTypeName}).");
            }

            var logHelper = Activator.CreateInstance(logHelperType) as ILogHelper;
            if (logHelper == null)
            {
                throw new Exception($"Can not create log helper instance ({mLogHelperTypeName}).");
            }
            
            Log.SetLogHelper(logHelper);
        }

        private void InitVersionHelper()
        {
            if (string.IsNullOrEmpty(mVersionHelperTypeName))
            {
                return;
            }

            var versionHelperType = Framework.Utility.Assembly.GetType(mVersionHelperTypeName);
            if (versionHelperType == null)
            {
                throw new Exception($"Can not find version helper type ({mVersionHelperTypeName}).");
            }

            var versionHelper = Activator.CreateInstance(versionHelperType) as Framework.Version.IVersionHelper;
            if (versionHelper == null)
            {
                throw new Exception($"Can not create version helper instance ({mVersionHelperTypeName}).");
            }
            
            Framework.Version.SetVersionHelper(versionHelper);
        }

        private void OnLowMemory()
        {
            Log.Info("Low Memory reported...");

            var objectPoolComponent = MainEntry.Helper.GetComponent<ObjectPoolComponent>();
            if (objectPoolComponent != null)
            {
                objectPoolComponent.Release();
            }
        }
    }
}