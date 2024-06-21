/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 16:33:7
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework.Runtime
{
    /// <summary>
    /// 关闭游戏框架类型
    /// </summary>
    public enum ShutdownType : byte
    {
        /// <summary>
        /// 仅关闭游戏框架
        /// </summary>
        None = 0,
        
        /// <summary>
        /// 关闭游戏框架并重启游戏
        /// </summary>
        Restart,
        
        /// <summary>
        /// 关闭游戏框架并退出游戏
        /// </summary>
        Quit
    }
}