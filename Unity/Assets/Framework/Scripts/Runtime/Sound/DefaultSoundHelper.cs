// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/10 11:42:26
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 默认声音辅助器
    /// </summary>
    public class DefaultSoundHelper : SoundHelperBase
    {
        private ResourceComponent mResourceComponent = null;

        /// <summary>
        /// 释放声音资源
        /// </summary>
        /// <param name="soundAsset">声音资源</param>
        public override void ReleaseSoundAsset(object soundAsset)
        {
            mResourceComponent.UnloadAsset(soundAsset);
        }
        
        private void Start()
        {
            mResourceComponent = MainEntryHelper.GetComponent<ResourceComponent>();
            if (mResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
            }
        }
    }
}