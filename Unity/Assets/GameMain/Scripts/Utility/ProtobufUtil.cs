/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/26 10:53:13
 * Description:
 * Modify Record:
 *************************************************************/

using Google.Protobuf;

namespace Runtime
{
    public static class ProtobufUtil
    {
        public static byte[] Serialize(object message)
        {
            return ((IMessage)message).ToByteArray();
        }

        public static T Deserialize<T>(byte[] dataBytes) where T : class, IMessage, new()
        {
            var msg = new T();
            msg = msg.Descriptor.Parser.ParseFrom(dataBytes) as T;
            return msg;
        }
    }
}