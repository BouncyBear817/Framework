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
        private sealed class Vector4Processor : GenericDataProcessor<Vector4>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Vector4";

            public override string[] GetTypeStrings() => new[] { "vector4", "unityengine.vector4" };

            public override Vector4 Parse(string value)
            {
                var splitValue = value.Split(',');
                if (splitValue.Length == 4)
                {
                    return new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
                }

                return new Vector4();
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var vector4 = Parse(value);
                binaryWriter.Write(vector4.x);
                binaryWriter.Write(vector4.y);
                binaryWriter.Write(vector4.z);
                binaryWriter.Write(vector4.w);
            }
        }
    }
}