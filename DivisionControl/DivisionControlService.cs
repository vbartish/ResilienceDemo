﻿using System;
using System.Threading.Tasks;
using Geolocation;
using Google.Protobuf.Reflection;
using Grpc.Core;
using GrpcContract;
using GrpcDivisionControlUnit;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Bogus;
using Polly;
using Polly.Bulkhead;
using Polly.Contrib.Simmy.Outcomes;

namespace ResilienceDemo.DivisionControl
{
    public class DivisionControlService : DivisionControlUnit.DivisionControlUnitBase, IGrpcService
    {
        private const int RegisterUnitSuccessEvery = 5;
        private const int MeteoSuccessEvery = 3;
        private static int _registerUnitCounter;
        private static int _meteoCounter;

        private readonly IAsyncPolicy _reportingBulkhead =
            Policy.BulkheadAsync(2, 1, async context =>
            {
                Console.WriteLine("Bulkhead rejection happened");
            });
        
        private readonly Faker _faker = new Faker();

        public ServerServiceDefinition BindService() => DivisionControlUnit.BindService(this);

        ServiceDescriptor IGrpcService.Descriptor => DivisionControlUnit.Descriptor;

        public override Task<RePositionCommand> RegisterUnit(RegisterArtilleryUnitRequest request, ServerCallContext context)
        {
            var latencyPolicy = MonkeyPolicy.InjectLatencyAsync<RePositionCommand>(
                with =>
                {
                    with
                        .Latency(TimeSpan.FromSeconds(2))
                        .InjectionRate(1)
                        .EnabledWhen((_, __) => Task.FromResult(++_registerUnitCounter % RegisterUnitSuccessEvery != 0));
                });
            
            return latencyPolicy.ExecuteAsync(() =>
            {
                var boundaries = new CoordinateBoundaries(
                    request.Position.Latitude,
                    request.Position.Longitude,
                    60,
                    DistanceUnit.Kilometers);
                
                return Task.FromResult(new RePositionCommand
                {
                    Position = new Position
                    {
                        Latitude = _faker.Random.Double(boundaries.MinLatitude, boundaries.MaxLatitude),
                        Longitude = _faker.Random.Double(boundaries.MinLongitude, boundaries.MaxLongitude)    
                    },
                    MainFiringDirection = Math.Round(_faker.Random.Double(0, 360), 2)
                });
            });
        }

        public override Task<Meteo> GetMeteo(Position position, ServerCallContext context)
        {
            var latencyPolicy = MonkeyPolicy.InjectLatencyAsync<Meteo>(
                with =>
                {
                    with
                        .Latency(TimeSpan.FromSeconds(2))
                        .InjectionRate(0.5)
                        .Enabled()
                        .EnabledWhen((_, __) => Task.FromResult(++_meteoCounter % MeteoSuccessEvery != 0));
                });

            return latencyPolicy.ExecuteAsync(
                () => Task.FromResult(new Meteo
                {
                    Temperature = Math.Round(_faker.Random.Double(-15, 40), 2),
                    Humidity = Math.Round(_faker.Random.Double(-15, 40), 2),
                    WindAngle = Math.Round(_faker.Random.Double(0, 360), 2),
                    WindSpeed = Math.Round(_faker.Random.Double(0, 100), 2)
                }));
        }

        public override Task<AssaultCommand> InPosition(Position position, ServerCallContext context)
        {
            var chaosPolicy = MonkeyPolicy.InjectExceptionAsync(with =>
                with.Fault(new RpcException(new Status(StatusCode.OutOfRange, "Targets are out of range.")))
                    .InjectionRate(0.7)
                    .Enabled());

            var boundaries = new CoordinateBoundaries(
                position.Latitude,
                position.Longitude,
                15.2,
                DistanceUnit.Kilometers);

            var unavailableBoundaries = new CoordinateBoundaries(
                position.Latitude,
                position.Longitude,
                4.2,
                DistanceUnit.Kilometers);

            return chaosPolicy.ExecuteAsync(
                () => Task.FromResult(new AssaultCommand
                {
                    Position = new Position
                    {
                        Latitude = _faker.Random.Double(unavailableBoundaries.MaxLatitude, boundaries.MaxLatitude),
                        Longitude = _faker.Random.Double(unavailableBoundaries.MaxLongitude, boundaries.MaxLongitude)
                    },
                    DirectionDeviation = Math.Round(_faker.Random.Double(0, 360), 2)
                }));
        }

        public override Task<AssaultCommand> GetCorrection(Position request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, "Correction is unavailable"));
        }

        public override async Task<Empty> BattleReport(Report request, ServerCallContext context)
        {
            try
            {
                await _reportingBulkhead.ExecuteAsync(async () => await Task.Delay(3000));
            }
            catch (BulkheadRejectedException e)
            {
                throw new RpcException(new Status(StatusCode.ResourceExhausted, "Bulkhead says hello."));
            }
            
            return new Empty();
        }
    }
}