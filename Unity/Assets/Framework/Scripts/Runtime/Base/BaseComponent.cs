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
    /// <summary>
    /// 基础组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Base")]
    public sealed class BaseComponent : FrameworkComponent
    {
        private const int DefaultDpi = 96;

        private float mGameSpeedBeforePause = 0f;

        [SerializeField] private bool mEditorResourceMode = true;

        [SerializeField] private int mFrameRate = 30;

        [SerializeField] private float mGameSpeed = 1f;

        [SerializeField] private bool mRunInBackground = true;

        [SerializeField] private bool mNeverSleep = true;

        [SerializeField] private string mVersionHelperTypeName = "Runtime.DefaultVersionHelper";

        [SerializeField] private string mLogHelperTypeName = "Runtime.DefaultLogHelper";

        [SerializeField] private string mJsonHelperTypeName = "Runtime.DefaultJsonHelper";

        [SerializeField] private string mCompressionHelperTypeName = "Runtime.DefaultCompressionHelper";

        /// <summary>
        /// 是否使用编辑器资源模式（仅编辑器内有效）
        /// </summary>
        public bool EditorResourceMode
        {
            get => mEditorResourceMode;
            set => mEditorResourceMode = value;
        }

        /// <summary>
        /// 编辑器资源辅助器
        /// </summary>
        public IResourceManager EditorResourceHelper { get; set; }

        /// <summary>
        /// 游戏帧率
        /// </summary>
        public int FrameRate
        {
            get => mFrameRate;
            set => mFrameRate = value;
        }

        /// <summary>
        /// 游戏速度
        /// </summary>
        public float GameSpeed
        {
            get => mGameSpeed;
            set => Time.timeScale = mGameSpeed = value >= 0f ? value : 0f;
        }

        /// <summary>
        /// 游戏是否暂停
        /// </summary>
        public bool IsGamePause => mGameSpeed <= 0f;

        /// <summary>
        /// 是否正常游戏速度
        /// </summary>
        public bool IsNormalGameSpeed => mGameSpeed == 1f;

        /// <summary>
        /// 是否允许后台运行
        /// </summary>
        public bool RunInBackground
        {
            get => mRunInBackground;
            set => Application.runInBackground = mRunInBackground = value;
        }

        /// <summary>
        /// 是否禁止休眠
        /// </summary>
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
            InitJsonHelper();
            InitCompressionHelper();

            Log.Info($"Framework Version : {Framework.Version.FrameworkVersion}");
            Log.Info($"Game Version : {Framework.Version.GameVersion} ({Framework.Version.InternalGameVersion})");
            Log.Info($"Unity Version : {Application.unityVersion}");

            Utility.Converter.ScreenDpi = Screen.dpi;
            if (Utility.Converter.ScreenDpi <= 0)
            {
                Utility.Converter.ScreenDpi = DefaultDpi;
            }

            mEditorResourceMode &= Application.isEditor;
            if (mEditorResourceMode)
            {
                Log.Info("Framework will use editor resource files!");
            }

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

        private void InitJsonHelper()
        {
            if (string.IsNullOrEmpty(mJsonHelperTypeName))
            {
                return;
            }

            var jsonHelperType = Framework.Utility.Assembly.GetType(mJsonHelperTypeName);
            if (jsonHelperType == null)
            {
                throw new Exception($"Can not find json helper type ({mJsonHelperTypeName}).");
            }

            var jsonHelper = Activator.CreateInstance(jsonHelperType) as Utility.Json.IJsonHelper;
            if (jsonHelper == null)
            {
                throw new Exception($"Can not create json helper instance ({mJsonHelperTypeName}).");
            }

            Utility.Json.SetJsonHelper(jsonHelper);
        }

        private void InitCompressionHelper()
        {
            if (string.IsNullOrEmpty(mCompressionHelperTypeName))
            {
                return;
            }

            var compressionHelperType = Framework.Utility.Assembly.GetType(mCompressionHelperTypeName);
            if (compressionHelperType == null)
            {
                throw new Exception($"Can not find compression helper type ({mCompressionHelperTypeName}).");
            }

            var compressionHelper = Activator.CreateInstance(compressionHelperType) as Utility.Compression.ICompressionHelper;
            if (compressionHelper == null)
            {
                throw new Exception($"Can not create compression helper instance ({mCompressionHelperTypeName}).");
            }

            Utility.Compression.SetCompressionHelper(compressionHelper);
        }

        private void OnLowMemory()
        {
            Log.Info("Low Memory reported...");

            var objectPoolComponent = MainEntryHelper.GetComponent<ObjectPoolComponent>();
            if (objectPoolComponent != null)
            {
                objectPoolComponent.Release();
            }

            var resourceComponent = MainEntryHelper.GetComponent<ResourceComponent>();
            if (resourceComponent != null)
            {
                resourceComponent.ForceUnloadUnusedAssets(true);
            }
        }
    }
}