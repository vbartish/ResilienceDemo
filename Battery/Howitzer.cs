using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;

namespace ResilienceDemo.Battery
{
    public class Howitzer : IHowitzer
    {
        private volatile bool _isOperational;
        private volatile bool _aimingDone;

        public Howitzer(int howitzerId)
        {
            Id = howitzerId;
        }

        public int Id { get; }
        
        public double Longitude { get; private set; }

        public double Latitude { get; private set; }
        
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
            var policy = MonkeyPolicy.InjectBehaviourAsync(with =>
                with
                    .Behaviour(() => Task.Delay(2000))
                    .InjectionRate(0.2).Enabled());
            return await policy.ExecuteAsync(() =>
            {
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
            token.ThrowIfCancellationRequested();
            var policy = MonkeyPolicy.InjectExceptionAsync(with =>
                with
                    .Fault((context, cancellationToken) =>
                        Task.FromException<Exception>(new InvalidOperationException("Unable to aim")))
                    .InjectionRate(0.2)
                    .Enabled());
            return policy.ExecuteAsync(() =>
            {
                token.ThrowIfCancellationRequested();
                HorizontalAngle = angleHorizontal;
                VerticalAngle = angleVertical;
                AimingDone = true;
                return Task.CompletedTask;
            });
        }

        public Task Fire(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            var policy = MonkeyPolicy.InjectLatencyAsync(with =>
                with
                    .Latency(TimeSpan.FromMilliseconds(500))
                    .InjectionRate(0.2)
                    .Enabled());
            return policy.ExecuteAsync(() =>
            {
                token.ThrowIfCancellationRequested();
                return Task.CompletedTask;
            });
        }
    }
}