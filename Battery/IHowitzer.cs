using System.Threading;
using System.Threading.Tasks;

namespace ResilienceDemo.Battery
{
    public interface IHowitzer
    {
        int Id { get; }
        
        bool IsOperational { get; }

        bool AimingDone { get; }
        
        double Longitude { get; }

        double Latitude { get; }

        int AmmunitionConsumption { get; }

        void AssignToBattery(IBattery battery);

        Task<Howitzer> ToArms();
        
        void RePosition(in double latitude, in double longitude);

        Task Aim(double angleHorizontal, double angleVertical, CancellationToken token);
        
        Task Fire(int ammunitionToConsume, CancellationToken token);

        Task BattleReport();
    }
}