// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/4/23 16:49:48
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIAutoBindTool : MonoBehaviour
{
#if UNITY_EDITOR
    [Serializable]
    public class BindData : IComparable
    {
        public string Name;
        public Component BindComponent;

        public BindData(string name, Component bindComponent)
        {
            Name = name;
            BindComponent = bindComponent;
        }

        public int CompareTo(object obj)
        {
            return string.Compare(this.Name, ((BindData)obj).Name, StringComparison.Ordinal);
        }
    }

    public List<BindData> mBindDataList = new List<BindData>();

    [SerializeField] private string mClassName;
    [SerializeField] private string mNamespace;
    [SerializeField] private string mComponentCodePath;
    [SerializeField] private string mMountCodePath;

    public string ClassName => mClassName;

    public string Namespace => mNamespace;

    public string ComponentCodePath => mComponentCodePath;

    public string MountCodePath => mMountCodePath;

#endif

    [SerializeField] public List<Component> mBindComponentList = new List<Component>();

    public T GetBindComponent<T>(int index) where T : Component
    {
        if (index >= mBindComponentList.Count)
        {
            Debug.LogError("Index is invalid.");
            return null;
        }

        var bindComponent = mBindComponentList[index] as T;
        if (bindComponent == null)
        {
            Debug.LogError($"Bind component is invalid, index is {index}.");
            return null;
        }

        return bindComponent;
    }
}