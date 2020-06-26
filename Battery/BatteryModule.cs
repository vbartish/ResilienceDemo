using System.Collections.Generic;
using Autofac;
using CommandDotNet.Rendering;
using GrpcContract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using DivisionControlUnitClient = GrpcDivisionControlUnit.DivisionControlUnit.DivisionControlUnitClient;
    
namespace ResilienceDemo.Battery
{
    public class BatteryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemConsole>().As<IConsole>().InstancePerLifetimeScope();
            builder.RegisterType<Battery>().As<IBattery>().SingleInstance();
            builder.Register(context =>
            {
                var console = context.Resolve<IConsole>();
                var policies = context.Resolve<IReadOnlyPolicyRegistry<string>>();
                var divisionControl = context.Resolve<DivisionControlUnitClient>();
                var result = new List<IHowitzer>();
                for (var howitzerId = 0; howitzerId < 6; howitzerId++)
                {
                    var howitzer = new Howitzer(howitzerId, console, policies, divisionControl);
                    result.Add(howitzer);
                }
                return result;
            }).As<List<IHowitzer>>().SingleInstance();
            builder.RegisterType<SeniorBatteryOfficer>().AsSelf().SingleInstance();

            builder.RegisterType<OptionsManager<MemoryCacheOptions>>().As<IOptions<MemoryCacheOptions>>().SingleInstance();
            builder.RegisterType<OptionsManager<MemoryCacheOptions>>().As<IOptionsSnapshot<MemoryCacheOptions>>().InstancePerLifetimeScope();
            builder.RegisterType<OptionsMonitor<MemoryCacheOptions>>().As<IOptionsMonitor<MemoryCacheOptions>>().SingleInstance();
            builder.RegisterType<OptionsFactory<MemoryCacheOptions>>().As<IOptionsFactory<MemoryCacheOptions>>().InstancePerDependency();
            builder.RegisterType<OptionsCache<MemoryCacheOptions>>().As<IOptionsMonitorCache<MemoryCacheOptions>>().SingleInstance();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
            builder.RegisterType<MemoryCacheProvider>().As<IAsyncCacheProvider>().SingleInstance();
            
            builder.AddGrpcClient(
                "localhost", 
                50050,
                channel => new DivisionControlUnitClient(channel));
            builder.AddDefaultPolicies();

        }
    }
}