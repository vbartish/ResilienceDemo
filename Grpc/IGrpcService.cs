using Google.Protobuf.Reflection;
using Grpc.Core;

namespace GrpcContract
{
    public interface IGrpcService
    {
        ServerServiceDefinition BindService();
        ServiceDescriptor Descriptor { get; }
    }
}