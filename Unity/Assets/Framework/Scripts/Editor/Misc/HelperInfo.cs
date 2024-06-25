// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 10:53:23
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public sealed class HelperInfo<T> where T : MonoBehaviour
    {
        private readonly string mName;

        private SerializedProperty mHelperTypeName;
        private SerializedProperty mCustomHelper;
        private string[] mHelperTypeNames;
        private int mHelperTypeNameIndex;

        public HelperInfo(string name)
        {
            mName = name;

            mHelperTypeName = null;
            mCustomHelper = null;
            mHelperTypeNames = null;
            mHelperTypeNameIndex = 0;
        }

        public void Init(SerializedObject serializedObject)
        {
            mHelperTypeName = serializedObject.FindProperty($"m{mName}HelperTypeName");
            mCustomHelper = serializedObject.FindProperty($"mCustom{mName}Helper");
        }

        public void Draw()
        {
            var displayName = Helper.FindNameForDisplay(mName);
            var selectedIndex = EditorGUILayout.Popup($"{displayName} Helper", mHelperTypeNameIndex, mHelperTypeNames);
            if (selectedIndex != mHelperTypeNameIndex)
            {
                mHelperTypeNameIndex = selectedIndex;
                mHelperTypeName.stringValue = selectedIndex <= 0 ? null : mHelperTypeNames[selectedIndex];
            }

            if (mHelperTypeNameIndex <= 0)
            {
                EditorGUILayout.PropertyField(mCustomHelper);
                if (mCustomHelper.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox($"You must set Custom {displayName} Helper.", MessageType.Error);
                }
            }
        }

        public void Refresh()
        {
            var helperTypeNameList = new List<string>() { Constant.CustomOptionName };
            helperTypeNameList.AddRange(HelperType.GetRuntimeTypeNames(typeof(T)));
            mHelperTypeNames = helperTypeNameList.ToArray();

            mHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(mHelperTypeName.stringValue))
            {
                mHelperTypeNameIndex = helperTypeNameList.IndexOf(mHelperTypeName.stringValue);
                if (mHelperTypeNameIndex <= 0)
                {
                    mHelperTypeNameIndex = 0;
                    mHelperTypeName.stringValue = null;
                }
            }
        }
    }


    public sealed class HelperInfo
    {
        private readonly string mName;
        private readonly Type mType;

        private SerializedProperty mHelperTypeName;
        private string[] mHelperTypeNames;
        private int mHelperTypeNameIndex;

        public HelperInfo(string name, Type type)
        {
            mName = name;
            mType = type;

            mHelperTypeName = null;
            mHelperTypeNames = null;
            mHelperTypeNameIndex = 0;
        }

        public void Init(SerializedObject serializedObject)
        {
            mHelperTypeName = serializedObject.FindProperty($"m{mName}HelperTypeName");
        }

        public void Draw()
        {
            var selectedIndex = EditorGUILayout.Popup($"{mName} Helper", mHelperTypeNameIndex, mHelperTypeNames);
            if (selectedIndex != mHelperTypeNameIndex)
            {
                mHelperTypeNameIndex = selectedIndex;
                mHelperTypeName.stringValue = selectedIndex <= 0 ? null : mHelperTypeNames[selectedIndex];
            }
        }

        public void Refresh()
        {
            var helperTypeNameList = new List<string>() { Constant.NoneOptionName };
            helperTypeNameList.AddRange(HelperType.GetRuntimeTypeNames(mType));
            mHelperTypeNames = helperTypeNameList.ToArray();

            mHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(mHelperTypeName.stringValue))
            {
                mHelperTypeNameIndex = helperTypeNameList.IndexOf(mHelperTypeName.stringValue);
                if (mHelperTypeNameIndex <= 0)
                {
                    mHelperTypeNameIndex = 0;
                    mHelperTypeName.stringValue = null;
                }
            }
        }
    }
}