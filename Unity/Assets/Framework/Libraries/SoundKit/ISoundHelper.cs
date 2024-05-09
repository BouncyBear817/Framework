/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/8 10:21:7
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 声音辅助器
    /// </summary>
    public interface ISoundHelper
    {
        /// <summary>
        /// 释放声音资源
        /// </summary>
        /// <param name="soundAsset">声音资源</param>
        void ReleaseSoundAsset(object soundAsset);
    }
}