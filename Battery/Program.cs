using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using CommandDotNet.Rendering;
using GrpcDivisionControlUnit;
using Microsoft.Extensions.DependencyInjection;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;

namespace ResilienceDemo.Battery
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddGrpcClient<DivisionControlUnit.DivisionControlUnitClient>(
                configOptions => configOptions.Address = new Uri("http://localhost:5000"));
                
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
            serviceCollection.AddTransient<IConsole, SystemConsole>();
            serviceCollection.AddSingleton<IBattery, Battery>();
            serviceCollection.AddSingleton(provider =>
            {
                var console = provider.GetService<IConsole>();
                var policies = provider.GetService<IReadOnlyPolicyRegistry<string>>();
                var divisionControl = provider.GetService<DivisionControlUnit.DivisionControlUnitClient>();
                var result = new List<IHowitzer>();
                for (var howitzerId = 0; howitzerId < 6; howitzerId++)
                {
                    var howitzer = new Howitzer(howitzerId, console, policies, divisionControl);
                    result.Add(howitzer);
                }
                return result;
            });

            serviceCollection.AddSingleton<SeniorBatteryOfficer>();
            serviceCollection.AddDefaultPolicies();
            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            Console.CancelKeyPress += (sender, eventArgs) => Environment.Exit(0);

            while (true)
            {
                var argsLine = Console.ReadLine();
                if (argsLine != null)
                    await new AppRunner<SeniorBatteryOfficer>()
                        .UseDefaultMiddleware()
                        .UseMicrosoftDependencyInjection(serviceProvider)
                        .RunAsync(argsLine.Split(' '));
                Console.WriteLine("Standing by...");
            }
            
        }
    }
}