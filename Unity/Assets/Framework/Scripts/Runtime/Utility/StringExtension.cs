/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:   
 * Modify Record: 
 *************************************************************/

namespace Framework.Runtime
{
    public static class StringExtension
    {
        /// <summary>
        /// 从字符串中指定位置处读取一行
        /// </summary>
        /// <param name="rawString">字符串</param>
        /// <param name="position">指定位置，读取后返回下一行的开始位置</param>
        /// <returns>读取的一行字符串</returns>
        public static string ReadLine(this string rawString, ref int position)
        {
            if (position < 0)
            {
                return null;
            }

            var length = rawString.Length;
            var offset = position;
            while (offset < length)
            {
                var ch = rawString[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset > position)
                        {
                            var line = rawString.Substring(position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && position < length && rawString[position] == '\n')
                            {
                                position++;
                            }

                            return line;
                        }

                        offset++;
                        position++;
                        break;
                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                var line = rawString.Substring(position, offset - position);
                position = offset;
                return line;
            }

            return null;
        }
    }
}