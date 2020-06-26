using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Polly;
using Polly.Registry;

namespace ResilienceDemo.Battery
{
    public class Battery : IBattery
    {
        private readonly IConsole _console;
        private readonly List<IHowitzer> _howitzers;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public Battery(IConsole console, List<IHowitzer> howitzers, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _console = console;
            _howitzers = howitzers;
            _policyRegistry = policyRegistry;
        }
        
        public Guid Id { get; } = Guid.NewGuid();
        
        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

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
            _howitzers.ForEach(h => h.RePosition(latitude, longitude));
            Longitude = longitude;
            Latitude = latitude;
            Console.WriteLine($"Position changed to lat: {latitude}; lon: {longitude};");
        }

        public Task Aim(double angleHorizontal, double angleVertical, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.WhenAll(_howitzers.Select(h => h.Aim(angleHorizontal, angleVertical, token)));
        }

        public Task Fire(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.WhenAll(_howitzers.Where(h=> h.AimingDone).Select(h => h.Fire(token)));
        }
    }
}