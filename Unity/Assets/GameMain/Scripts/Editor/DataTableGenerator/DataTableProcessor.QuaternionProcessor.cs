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
        private sealed class QuaternionProcessor : GenericDataProcessor<Quaternion>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "Quaternion";

            public override string[] GetTypeStrings() => new[] { "quaternion", "unityengine.quaternion" };

            public override Quaternion Parse(string value)
            {
                return DataTableExtension.ParseQuaternion(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var quaternion = Parse(value);
                binaryWriter.Write(quaternion.x);
                binaryWriter.Write(quaternion.y);
                binaryWriter.Write(quaternion.z);
                binaryWriter.Write(quaternion.w);
            }
        }
    }
}