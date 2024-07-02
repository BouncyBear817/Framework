// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 10:44:57
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    /// <summary>
    /// 框架Inspector抽象类
    /// </summary>
    public abstract class FrameworkInspector : UnityEditor.Editor
    {
        private bool mIsCompiling = false;

        public override void OnInspectorGUI()
        {
            if (mIsCompiling && !EditorApplication.isCompiling)
            {
                mIsCompiling = false;
                OnCompileComplete();
            }
            else if (!mIsCompiling && EditorApplication.isCompiling)
            {
                mIsCompiling = true;
                OnCompileStart();
            }
        }

        /// <summary>
        /// 编译开始事件
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件
        /// </summary>
        protected virtual void OnCompileComplete()
        {
            OnRefreshTypeNames();
        }

        protected virtual void OnRefreshTypeNames()
        {
            
        }

        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
        }

        protected bool DrawPropertyField(SerializedProperty serializedProperty)
        {
            return EditorGUILayout.PropertyField(serializedProperty, new GUIContent(Helper.FindNameForDisplay(serializedProperty.name)));
        }
    }
}