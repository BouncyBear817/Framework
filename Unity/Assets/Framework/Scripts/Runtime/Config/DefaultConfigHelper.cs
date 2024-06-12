/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 14:56:59
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
    public class DefaultConfigHelper : ConfigHelperBase
    {
        private static readonly string[] sColumnSplitSeparator = new string[] { "\t" };
        private static readonly string sBytesAssetExtension = ".bytes";
        private const int ColumnCount = 4;

        private ResourceComponent mResourceComponent = null;

        public override bool ReadData(IConfigManager dataProviderOwner, string dataAssetName, object dataAsset,
            object userData)
        {
            var configTextAsset = dataAsset as TextAsset;
            if (configTextAsset != null)
            {
                if (dataAssetName.EndsWith(sBytesAssetExtension, StringComparison.Ordinal))
                {
                    return dataProviderOwner.ParseData(configTextAsset.bytes, userData);
                }
                else
                {
                    return dataProviderOwner.ParseData(configTextAsset.text, userData);
                }
            }

            Log.Warning($"Config asset ({dataAssetName}) is invalid.");
            return false;
        }

        public override bool ReadData(IConfigManager dataProviderOwner, string dataAssetName, byte[] dataBytes,
            int startIndex, int length,
            object userData)
        {
            if (dataAssetName.EndsWith(sBytesAssetExtension, StringComparison.Ordinal))
            {
                return dataProviderOwner.ParseData(dataBytes, startIndex, length, userData);
            }
            else
            {
                return dataProviderOwner.ParseData(Utility.Converter.GetString(dataBytes, startIndex, length),
                    userData);
            }
        }

        public override bool ParseData(IConfigManager dataProviderOwner, string dataString, object userData)
        {
            try
            {
                var position = 0;
                string configLineString = null;
                while ((configLineString = dataString.ReadLine(ref position)) != null)
                {
                    if (configLineString[0] == '#')
                    {
                        continue;
                    }

                    var splitedLine = configLineString.Split(sColumnSplitSeparator, StringSplitOptions.None);
                    if (splitedLine.Length != ColumnCount)
                    {
                        Log.Warning(
                            $"Can not parse config line string ({configLineString}) while column count is invalid.");
                        return false;
                    }

                    var configName = splitedLine[1];
                    var configValue = splitedLine[3];
                    if (!dataProviderOwner.AddConfig(configName, configValue))
                    {
                        Log.Warning(
                            $"Can not add config with config name ({configName}) which may be invalid or duplicate.");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Warning($"Can not parse config string with exception ({e})");
                throw;
            }
        }

        public override bool ParseData(IConfigManager dataProviderOwner, byte[] dataBytes, int startIndex, int length,
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
                            var configName = binaryReader.ReadString();
                            var configValue = binaryReader.ReadString();
                            if (!dataProviderOwner.AddConfig(configName, configValue))
                            {
                                Log.Warning(
                                    $"Can not add config with config name ({configName}) which may be invalid or duplicate.");
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Warning($"Can not parse config string with exception ({e})");
                throw;
            }
        }

        public override void ReleaseDataAsset(IConfigManager dataProviderOwner, object dataAsset)
        {
            mResourceComponent.UnloadAsset(dataAsset);
        }

        private void Start()
        {
            mResourceComponent = MainEntryHelper.GetComponent<ResourceComponent>();
            if (mResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
            }
        }
    }
}