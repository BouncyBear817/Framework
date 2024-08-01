// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/16 14:31:36
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameMain.Editor
{
    public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);

    public static class DataTableGenerator
    {
        private static readonly Regex EndWithNumberRegex = new Regex(@"\d+$");
        private static readonly Regex NameRegex = new Regex(@"^[A-Z][A-Za-z0-9_]*$");

        public static DataTableProcessor Create(string dataTableName)
        {
            return new DataTableProcessor(GetDataTablePath(dataTableName), Encoding.GetEncoding("GB2312"), 1, 2, null, 3, 4, 1);
        }

        public static bool CheckRawData(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                var name = dataTableProcessor.GetName(i);
                if (string.IsNullOrEmpty(name) || name == DataTableConstant.CommentLineSeparator)
                {
                    continue;
                }

                if (!NameRegex.IsMatch(name))
                {
                    Debug.LogWarning($"Check raw data failure. DataTableName({dataTableName}), Name({name}).");
                    return false;
                }
            }

            return true;
        }

        public static void GenerateDataFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            var binaryDataFileName = GetDataTableBytesPath(dataTableName);
            if (!dataTableProcessor.GenerateDataFile(binaryDataFileName) && File.Exists(binaryDataFileName))
            {
                File.Delete(binaryDataFileName);
            }
        }

        public static void GenerateCodeFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            dataTableProcessor.SetCodeTemplate(DataTableConstant.CSharpCodeTemplateFilePath, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            var csharpCodeFileName = GetCSharpCodePath(dataTableName);
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private static string GetDataTablePath(string dataTableName) =>
            Framework.Utility.Path.GetRegularPath(Path.Combine(DataTableConstant.DataTablePath, dataTableName + DataTableConstant.TxtSuffix));

        private static string GetDataTableBytesPath(string dataTableName) =>
            Framework.Utility.Path.GetRegularPath(Path.Combine(DataTableConstant.DataTablePath, dataTableName + DataTableConstant.BytesSuffix));

        private static string GetCSharpCodePath(string dataTableName) =>
            Framework.Utility.Path.GetRegularPath(Path.Combine(DataTableConstant.CSharpCodePath, DataTableConstant.PrefixName + dataTableName + DataTableConstant.CSharpSuffix));

        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            var dataTableName = userData as string;

            codeContent.Replace("__DATA_TABLE_NAME_SPACE__", "GameMain");
            codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1));
            codeContent.Replace("__DATA_TABLE_CLASS_NAME__", DataTableConstant.PrefixName + dataTableName);
            codeContent.Replace("__DATA_TABLE_ID_COMMENT__", dataTableProcessor.GetComment(dataTableProcessor.IdColumn));
            codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_PARSER__", GenerateDataTableParser(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_PROPERTY_ARRAY__", GenerateDataTablePropertyArray(dataTableProcessor));
        }

        private static string GenerateDataTableProperties(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();
            var firstProperty = true;
            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    continue;
                }

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder
                    .AppendLine("        /// <summary>")
                    .AppendFormat("        /// {0}", dataTableProcessor.GetComment(i)).AppendLine()
                    .AppendLine("        /// </summary>")
                    .AppendLine($"        public {dataTableProcessor.GetLanguageKeyword(i)} {dataTableProcessor.GetName(i)}" + " {" + "get; private set;" + "}");
            }

            return stringBuilder.ToString();
        }

        private static string GenerateDataTableParser(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(
                @"        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (var i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            var index = 0;").AppendLine();

            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    stringBuilder.AppendLine("            index++;");
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    stringBuilder.AppendLine("            mId = int.Parse(columnStrings[index++]);");
                    continue;
                }

                if (dataTableProcessor.IsSystem(i))
                {
                    var languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                    if (languageKeyword == "string")
                    {
                        stringBuilder.AppendLine($"            {dataTableProcessor.GetName(i)} = columnStrings[index++];");
                    }
                    else
                    {
                        stringBuilder.AppendLine($"            {dataTableProcessor.GetName(i)} = {languageKeyword}.Parse(columnStrings[index++]);");
                    }
                }
                else
                {
                    stringBuilder.AppendLine(
                        $"            {dataTableProcessor.GetName(i)} = DataTableExtension.Parse{dataTableProcessor.GetType(i).Name}.Parse(columnStrings[index++]);").AppendLine();
                }
            }

            stringBuilder
                .Append(@"
            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (var memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {")
                .AppendLine();

            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    stringBuilder.AppendLine("                    mId = binaryReader.Read7BitEncodedInt32();");
                    continue;
                }

                var languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                if (languageKeyword == "int" || languageKeyword == "uint" || languageKeyword == "long" || languageKeyword == "ulong")
                {
                    stringBuilder.AppendLine($"                    {dataTableProcessor.GetName(i)} = binaryReader.Read7BitEncoded{dataTableProcessor.GetType(i).Name}();");
                }
                else
                {
                    stringBuilder.AppendLine($"                    {dataTableProcessor.GetName(i)} = binaryReader.Read{dataTableProcessor.GetType(i).Name}();");
                }
            }

            stringBuilder.Append(
                @"                }
            }

            GeneratePropertyArray();
            return true;
        }");

            return stringBuilder.ToString();
        }

        private static string GenerateDataTablePropertyArray(DataTableProcessor dataTableProcessor)
        {
            var propertyCollections = new List<PropertyCollection>();

            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i) || dataTableProcessor.IsIdColumn(i))
                {
                    continue;
                }

                var name = dataTableProcessor.GetName(i);
                if (!EndWithNumberRegex.IsMatch(name))
                {
                    continue;
                }

                var propertyCollectionName = EndWithNumberRegex.Replace(name, string.Empty);
                var id = int.Parse(EndWithNumberRegex.Match(name).Value);

                PropertyCollection propertyCollection = null;
                foreach (var pc in propertyCollections)
                {
                    if (pc.Name == propertyCollectionName)
                    {
                        propertyCollection = pc;
                        break;
                    }
                }

                if (propertyCollection == null)
                {
                    propertyCollection = new PropertyCollection(propertyCollectionName, dataTableProcessor.GetLanguageKeyword(i));
                    propertyCollections.Add(propertyCollection);
                }

                propertyCollection.AddItem(id, name);
            }

            var stringBuilder = new StringBuilder();
            bool firstProperty = true;
            foreach (var pc in propertyCollections)
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder
                    .AppendLine($"        private KeyValuePair<int, {pc.LanguageKeyword}>[] m{pc.Name} = null;").AppendLine()
                    .AppendLine($"        public int {pc.Name}Count => m{pc.Name}.Length;").AppendLine()
                    .AppendLine($"        public {pc.LanguageKeyword} Get{pc.Name}(int id)")
                    .AppendLine("        {")
                    .AppendLine($"            foreach (var i in m{pc.Name})")
                    .Append(
                        @"            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }")
                    .AppendLine()
                    .AppendLine()
                    .AppendLine($"            throw new Exception($\"Get{pc.Name} with invalid id ({{id}}).\");")
                    .AppendLine("        }")
                    .AppendLine()
                    .AppendLine($"        public {pc.LanguageKeyword} Get{pc.Name}At(int index)")
                    .AppendLine("        {")
                    .AppendLine($"            if (index < 0 || index >= m{pc.Name}.Length)")
                    .AppendLine("            {")
                    .AppendLine($"                throw new Exception($\"Get{pc.Name}At with invalid index ({{index}}).\");")
                    .AppendLine("            }")
                    .AppendLine()
                    .AppendLine($"            return m{pc.Name}[index].Value;")
                    .AppendLine("        }");
            }

            stringBuilder.Append(@"
        private void GeneratePropertyArray()
        {").AppendLine();

            firstProperty = true;
            foreach (var pc in propertyCollections)
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder
                    .AppendLine($"            m{pc.Name} = new KeyValuePair<int, {pc.LanguageKeyword}>[]")
                    .AppendLine("            {");

                var itemCount = pc.ItemCount;
                for (var i = 0; i < itemCount; i++)
                {
                    var item = pc.GetItem(i);
                    stringBuilder.AppendLine($"                new KeyValuePair<int, {pc.LanguageKeyword}>({item.Key}, {item.Value}),");
                }

                stringBuilder.AppendLine("            };");
            }

            stringBuilder.Append("        }");

            return stringBuilder.ToString();
        }

        private sealed class PropertyCollection
        {
            private readonly string mName;
            private readonly string mLanguageKeyword;
            private readonly List<KeyValuePair<int, string>> mItems;

            public PropertyCollection(string name, string languageKeyword)
            {
                mName = name;
                mLanguageKeyword = languageKeyword;
                mItems = new List<KeyValuePair<int, string>>();
            }

            public string Name => mName;

            public string LanguageKeyword => mLanguageKeyword;

            public int ItemCount => mItems.Count;

            public KeyValuePair<int, string> GetItem(int index)
            {
                if (index < 0 || index > ItemCount)
                {
                    throw new Exception($"GetItem index ({index}) is out of range.");
                }

                return mItems[index];
            }

            public void AddItem(int id, string propertyName)
            {
                mItems.Add(new KeyValuePair<int, string>(id, propertyName));
            }
        }
    }
}