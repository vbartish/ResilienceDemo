// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: DivisionControlUnit.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GrpcDivisionControlUnit {
  public static partial class DivisionControlUnit
  {
    static readonly string __ServiceName = "GrpcDivisionControlUnit.DivisionControlUnit";

    static readonly grpc::Marshaller<global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest> __Marshaller_GrpcDivisionControlUnit_RegisterArtilleryUnitRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GrpcDivisionControlUnit.RePositionCommand> __Marshaller_GrpcDivisionControlUnit_RePositionCommand = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcDivisionControlUnit.RePositionCommand.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GrpcDivisionControlUnit.Position> __Marshaller_GrpcDivisionControlUnit_Position = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcDivisionControlUnit.Position.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GrpcDivisionControlUnit.Meteo> __Marshaller_GrpcDivisionControlUnit_Meteo = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcDivisionControlUnit.Meteo.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GrpcDivisionControlUnit.AssaultCommand> __Marshaller_GrpcDivisionControlUnit_AssaultCommand = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcDivisionControlUnit.AssaultCommand.Parser.ParseFrom);

    static readonly grpc::Method<global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest, global::GrpcDivisionControlUnit.RePositionCommand> __Method_RegisterUnit = new grpc::Method<global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest, global::GrpcDivisionControlUnit.RePositionCommand>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterUnit",
        __Marshaller_GrpcDivisionControlUnit_RegisterArtilleryUnitRequest,
        __Marshaller_GrpcDivisionControlUnit_RePositionCommand);

    static readonly grpc::Method<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.Meteo> __Method_GetMeteo = new grpc::Method<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.Meteo>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetMeteo",
        __Marshaller_GrpcDivisionControlUnit_Position,
        __Marshaller_GrpcDivisionControlUnit_Meteo);

    static readonly grpc::Method<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.AssaultCommand> __Method_InPosition = new grpc::Method<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.AssaultCommand>(
        grpc::MethodType.Unary,
        __ServiceName,
        "InPosition",
        __Marshaller_GrpcDivisionControlUnit_Position,
        __Marshaller_GrpcDivisionControlUnit_AssaultCommand);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of DivisionControlUnit</summary>
    [grpc::BindServiceMethod(typeof(DivisionControlUnit), "BindService")]
    public abstract partial class DivisionControlUnitBase
    {
      public virtual global::System.Threading.Tasks.Task<global::GrpcDivisionControlUnit.RePositionCommand> RegisterUnit(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GrpcDivisionControlUnit.Meteo> GetMeteo(global::GrpcDivisionControlUnit.Position request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GrpcDivisionControlUnit.AssaultCommand> InPosition(global::GrpcDivisionControlUnit.Position request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for DivisionControlUnit</summary>
    public partial class DivisionControlUnitClient : grpc::ClientBase<DivisionControlUnitClient>
    {
      /// <summary>Creates a new client for DivisionControlUnit</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DivisionControlUnitClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DivisionControlUnit that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DivisionControlUnitClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DivisionControlUnitClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DivisionControlUnitClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::GrpcDivisionControlUnit.RePositionCommand RegisterUnit(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterUnit(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GrpcDivisionControlUnit.RePositionCommand RegisterUnit(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_RegisterUnit, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.RePositionCommand> RegisterUnitAsync(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterUnitAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.RePositionCommand> RegisterUnitAsync(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_RegisterUnit, null, options, request);
      }
      public virtual global::GrpcDivisionControlUnit.Meteo GetMeteo(global::GrpcDivisionControlUnit.Position request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMeteo(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GrpcDivisionControlUnit.Meteo GetMeteo(global::GrpcDivisionControlUnit.Position request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetMeteo, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.Meteo> GetMeteoAsync(global::GrpcDivisionControlUnit.Position request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMeteoAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.Meteo> GetMeteoAsync(global::GrpcDivisionControlUnit.Position request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetMeteo, null, options, request);
      }
      public virtual global::GrpcDivisionControlUnit.AssaultCommand InPosition(global::GrpcDivisionControlUnit.Position request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return InPosition(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GrpcDivisionControlUnit.AssaultCommand InPosition(global::GrpcDivisionControlUnit.Position request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_InPosition, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.AssaultCommand> InPositionAsync(global::GrpcDivisionControlUnit.Position request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return InPositionAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GrpcDivisionControlUnit.AssaultCommand> InPositionAsync(global::GrpcDivisionControlUnit.Position request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_InPosition, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DivisionControlUnitClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DivisionControlUnitClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(DivisionControlUnitBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_RegisterUnit, serviceImpl.RegisterUnit)
          .AddMethod(__Method_GetMeteo, serviceImpl.GetMeteo)
          .AddMethod(__Method_InPosition, serviceImpl.InPosition).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, DivisionControlUnitBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_RegisterUnit, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest, global::GrpcDivisionControlUnit.RePositionCommand>(serviceImpl.RegisterUnit));
      serviceBinder.AddMethod(__Method_GetMeteo, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.Meteo>(serviceImpl.GetMeteo));
      serviceBinder.AddMethod(__Method_InPosition, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcDivisionControlUnit.Position, global::GrpcDivisionControlUnit.AssaultCommand>(serviceImpl.InPosition));
    }

  }
}
#endregion