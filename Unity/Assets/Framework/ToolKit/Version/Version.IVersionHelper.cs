namespace Framework
{
    public static partial class Version
    {
        /// <summary>
        /// 版本号辅助器接口
        /// </summary>
        public interface IVersionHelper
        {
            /// <summary>
            /// 游戏版本号
            /// </summary>
            string GameVersion { get; }

            /// <summary>
            /// 内部游戏版号
            /// </summary>
            int InternalGameVersion { get; }
        }
    }
}