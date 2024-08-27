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
        
        public static Color32 ParseColor32(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 4)
            {
                return new Color32(byte.Parse(splitValue[0]), byte.Parse(splitValue[1]), byte.Parse(splitValue[2]), byte.Parse(splitValue[3]));
            }

            return new Color32();
        }
        
        public static Color ParseColor(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 4)
            {
                return new Color(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            return Color.white;
        }
        
        public static Quaternion ParseQuaternion(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 4)
            {
                return new Quaternion(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            return new Quaternion();
        }
        
        public static Rect ParseRect(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 4)
            {
                return new Rect(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            return new Rect();
        }
        
        public static Vector2 ParseVector2(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 2)
            {
                return new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));
            }

            return new Vector2();
        }
        
        public static Vector3 ParseVector3(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 3)
            {
                return new Vector3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));
            }

            return new Vector3();
        }
        
        public static Vector4 ParseVector4(string value)
        {
            var splitValue = value.Split(',');
            if (splitValue.Length == 4)
            {
                return new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            return new Vector4();
        }
    }
}