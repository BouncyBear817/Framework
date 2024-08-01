// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/19 16:48:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework;
using Framework.Runtime;
using UnityEngine;

namespace GameMain
{
    public static class DataTableExtension
    {
        private const string DataRowClassPrefixName = "GameMain.DT";
        public static readonly char[] DataSplitSeparators = new[] { '\t' };
        public static readonly char[] DataTrimSeparators = new[] { '\"' };

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, string dataTableAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(dataTableAssetName))
            {
                Log.Warning("Data table asset name is invalid.");
                return;
            }

            var splitedNames = dataTableName.Split('_');
            if (splitedNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            var dataRowClassName = DataRowClassPrefixName + splitedNames[0];
            var dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning($"Can not get data row type with class name ({dataRowClassName}).");
                return;
            }

            var name = splitedNames.Length > 1 ? splitedNames[1] : null;
            var dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
            dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
        }
    }
}