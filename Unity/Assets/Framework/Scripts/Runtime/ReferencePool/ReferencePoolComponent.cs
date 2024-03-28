/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/4 15:03:22
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 引用池组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Reference Pool")]
    public sealed class ReferencePoolComponent : FrameworkComponent
    {
        /// <summary>
        /// 引用强制检查类型
        /// </summary>
        private enum ReferenceStrictCheckType : byte
        {
            /// <summary>
            /// 总是启用
            /// </summary>
            AlwaysEnable = 0,

            /// <summary>
            /// 仅在开发模式下启用
            /// </summary>
            OnlyEnableWhenDevelopment,

            /// <summary>
            /// 仅在编辑器中启用
            /// </summary>
            OnlyEnableInEditor,

            /// <summary>
            /// 总是禁用
            /// </summary>
            AlwaysDisable
        }

        [SerializeField] private ReferenceStrictCheckType mEnableStrictCheck = ReferenceStrictCheckType.AlwaysEnable;

        /// <summary>
        /// 是否启用引用强制检查
        /// </summary>
        public bool EnableStrictCheck
        {
            get => ReferencePool.EnableStrictCheck;
            set
            {
                ReferencePool.EnableStrictCheck = value;
                if (value)
                {
                    Log.Info(
                        "Strict checking is enable for the Reference Pool. It will drastically effect the performance.");
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            switch (mEnableStrictCheck)
            {
                case ReferenceStrictCheckType.AlwaysEnable:
                    EnableStrictCheck = true;
                    break;
                case ReferenceStrictCheckType.OnlyEnableWhenDevelopment:
                    EnableStrictCheck = Debug.isDebugBuild;
                    break;
                case ReferenceStrictCheckType.OnlyEnableInEditor:
                    EnableStrictCheck = Application.isEditor;
                    break;
                case ReferenceStrictCheckType.AlwaysDisable:
                    EnableStrictCheck = false;
                    break;
            }
        }
    }
}