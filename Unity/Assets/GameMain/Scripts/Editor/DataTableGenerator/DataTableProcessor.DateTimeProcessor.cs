// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class DateTimeProcessor : GenericDataProcessor<DateTime>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "DateTime";

            public override string[] GetTypeStrings() => new[] { "datetime", "system.datetime" };

            public override DateTime Parse(string value)
            {
                return DateTime.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value).Ticks);
            }
        }
    }
}