// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 11:58:51
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 游戏配置管理器
    /// </summary>
    public class SettingManager : FrameworkModule, ISettingManager
    {
        private ISettingHelper mSettingHelper;

        public SettingManager()
        {
            mSettingHelper = null;
        }

        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        public int Count
        {
            get
            {
                if (mSettingHelper == null)
                {
                    throw new Exception("Setting helper is invalid.");
                }

                return mSettingHelper.Count;
            }
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            Save();
        }


        /// <summary>
        /// 设置游戏配置辅助器
        /// </summary>
        /// <param name="settingHelper">游戏配置辅助器</param>
        public void SetSettingHelper(ISettingHelper settingHelper)
        {
            if (settingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            mSettingHelper = settingHelper;
        }

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        /// <returns>是否成功加载游戏配置</returns>
        public bool Load()
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            return mSettingHelper.Load();
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <returns>是否成功保存游戏配置</returns>
        public bool Save()
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            return mSettingHelper.Save();
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        public string[] GetAllSettingNames()
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            return mSettingHelper.GetAllSettingNames();
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        public void GetAllSettingNames(List<string> results)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            mSettingHelper.GetAllSettingNames(results);
        }

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        public bool HasSetting(string settingName)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.HasSetting(settingName);
        }

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        public bool RemoveSetting(string settingName)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.RemoveSetting(settingName);
        }

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        public void RemoveAllSettings()
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            mSettingHelper.RemoveAllSettings();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string settingName, bool defaultValue = false)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetBool(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        public void SetBool(string settingName, bool value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetBool(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string settingName, int defaultValue = 0)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        public void SetInt(string settingName, int value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string settingName, float defaultValue = 0f)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        public void SetFloat(string settingName, float value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string settingName, string defaultValue = null)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        public void SetString(string settingName, string value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetString(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        public T GetObject<T>(string settingName)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetObject<T>(settingName);
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
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetObject(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        /// <typeparam name="T">对象类型</typeparam>
        public void SetObject<T>(string settingName, T value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetObject(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>读取的对象</returns>
        public object GetObject(Type objectType, string settingName)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetObject(objectType, settingName);
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
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            return mSettingHelper.GetObject(objectType, settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        public void SetObject(string settingName, object value)
        {
            if (mSettingHelper == null)
            {
                throw new Exception("Setting helper is invalid.");
            }

            if (string.IsNullOrEmpty(settingName))
            {
                throw new Exception("Setting name is invalid.");
            }

            mSettingHelper.SetObject(settingName, value);
        }
    }
}