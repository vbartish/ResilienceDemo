using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ResilienceDemo.DivisionControl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton(new ConsoleLifetimeOptions { SuppressStatusMessages = false });
                })
                .Build()
                .RunAsync();
        }
    }
}