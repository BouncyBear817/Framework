// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 17:5:31
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Framework.Runtime
{
   /// <summary>
   /// 游戏配置组件
   /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Setting")]
    public sealed class SettingComponent : FrameworkComponent
    {
        private ISettingManager mSettingManager;

        [SerializeField] private string mSettingHelperTypeName = "Framework.Runtime.DefaultSettingHelper";
        [SerializeField] private SettingHelperBase mCustomSettingHelper = null;

        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        public int Count => mSettingManager.Count;
        
        protected override void Awake()
        {
            base.Awake();

            mSettingManager = FrameworkEntry.GetModule<ISettingManager>();
            if (mSettingManager == null)
            {
                Log.Fatal("Setting manager is invalid.");
                return;
            }

            var settingHelper = Helper.CreateHelper(mSettingHelperTypeName, mCustomSettingHelper);
            if (settingHelper == null)
            {
                Log.Error("Setting helper is invalid.");
                return;
            }

            settingHelper.gameObject.SetHelperTransform("Setting Helper", transform);

            mSettingManager.SetSettingHelper(settingHelper);
        }


        private void Start()
        {
            if (!mSettingManager.Load())
            {
                Log.Error("Load setting failure.");
            }
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <returns>是否成功保存游戏配置</returns>
        public bool Save()
        {
            return mSettingManager.Save();
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        public string[] GetAllSettingNames()
        {
           return mSettingManager.GetAllSettingNames();
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        public void GetAllSettingNames(List<string> results)
        {
           mSettingManager.GetAllSettingNames(results);
        }

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        public bool HasSetting(string settingName)
        {
           return mSettingManager.HasSetting(settingName);
        }

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        public bool RemoveSetting(string settingName)
        {
           return mSettingManager.RemoveSetting(settingName);
        }

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        public void RemoveAllSettings()
        {
           mSettingManager.RemoveAllSettings();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string settingName, bool defaultValue = false)
        {
           return mSettingManager.GetBool(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        public void SetBool(string settingName, bool value)
        {
           mSettingManager.SetBool(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string settingName, int defaultValue = 0)
        {
           return mSettingManager.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        public void SetInt(string settingName, int value)
        {
           mSettingManager.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string settingName, float defaultValue = 0f)
        {
           return mSettingManager.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        public void SetFloat(string settingName, float value)
        {
           mSettingManager.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string settingName, string defaultValue = null)
        {
           return mSettingManager.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        public void SetString(string settingName, string value)
        {
           mSettingManager.SetString(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        public T GetObject<T>(string settingName)
        {
           return mSettingManager.GetObject<T>(settingName);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        public T GetObject<T>(string settingName, T defaultValue)
        {
           return mSettingManager.GetObject(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        /// <typeparam name="T">对象类型</typeparam>
        public void SetObject<T>(string settingName, T value)
        {
           mSettingManager.SetObject(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>读取的对象</returns>
        public object GetObject(Type objectType, string settingName)
        {
           return mSettingManager.GetObject(objectType, settingName);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的对象</returns>
        public object GetObject(Type objectType, string settingName, object defaultValue)
        {
           return mSettingManager.GetObject(objectType, settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        public void SetObject(string settingName, object value)
        {
           mSettingManager.SetObject(settingName, value);
        }
    }
}