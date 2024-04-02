// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/3/29 14:43:17
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ConfigManager : FrameworkModule, IConfigManager
    {
        private struct ConfigData
        {
            private readonly bool mBoolValue;
            private readonly int mIntValue;
            private readonly float mFloatValue;
            private readonly string mStringValue;

            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                mBoolValue = boolValue;
                mIntValue = intValue;
                mFloatValue = floatValue;
                mStringValue = stringValue;
            }

            public bool BoolValue => mBoolValue;

            public int IntValue => mIntValue;

            public float FloatValue => mFloatValue;

            public string StringValue => mStringValue;
        }
    }
}