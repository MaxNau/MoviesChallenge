using ProtoBuf;
using System.IO;

namespace ApiApplication.Serializers
{
    public interface ISerializer
    {
        void Serialize<T>(T instance, Stream destination);
        T Deserialize<T>(Stream source);
    }
}
