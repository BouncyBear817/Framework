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
        private sealed class FloatProcessor : GenericDataProcessor<float>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "float";

            public override string[] GetTypeStrings() => new[] { "float", "single", "system.single" };

            public override float Parse(string value)
            {
                return float.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}