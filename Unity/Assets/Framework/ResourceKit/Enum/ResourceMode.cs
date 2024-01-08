/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 资源模式
    /// </summary>
    public enum ResourceMode : byte
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,
        
        /// <summary>
        /// 单机模式，适用单机游戏
        /// </summary>
        StandalonePackage,
        
        /// <summary>
        /// 预下载的可更新模式，适用大型游戏
        /// </summary>
        Updatable,
        
        /// <summary>
        /// 使用时下载的可更新模式，适用小型游戏
        /// </summary>
        UpdatableWhilePlaying
    }
}