/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 10:11:42
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 声音辅助器基类
    /// </summary>
    public abstract class SoundHelperBase : MonoBehaviour, ISoundHelper
    {
        /// <summary>
        /// 释放声音资源
        /// </summary>
        /// <param name="soundAsset">声音资源</param>
        public abstract void ReleaseSoundAsset(object soundAsset);
    }
}