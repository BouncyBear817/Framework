/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 默认版本号辅助器
    /// </summary>
    public class DefaultVersionHelper : Version.IVersionHelper
    {
        /// <summary>
        /// 游戏版本号
        /// </summary>
        public string GameVersion => Application.version;

        /// <summary>
        /// 内部游戏版号
        /// </summary>
        public int InternalGameVersion => 0;
    }
}