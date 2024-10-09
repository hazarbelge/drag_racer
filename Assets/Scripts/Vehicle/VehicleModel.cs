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
        public readonly float TireRadius;

        public float TotalDistance { get; private set; }
        public float CurrentSpeed { get; private set; }
        public int CurrentGear { get; private set; } 
        public float EngineRpm { get; private set; }
        
        private VehicleAudio _vehicleAudio;
        private bool _isThrottleActive;
        private bool _isBrakeActive;
        private bool _isDecelerationActive;
        
        public VehicleModel(float[] maxSpeedsByGear, float shiftDownRpm, float shiftUpRpm, float maxRpm, float tireRadius)
        {
            MaxSpeedsByGear = maxSpeedsByGear;
            ShiftDownRpm = shiftDownRpm;
            ShiftUpRpm = shiftUpRpm;
            MaxRpm = maxRpm;
            TireRadius = tireRadius;
            
            SetDefaultAttributes();
        }

        public void SetDefaultAttributes()
        {
            TotalDistance = 0f;
            CurrentSpeed = 0f;
            CurrentGear = 0;
            EngineRpm = 1000f;
        }
        
        public void SetVehicleAudio(VehicleAudio vehicleAudio)
        {
            _vehicleAudio = vehicleAudio;
        }
        
        private float MaxSpeed => MaxSpeedsByGear[CurrentGear];
        
        private void CalculateEngineRpmOnGearChange()
        {
            EngineRpm = ShiftUpRpm * (CurrentSpeed / MaxSpeed);
            _vehicleAudio.UpdateEngineSound();
        }
        
        private void ShiftUp()
        {
            if (CurrentGear >= MaxSpeedsByGear.Length - 1)
            {
                return;
            }
            
            CurrentGear++;
            CalculateEngineRpmOnGearChange();
        }

        private void ShiftDown()
        {
            if (CurrentGear <= 0)
            {
                return;
            }
            
            CurrentGear--;
            CalculateEngineRpmOnGearChange();
        }
        
        private float SpeedRpmIncreaseMultiplier => CurrentGear switch
        {
            0 => 2.5f,
            1 => 2f,
            2 => 1.5f,
            3 => 1.2f,
            4 => 0.8f,
            _ => 1f
        } * Time.deltaTime;

        public void ApplyThrottle(bool isThrottleActive)
        {
            _isThrottleActive = isThrottleActive;
            
            if (!_isThrottleActive) return;
            
            var increaseRate = 600f * SpeedRpmIncreaseMultiplier;
            var limitDecreaseRate = 3500f * SpeedRpmIncreaseMultiplier;
            
            EngineRpm = Mathf.Clamp(EngineRpm + increaseRate, ShiftDownRpm, MaxRpm);
            
            if (EngineRpm >= ShiftUpRpm + increaseRate * 4f)
            {
                EngineRpm = Mathf.Clamp(EngineRpm - limitDecreaseRate, ShiftDownRpm, MaxRpm);
            }
        }
        
        public void ApplyBrake(bool isBrakeActive, bool forceStop = false)
        {
            _isBrakeActive = isBrakeActive;
            
            if (!_isBrakeActive) return;
            
            var brakeRate = 1500f * SpeedRpmIncreaseMultiplier * (forceStop ? 5f : 1f);
            
            EngineRpm = Mathf.Clamp(EngineRpm - brakeRate, ShiftDownRpm, MaxRpm);
        }
        
        public void ApplyDeceleration(bool isDecelerationActive)
        {
            _isDecelerationActive = isDecelerationActive;
            
            if (!_isDecelerationActive) return;
            
            var decreaseRate = 300f * SpeedRpmIncreaseMultiplier;
            
            EngineRpm = Mathf.Clamp(EngineRpm - decreaseRate, ShiftDownRpm, MaxRpm);
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
                CurrentSpeed = Mathf.Clamp(CurrentSpeed + 1f * SpeedRpmIncreaseMultiplier * (CurrentGear + 1) , 0, MaxSpeed);
            } 
            else if (_isBrakeActive)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed - 3f * SpeedRpmIncreaseMultiplier * (CurrentGear + 1), 0, MaxSpeed);
            }
            else if (_isDecelerationActive)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed - 0.5f * SpeedRpmIncreaseMultiplier * (CurrentGear + 1), 0, MaxSpeed);
            }
            
            TotalDistance += CurrentSpeed * deltaTime;
        }
    }
}