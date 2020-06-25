using System;
using CommandDotNet;
using CommandDotNet.Rendering;
using GrpcDivisionControlUnit;
using Polly;

namespace ResilienceDemo.Battery
{
    public class Battery : IBattery
    {
        private readonly IConsole _console;
        private Meteo _currentMeteo;

        public Battery(IConsole console)
        {
            _console = console;
        }
        
        public Guid Id { get; } = Guid.NewGuid();

        public void ToArms()
        {
            _console.Out.WriteLine($"Battery {Id} reporting for duty.");
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