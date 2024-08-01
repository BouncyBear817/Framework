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
        private sealed class DoubleTimeProcessor : GenericDataProcessor<double>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "double";

            public override string[] GetTypeStrings() => new[] { "double", "system.double" };

            public override double Parse(string value)
            {
                return double.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}