using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
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
        private readonly Faker _faker = new Faker();
        private readonly IBattery _battery;
        private readonly IConsole _console;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private double _mainFiringDirection;
        private Meteo _meteo;

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
            [Option(ShortName = "r")] RetryPolicyKey retryPolicyKey = RetryPolicyKey.RetryOnRpcWithJitter,
            [Option(ShortName = "c")] CachePolicyKey cachePolicyKey = CachePolicyKey.InMemoryCache,
            [Option(ShortName = "t")] TimeoutPolicyKey timeoutPolicyKey = TimeoutPolicyKey.DefaultPessimisticTimeout,
            double? latitude = null, double? longitude = null)
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            
            await _battery.ToArms(timeoutPolicyKey);
            var coords = new Coordinate(latitude ?? DefaultLatitude, longitude ?? DefaultLongitude);
            
            var meteoTask = meteoPolicy.ExecuteAndCaptureAsync(_ => GetMeteo(coords), new Context("Get meteo."));
            var registrationTask = retryPolicy.ExecuteAndCaptureAsync(_ => RegisterAsync(coords), new Context("Register unit."));
            await Task.WhenAll(registrationTask, meteoTask);

            var registrationPolicyCapture = registrationTask.Result;
            var meteoPolicyCapture = meteoTask.Result;

            if (registrationPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine(registrationPolicyCapture.FinalException.Message);
            }
            else
            {
                _mainFiringDirection = registrationPolicyCapture.Result.MainFiringDirection;
            }
            
            if (meteoPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine(meteoPolicyCapture.FinalException.Message);
            }
        }

        public async Task RePosition(
            double latitude,
            double longitude,
            [Option(ShortName = "r")] RetryPolicyKey retryPolicyKey = RetryPolicyKey.RetryOnRpcWithJitter,
            [Option(ShortName = "c")] CachePolicyKey cachePolicyKey = CachePolicyKey.InMemoryCache)
        {
            _battery.RePosition(latitude, longitude);
            
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            
            var coords = new Coordinate(latitude, longitude);
            var meteoPolicyResult = await meteoPolicy.ExecuteAndCaptureAsync(
                _ => GetMeteo(coords),
                new Context("Get meteo."));
            if (meteoPolicyResult.Outcome == OutcomeType.Successful)
            {
                _meteo = meteoPolicyResult.Result;
            }
            else
            {
                _console.Out.WriteLine(meteoPolicyResult.FinalException.Message);
            }
            
            var inPositionPolicyResult = await retryPolicy.ExecuteAndCaptureAsync(
                _ => ReportPosition(coords),
                new Context("In position."));

            if (inPositionPolicyResult.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine(meteoPolicyResult.FinalException.Message);
            }
        }

        public async Task Assault(
            double targetLatitude,
            double targetLongitude,
            double directionDeviation,
            [Option(ShortName = "t")] TimeoutPolicyKey timeoutPolicyKey = TimeoutPolicyKey.DefaultOptimisticTimeout,
            [Option(ShortName = "c")] TimeoutPolicyKey correctionTimeoutPolicyKey = TimeoutPolicyKey.DefaultOptimisticTimeout)
        {
            
            var (horizontal, vertical) = GetAngles(targetLatitude, targetLongitude, directionDeviation);
            
            await _battery.Aim(horizontal, vertical, timeoutPolicyKey);
            await _battery.Fire(40, timeoutPolicyKey);

            var correctionTimeoutPolicy = _policyRegistry.Get<IAsyncPolicy>(correctionTimeoutPolicyKey.ToString());
            var correctionFallbackPolicy = Policy
                .Handle<RpcException>()
                .FallbackAsync(
                    async token =>
                    {
                        var coordBoundaries = new CoordinateBoundaries(_battery.Latitude, _battery.Longitude, 2,
                            DistanceUnit.Kilometers);
                        _battery.RePosition(
                            _faker.Random.Double(coordBoundaries.MinLatitude, coordBoundaries.MaxLatitude),
                            _faker.Random.Double(coordBoundaries.MinLongitude, coordBoundaries.MaxLongitude)
                        );
                        var assaultCommand = await _client.InPositionAsync(new Position
                        {
                            Latitude = _battery.Latitude,
                            Longitude = _battery.Longitude
                        }, deadline: DateTime.UtcNow.AddMilliseconds(100));

                        _console.Out.WriteLine(
                            $"Assault command received. Target: lat: {assaultCommand.Position.Latitude}; lon:{assaultCommand.Position.Longitude}" +
                            $" direction: {assaultCommand.DirectionDeviation};");
                    },
                    async exception =>
                    {
                        _console.Out.WriteLine("Could not get correction, falling back to new position.");
                        await Task.CompletedTask;
                    });

            var lastResortPolicy = Policy.Handle<RpcException>().FallbackAsync(token => _battery.Disengage());

            var policyWrap = Policy.WrapAsync(lastResortPolicy, correctionFallbackPolicy, correctionTimeoutPolicy);

            await policyWrap.ExecuteAsync(async (token) =>
                {
                    var correction = await _client.GetCorrectionAsync(
                        new Position
                        {
                            Latitude = _battery.Latitude,
                            Longitude = _battery.Longitude
                        },
                        cancellationToken: token);

                    _console.Out.WriteLine(
                        $"Assault command correction received. Target: lat: {correction.Position.Latitude}; lon:{correction.Position.Longitude}" +
                        $" direction: {correction.DirectionDeviation};");
                },
                CancellationToken.None);

        }

        private (double Horizontal, double Vertical) GetAngles(
            in double targetLatitude,
            in double targetLongitude,
            in double directionDeviation)
        {
            return (Math.Round(_faker.Random.Double(0, 360), 2), Math.Round(_faker.Random.Double(-3, 70), 2));
        }

        private async Task<AssaultCommand> ReportPosition(Coordinate coords)
        {
            var assaultCommand = await _client.InPositionAsync(new Position
                               {
                                   Latitude = coords.Latitude,
                                   Longitude = coords.Longitude
                               }, deadline: DateTime.UtcNow.AddMilliseconds(100));
            
            _console.Out.WriteLine($"Assault command received. Target: lat: {assaultCommand.Position.Latitude}; lon:{assaultCommand.Position.Longitude}" +
                                   $" direction: {assaultCommand.DirectionDeviation};");
            return assaultCommand;
        }

        private async Task<Meteo> GetMeteo(Coordinate coordinate)
        {
            var response = await _client.GetMeteoAsync(new Position
            {
                Latitude = coordinate.Latitude,
                Longitude = coordinate.Longitude
            }, deadline: DateTime.UtcNow.AddMilliseconds(100));

            _console.Out.WriteLine($"Got meteo for (lat: {coordinate.Latitude}; lon: {coordinate.Longitude}):" +
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
                    Position = new Position
                    {
                        Latitude = coordinate.Latitude,
                        Longitude = coordinate.Longitude
                    }
                }, deadline: DateTime.UtcNow.AddMilliseconds(100));
            
            _console.Out.WriteLine($"Unit {_battery.Id} reported for duty. Re-position command: " +
                                   $"lat: {response.Position.Latitude}; lon: {response.Position.Longitude}; " +
                                   $"direction: {response.MainFiringDirection}.");
            
            return response;
        }
    }
}