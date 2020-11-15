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
using Polly.Timeout;

namespace ResilienceDemo.Battery
{
    public class SeniorBatteryOfficer
    {
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
            [Option(ShortName = "r")] RetryPolicyKey retryPolicyKey = RetryPolicyKey.NoRetry,
            [Option(ShortName = "c")] CachePolicyKey cachePolicyKey = CachePolicyKey.NoCache,
            [Option(ShortName = "t")] TimeoutPolicyKey timeoutPolicyKey = TimeoutPolicyKey.NoTimeout,
            double? latitude = null, double? longitude = null)
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            
            await _battery.ToArms(timeoutPolicyKey);
            var coords = new Coordinate(latitude ?? Defaults.DefaultLatitude, longitude ?? Defaults.DefaultLongitude);
            
            var meteoTask = meteoPolicy.ExecuteAndCaptureAsync(_ => GetMeteo(coords), new Context("Get meteo"));
            var registrationTask = retryPolicy.ExecuteAndCaptureAsync(_ => RegisterAsync(coords), new Context("Register unit"));
            await Task.WhenAll(registrationTask, meteoTask);

            var registrationPolicyCapture = registrationTask.Result;
            var meteoPolicyCapture = meteoTask.Result;

            if (registrationPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine($"Register unit failed: {registrationPolicyCapture.FinalException.Message}");
            }
            
            if (meteoPolicyCapture.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine($"Get meteo failed: {meteoPolicyCapture.FinalException.Message}");
            }
        }

        public async Task RePosition(
            double latitude,
            double longitude,
            [Option(ShortName = "r")] RetryPolicyKey retryPolicyKey = RetryPolicyKey.NoRetry,
            [Option(ShortName = "c")] CachePolicyKey cachePolicyKey = CachePolicyKey.NoCache)
        {
            _battery.RePosition(latitude, longitude);
            
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy>(retryPolicyKey.ToString());
            var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(cachePolicyKey.ToString());
            var meteoPolicy = Policy.WrapAsync(cachePolicy, retryPolicy);
            
            var coords = new Coordinate(latitude, longitude);
            var meteoPolicyResult = await meteoPolicy.ExecuteAndCaptureAsync(_ => GetMeteo(coords), new Context("Get meteo"));
            var inPositionPolicyResult = await retryPolicy.ExecuteAndCaptureAsync(_ => ReportPosition(coords), new Context("In position."));

            if (meteoPolicyResult.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine($"Get meteo failed: {meteoPolicyResult.FinalException.Message}");
            }

            if (inPositionPolicyResult.Outcome == OutcomeType.Failure)
            {
                _console.Out.WriteLine($"Re-position failed: {inPositionPolicyResult.FinalException.Message}");
            }
        }

        public async Task Assault(
            double targetLatitude,
            double targetLongitude,
            double directionDeviation,
            [Option(ShortName = "t")] TimeoutPolicyKey timeoutPolicyKey = TimeoutPolicyKey.NoTimeout,
            [Option(ShortName = "c")] TimeoutPolicyKey correctionTimeoutPolicyKey = TimeoutPolicyKey.NoTimeout)
        {

            var (horizontal, vertical) = GetAngles(targetLatitude, targetLongitude, _mainFiringDirection + directionDeviation, _meteo);

            await _battery.Aim(horizontal, vertical, timeoutPolicyKey);
            await _battery.Fire(Defaults.DefaultAssaultDensity, timeoutPolicyKey);

            var correctionTimeoutPolicy = _policyRegistry.Get<IAsyncPolicy>(correctionTimeoutPolicyKey.ToString());
            var correctionFallbackPolicy = Policy
                .Handle<RpcException>()
                .Or<TimeoutRejectedException>()
                .FallbackAsync(CorrectionFallbackAction, OnCorrectionFallback);
            var lastResortPolicy = Policy
                .Handle<RpcException>()
                .Or<TimeoutRejectedException>()
                .FallbackAsync(token => _battery.Disengage());
            var policyWrap = Policy.WrapAsync(lastResortPolicy, correctionFallbackPolicy, correctionTimeoutPolicy);

            await policyWrap.ExecuteAsync(CorrectionAction, CancellationToken.None);

            async Task CorrectionAction(CancellationToken token)
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
            }

            async Task CorrectionFallbackAction(CancellationToken token)
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
                }, new CallOptions()
                .WithCancellationToken(token)
                .WithDeadline(Defaults.DefaultGrpcDeadline));

                _console.Out.WriteLine(
                    $"Assault command received. Target: lat: {assaultCommand.Position.Latitude}; lon:{assaultCommand.Position.Longitude}" +
                    $" direction: {assaultCommand.DirectionDeviation};");
            }

            Task OnCorrectionFallback(Exception exception)
            {
                _console.Out.WriteLine($"Could not get correction, falling back to new position. Exception: {exception.Message}");
                return Task.CompletedTask;
            }

            (double Horizontal, double Vertical) GetAngles(
            in double targetLatitude,
            in double targetLongitude,
            in double directionDeviation,
            Meteo _meteo)
            {
                return (Math.Round(_faker.Random.Double(Defaults.MinHorizontalAngle, Defaults.MaxHorizontalAngle), Defaults.RoundingPrecision), 
                    Math.Round(_faker.Random.Double(Defaults.MinVerticalAngle, Defaults.MaxVerticalAngle), Defaults.RoundingPrecision));
            } 
        }

        public async Task BattleReport()
        {
            await _battery.BattleReport();
        }

        private async Task<AssaultCommand> ReportPosition(Coordinate coords)
        {
            var assaultCommand = await _client.InPositionAsync(new Position
                               {
                                   Latitude = coords.Latitude,
                                   Longitude = coords.Longitude
                               }, deadline: Defaults.DefaultGrpcDeadline);
            
            _console.Out.WriteLine($"Assault command received. Target: lat: {assaultCommand.Position.Latitude}; lon:{assaultCommand.Position.Longitude}" +
                                   $" direction: {assaultCommand.DirectionDeviation};");
            return assaultCommand;
        }

        private async Task<Meteo> GetMeteo(Coordinate coordinate)
        {
            _meteo = await _client.GetMeteoAsync(new Position
            {
                Latitude = coordinate.Latitude,
                Longitude = coordinate.Longitude
            }, deadline: Defaults.DefaultGrpcDeadline);

            _console.Out.WriteLine($"Got meteo for (lat: {coordinate.Latitude}; lon: {coordinate.Longitude}):" +
                                   $" temperature: {_meteo.Temperature}; wind angle: {_meteo.WindAngle};" +
                                   $" wind speed: {_meteo.WindSpeed}; humidity: {_meteo.Humidity}.");
            return _meteo;
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
                }, deadline: Defaults.DefaultGrpcDeadline);

            _mainFiringDirection = response.MainFiringDirection;

            _console.Out.WriteLine($"Unit {_battery.Id} reported for duty. Re-position command: " +
                                   $"lat: {response.Position.Latitude}; lon: {response.Position.Longitude}; " +
                                   $"direction: {response.MainFiringDirection}.");
            
            return response;
        }
    }
}