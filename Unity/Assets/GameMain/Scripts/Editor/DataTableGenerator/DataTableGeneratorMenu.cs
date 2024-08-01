// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 10:55:32
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public sealed class DataTableGeneratorMenu : EditorWindow
    {
        [MenuItem("Assets/Tools/Generate Data Table", priority = 1)]
        private static void GenerateDataTable()
        {
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                if (!string.IsNullOrEmpty(path) || path.EndsWith(".txt") || path.EndsWith(".csv"))
                {
                    var str = path.Split('/');
                    var dataTableName = str[str.Length - 1];
                    dataTableName = dataTableName.Replace(".txt", "").Replace(".csv", "");
                    GenerateDataAndCodeFile(dataTableName);
                }
            }

            AssetDatabase.Refresh();
        }

        private static void GenerateDataAndCodeFile(string dataTableName)
        {
            var dataTableProcessor = DataTableGenerator.Create(dataTableName);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
            {
                Debug.LogError($"Check raw data failure. DataTableName = {dataTableName}.");
                return;
            }

            DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
            DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
        }
    }
}