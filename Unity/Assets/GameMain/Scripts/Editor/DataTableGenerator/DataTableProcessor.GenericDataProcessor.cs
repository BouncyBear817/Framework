// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/17 15:45:20
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        public abstract class GenericDataProcessor<T> : DataProcessor
        {
            public override Type Type => typeof(T);
            public override bool IsId => false;
            public override bool IsComment => false;

            public abstract T Parse(string value);
        }
    }
}