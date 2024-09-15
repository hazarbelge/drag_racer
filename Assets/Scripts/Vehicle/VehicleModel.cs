using System;
using UnityEngine;

namespace Vehicle
{
    [Serializable]
    public class VehicleModel
    {
        public readonly float[] MaxSpeedsByGear;
        public readonly AnimationCurve SpeedRpmCurve;
        public readonly float ShiftDownRpm;
        public readonly float ShiftUpRpm;
        public readonly float MaxRpm;
        public readonly float TireDiameter;

        public float TotalDistance { get; private set; }
        public float CurrentSpeed { get; private set; }
        public int CurrentGear { get; private set; } 
        public float EngineRpm { get; private set; }
        
        public VehicleModel(float[] maxSpeedsByGear, AnimationCurve speedRpmCurve, float shiftDownRpm, float shiftUpRpm, float maxRpm, float tireDiameter)
        {
            MaxSpeedsByGear = maxSpeedsByGear;
            SpeedRpmCurve = speedRpmCurve;
            ShiftDownRpm = shiftDownRpm;
            ShiftUpRpm = shiftUpRpm;
            MaxRpm = maxRpm;
            TireDiameter = tireDiameter;
            
            Init();
        }

        private void Init()
        {
            TotalDistance = 0f;
            CurrentSpeed = 0f;
            CurrentGear = 0;
            EngineRpm = 1000f;
        }

        private void ShiftUp()
        {
            if (CurrentGear >= MaxSpeedsByGear.Length - 1)
            {
                return;
            }
            
            CurrentGear++;
            EngineRpm = 1000f;
        }

        private void ShiftDown()
        {
            if (CurrentGear <= 0)
            {
                return;
            }
            
            CurrentGear--;
            EngineRpm = ShiftUpRpm;
        }
        
        private float RpmMultiplier => CurrentGear switch
        {
            0 => 2f,
            1 => 1.5f,
            2 => 1.25f,
            3 => 1f,
            4 => 0.5f,
            _ => 1f
        };

        public void ApplyThrottle(bool isThrottleActive)
        {
            if (isThrottleActive)
            {
                EngineRpm = Mathf.Clamp(EngineRpm + 10f * RpmMultiplier, ShiftDownRpm, MaxRpm);
            }
        }
        
        public void ApplyBrake(bool isBrakeActive)
        {
            if (isBrakeActive)
            {
                EngineRpm = Mathf.Clamp(EngineRpm - 20f * RpmMultiplier, ShiftDownRpm, MaxRpm);
            }
        }
        
        public void ApplyDeceleration(bool isDecelerationActive)
        {
            if (isDecelerationActive)
            {
                EngineRpm = Mathf.Clamp(EngineRpm - 5f * RpmMultiplier, ShiftDownRpm, MaxRpm);
            }
        }
        
        public void UpdateSpeed(float deltaTime)
        {
            if (EngineRpm >= ShiftUpRpm)
            {
                ShiftUp();
            }
            else if (EngineRpm <= ShiftDownRpm)
            {
                ShiftDown();
            }
            
            var currentLerpRangeByGear = 0.2f * (CurrentGear + 1);
            var currentSpeedValue = SpeedRpmCurve.Evaluate(Mathf.Lerp(currentLerpRangeByGear - 0.2f, currentLerpRangeByGear, Mathf.InverseLerp(ShiftDownRpm, MaxRpm, EngineRpm)));
            var currentMinSpeedValue = SpeedRpmCurve.Evaluate(Mathf.Lerp(currentLerpRangeByGear - 0.2f, currentLerpRangeByGear, 0));
            var currentMaxSpeedValue = SpeedRpmCurve.Evaluate(Mathf.Lerp(currentLerpRangeByGear - 0.2f, currentLerpRangeByGear, 1));
            var currentMinSpeed = CurrentGear == 0 ? 0f : MaxSpeedsByGear[CurrentGear - 1];
            var currentMaxSpeed = MaxSpeedsByGear[CurrentGear];
            CurrentSpeed = Mathf.Lerp(currentMinSpeed, currentMaxSpeed, Mathf.InverseLerp(currentMinSpeedValue, currentMaxSpeedValue, currentSpeedValue));
            TotalDistance += CurrentSpeed * deltaTime;
             
            Debug.Log($"Speed: {CurrentSpeed * 3.6f} km/h\nDistance: {TotalDistance} m\nGear: {CurrentGear + 1}\nRPM: {EngineRpm}");
        }
        
        public void OnRaceStart()
        {
            var distanceToShiftUp = Mathf.Abs(EngineRpm - ShiftUpRpm);
            var bonusRpm = distanceToShiftUp <= 200 ? (200 - distanceToShiftUp) * 5f : 0;
            EngineRpm = 1000f + bonusRpm;
        }
    }
}