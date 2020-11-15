using CommandDotNet.Rendering;
using GrpcDivisionControlUnit;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using System;
using System.Collections.Generic;

namespace ResilienceDemo.Battery
{
    internal static class ServiceCollectionContainerBuilderExtensions
    {
        public static IServiceCollection AddDivisionControlGrpc(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddGrpcClient<DivisionControlUnit.DivisionControlUnitClient>(
                configOptions => configOptions.Address = new Uri(Defaults.DefaultDivisionControlAddress));
            return serviceCollection;
        }
        public static IServiceCollection AddArtilleryUnit(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var console = provider.GetService<IConsole>();
                var policies = provider.GetService<IReadOnlyPolicyRegistry<string>>();
                var divisionControl = provider.GetService<DivisionControlUnit.DivisionControlUnitClient>();
                var howitzers = new List<IHowitzer>();
                for (var howitzerId = 0; howitzerId < Defaults.DefaultHowitzersPerBattery; howitzerId++)
                {
                    var howitzer = new Howitzer(howitzerId, console, policies, divisionControl);
                    howitzers.Add(howitzer);
                }
                return new Battery(console, howitzers, policies);
            });

            serviceCollection.AddSingleton<SeniorBatteryOfficer>();
            return serviceCollection;
        }
        
    }
}
