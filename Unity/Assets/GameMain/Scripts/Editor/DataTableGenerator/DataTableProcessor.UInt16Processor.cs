// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class UInt16Processor : GenericDataProcessor<ushort>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "ushort";

            public override string[] GetTypeStrings() => new[] { "ushort", "uint16", "system.uint16" };

            public override ushort Parse(string value)
            {
                return ushort.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}