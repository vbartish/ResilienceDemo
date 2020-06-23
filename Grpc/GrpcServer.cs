using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GrpcContract
{
    public class GrpcServer : IHostedService
    {
        private readonly IEnumerable<IGrpcService> _services;
        private readonly IConfiguration _configuration;
        private readonly Server _server;

        public static async Task Run(Action<HostBuilderContext, ContainerBuilder> registerDependencies, CancellationToken shutdownToken = default)
        {
            var builder = new HostBuilder()
                .ConfigureHostConfiguration(configBuilder => configBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, false)
                    .AddEnvironmentVariables())
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostBuilder, containerBuilder) =>
                {
                    registerDependencies?.Invoke(hostBuilder, containerBuilder);
                    containerBuilder.RegisterType<GrpcServer>().As<IHostedService>().SingleInstance();
                    containerBuilder.Register(_ => new ConsoleLifetimeOptions {SuppressStatusMessages = false});
                });
            await builder.RunConsoleAsync(shutdownToken);
        }

        public GrpcServer(IEnumerable<IGrpcService> services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
            _server = new Server();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            var descriptors = new List<Google.Protobuf.Reflection.ServiceDescriptor>
            {
                ServerReflection.Descriptor
            };

            foreach (var grpcService in _services)
            {
                _server.Services.Add(grpcService.BindService());
                if (grpcService.Descriptor != null)
                {
                    descriptors.Add(grpcService.Descriptor);
                }
            }
            
            var reflectionServiceImpl = new ReflectionServiceImpl(descriptors);
            _server.Services.Add(ServerReflection.BindService(reflectionServiceImpl));
            var host = _configuration.GetValue("GrpcHost", "localhost");
            var port = _configuration.GetValue("GrpcPort", 50050);

            _server.Ports.Add(host, port, ServerCredentials.Insecure);
            _server.Start();
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.ShutdownAsync();
        }
    }
}