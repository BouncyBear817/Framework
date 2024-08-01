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
        private sealed class ColorProcessor : GenericDataProcessor<Color>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Color";

            public override string[] GetTypeStrings() => new[] { "color", "unityengine.color" };

            public override Color Parse(string value)
            {
                var splitValue = value.Split(',');
                if (splitValue.Length == 4)
                {
                    return new Color(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
                }

                return Color.white;
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