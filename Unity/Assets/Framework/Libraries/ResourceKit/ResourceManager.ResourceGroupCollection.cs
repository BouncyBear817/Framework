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
        /// <summary>
        /// 资源组集合
        /// </summary>
        private sealed class ResourceGroupCollection : IResourceGroupCollection
        {
            private readonly ResourceGroup[] mResourceGroups;
            private readonly Dictionary<ResourceName, ResourceInfo> mResourceInfos;
            private readonly HashSet<ResourceName> mResourceNames;
            private long mTotalLength;
            private long mTotalCompressedLength;

            public ResourceGroupCollection(ResourceGroup[] resourceGroups,
                Dictionary<ResourceName, ResourceInfo> resourceInfos)
            {
                if (resourceGroups == null || resourceGroups.Length < 1)
                {
                    throw new Exception("Resource groups is invalid.");
                }

                if (resourceInfos == null)
                {
                    throw new Exception("Resource infos is invalid.");
                }

                for (var i = 0; i < resourceGroups.Length; i++)
                {
                    if (resourceGroups[i] == null)
                    {
                        throw new Exception($"Resource group index ({i}) is invalid.");
                    }

                    for (var j = i + 1; j < resourceGroups.Length; j++)
                    {
                        if (resourceGroups[i] == resourceGroups[j])
                        {
                            throw new Exception($"Resource group ({resourceGroups[i].Name}) is duplicated.");
                        }
                    }
                }

                mResourceGroups = resourceGroups;
                mResourceInfos = resourceInfos;
                mResourceNames = new HashSet<ResourceName>();
                mTotalLength = 0L;
                mTotalCompressedLength = 0L;

                var cachedResourceNames = new List<ResourceName>();
                foreach (var resourceGroup in mResourceGroups)
                {
                    resourceGroup.InternalGetResourceNames(cachedResourceNames);
                    foreach (var resourceName in cachedResourceNames)
                    {
                        if (!mResourceInfos.TryGetValue(resourceName, out var resourceInfo))
                        {
                            throw new Exception($"Resource info ({resourceName.FullName}) is invalid.");
                        }

                        if (mResourceNames.Add(resourceName))
                        {
                            mTotalLength += resourceInfo.Length;
                            mTotalCompressedLength += resourceInfo.CompressedLength;
                        }
                    }
                }
            }

            /// <summary>
            /// 资源组集合是否准备完毕
            /// </summary>
            public bool Ready => ReadyCount >= TotalCount;

            /// <summary>
            /// 资源组集合内含资源数量
            /// </summary>
            public int TotalCount => mResourceNames.Count;

            /// <summary>
            /// 资源组集合内已准备完成的资源数量
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
            /// 资源组集合内含资源数量的总大小
            /// </summary>
            public long TotalLength => mTotalLength;

            /// <summary>
            /// 资源组集合内含资源数量的压缩后总大小
            /// </summary>
            public long TotalCompressedLength => mTotalCompressedLength;

            /// <summary>
            /// 资源组集合内已准备完成的资源数量的大小
            /// </summary>
            public long ReadyLength
            {
                get
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

            /// <summary>
            /// 资源组集合内已准备完成的资源数量的压缩后大小
            /// </summary>
            public long ReadyCompressedLength
            {
                get
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

            /// <summary>
            /// 资源组集合的完成进度
            /// </summary>
            public float Progress => mTotalLength > 0L ? (float)ReadyLength / mTotalLength : 1f;

            /// <summary>
            /// 获取资源组集合包含的资源组列表
            /// </summary>
            /// <returns>资源组列表</returns>
            public IResourceGroup[] GetResourceGroups()
            {
                return mResourceGroups as IResourceGroup[];
            }

            /// <summary>
            /// 获取资源组集合包含的资源组名称列表
            /// </summary>
            /// <returns>资源组名称列表</returns>
            public string[] GetResourceNames()
            {
                var index = 0;
                var resourceNames = new string[mResourceNames.Count];
                foreach (var resourceName in mResourceNames)
                {
                    resourceNames[index++] = resourceName.FullName;
                }

                return resourceNames;
            }

            /// <summary>
            /// 获取资源组集合包含的资源组名称列表
            /// </summary>
            /// <param name="results">资源组名称列表</param>
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
        }
    }
}