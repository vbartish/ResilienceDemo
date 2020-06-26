using System;
using System.Threading;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
namespace ResilienceDemo.Battery
{
    public class Howitzer : IHowitzer
    {
        private readonly IConsole _console;
        private volatile bool _isOperational;
        private volatile bool _aimingDone;
        private bool _overheat;

        public Howitzer(int howitzerId, IConsole console)
        {
            _console = console;
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

        public async Task<Howitzer> ToArms()
        {
            IsOperational = false;
            var policy = MonkeyPolicy.InjectLatencyAsync(with =>
                with
                    .Latency(TimeSpan.FromSeconds(2))
                    .InjectionRate(0.2).Enabled());
            
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
            var policy = MonkeyPolicy.InjectBehaviourAsync(with =>
                with
                    .Behaviour(async () => await Task.Delay(2000, token))
                    .InjectionRate(0.2)
                    .Enabled(true));
            return policy.ExecuteAsync(() =>
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
                        await Task.Delay(400, token);
                        _overheat = false;
                    }

                    AmmunitionConsumption++;
                });
            }
        }
    }
}