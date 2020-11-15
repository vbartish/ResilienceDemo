using System;

namespace ResilienceDemo.Battery
{
    internal static class Defaults
    {
        public static DateTime DefaultGrpcDeadline => DateTime.UtcNow.AddSeconds(1);
        public const double DefaultLatitude = 50.014495;
        public const double DefaultLongitude = 23.730392;
        public const int DefaultAssaultDensity = 40;
        public const int MinHorizontalAngle = 0;
        public const int MaxHorizontalAngle = 360;
        public const int MinVerticalAngle = -3;
        public const int MaxVerticalAngle = 70;
        public const int RoundingPrecision = 2;
        public const int DefaultHowitzersPerBattery = 6;
        public const string DefaultDivisionControlAddress = "http://localhost:5000";
    }
}
