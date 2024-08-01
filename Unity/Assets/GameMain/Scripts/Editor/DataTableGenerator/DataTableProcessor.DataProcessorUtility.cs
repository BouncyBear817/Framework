// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/17 15:45:20
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        public static class DataProcessorUtility
        {
            private static readonly IDictionary<string, DataProcessor> sDataProcessors = new SortedDictionary<string, DataProcessor>(StringComparer.Ordinal);

            static DataProcessorUtility()
            {
                var dataProcessorBaseType = typeof(DataProcessor);
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsClass || type.IsAbstract)
                    {
                        continue;
                    }

                    if (dataProcessorBaseType.IsAssignableFrom(type))
                    {
                        var dataProcessor = Activator.CreateInstance(type) as DataProcessor;
                        if (dataProcessor != null)
                        {
                            foreach (var typeString in dataProcessor.GetTypeStrings())
                            {
                                sDataProcessors.Add(typeString.ToLowerInvariant(), dataProcessor);
                            }
                        }
                    }
                }
            }

            public static DataProcessor GetDataProcessor(string type)
            {
                if (type == null)
                {
                    type = string.Empty;
                }

                if (sDataProcessors.TryGetValue(type.ToLowerInvariant(), out var dataProcessor))
                {
                    return dataProcessor;
                }

                throw new Exception($"Not supported data processor type ({type})");
            }
        }
    }
}