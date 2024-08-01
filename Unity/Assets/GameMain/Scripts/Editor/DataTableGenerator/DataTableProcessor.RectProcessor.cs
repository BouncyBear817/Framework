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
        private sealed class RectProcessor : GenericDataProcessor<Rect>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Rect";

            public override string[] GetTypeStrings() => new[] { "rect", "unityengine.rect" };

            public override Rect Parse(string value)
            {
                var splitValue = value.Split(',');
                if (splitValue.Length == 4)
                {
                    return new Rect(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
                }

                return new Rect();
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var rect = Parse(value);
                binaryWriter.Write(rect.x);
                binaryWriter.Write(rect.y);
                binaryWriter.Write(rect.width);
                binaryWriter.Write(rect.height);
            }
        }
    }
}