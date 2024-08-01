// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/17 10:10:50
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Runtime;
using UnityEngine;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private static readonly char[] DataSplitSeparators = new[] { '\t' };
        private static readonly char[] DataTrimSeparators = new[] { '\"' };

        private readonly string[] mNameRow;
        private readonly string[] mTypeRow;
        private readonly string[] mDefaultValueRow;
        private readonly string[] mCommentRow;
        private readonly int mContentStartRow;
        private readonly int mIdColumn;

        private readonly DataProcessor[] mDataProcessors;
        private readonly string[][] mRawValues;
        private readonly string[] mStrings;

        private string mCodeTemplate;
        private DataTableCodeGenerator mCodeGenerator;

        public DataTableProcessor(string dataTableFileName, Encoding encoding, int nameRow, int typeRow, int? defaultValueRow, int? commentRow,
            int contentStartRow, int idColumn)
        {
            if (string.IsNullOrEmpty(dataTableFileName))
            {
                throw new Exception("Data table file name is invalid.");
            }

            if (!dataTableFileName.EndsWith(".txt") && !dataTableFileName.EndsWith(".csv"))
            {
                throw new Exception($"Data table file ({dataTableFileName}) is not a txt or a csv.");
            }

            if (!File.Exists(dataTableFileName))
            {
                throw new Exception($"Data table file ({dataTableFileName}) is not exist.");
            }

            var lines = File.ReadAllLines(dataTableFileName, encoding);
            var rawRowCount = lines.Length;

            var rawColumnCount = 0;
            var rawValues = new List<string[]>();
            for (var i = 0; i < lines.Length; i++)
            {
                var rawValue = lines[i].Split(DataSplitSeparators);
                for (var j = 0; j < rawValue.Length; j++)
                {
                    rawValue[j] = rawValue[j].Trim(DataTrimSeparators);
                }

                if (i == 0)
                {
                    rawColumnCount = rawValue.Length;
                }
                else if (rawValue.Length != rawColumnCount)
                {
                    throw new Exception($"Data table file ({dataTableFileName}), raw column is ({rawColumnCount}), but line ({i}) column is ({rawValue.Length}).");
                }

                rawValues.Add(rawValue);
            }

            mRawValues = rawValues.ToArray();

            if (nameRow < 0 || typeRow < 0 || contentStartRow < 0 || idColumn < 0)
            {
                throw new Exception($"Name row ({nameRow}) or type row ({typeRow}) or content start row ({contentStartRow}) or id column ({idColumn}) is invalid.");
            }

            if (nameRow >= rawRowCount || typeRow >= rawRowCount)
            {
                throw new Exception($"Name row ({nameRow}) or type row ({typeRow}) >= raw row count ({rawRowCount}) is not allow.");
            }

            if ((defaultValueRow.HasValue && defaultValueRow.Value >= rawRowCount) || (commentRow.HasValue && commentRow.Value >= rawRowCount))
            {
                throw new Exception($"Default value row ({defaultValueRow}) or comment row ({commentRow}) >= raw row count ({rawRowCount}) is not allow.");
            }

            if (contentStartRow > rawRowCount)
            {
                throw new Exception($"Content start row ({contentStartRow}) > raw row count ({rawRowCount}) is not allow.");
            }

            if (idColumn >= rawColumnCount)
            {
                throw new Exception($"Id column ({idColumn}) >= raw column count ({rawColumnCount}) is not allow.");
            }

            mNameRow = mRawValues[nameRow];
            mTypeRow = mRawValues[typeRow];
            mDefaultValueRow = defaultValueRow.HasValue ? mRawValues[defaultValueRow.Value] : null;
            mCommentRow = commentRow.HasValue ? mRawValues[commentRow.Value] : null;
            mContentStartRow = contentStartRow;
            mIdColumn = idColumn;

            mDataProcessors = new DataProcessor[rawColumnCount];
            for (var i = 0; i < rawColumnCount; i++)
            {
                if (i == idColumn)
                {
                    mDataProcessors[i] = DataProcessorUtility.GetDataProcessor("id");
                }
                else
                {
                    mDataProcessors[i] = DataProcessorUtility.GetDataProcessor(mTypeRow[i]);
                }
            }

            var strings = new Dictionary<string, int>(StringComparer.Ordinal);
            for (var i = contentStartRow; i < rawRowCount; i++)
            {
                if (IsCommentRow(i))
                {
                    continue;
                }

                for (int j = 0; j < rawColumnCount; j++)
                {
                    if (mDataProcessors[j].LanguageKeyword != "string")
                    {
                        continue;
                    }

                    var str = mRawValues[i][j];
                    if (strings.ContainsKey(str))
                    {
                        strings[str]++;
                    }
                    else
                    {
                        strings[str] = 1;
                    }
                }
            }

            mStrings = strings.OrderBy(value => value.Key).ThenByDescending(value => value.Value).Select(value => value.Key).ToArray();

            mCodeTemplate = null;
            mCodeGenerator = null;
        }

        public int RawRowCount => mRawValues.Length;

        public int RawColumnCount => mRawValues.Length > 0 ? mRawValues[0].Length : 0;

        public int StringCount => mStrings.Length;

        public int ContentStartRow => mContentStartRow;

        public int IdColumn => mIdColumn;

        public bool IsIdColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mDataProcessors[rawColumn].IsId;
        }

        public bool IsCommentRow(int rawRow)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new Exception($"Raw row ({rawRow}) is out of range.");
            }

            return GetValue(rawRow, 0).StartsWith(DataTableConstant.CommentLineSeparator, StringComparison.Ordinal);
        }

        public bool IsCommentColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return string.IsNullOrEmpty(GetName(rawColumn)) || mDataProcessors[rawColumn].IsComment;
        }

        public string GetName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            if (IsIdColumn(rawColumn))
            {
                return "Id";
            }

            return mNameRow[rawColumn];
        }

        public bool IsSystem(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mDataProcessors[rawColumn].IsSystem;
        }

        public Type GetType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mDataProcessors[rawColumn].Type;
        }

        public string GetLanguageKeyword(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mDataProcessors[rawColumn].LanguageKeyword;
        }

        public string GetDefaultValue(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mDefaultValueRow != null ? mDefaultValueRow[rawColumn] : null;
        }

        public string GetComment(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mCommentRow != null ? mCommentRow[rawColumn] : null;
        }

        public string GetValue(int rawRow, int rawColumn)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new Exception($"Raw row ({rawRow}) is out of range.");
            }

            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception($"Raw column ({rawColumn}) is out of range.");
            }

            return mRawValues[rawRow][rawColumn];
        }

        public string GetString(int index)
        {
            if (index < 0 || index > StringCount)
            {
                throw new Exception($"String index ({index}) is out of range.");
            }

            return mStrings[index];
        }

        public int GetStringIndex(string str)
        {
            for (int i = 0; i < StringCount; i++)
            {
                if (mStrings[i] == str)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool GenerateDataFile(string outputFileName)
        {
            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new Exception("Output file name is invalid.");
            }

            try
            {
                using (var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
                    {
                        for (int rawRow = ContentStartRow; rawRow < RawRowCount; rawRow++)
                        {
                            if (IsCommentRow(rawRow))
                            {
                                continue;
                            }

                            var bytes = GetRowBytes(outputFileName, rawRow);
                            binaryWriter.Write7BitEncodedInt32(bytes.Length);
                            binaryWriter.Write(bytes);
                        }
                    }
                }

                Debug.Log($"Parse data table ({outputFileName}) success.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Parse data table ({outputFileName}) failure, exception is ({e}).");
                return false;
            }
        }

        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                mCodeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
                Debug.Log($"Set code template ({codeTemplateFileName}) success.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Set code template ({codeTemplateFileName}) failure, exception is ({e}).");
                return false;
            }
        }

        public void SetCodeGenerator(DataTableCodeGenerator codeGenerator)
        {
            mCodeGenerator = codeGenerator;
        }

        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData = null)
        {
            if (string.IsNullOrEmpty(mCodeTemplate))
            {
                throw new Exception("You must set code template first.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new Exception("Output file name is invalid.");
            }

            try
            {
                var stringBuilder = new StringBuilder(mCodeTemplate);
                mCodeGenerator?.Invoke(this, stringBuilder, userData);

                using (var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(fileStream, encoding))
                    {
                        streamWriter.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log($"Generate code file ({outputFileName}) success.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Generate code file ({outputFileName}) failure, exception is ({e}).");
                return false;
            }
        }

        private byte[] GetRowBytes(string outputFileName, int rawRow)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
                {
                    for (int rawColumn = 0; rawColumn < RawColumnCount; rawColumn++)
                    {
                        if (IsCommentColumn(rawColumn))
                        {
                            continue;
                        }

                        try
                        {
                            mDataProcessors[rawColumn].WriteToStream(this, binaryWriter, GetValue(rawRow, rawColumn));
                        }
                        catch
                        {
                            if (mDataProcessors[rawColumn].IsId || string.IsNullOrEmpty(GetDefaultValue(rawColumn)))
                            {
                                Debug.LogError(
                                    $"Parse raw value failure. OutputFileName({outputFileName}), RawRow({rawRow}), RawColumn({rawColumn}), Name({GetName(rawColumn)}), Type({GetType(rawColumn)}), RawValue({GetValue(rawRow, rawColumn)}).");
                            }
                            else
                            {
                                Debug.LogWarning(
                                    $"Parse raw value failure. OutputFileName({outputFileName}), RawRow({rawRow}), RawColumn({rawColumn}), Name({GetName(rawColumn)}), Type({GetType(rawColumn)}), RawValue({GetValue(rawRow, rawColumn)}).");
                                try
                                {
                                    mDataProcessors[rawColumn].WriteToStream(this, binaryWriter, GetDefaultValue(rawColumn));
                                }
                                catch
                                {
                                    Debug.LogWarning(
                                        $"Parse raw value failure. OutputFileName({outputFileName}), RawRow({rawRow}), RawColumn({rawColumn}), Name({GetName(rawColumn)}), Type({GetType(rawColumn)}), RawValue({GetComment(rawColumn)}).");
                                    return null;
                                }
                            }
                        }
                    }

                    return memoryStream.ToArray();
                }
            }
        }
    }
}