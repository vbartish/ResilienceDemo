using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using GrpcDivisionControlUnit;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;

namespace ResilienceDemo.Battery
{
    public class Howitzer : IHowitzer
    {
        private readonly IConsole _console;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly DivisionControlUnit.DivisionControlUnitClient _client;
        private volatile bool _isOperational;
        private volatile bool _aimingDone;
        private bool _overheat;
        private IBattery _battery;

        public Howitzer(
            int howitzerId,
            IConsole console,
            IReadOnlyPolicyRegistry<string> policyRegistry,
            DivisionControlUnit.DivisionControlUnitClient client)
        {
            _console = console;
            _policyRegistry = policyRegistry;
            _client = client;
            Id = howitzerId;
        }

        public int Id { get; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public int AmmunitionConsumption { get; private set; }

        public double VerticalAngle { get; private set; }

        public double HorizontalAngle { get; private set; }

        public bool IsOperational
        {
            get => _isOperational;
            private set => _isOperational = value;
        }

        public bool AimingDone
        {
            get => _aimingDone;
            private set => _aimingDone = value;
        }

        public void AssignToBattery(IBattery battery)
        {
            _battery = battery ?? throw new ArgumentNullException(nameof(battery));
        }

        public async Task<Howitzer> ToArms()
        {
            IsOperational = false;
            var policy = MonkeyPolicy.InjectLatencyAsync(with =>
                with
                    .Latency(TimeSpan.FromSeconds(2))
                    .InjectionRate(1d/_battery.HowitzersCount)
                    .Enabled());

            return await policy.ExecuteAsync(() =>
            {
                _console.Out.WriteLine($"Howitzer {Id} is operational.");
                IsOperational = true;
                return Task.FromResult(this);
            });
        }

        public void RePosition(in double latitude, in double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Task Aim(double angleHorizontal, double angleVertical, CancellationToken token)
        {
            AimingDone = false;
            var aimingPolicy = MonkeyPolicy.InjectBehaviourAsync(with =>
                with
                    .Behaviour(async () => await Task.Delay(2000, token))
                    .InjectionRate(1d/_battery.HowitzersCount)
                    .Enabled());
            return aimingPolicy.ExecuteAsync(() =>
            {
                if (token.IsCancellationRequested)
                {
                    _console.Out.WriteLine($"Howitzer {Id} aiming canceled.");
                }

                token.ThrowIfCancellationRequested();

                HorizontalAngle = angleHorizontal;
                VerticalAngle = angleVertical;
                AimingDone = true;
                _console.Out.WriteLine($"Howitzer {Id} aiming done.");
                return Task.CompletedTask;
            });
        }

        public async Task Fire(int ammunitionToConsume, CancellationToken token)
        {
            _overheat = false;
            var policy = MonkeyPolicy.InjectBehaviourAsync(with =>
                with
                    .Behaviour(() =>
                    {
                        _overheat = true;
                        return Task.CompletedTask;
                    })
                    .InjectionRate(0.05)
                    .Enabled());

            for (var ammunitionCounter = 0; ammunitionCounter < ammunitionToConsume; ammunitionCounter++)
            {
                await policy.ExecuteAsync(async () =>
                {
                    if (token.IsCancellationRequested)
                    {
                        _console.Out.WriteLine($"Howitzer {Id} firing canceled.");
                    }

                    token.ThrowIfCancellationRequested();

                    if (_overheat)
                    {
                        _console.Out.WriteLine($"Howitzer {Id} overheated. Waiting for cooldown.");
                        await Task.Delay(1000, token);
                        _overheat = false;
                    }

                    AmmunitionConsumption++;
                });
            }
        }

        public async Task BattleReport()
        {
            var retryPolicy =
                Policy
                    .Handle<BrokenCircuitException>()
                    .Or<RpcException>(e => e.Status.StatusCode == StatusCode.ResourceExhausted)
                    .WaitAndRetryForeverAsync((retryAttempt, exception, context) =>
                        {
                            var backoffSpans =
                                Backoff
                                    .DecorrelatedJitterBackoffV2(
                                        TimeSpan.FromSeconds(1),
                                        retryAttempt + 1)
                                    .ToList();
                            return backoffSpans[retryAttempt];
                        },
                        (exception, retryAttempt, timeSpan, context) =>
                        {
                            _console.Out.WriteLine(
                                $"Operation: {context.OperationKey}; TimeSpan: {timeSpan.ToString()}. Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        });

            var circuitBreakerPolicy =
                _policyRegistry.Get<IAsyncPolicy>(CircuitBreakerPolicyKey.DefaultCircuitBreaker.ToString());

            var wrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            await Task.WhenAll(
                wrap.ExecuteAsync(async context =>
                {
                    _console.Out.WriteLine($"Howitzer {Id} sending first part of the report.");
                    await _client.BattleReportAsync(new Report
                    {
                        ReportData = "report data 1"
                    });
                    _console.Out.WriteLine($"Howitzer {Id} sent first part of the report.");
                }, new Context($"Howitzer {Id}, Battle report part 1")),
                wrap.ExecuteAsync(async context =>
                {
                    _console.Out.WriteLine($"Howitzer {Id} sending second part of the report.");
                    await _client.BattleReportAsync(new Report
                    {
                        ReportData = "report data 2"
                    });
                    _console.Out.WriteLine($"Howitzer {Id} sent second part of the report.");
                }, new Context($"Howitzer {Id}, Battle report part 2")),
                wrap.ExecuteAsync(async context =>
                {
                    _console.Out.WriteLine($"Howitzer {Id} sending third part of the report.");
                    await _client.BattleReportAsync(new Report
                    {
                        ReportData = "report data 3"
                    });
                    _console.Out.WriteLine($"Howitzer {Id} sent third part of the report.");
                }, new Context($"Howitzer {Id}, Battle report part 3")));
            _console.Out.WriteLine($"Howitzer {Id} battle report done.");
        }
    }
}