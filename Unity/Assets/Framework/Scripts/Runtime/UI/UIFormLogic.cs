/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 16:22:32
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 界面逻辑基类
    /// </summary>
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool mAvailable = false;
        private bool mVisible = false;
        private UIForm mUIForm = null;
        private Transform mCachedTransform = null;
        private int mOriginalLayer = 0;
        
        /// <summary>
        /// 界面名称
        /// </summary>
        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        /// <summary>
        /// 界面是否可用
        /// </summary>
        public bool Available => mAvailable;

        /// <summary>
        /// 界面是否可见
        /// </summary>
        public bool Visible
        {
            get => mVisible && mAvailable;
            set
            {
                if (!mAvailable)
                {
                    Log.Warning($"UI form ({Name}) is not available.");
                    return;
                }

                if (mVisible == value)
                {
                    return;
                }

                mVisible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 界面对象
        /// </summary>
        public UIForm UIForm => mUIForm;

        /// <summary>
        /// 已缓存的界面transform
        /// </summary>
        public Transform CachedTransform => mCachedTransform;

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        protected internal virtual void OnInit(object userData)
        {
            if (mCachedTransform == null)
            {
                mCachedTransform = transform;
            }

            mUIForm = GetComponent<UIForm>();
            mOriginalLayer = gameObject.layer;
        }

        /// <summary>
        /// 界面回收
        /// </summary>
        protected internal virtual void OnRecycle()
        {
        }

        /// <summary>
        /// 界面打开
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        protected internal virtual void OnOpen(object userData)
        {
            mAvailable = true;
            Visible = true;
        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        /// <param name="isShutdown">是否关闭界面管理器时触发</param>
        /// <param name="userData">用户自定义数据</param>
        protected internal virtual void OnClose(bool isShutdown, object userData)
        {
            gameObject.SetLayerRecursively(mOriginalLayer);
            mAvailable = false;
            Visible = false;
        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        protected internal virtual void OnPause()
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        protected internal virtual void OnResume()
        {
            Visible = true;
        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        protected internal virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        protected internal virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        protected internal virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度</param>
        protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        /// <summary>
        /// 设置界面可见性
        /// </summary>
        /// <param name="visible">是否可见</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}