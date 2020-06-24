using System;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using CommandDotNet;
using CommandDotNet.IoC.Autofac;

namespace ResilienceDemo.Battery
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<BatteryModule>();
            await using var container = containerBuilder.Build();
            Console.CancelKeyPress += (sender, eventArgs) => Environment.Exit(0);

            while (true)
            {
                var argsLine = Console.ReadLine();
                if (argsLine != null)
                    await new AppRunner<SeniorBatteryOfficer>()
                        .UseDefaultMiddleware()
                        .UseAutofac(container)
                        .RunAsync(argsLine.Split(' '));
                Console.WriteLine("Standing by...");
            }
            
        }
    }
}