using System;
using Autofac;
using Grpc.Core;

namespace GrpcContract
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddGrpcClient<T>(
            this ContainerBuilder builder,
            string host, 
            int port,
            Func<Channel, T> createClient,
            string namedRegistration = null) where T : ClientBase<T>
        {
            var someString = Guid.NewGuid().ToString();
            builder.Register(c => new Channel(
                    host,
                    port,
                    ChannelCredentials.Insecure))
                .Named<Channel>(someString)
                .SingleInstance()
                .OnRelease(c => c.ShutdownAsync().ConfigureAwait(false).GetAwaiter().GetResult());

            if (string.IsNullOrWhiteSpace(namedRegistration))
            {
                builder.Register(c =>
                {
                    var ctx = c.Resolve<IComponentContext>();
                    return createClient(ctx.ResolveNamed<Channel>(someString));
                }).InstancePerLifetimeScope();
                return builder;
            }
            builder.Register(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return createClient(ctx.ResolveNamed<Channel>(someString));
            })            
            .Named<T>(namedRegistration)
            .InstancePerLifetimeScope();
            return builder;
        }
    }
}