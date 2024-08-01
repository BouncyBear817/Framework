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
        private sealed class StringProcessor : GenericDataProcessor<string>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "string";

            public override string[] GetTypeStrings() => new[] { "string", "system.string" };

            public override string Parse(string value)
            {
                return value;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}