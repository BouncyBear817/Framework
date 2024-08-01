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
        private sealed class UInt32Processor : GenericDataProcessor<uint>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "uint";

            public override string[] GetTypeStrings() => new[] { "uint", "uint32", "system.uint32" };

            public override uint Parse(string value)
            {
                return uint.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedUInt32(Parse(value));
            }
        }
    }
}