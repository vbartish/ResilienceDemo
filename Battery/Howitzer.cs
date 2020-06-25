using System;
using System.Threading.Tasks;
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

        public Howitzer(int howitzerId, IConsole console)
        {
            _console = console;
            Id = howitzerId;
        }

        public int Id { get; }

        public bool IsOperational
        {
            get => _isOperational;
            private set => _isOperational = value;
        }

        public async Task<Howitzer> ToArms()
        {
            var latencyPolicy = MonkeyPolicy.InjectBehaviourAsync(with =>
                with
                    .Behaviour(() => Task.Delay(2000))
                    .InjectionRate(0.2).Enabled());
            return await latencyPolicy.ExecuteAsync(() =>
            {
                _isOperational = true;
                return Task.FromResult(this);
            });
        }
    }
}