// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/12 13:58:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework.Runtime
{
    /// <summary>
    /// 内置版本资源列表序列化器
    /// </summary>
    public static partial class BuiltinVersionListSerializer
    {
        private const string DefaultExtension = "dat";
        private const int CachedHashBytesLength = 4;

        private static readonly byte[] sCachedHashBytes = new byte[CachedHashBytesLength];

        private static int AssetNameToDependencyAssetNamesComparer(KeyValuePair<string, string[]> a, KeyValuePair<string, string[]> b)
        {
            return string.Compare(a.Key, b.Key, StringComparison.Ordinal);
        }

        private static int GetAssetNameIndex(List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames, string assetName)
        {
            return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, 0, assetNameToDependencyAssetNames.Count - 1);
        }

        private static int GetAssetNameIndexWithBinarySearch(List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames, string assetName, int leftIndex, int rightIndex)
        {
            if (leftIndex > rightIndex)
            {
                return -1;
            }

            var middleIndex = (leftIndex + rightIndex) / 2;
            if (assetNameToDependencyAssetNames[middleIndex].Key == assetName)
            {
                return middleIndex;
            }

            if (string.Compare(assetNameToDependencyAssetNames[middleIndex].Key, assetName, StringComparison.Ordinal) > 0)
            {
                return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, leftIndex, middleIndex - 1);
            }
            else
            {
                return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, middleIndex + 1, rightIndex);
            }
        }
    }
}