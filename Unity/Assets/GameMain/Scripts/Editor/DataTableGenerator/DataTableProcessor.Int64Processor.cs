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
        private sealed class Int64Processor : GenericDataProcessor<long>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "long";

            public override string[] GetTypeStrings() => new[] { "long", "int64", "system.int64" };

            public override long Parse(string value)
            {
                return long.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt64(Parse(value));
            }
        }
    }
}