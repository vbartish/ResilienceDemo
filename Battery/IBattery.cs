using System;
using System.Threading.Tasks;
using GrpcDivisionControlUnit;
using Polly;

namespace ResilienceDemo.Battery
{
    public interface IBattery
    {
        Guid Id { get; }

        Task ToArms(TimeoutPolicyKey policyKey);

        void RePosition(double latitude, double longitude);
        void UseMeteo(Meteo meteo);
    }
}