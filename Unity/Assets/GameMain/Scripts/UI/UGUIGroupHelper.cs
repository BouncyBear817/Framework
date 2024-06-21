// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/4/19 15:14:48
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework.Runtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UGUI界面组辅助器
/// </summary>
public class UGUIGroupHelper : UIGroupHelperBase
{
    public const int DepthFactor = 10000;

    private int mDepth = 0;
    private Canvas mCachedCanvas = null;

    /// <summary>
    /// 设置界面组深度
    /// </summary>
    /// <param name="depth">界面组深度</param>
    public override void SetDepth(int depth)
    {
        mDepth = depth;
        mCachedCanvas.overrideSorting = true;
        mCachedCanvas.sortingOrder = DepthFactor * depth;
    }

    private void Awake()
    {
        mCachedCanvas = gameObject.GetOrAddComponent<Canvas>();
        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        mCachedCanvas.overrideSorting = true;
        mCachedCanvas.sortingOrder = DepthFactor * mDepth;

        var trans = GetComponent<RectTransform>();
        trans.anchorMin = Vector2.zero;
        trans.anchorMax = Vector2.one;
        trans.anchoredPosition = Vector2.zero;
        trans.sizeDelta = Vector2.one;
    }
}