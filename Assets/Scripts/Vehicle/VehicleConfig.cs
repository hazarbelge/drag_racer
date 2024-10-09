namespace Vehicle
{
    public static class VehicleConfig
    {
        public static readonly float[] MaxSpeedsInMs = { 14.69f, 24.96f, 35.33f, 48.37f, 67.13f};
        public const float ShiftDownRpm = 1000f;
        public const float ShiftUpRpm = 5800f;
        public const float MaxRpm = 6000f;
        public const float TireRadius = 0.33f;
    }
}