// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 10:20:43
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed class ResourceGroup : IResourceGroup
        {
            private readonly string mName;
            private readonly Dictionary<ResourceName, ResourceInfo> mResourceInfos;
            private readonly HashSet<ResourceName> mResourceNames;
            private long mTotalLength;
            private long mTotalCompressedLength;

            public ResourceGroup(string name, Dictionary<ResourceName, ResourceInfo> resourceInfos)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid.");
                }

                if (resourceInfos == null)
                {
                    throw new Exception("Resource infos is invalid.");
                }

                mName = name;
                mResourceInfos = resourceInfos;
                mResourceNames = new HashSet<ResourceName>();
            }

            /// <summary>
            /// 资源组名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 资源组是否准备完毕
            /// </summary>
            public bool Ready => ReadyCount >= TotalCount;

            /// <summary>
            /// 资源组中已准备完成资源数量
            /// </summary>
            public int ReadyCount
            {
                get
                {
                    var readyCount = 0;
                    foreach (var resourceName in mResourceNames)
                    {
                        if (mResourceInfos.TryGetValue(resourceName, out var resourceInfo) && resourceInfo.Ready)
                        {
                            readyCount++;
                        }
                    }

                    return readyCount;
                }
            }

            /// <summary>
            /// 资源组中已准备完成资源大小
            /// </summary>
            public long ReadyLength
            {
                get
                {
                    {
                        var readyLength = 0L;
                        foreach (var resourceName in mResourceNames)
                        {
                            if (mResourceInfos.TryGetValue(resourceName, out var resourceInfo) && resourceInfo.Ready)
                            {
                                readyLength += resourceInfo.Length;
                            }
                        }

                        return readyLength;
                    }
                }
            }

            /// <summary>
            /// 资源组中已准备完成资源压缩后大小
            /// </summary>
            public long ReadyCompressedLength
            {
                get
                {
                    {
                        var readyCompressedLength = 0L;
                        foreach (var resourceName in mResourceNames)
                        {
                            if (mResourceInfos.TryGetValue(resourceName, out var resourceInfo) && resourceInfo.Ready)
                            {
                                readyCompressedLength += resourceInfo.CompressedLength;
                            }
                        }

                        return readyCompressedLength;
                    }
                }
            }

            /// <summary>
            /// 资源组中包含资源数量
            /// </summary>
            public int TotalCount => mResourceNames.Count;

            /// <summary>
            /// 资源组中包含资源总大小
            /// </summary>
            public long TotalLength => mTotalLength;

            /// <summary>
            /// 资源组中包含资源压缩后总大小
            /// </summary>
            public long TotalCompressedLength => mTotalCompressedLength;

            /// <summary>
            /// 资源组的完成进度
            /// </summary>
            public float Progress => mTotalLength > 0L ? (float)ReadyLength / mTotalLength : 1f;

            /// <summary>
            /// 资源组包含的资源名称列表
            /// </summary>
            /// <returns>资源名称列表</returns>
            public string[] GetResourceNames()
            {
                var results = new List<string>();
                foreach (var resourceName in mResourceNames)
                {
                    results.Add(resourceName.FullName);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 资源组包含的资源名称列表
            /// </summary>
            /// <param name="results">资源名称列表</param>
            public void GetResourceNames(List<string> results)
            {
                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var resourceName in mResourceNames)
                {
                    results.Add(resourceName.FullName);
                }
            }

            /// <summary>
            /// 资源组包含的资源名称列表
            /// </summary>
            /// <returns>资源名称列表</returns>
            public ResourceName[] InternalGetResourceNames()
            {
                var results = new List<ResourceName>();
                foreach (var resourceName in mResourceNames)
                {
                    results.Add(resourceName);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 资源组包含的资源名称列表
            /// </summary>
            /// <param name="results">资源名称列表</param>
            public void InternalGetResourceNames(List<ResourceName> results)
            {
                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var resourceName in mResourceNames)
                {
                    results.Add(resourceName);
                }
            }

            /// <summary>
            /// 检查资源组中是否存在资源
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <returns>资源组中是否存在资源</returns>
            public bool HasResource(ResourceName resourceName)
            {
                return mResourceNames.Contains(resourceName);
            }

            /// <summary>
            /// 在资源组中增加资源
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <param name="length">资源大小</param>
            /// <param name="compressedLength">压缩后资源大小</param>
            public void AddResource(ResourceName resourceName, int length, int compressedLength)
            {
                if (!HasResource(resourceName))
                {
                    mResourceNames.Add(resourceName);
                    mTotalLength += length;
                    mTotalCompressedLength += compressedLength;
                }
            }
        }
    }
}