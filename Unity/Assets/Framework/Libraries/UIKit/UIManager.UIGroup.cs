/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/8 10:40:5
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class UIManager : FrameworkModule, IUIManager
    {
        /// <summary>
        /// 界面组
        /// </summary>
        private sealed partial class UIGroup : IUIGroup
        {
            private readonly string mName;
            private int mDepth;
            private bool mPause;
            private readonly IUIGroupHelper mUIGroupHelper;
            private readonly LinkedList<UIFormInfo> mUIFormInfos;
            private LinkedListNode<UIFormInfo> mCachedNode;

            public UIGroup(string name, int depth, IUIGroupHelper uiGroupHelper)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("UI group name is invalid.");
                }

                if (uiGroupHelper == null)
                {
                    throw new Exception("UI group helper is invalid.");
                }

                mName = name;
                mDepth = depth;
                mPause = false;
                mUIGroupHelper = uiGroupHelper;
                mUIFormInfos = new LinkedList<UIFormInfo>();
                mCachedNode = null;
                Depth = depth;
            }

            /// <summary>
            /// 界面组名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 界面组深度
            /// </summary>
            public int Depth
            {
                get => mDepth;
                set
                {
                    if (mDepth == value)
                    {
                        return;
                    }

                    mDepth = value;
                    mUIGroupHelper.SetDepth(mDepth);
                    Refresh();
                }
            }

            /// <summary>
            /// 界面组是否暂停
            /// </summary>
            public bool Pause
            {
                get => mPause;
                set
                {
                    if (mPause == value)
                    {
                        return;
                    }

                    mPause = value;
                    Refresh();
                }
            }

            /// <summary>
            /// 界面组中界面数量
            /// </summary>
            public int UIFormCount => mUIFormInfos.Count;

            /// <summary>
            /// 界面组中当前界面
            /// </summary>
            public IUIForm CurrentUIForm => mUIFormInfos.First != null ? mUIFormInfos.First.Value.UIForm : null;

            /// <summary>
            /// 界面组辅助器
            /// </summary>
            public IUIGroupHelper GroupHelper => mUIGroupHelper;

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                var current = mUIFormInfos.First;
                while (current != null)
                {
                    if (current.Value.Paused)
                    {
                        break;
                    }

                    mCachedNode = current.Next;
                    current.Value.UIForm.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = mCachedNode;
                    mCachedNode = null;
                }
            }

            /// <summary>
            /// 界面组中是否存在界面
            /// </summary>
            /// <param name="serialId">界面序列号</param>
            /// <returns>界面组中是否存在界面</returns>
            public bool HasUIForm(int serialId)
            {
                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.SerialId == serialId)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 界面组中是否存在界面
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称</param>
            /// <returns>界面组中是否存在界面</returns>
            public bool HasUIForm(string uiFormAssetName)
            {
                if (string.IsNullOrEmpty(uiFormAssetName))
                {
                    throw new Exception("UI form asset name is invalid.");
                }

                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 获取界面组中的界面
            /// </summary>
            /// <param name="serialId">界面序列号</param>
            /// <returns>界面组中的界面</returns>
            public IUIForm GetUIForm(int serialId)
            {
                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.SerialId == serialId)
                    {
                        return info.UIForm;
                    }
                }

                return null;
            }

            /// <summary>
            /// 获取界面组中的界面
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称</param>
            /// <returns>界面组中的界面</returns>
            public IUIForm GetUIForm(string uiFormAssetName)
            {
                if (string.IsNullOrEmpty(uiFormAssetName))
                {
                    throw new Exception("UI form asset name is invalid.");
                }

                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        return info.UIForm;
                    }
                }

                return null;
            }

            /// <summary>
            /// 获取界面组中的界面
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称</param>
            /// <returns>界面组中的界面</returns>
            public IUIForm[] GetUIForms(string uiFormAssetName)
            {
                if (string.IsNullOrEmpty(uiFormAssetName))
                {
                    throw new Exception("UI form asset name is invalid.");
                }

                var results = new List<IUIForm>();
                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        results.Add(info.UIForm);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取界面组中的界面
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称</param>
            /// <param name="results">界面组中的界面</param>
            public void GetUIForms(string uiFormAssetName, List<IUIForm> results)
            {
                if (string.IsNullOrEmpty(uiFormAssetName))
                {
                    throw new Exception("UI form asset name is invalid.");
                }

                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var info in mUIFormInfos)
                {
                    if (info.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        results.Add(info.UIForm);
                    }
                }
            }

            /// <summary>
            /// 获取界面组中的所有界面
            /// </summary>
            /// <returns>界面组中的所有界面</returns>
            public IUIForm[] GetAllUIForms()
            {
                var results = new List<IUIForm>();
                foreach (var info in mUIFormInfos)
                {
                    results.Add(info.UIForm);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取界面组中的所有界面
            /// </summary>
            /// <param name="results">界面组中的所有界面</param>
            public void GetAllUIForms(List<IUIForm> results)
            {
                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var info in mUIFormInfos)
                {
                    results.Add(info.UIForm);
                }
            }

            /// <summary>
            /// 在界面组中增加界面
            /// </summary>
            /// <param name="uiForm">要增加的界面</param>
            public void AddUIForm(IUIForm uiForm)
            {
                mUIFormInfos.AddFirst(UIFormInfo.Create(uiForm));
            }

            /// <summary>
            /// 从界面组移除界面
            /// </summary>
            /// <param name="uiForm">要移除的界面</param>
            /// <exception cref="Exception"></exception>
            public void RemoveUIForm(IUIForm uiForm)
            {
                var uiFormInfo = GetUIFormInfo(uiForm);
                if (uiFormInfo == null)
                {
                    throw new Exception(
                        $"Can not find UI form info for serial id ({uiForm.SerialId}), UI form asset name is ({uiForm.UIFormAssetName}).");
                }

                if (!uiFormInfo.Covered)
                {
                    uiFormInfo.Covered = true;
                    uiForm.OnCover();
                }

                if (!uiFormInfo.Paused)
                {
                    uiFormInfo.Paused = true;
                    uiForm.OnPause();
                }

                if (mCachedNode != null && mCachedNode.Value.UIForm == uiForm)
                {
                    mCachedNode = mCachedNode.Next;
                }

                if (!mUIFormInfos.Remove(uiFormInfo))
                {
                    throw new Exception(
                        $"UI group ({mName}) not exists specified UI form ([{uiForm.SerialId}]{uiForm.UIFormAssetName}).");
                }

                ReferencePool.Release(uiFormInfo);
            }

            /// <summary>
            /// 激活界面
            /// </summary>
            /// <param name="uiForm">界面</param>
            /// <param name="userData">用户自定义数据</param>
            /// <exception cref="Exception"></exception>
            public void RefocusUIForm(IUIForm uiForm, object userData)
            {
                var uiFormInfo = GetUIFormInfo(uiForm);
                if (uiFormInfo == null)
                {
                    throw new Exception(
                        $"Can not find UI form info for serial id ({uiForm.SerialId}), UI form asset name is ({uiForm.UIFormAssetName}).");
                }

                mUIFormInfos.Remove(uiFormInfo);
                mUIFormInfos.AddFirst(uiFormInfo);
            }

            /// <summary>
            /// 刷新界面组
            /// </summary>
            public void Refresh()
            {
                var current = mUIFormInfos.First;
                var pause = mPause;
                var cover = false;
                var depth = UIFormCount;
                while (current != null && current.Value != null)
                {
                    var next = current.Next;
                    current.Value.UIForm.OnDepthChanged(Depth, depth--);

                    if (pause)
                    {
                        if (!current.Value.Covered)
                        {
                            current.Value.Covered = true;
                            current.Value.UIForm.OnCover();
                        }

                        if (!current.Value.Paused)
                        {
                            current.Value.Paused = true;
                            current.Value.UIForm.OnPause();
                        }
                    }
                    else
                    {
                        if (current.Value.Paused)
                        {
                            current.Value.Paused = false;
                            current.Value.UIForm.OnResume();
                        }

                        if (current.Value.UIForm.PauseCoveredUIForm)
                        {
                            pause = true;
                        }

                        if (cover)
                        {
                            if (!current.Value.Covered)
                            {
                                current.Value.Covered = true;
                                current.Value.UIForm.OnCover();
                            }
                        }
                        else
                        {
                            if (current.Value.Covered)
                            {
                                current.Value.Covered = false;
                                current.Value.UIForm.OnReveal();
                            }

                            cover = true;
                        }
                    }

                    current = next;
                }
            }

            internal void InternalGetUIForms(string uiFormAssetName, List<IUIForm> results)
            {
                foreach (var uiFormInfo in mUIFormInfos)
                {
                    if (uiFormInfo.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        results.Add(uiFormInfo.UIForm);
                    }
                }
            }

            internal void InternalGetAllUIForms(List<IUIForm> results)
            {
                foreach (var uiFormInfo in mUIFormInfos)
                {
                    results.Add(uiFormInfo.UIForm);
                }
            }

            private UIFormInfo GetUIFormInfo(IUIForm uiForm)
            {
                if (uiForm == null)
                {
                    throw new Exception("UI form is invalid.");
                }

                foreach (var uiFormInfo in mUIFormInfos)
                {
                    if (uiFormInfo.UIForm == uiForm)
                    {
                        return uiFormInfo;
                    }
                }

                return null;
            }
        }
    }
}