// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using Framework.Runtime;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class IdProcessor : DataProcessor
        {
            public override Type Type => typeof(int);

            public override bool IsId => true;

            public override bool IsComment => false;

            public override bool IsSystem => false;

            public override string LanguageKeyword => "int";

            public override string[] GetTypeStrings() => new[] { "id" };

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt32(int.Parse(value));
            }
        }
    }
}