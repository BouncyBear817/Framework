// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class CommentProcessor : DataProcessor
        {
            public override Type Type => null;

            public override bool IsId => false;

            public override bool IsComment => true;

            public override bool IsSystem => false;

            public override string LanguageKeyword => null;

            public override string[] GetTypeStrings() => new[] { string.Empty, "#", "comment" };

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
            }
        }
    }
}