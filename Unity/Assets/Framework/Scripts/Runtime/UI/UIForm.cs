/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/9 16:16:16
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 界面
    /// </summary>
    public sealed class UIForm : MonoBehaviour, IUIForm
    {
        private int mSerialId;
        private string mUIFormAssetName;
        private IUIGroup mUIGroup;
        private int mDepthInUIGroup;
        private bool mPauseCoveredUIForm;
        private UIFormLogic mUIFormLogic;

        /// <summary>
        /// 界面序列号
        /// </summary>
        public int SerialId => mSerialId;

        /// <summary>
        /// 界面资源名称
        /// </summary>
        public string UIFormAssetName => mUIFormAssetName;

        /// <summary>
        /// 界面实例
        /// </summary>
        public object Handle => gameObject;

        /// <summary>
        /// 界面所属界面组
        /// </summary>
        public IUIGroup UIGroup => mUIGroup;

        /// <summary>
        /// 界面在界面组中的深度
        /// </summary>
        public int DepthInUIGroup => mDepthInUIGroup;

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm => mPauseCoveredUIForm;

        /// <summary>
        /// 界面逻辑
        /// </summary>
        public UIFormLogic UIFormLogic => mUIFormLogic;

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="serialId">界面序列号</param>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroup">界面所属界面组</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面</param>
        /// <param name="isNewInstance">是否为新实例</param>
        /// <param name="userData">用户自定义数据</param>
        public void OnInit(int serialId, string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm,
            bool isNewInstance,
            object userData)
        {
            mSerialId = serialId;
            mUIFormAssetName = uiFormAssetName;
            mUIGroup = uiGroup;
            mDepthInUIGroup = 0;
            mPauseCoveredUIForm = pauseCoveredUIForm;

            if (!isNewInstance)
            {
                return;
            }

            mUIFormLogic = GetComponent<UIFormLogic>();
            if (mUIFormLogic == null)
            {
                Log.Error($"UI form ({uiFormAssetName}) can not get UI form logic.");
                return;
            }

            try
            {
                mUIFormLogic.OnInit(userData);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnInit with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面回收
        /// </summary>
        public void OnRecycle()
        {
            try
            {
                mUIFormLogic.OnRecycle();
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnRecycle with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面打开
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        public void OnOpen(object userData)
        {
            try
            {
                mUIFormLogic.OnOpen(userData);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnOpen with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        /// <param name="isShutdown">是否关闭界面管理器时触发</param>
        /// <param name="userData">用户自定义数据</param>
        public void OnClose(bool isShutdown, object userData)
        {
            try
            {
                mUIFormLogic.OnClose(isShutdown, userData);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnClose with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        public void OnPause()
        {
            try
            {
                mUIFormLogic.OnPause();
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnPause with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        public void OnResume()
        {
            try
            {
                mUIFormLogic.OnResume();
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnResume with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        public void OnCover()
        {
            try
            {
                mUIFormLogic.OnCover();
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnCover with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        public void OnReveal()
        {
            try
            {
                mUIFormLogic.OnReveal();
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnReveal with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面激活
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        public void OnRefocus(object userData)
        {
            try
            {
                mUIFormLogic.OnRefocus(userData);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnRefocus with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                mUIFormLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnUpdate with exception ({e}).");
            }
        }

        /// <summary>
        /// 界面深度改变
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度</param>
        public void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            mDepthInUIGroup = depthInUIGroup;
            try
            {
                mUIFormLogic.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            }
            catch (Exception e)
            {
                Log.Error($"UI form ([{mSerialId}]{mUIFormAssetName}) OnDepthChanged with exception ({e}).");
            }
        }
    }
}