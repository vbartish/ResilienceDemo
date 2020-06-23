using System;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using GrpcDivisionControlUnit;
using Polly;

namespace ResilienceDemo.Battery
{
    public class Battery
    {
        private readonly DivisionControlUnit.DivisionControlUnitClient _client;

        public Battery(DivisionControlUnit.DivisionControlUnitClient client)
        {
            _client = client;
        }
            
        public async Task ReportForDuty(IConsole console)
        {
            var retryPolicy = Policy
                .Handle<RpcException>(exception => exception.Status.StatusCode == StatusCode.DeadlineExceeded)
                .RetryAsync(5, async (exception, retryAttempt) =>
                {
                    console.Out.WriteLine($"Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                    await Task.CompletedTask;
                });
            var result = await retryPolicy.ExecuteAndCaptureAsync(
                async () => await _client.RegisterUnitAsync(
                    new RegisterArtilleryUnitRequest
                    {
                        UnitId = Guid.NewGuid().ToString(),
                        Longitude = 500,
                        Latitude = 200
                    }, deadline: DateTime.UtcNow.AddMilliseconds(100)));
            console.Out.WriteLine(result.Outcome == OutcomeType.Failure ? result.FinalException.Message : result.Result.Message );
        }
    }
}