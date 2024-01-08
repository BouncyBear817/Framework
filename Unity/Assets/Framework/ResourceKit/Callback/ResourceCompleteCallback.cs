namespace Framework
{
    /// <summary>
    /// 使用可更新模式并应用资源完成时的回调函数
    /// </summary>
    public delegate void ApplyResourceCompleteCallback(string resourcePackPath, bool result);

    /// <summary>
    /// 使用可更新模式并检查资源完成时的回调函数
    /// </summary>
    public delegate void CheckResourceCompleteCallback(int movedCount, int removedCount, int updateCount,
        long updateTotalLength, long updateTotalCompressedLength);
    
    /// <summary>
    /// 使用可更新模式并校验资源完成时的回调函数
    /// </summary>
    public delegate void VerifyResourceCompleteCallback(bool result);
}