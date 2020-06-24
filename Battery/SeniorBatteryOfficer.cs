using System;
using System.Threading;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Geolocation;
using Grpc.Core;
using GrpcDivisionControlUnit;
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace ResilienceDemo.Battery
{
    public class SeniorBatteryOfficer
    {
        private const double DefaultLatitude = 50.014495;
        private const double DefaultLongitude = 23.730392;
        private readonly DivisionControlUnit.DivisionControlUnitClient _client;
        private readonly IBattery _battery;
        private readonly IConsole _console;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public SeniorBatteryOfficer(
            DivisionControlUnit.DivisionControlUnitClient client,
            IBattery battery,
            IConsole console,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _client = client;
            _battery = battery;
            _console = console;
            _policyRegistry = policyRegistry;
        }
            
        public async Task ToArms(
            [Option] RetryPolicyKey retryPolicyKey = RetryPolicyKey.BasicRetryOnRpc,
            [Option] CachePolicyKey cachePolicyKey = CachePolicyKey.InMemoryCache,
            double? latitude = null, double? longitude = null)
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            _battery.ToArms();
            var coords = new Coordinate(latitude ?? DefaultLatitude, longitude ?? DefaultLongitude);
            var registrationTask = retryPolicy.ExecuteAndCaptureAsync(_ => RegisterAsync(coords), new Context("Register unit."));

            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            var meteoTask = meteoPolicy.ExecuteAndCaptureAsync(_ => GetMeteo(coords), new Context("Get meteo."));
            await Task.WhenAll(registrationTask, meteoTask);

            var registrationPolicyCapture = registrationTask.Result;
            var meteoPolicyCapture = meteoTask.Result;

            if (registrationPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine(registrationPolicyCapture.FinalException.Message);
            }
            
            if (meteoPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine(meteoPolicyCapture.FinalException.Message);
            }
        }

        public async Task RePosition(double latitude, double longitude, [Option] RetryPolicyKey retryPolicyKey = RetryPolicyKey.BasicRetryOnRpc,
            [Option] CachePolicyKey cachePolicyKey = CachePolicyKey.InMemoryCache)
        {
            _battery.RePosition(latitude, longitude);
            
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            var coords = new Coordinate(latitude, longitude);
            
            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            var meteo = await meteoPolicy.ExecuteAndCaptureAsync(_ => GetMeteo(coords), new Context("Get meteo."));
            //TODO[vbart]
        }

        private async Task<Meteo> GetMeteo(Coordinate coordinate)
        {
            var response = await _client.GetMeteoAsync(new GetMeteoRequest
            {
                Latitude = coordinate.Latitude,
                Longitude = coordinate.Longitude
            }, deadline: DateTime.UtcNow.AddMilliseconds(100));

            _console.Out.WriteLine($"Got meteo for (lat: {coordinate.Latitude}; lon: {coordinate.Longitude}): {Environment.NewLine}" +
                                   $" temperature: {response.Temperature}; wind angle: {response.WindAngle};" +
                                   $" wind speed: {response.WindSpeed}; humidity: {response.Humidity}.");
            return response;
        }

        private async Task<RePositionCommand> RegisterAsync(Coordinate coordinate)
        {
            var response = await _client.RegisterUnitAsync(
                new RegisterArtilleryUnitRequest
                {
                    UnitId = _battery.Id.ToString(),
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude
                }, deadline: DateTime.UtcNow.AddMilliseconds(100));
            
            _console.Out.WriteLine($"Unit {_battery.Id} reported for duty. Re-position command: " +
                                   $"lat: {response.Latitude}; lon: {response.Longitude}; direction: {response.MainFiringDirection}.");
            
            return response;
        }
    }
}