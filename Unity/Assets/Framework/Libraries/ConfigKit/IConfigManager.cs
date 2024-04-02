/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/29 14:13:28
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 全局配置管理器接口
    /// </summary>
    public interface IConfigManager : IDataProvider<IConfigManager>
    {
        /// <summary>
        /// 全局配置项数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        int CachedBytesSize { get; }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置全局配置数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">全局配置数据提供者辅助器</param>
        void SetDataProviderHelper(IDataProviderHelper<IConfigManager> dataProviderHelper);

        /// <summary>
        /// 设置全局配置辅助器
        /// </summary>
        /// <param name="configHelper">全局配置辅助器</param>
        void SetConfigHelper(IConfigHelper configHelper);

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize"></param>
        void EnsureCachedBytesSize(int ensureSize);

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        void FreeCachedBytes();

        /// <summary>
        /// 检查是否存在全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>是否存在指定的全局配置项</returns>
        bool HasConfig(string configName);

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的布尔值</returns>
        bool GetBool(string configName);

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        bool GetBool(string configName, bool defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的整数值</returns>
        int GetInt(string configName);

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的整数值</returns>
        int GetInt(string configName, int defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的浮点数值</returns>
        float GetFloat(string configName);

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        float GetFloat(string configName, float defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的字符串</returns>
        string GetString(string configName);

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的字符串</returns>
        string GetString(string configName, string defaultValue);

        /// <summary>
        /// 增加指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="configValue">全局配置项的值</param>
        /// <returns>是否增加全局配置项成功</returns>
        bool AddConfig(string configName, string configValue);

        /// <summary>
        /// 增加指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="boolValue">全局配置项布尔值</param>
        /// <param name="intValue">全局配置项整数值</param>
        /// <param name="floatValue">全局配置项浮点数值</param>
        /// <param name="stringValue">全局配置项字符串</param>
        /// <returns>是否增加全局配置项成功</returns>
        bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue);

        /// <summary>
        /// 移除指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>是否移除指定全局配置项成功</returns>
        bool RemoveConfig(string configName);

        /// <summary>
        /// 清空所有全局配置项
        /// </summary>
        void RemoveAllConfigs();
    }
}