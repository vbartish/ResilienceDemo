using System;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using GrpcDivisionControlUnit;
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace ResilienceDemo.Battery
{
    public class Battery
    {
        private readonly DivisionControlUnit.DivisionControlUnitClient _client;
        private readonly IConsole _console;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public Battery(
            DivisionControlUnit.DivisionControlUnitClient client,
            IConsole console,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _client = client;
            _console = console;
            _policyRegistry = policyRegistry;
        }
            
        public async Task ReportForDuty([Option] PolicyKey policyKey = PolicyKey.BasicRetryOnRpc)
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(policyKey.ToString());
            var result = await retryPolicy.ExecuteAndCaptureAsync(
                async () => await _client.RegisterUnitAsync(
                    new RegisterArtilleryUnitRequest
                    {
                        UnitId = Guid.NewGuid().ToString(),
                        Longitude = 500,
                        Latitude = 200
                    }, deadline: DateTime.UtcNow.AddMilliseconds(100)));
            _console.Out.WriteLine(result.Outcome == OutcomeType.Failure ? result.FinalException.Message : result.Result.Message );
        }
    }
}