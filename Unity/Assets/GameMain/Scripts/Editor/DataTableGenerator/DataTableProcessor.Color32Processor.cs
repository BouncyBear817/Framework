// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 14:9:56
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;
using UnityEngine;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Color32Processor : GenericDataProcessor<Color32>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Color32";

            public override string[] GetTypeStrings() => new[] { "color32", "unityengine.color32" };

            public override Color32 Parse(string value)
            {
                return DataTableExtension.ParseColor32(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var color = Parse(value);
                binaryWriter.Write(color.r);
                binaryWriter.Write(color.g);
                binaryWriter.Write(color.b);
                binaryWriter.Write(color.a);
            }
        }
    }
}