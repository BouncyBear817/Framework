/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.IO;
using System.Text;
using Framework;
using UnityEngine;

namespace Runtime
{
    public class DefaultDataTableHelper : DataTableHelperBase
    {
        private static readonly string sBytesAssetExtension = ".byte";

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="dataAsset">数据资源</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否读取数据成功</returns>
        public override bool ReadData(DataTableBase dataProviderOwner, string dataAssetName, object dataAsset,
            object userData)
        {
            var dataTableAsset = dataAsset as TextAsset;
            if (dataTableAsset != null)
            {
                if (dataAssetName.EndsWith(sBytesAssetExtension, StringComparison.Ordinal))
                {
                    return dataProviderOwner.ParseData(dataTableAsset.bytes, userData);
                }
                else
                {
                    return dataProviderOwner.ParseData(dataTableAsset.text, userData);
                }
            }

            Debug.LogWarning($"Data table asset ({dataAssetName}) is invalid.");
            return false;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否读取数据成功</returns>
        public override bool ReadData(DataTableBase dataProviderOwner, string dataAssetName, byte[] dataBytes,
            int startIndex, int length,
            object userData)
        {
            if (dataAssetName.EndsWith(sBytesAssetExtension, StringComparison.Ordinal))
            {
                return dataProviderOwner.ParseData(dataBytes, startIndex, length, userData);
            }
            else
            {
                return dataProviderOwner.ParseData(Framework.Utility.Converter.GetString(dataBytes, startIndex, length),
                    userData);
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        public override bool ParseData(DataTableBase dataProviderOwner, string dataString, object userData)
        {
            try
            {
                var position = 0;
                string dataRowString = null;
                while ((dataRowString = dataString.ReadLine(ref position)) != null)
                {
                    if (dataString[0] == '#')
                    {
                        continue;
                    }

                    if (!dataProviderOwner.AddDataRow(dataRowString, userData))
                    {
                        Log.Warning($"Can not parse data row string ({dataRowString})");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Warning($"Can not parse data row string with exception ({e})");
                return false;
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        public override bool ParseData(DataTableBase dataProviderOwner, byte[] dataBytes, int startIndex, int length,
            object userData)
        {
            try
            {
                using (var memoryStream = new MemoryStream(dataBytes, startIndex, length, false))
                {
                    using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                        {
                            var dataRowBytesLength = binaryReader.Read7BitEncodedInt32();
                            if (!dataProviderOwner.AddDataRow(dataBytes, (int)binaryReader.BaseStream.Position,
                                    dataRowBytesLength, userData))
                            {
                                Log.Warning("Can not parse  data row bytes.");
                                return false;
                            }

                            binaryReader.BaseStream.Position += dataRowBytesLength;
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Warning($"Can not parse dictionary bytes with exception ({e})");
                return false;
            }
        }

        /// <summary>
        /// 释放内容资源
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAsset">内容资源</param>
        public override void ReleaseDataAsset(DataTableBase dataProviderOwner, object dataAsset)
        {
        }
    }
}