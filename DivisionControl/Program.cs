using System.Threading.Tasks;
using Autofac;
using GrpcContract;

namespace ResilienceDemo.DivisionControl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GrpcServer.Run((context, builder) =>
            {
                builder.RegisterType<DivisionControlService>().As<IGrpcService>();
            });
        }
    }
}