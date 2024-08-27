// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/20 14:40:29
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace GameMain
{
    /// <summary>
    /// 版本信息内容
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool ForceUpdateGame { get; set; }

        /// <summary>
        /// 最新的游戏版本号
        /// </summary>
        public string LatestGameVersion { get; set; }

        /// <summary>
        /// 最新的内部游戏版号
        /// </summary>
        public int InternalGameVersion { get; set; }

        /// <summary>
        /// 最新的内部资源版本号
        /// </summary>
        public int InternalResourceVersion { get; set; }

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        public string UpdatePrefixUri { get; set; }

        /// <summary>
        /// 资源版本列表长度
        /// </summary>
        public int VersionListLength { get; set; }

        /// <summary>
        /// 资源版本列表哈希值
        /// </summary>
        public int VersionListHashCode { get; set; }

        /// <summary>
        /// 资源版本列表长度
        /// </summary>
        public int VersionListCompressedLength { get; set; }

        /// <summary>
        /// 资源版本列表长度
        /// </summary>
        public int VersionListCompressedHashCode { get; set; }
    }
}