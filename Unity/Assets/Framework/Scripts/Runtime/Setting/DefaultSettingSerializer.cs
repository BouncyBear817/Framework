// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 15:52:32
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认游戏配置序列化器
    /// </summary>
    public sealed class DefaultSettingSerializer : FrameworkSerializer<DefaultSetting>
    {
        private static readonly byte[] Header = new[] { (byte)'F', (byte)'S', (byte)'S' };

        public DefaultSettingSerializer()
        {
        }

        /// <summary>
        /// 获取数据头标识
        /// </summary>
        /// <returns>数据头标识</returns>
        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}