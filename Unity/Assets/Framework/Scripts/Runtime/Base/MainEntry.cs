/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 主入口
    /// </summary>
    public sealed partial class MainEntry : MonoBehaviour
    {
        /// <summary>
        /// 框架所在的场景编号
        /// </summary>
        public const int FrameworkSceneId = 0;
        
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }

       
    }
}