using System;
using UnityEngine;

namespace Vehicle
{
    [Serializable]
    public class VehicleModel
    {
        public readonly float[] MaxSpeedsByGear;
        public readonly float ShiftDownRpm;
        public readonly float ShiftUpRpm;
        public readonly float MaxRpm;
        public readonly float TireDiameter;

        public float TotalDistance { get; private set; }
        public float CurrentSpeed { get; private set; }
        public int CurrentGear { get; private set; } 
        public float EngineRpm { get; private set; }
        
        private bool _isThrottleActive;
        private bool _isBrakeActive;
        private bool _isDecelerationActive;
        
        public VehicleModel(float[] maxSpeedsByGear, float shiftDownRpm, float shiftUpRpm, float maxRpm, float tireDiameter)
        {
            MaxSpeedsByGear = maxSpeedsByGear;
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
        
        private float MaxSpeed => MaxSpeedsByGear[CurrentGear];
        
        private void ShiftUp()
        {
            if (CurrentGear >= MaxSpeedsByGear.Length - 1)
            {
                return;
            }
            
            CurrentGear++;
            EngineRpm = ShiftUpRpm * (CurrentSpeed / MaxSpeed);
        }

        private void ShiftDown()
        {
            if (CurrentGear <= 0)
            {
                return;
            }
            
            CurrentGear--;
            EngineRpm = ShiftUpRpm * (CurrentSpeed / MaxSpeed);
        }
        
        private float RpmMultiplier => CurrentGear switch
        {
            0 => 2.5f,
            1 => 2f,
            2 => 1.5f,
            3 => 1f,
            4 => 0.75f,
            _ => 1f
        };

        public void ApplyThrottle(bool isThrottleActive)
        {
            _isThrottleActive = isThrottleActive;
            
            if (!_isThrottleActive) return;
            
            EngineRpm = Mathf.Clamp(EngineRpm + 5f * RpmMultiplier, ShiftDownRpm, MaxRpm);
        }
        
        public void ApplyBrake(bool isBrakeActive)
        {
            _isBrakeActive = isBrakeActive;
            
            if (!_isBrakeActive) return;
            
            EngineRpm = Mathf.Clamp(EngineRpm - 20f * RpmMultiplier, ShiftDownRpm, MaxRpm);
        }
        
        public void ApplyDeceleration(bool isDecelerationActive)
        {
            _isDecelerationActive = isDecelerationActive;
            
            if (!_isDecelerationActive) return;
            
            EngineRpm = Mathf.Clamp(EngineRpm - 2.5f * RpmMultiplier, ShiftDownRpm, MaxRpm);
        }
        
        public void UpdateVehicle(float deltaTime)
        {
            if (EngineRpm >= ShiftUpRpm)
            {
                ShiftUp();
            }
            else if (EngineRpm <= ShiftDownRpm)
            {
                ShiftDown();
            }

            if (_isThrottleActive)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed + 0.025f, 0, MaxSpeed);
            } 
            else if (_isBrakeActive)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed - 0.1f, 0, MaxSpeed);
            }
            else if (_isDecelerationActive)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed - 0.0125f, 0, MaxSpeed);
            }
            TotalDistance += CurrentSpeed * deltaTime;
             
            Debug.Log($"Speed: {CurrentSpeed * 3.6f} km/h\nDistance: {TotalDistance} m\nGear: {CurrentGear + 1}\nRPM: {EngineRpm}");
        }
    }
}