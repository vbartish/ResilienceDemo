using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using GrpcDivisionControlUnit;
using Polly;
using Polly.Registry;

namespace ResilienceDemo.Battery
{
    public class Battery : IBattery
    {
        private readonly IConsole _console;
        private Meteo _currentMeteo;
        private readonly List<IHowitzer> _howitzers;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public Battery(IConsole console, List<IHowitzer> howitzers, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _console = console;
            _howitzers = howitzers;
            _policyRegistry = policyRegistry;
        }
        
        public Guid Id { get; } = Guid.NewGuid();

        public async Task ToArms(TimeoutPolicyKey policyKey)
        {
            var policy = _policyRegistry.Get<IAsyncPolicy>(policyKey.ToString());
            var watch = new Stopwatch();
            watch.Start();

            var policyResult = await policy
                .ExecuteAndCaptureAsync(
                    _ => Task.WhenAll(_howitzers.Select(howitzer => howitzer.ToArms())), 
                    new Context("Battery to arms."));

            if (policyResult.Outcome == OutcomeType.Failure)
            {
                _howitzers.RemoveAll(h => !h.IsOperational);
            }

            watch.Stop();
            _console.Out.WriteLine($"Battery {Id} with {_howitzers.Count} howitzers reporting for duty within {watch.ElapsedMilliseconds} ms.");
        }

        public void RePosition(double latitude, double longitude)
        {
            Console.WriteLine($"Position changed to lat: {latitude}; lon: {longitude};");
        }

        public void UseMeteo(Meteo meteo)
        {
            _currentMeteo = meteo;
        }
    }
}