using System;
using Framework;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Base")]
    public sealed class BaseComponent : FrameworkComponent
    {
        [SerializeField] private int mFrameRate = 30;

        [SerializeField] private float mGameSpeed = 1f;

        [SerializeField] private bool mRunInBackground = true;

        [SerializeField] private bool mNeverSleep = true;

        [SerializeField] private string mLogHelperTypeName = "Runtime.DefaultLogHelper";

        public int FrameRate
        {
            get => mFrameRate;
            set => mFrameRate = value;
        }

        public float GameSpeed
        {
            get => mGameSpeed;
            set => mGameSpeed = value;
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