using Autofac;
using GrpcContract;
using DivisionControlUnitClient = GrpcDivisionControlUnit.DivisionControlUnit.DivisionControlUnitClient;
    
namespace ResilienceDemo.Battery
{
    public class BatteryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Battery>().AsSelf().SingleInstance();
            builder.AddGrpcClient(
                "localhost", 
                50050,
                channel => new DivisionControlUnitClient(channel));
        }
    }
}