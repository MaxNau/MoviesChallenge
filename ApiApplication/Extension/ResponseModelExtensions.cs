using Google.Protobuf;
using ProtoDefinitions;

namespace ApiApplication.Extension
{
    public static class ResponseModelExtensions
    {
        public static T SafeUnpack<T>(this responseModel responseModel) where T : class, IMessage, new()
        {
            responseModel.Data.TryUnpack<T>(out var data);
            return data;
        }
    }
}
