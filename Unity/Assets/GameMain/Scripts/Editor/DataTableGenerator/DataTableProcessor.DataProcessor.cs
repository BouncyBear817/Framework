// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/7/17 15:45:20
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;

namespace GameMain.Editor
{
    public sealed partial class DataTableProcessor
    {
        public abstract class DataProcessor
        {
            public abstract System.Type Type { get; }

            public abstract bool IsId { get; }

            public abstract bool IsComment { get; }

            public abstract bool IsSystem { get; }

            public abstract string LanguageKeyword { get; }

            public abstract string[] GetTypeStrings();

            public abstract void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value);
        }
    }
}