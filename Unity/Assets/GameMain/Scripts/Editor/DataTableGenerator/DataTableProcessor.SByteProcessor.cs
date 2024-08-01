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
        private sealed class SByteProcessor : GenericDataProcessor<sbyte>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "sbyte";

            public override string[] GetTypeStrings() => new[] { "sbyte", "system.sbyte" };

            public override sbyte Parse(string value)
            {
                return sbyte.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}