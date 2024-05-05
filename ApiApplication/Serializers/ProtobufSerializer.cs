using ProtoBuf;
using System.IO;

namespace ApiApplication.Serializers
{
    public class ProtobufSerializer : ISerializer
    {
        public T Deserialize<T>(Stream source)
        {
            return Serializer.Deserialize<T>(source);
        }

        public void Serialize<T>(T instance, Stream destination)
        {
            Serializer.Serialize(destination, instance);
        }
    }
}
