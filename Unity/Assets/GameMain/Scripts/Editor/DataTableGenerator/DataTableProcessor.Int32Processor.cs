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
        private sealed class Int32Processor : GenericDataProcessor<int>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "int";

            public override string[] GetTypeStrings() => new[] { "int", "int32", "system.int32" };

            public override int Parse(string value)
            {
                return int.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt32(Parse(value));
            }
        }
    }
}