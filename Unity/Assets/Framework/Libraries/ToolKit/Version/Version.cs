namespace Framework
{
    /// <summary>
    /// 版本号类
    /// </summary>
    public static partial class Version
    {
        private const string FrameworkVersionString = "2024.01.29";

        private static IVersionHelper sVersionHelper = null;

        /// <summary>
        /// 获取框架版本号
        /// </summary>
        public static string FrameworkVersion => FrameworkVersionString;

        /// <summary>
        /// 游戏版本号
        /// </summary>
        public static string GameVersion
        {
            get
            {
                if (sVersionHelper == null)
                {
                    return string.Empty;
                }

                return sVersionHelper.GameVersion;
            }
        }

        /// <summary>
        /// 内部游戏版号
        /// </summary>
        public static int InternalGameVersion
        {
            get
            {
                if (sVersionHelper == null)
                {
                    return 0;
                }

                return sVersionHelper.InternalGameVersion;
            }
        }

        /// <summary>
        /// 设置版本号辅助器
        /// </summary>
        /// <param name="versionHelper">版本号辅助器</param>
        public static void SetVersionHelper(IVersionHelper versionHelper)
        {
            sVersionHelper = versionHelper;
        }
    }
}