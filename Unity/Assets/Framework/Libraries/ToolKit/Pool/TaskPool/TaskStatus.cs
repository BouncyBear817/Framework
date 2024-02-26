/************************************************************
* Unity Version: 2022.3.15f1c1
* Author:        bear
* CreateTime:    2024/01/09 14:39:14
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus : byte
    {
        /// <summary>
        /// 未开始
        /// </summary>
        Todo = 0,

        /// <summary>
        /// 执行中
        /// </summary>
        Doing,

        /// <summary>
        /// 完成
        /// </summary>
        Done
    }
}