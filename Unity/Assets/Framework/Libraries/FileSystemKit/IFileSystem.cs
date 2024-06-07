// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 14:32:53
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using System.IO;

namespace Framework
{
    /// <summary>
    /// 文件系统接口
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// 文件系统完整地址
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// 文件系统访问方式
        /// </summary>
        FileSystemAccess Access { get; }

        /// <summary>
        /// 文件数量
        /// </summary>
        int FileCount { get; }

        /// <summary>
        /// 最大文件数量
        /// </summary>
        int MaxFileCount { get; }

        /// <summary>
        /// 获取指定文件信息
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>文件信息</returns>
        FileInfo GetFileInfo(string name);

        /// <summary>
        /// 获取所有文件信息
        /// </summary>
        /// <returns>所有文件信息</returns>
        FileInfo[] GetAllFileInfos();

        /// <summary>
        /// 获取所有文件信息
        /// </summary>
        /// <param name="results">所有文件信息</param>
        void GetAllFileInfos(List<FileInfo> results);

        /// <summary>
        /// 检查是否存在指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>是否存在指定文件</returns>
        bool HasFile(string name);

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>文件的二进制流</returns>
        byte[] ReadFile(string name);

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFile(string name, byte[] buffer);

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <param name="startIndex">文件的二进制流的起始位置</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFile(string name, byte[] buffer, int startIndex);

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件的二进制流</param>
        /// <param name="startIndex">文件的二进制流的起始位置</param>
        /// <param name="length">文件的二进制流的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFile(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">文件的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFile(string name, Stream stream);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="length">片段的长度</param>
        /// <returns>片段的二进制流</returns>
        byte[] ReadFileSegment(string name, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="length">片段的长度</param>
        /// <returns>片段的二进制流</returns>
        byte[] ReadFileSegment(string name, int offset, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, byte[] buffer);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, byte[] buffer, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="startIndex">片段的起始位置</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="buffer">片段的二进制流</param>
        /// <param name="startIndex">片段的起始位置</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, Stream stream, int length);

        /// <summary>
        /// 读取指定文件的指定片段
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="offset">片段的偏移</param>
        /// <param name="stream">片段的二进制流</param>
        /// <param name="length">片段的长度</param>
        /// <returns>实际读取了多少字节</returns>
        int ReadFileSegment(string name, int offset, Stream stream, int length);

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <returns>是否成功写入指定的文件</returns>
        bool WriteFile(string name, byte[] buffer);

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <returns>是否成功写入指定的文件</returns>
        bool WriteFile(string name, byte[] buffer, int startIndex);

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="buffer">文件内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>是否成功写入指定的文件</returns>
        bool WriteFile(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="stream">文件内容的二进制流</param>
        /// <returns>是否成功写入指定的文件</returns>
        bool WriteFile(string name, Stream stream);

        /// <summary>
        /// 写入指定的文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="filePath">存储写入文件内容的文件路径</param>
        /// <returns>是否成功写入指定的文件</returns>
        bool WriteFile(string name, string filePath);

        /// <summary>
        /// 将指定文件另存为物理文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="filePath">存储写入文件内容的文件路径</param>
        /// <returns>是否成功将指定文件另存为物理文件</returns>
        bool SaveAsFile(string name, string filePath);

        /// <summary>
        /// 重命名指定文件
        /// </summary>
        /// <param name="oldName">文件的旧名称</param>
        /// <param name="newName">文件的新名称</param>
        /// <returns>是否成功重命名指定文件</returns>
        bool RenameFile(string oldName, string newName);

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns>是否成功删除指定文件</returns>
        bool DeleteFile(string name);
    }
}