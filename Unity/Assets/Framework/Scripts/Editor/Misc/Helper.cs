// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 15:34:7
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Text.RegularExpressions;

namespace Framework.Editor
{
    public static class Helper
    {
        public static string FindNameForDisplay(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var str = Regex.Replace(name, "^m", string.Empty);
            str = Regex.Replace(str, @"((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", @" $1").TrimStart();
            return str;
        }
    }
}