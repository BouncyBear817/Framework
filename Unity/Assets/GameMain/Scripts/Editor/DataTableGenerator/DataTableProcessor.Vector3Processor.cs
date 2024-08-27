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
        private sealed class Vector3Processor : GenericDataProcessor<Vector3>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Vector3";

            public override string[] GetTypeStrings() => new[] { "vector3", "unityengine.vector3" };

            public override Vector3 Parse(string value)
            {
                return DataTableExtension.ParseVector3(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var vector3 = Parse(value);
                binaryWriter.Write(vector3.x);
                binaryWriter.Write(vector3.y);
                binaryWriter.Write(vector3.z);
            }
        }
    }
}