using System;

namespace ResilienceDemo.Battery
{
    public interface IBattery
    {
        Guid Id { get; }

        void ToArms();

        void RePosition(double latitude, double longitude);
    }
}