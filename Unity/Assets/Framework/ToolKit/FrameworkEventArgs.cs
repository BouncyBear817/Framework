/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/25 14:15:11
* Description:   包含事件数据的类的基类
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 包含事件数据的类的基类
    /// </summary>
    public abstract class FrameworkEventArgs : EventArgs , IReference
    {
        public FrameworkEventArgs(){}

        /// <summary>
        /// 清理引用
        /// </summary>
        public abstract void Clear();
    }
}