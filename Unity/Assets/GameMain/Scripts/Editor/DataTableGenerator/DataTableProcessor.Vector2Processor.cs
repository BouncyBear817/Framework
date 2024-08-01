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
        private sealed class Vector2Processor : GenericDataProcessor<Vector2>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Vector2";

            public override string[] GetTypeStrings() => new[] { "vector2", "unityengine.vector2" };

            public override Vector2 Parse(string value)
            {
                var splitValue = value.Split(',');
                if (splitValue.Length == 2)
                {
                    return new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));
                }

                return new Vector2();
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var vector2 = Parse(value);
                binaryWriter.Write(vector2.x);
                binaryWriter.Write(vector2.y);
            }
        }
    }
}