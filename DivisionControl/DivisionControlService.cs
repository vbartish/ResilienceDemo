using System;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Grpc.Core;
using GrpcContract;
using GrpcDivisionControlUnit;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;

namespace ResilienceDemo.DivisionControl
{
    public class DivisionControlService : DivisionControlUnit.DivisionControlUnitBase, IGrpcService
    {
        private const int RegisterUnitSuccessEvery = 5;
        
        private static int _registerUnitCounter;
        
        public ServerServiceDefinition BindService() => DivisionControlUnit.BindService(this);

        ServiceDescriptor IGrpcService.Descriptor => DivisionControlUnit.Descriptor;

        public override Task<RegisterArtilleryUnitResponse> RegisterUnit(RegisterArtilleryUnitRequest request, ServerCallContext context)
        {
            var latencyPolicy = MonkeyPolicy.InjectLatencyAsync<RegisterArtilleryUnitResponse>(
                with => with
                    .Latency(TimeSpan.FromMilliseconds(2000))
                    .InjectionRate(1)
                    .EnabledWhen((_, __) => Task.FromResult(++_registerUnitCounter % RegisterUnitSuccessEvery != 0)));
            return latencyPolicy.ExecuteAsync(() => Task.FromResult(new RegisterArtilleryUnitResponse
            {
                Message = "all good"
            }));
        }
    }
}