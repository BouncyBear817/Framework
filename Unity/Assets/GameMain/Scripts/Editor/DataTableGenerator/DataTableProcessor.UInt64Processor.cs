// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;
using Framework.Runtime;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class UInt64Processor : GenericDataProcessor<ulong>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "ulong";

            public override string[] GetTypeStrings() => new[] { "ulong", "uint64", "system.uint64" };

            public override ulong Parse(string value)
            {
                return ulong.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedUInt64(Parse(value));
            }
        }
    }
}