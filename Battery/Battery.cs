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

            await policy
                .ExecuteAndCaptureAsync(
                    _ => Task.WhenAll(_howitzers.Select(howitzer => howitzer.ToArms())), 
                    new Context("Battery to arms."));

            _howitzers.RemoveAll(h => !h.IsOperational);

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

        public async Task Aim(double angleHorizontal, double angleVertical, TimeoutPolicyKey timeoutPolicyKey)
        {
            var timeoutPolicy = _policyRegistry.Get<IAsyncPolicy>(timeoutPolicyKey.ToString());
            var watch = new Stopwatch();
            watch.Start();

            await timeoutPolicy.ExecuteAndCaptureAsync(
                (policyContext, token) => Task.WhenAll(_howitzers.Select(h => h.Aim(
                    angleHorizontal,
                    angleVertical,
                    token))),
                new Context("Battery aiming"),
                CancellationToken.None); // CancellationToken.None means we don't want independent cancellation control
            
            watch.Stop();
            _console.Out.WriteLine($"Battery {Id} with {_howitzers.Count(h=>h.AimingDone)} howitzers ready to fire within {watch.ElapsedMilliseconds} ms.");
        }

        public async Task Fire(int ammunitionPerHowitzer, TimeoutPolicyKey timeoutPolicyKey)
        {
            var timeoutPolicy = _policyRegistry.Get<IAsyncPolicy>(timeoutPolicyKey.ToString());
            var watch = new Stopwatch();
            watch.Start();

            await timeoutPolicy.ExecuteAndCaptureAsync(
                (policyContext, token) => Task.WhenAll(_howitzers
                    .Where(h => h.AimingDone)
                    .Select(h => h.Fire(ammunitionPerHowitzer, token))),
                new Context("Battery firing"),
                CancellationToken.None); // CancellationToken.None means we don't want independent cancellation control);
            
            watch.Stop();
            _console.Out.WriteLine($"Battery {Id} firing done within {watch.ElapsedMilliseconds} ms.");
            foreach (var howitzer in _howitzers)
            {
                _console.Out.WriteLine($"Howitzer {howitzer.Id} ammunition consumption {howitzer.AmmunitionConsumption}.");
            }
        }

        public Task Disengage()
        {
            _console.Out.WriteLine("Battery disengaged.");
            return Task.CompletedTask;
        }
    }
}