using System;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using CommandDotNet.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Polly.Caching;
using Polly.Caching.Memory;

namespace ResilienceDemo.Battery
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDivisionControlGrpc()
                .AddMemoryCache()
                .AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>()
                .AddDefaultPolicies()
                .AddArtilleryUnit()
                .AddTransient<IConsole, SystemConsole>()
                .BuildServiceProvider(true);

            Console.CancelKeyPress += (sender, eventArgs) => Environment.Exit(0);

            while (true)
            {
                Console.WriteLine("Standing by...");
                var argsLine = Console.ReadLine();
                if (argsLine != null)
                {
                    await new AppRunner<SeniorBatteryOfficer>()
                        .UseDefaultMiddleware()
                        .UseMicrosoftDependencyInjection(serviceProvider)
                        .RunAsync(argsLine.Split(' '));
                }
            }
        }
    }
}