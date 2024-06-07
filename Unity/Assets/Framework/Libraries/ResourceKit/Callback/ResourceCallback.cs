namespace Framework
{
    /// <summary>
    /// 使用单机模式并初始化资源完成时回调
    /// </summary>
    public delegate void InitResourcesCompleteCallback();

    /// <summary>
    /// 使用可更新模式并应用资源完成时的回调函数
    /// </summary>
    /// <param name="resourcePackPath">应用的资源包路径。</param>
    /// <param name="result">应用资源包资源结果，全部成功为 true，否则为 false。</param>
    public delegate void ApplyResourcesCompleteCallback(string resourcePackPath, bool result);

    /// <summary>
    /// 使用可更新模式并检查资源完成时的回调函数
    /// </summary>
    /// <param name="movedCount">已移动的资源数量</param>
    /// <param name="removedCount">已移除的资源数量</param>
    /// <param name="updateCount">可更新的资源数量</param>
    /// <param name="updateTotalLength">可更新的资源总大小</param>
    /// <param name="updateTotalCompressedLength">可更新的压缩后总大小</param>
    public delegate void CheckResourcesCompleteCallback(int movedCount, int removedCount, int updateCount,
        long updateTotalLength, long updateTotalCompressedLength);

    /// <summary>
    /// 使用可更新模式并校验资源完成时的回调函数
    /// </summary>
    /// <param name="result">校验资源结果，全部成功为 true，否则为 false。</param>
    public delegate void VerifyResourcesCompleteCallback(bool result);

    /// <summary>
    /// 使用可更新模式并更新资源完成时的回调函数
    /// </summary>
    /// <param name="resourceGroup">更新的资源组</param>
    /// <param name="result">更新资源结果，全部成功为 true，否则为 false。</param>
    public delegate void UpdateResourcesCompleteCallback(IResourceGroup resourceGroup, bool result);

    /// <summary>
    /// 解密资源回调函数
    /// </summary>
    /// <param name="bytes">要解密的资源二进制流</param>
    /// <param name="startIndex">解密二进制流的起始位置</param>
    /// <param name="count">解密二进制流的长度</param>
    /// <param name="name">资源名称</param>
    /// <param name="variant">变体名称</param>
    /// <param name="extension">扩展名称</param>
    /// <param name="storageInReadOnly">资源是否在只读区</param>
    /// <param name="fileSystem">文件系统名称</param>
    /// <param name="loadType">资源加载方式</param>
    /// <param name="length">资源大小</param>
    /// <param name="hashCode">资源哈希值</param>
    public delegate void DecryptResourceCallback(byte[] bytes, int startIndex, int count, string name, string variant,
        string extension, bool storageInReadOnly, string fileSystem, byte loadType, int length, int hashCode);
}