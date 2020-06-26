using System;
using System.Threading;
using System.Threading.Tasks;
using GrpcDivisionControlUnit;
using Polly;

namespace ResilienceDemo.Battery
{
    public interface IBattery
    {
        Guid Id { get; }
        
        double Longitude { get; }

        double Latitude { get; }

        Task ToArms(TimeoutPolicyKey policyKey);

        void RePosition(double latitude, double longitude);
        
        Task Aim(double anglesHorizontal, double anglesVertical, TimeoutPolicyKey timeoutPolicyKey);

        Task Fire(int ammunitionPerHowitzer, TimeoutPolicyKey token);
        
        Task Disengage();
    }
}