using Controller;
using UnityEngine;
using static Enums;

namespace Vehicle
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private Transform vehicleTransform;
        [SerializeField] private Transform[] wheels = new Transform[4];
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform finishLinePos;
        
        private GameController _gameController;
        private VehicleModel _vehicleModel;
        private VehicleAudio _vehicleAudio;
        
        private bool _throttleInputByButton;
        private bool _brakeInputByButton;
        
        public void Init(GameController gameController)
        {
            _gameController = gameController;
            
            InitVehicleModel();
            InitVehicleAudio();
        }

        private void InitVehicleModel()
        {
            _vehicleModel = new VehicleModel(
                VehicleConfig.MaxSpeedsInMs,
                VehicleConfig.ShiftDownRpm,
                VehicleConfig.ShiftUpRpm,
                VehicleConfig.MaxRpm,
                VehicleConfig.TireDiameter
            );
        }
        
        private void InitVehicleAudio()
        {
            _vehicleAudio = vehicleTransform.GetComponent<VehicleAudio>();
            _vehicleAudio.Init(_vehicleModel);
        }

        private void Update()
        {
            if (_gameController.GameState == GameState.Menu) return;
            
            ApplyInputs();
            UpdateCarState();
        }

        private void ApplyInputs()
        {
            if (_gameController.GameState is GameState.Racing or GameState.Countdown)
            {
                _vehicleModel.ApplyBrake(BrakeInput);
                _vehicleModel.ApplyThrottle(ThrottleInput && !BrakeInput);
                _vehicleModel.ApplyDeceleration(!ThrottleInput && !BrakeInput);
            } 
            else
            {
                _vehicleModel.ApplyBrake(true, true);
            }
            
            _vehicleAudio.UpdateEngineSound();
        }

        private void UpdateCarState()
        {
            if (_gameController.GameState is GameState.Racing or GameState.PostRace)
            {
                var deltaTime = Time.deltaTime;
            
                _vehicleModel.UpdateVehicle(deltaTime);
            
                var distance = _vehicleModel.CurrentSpeed * deltaTime;
            
                UpdateWheelRotation(distance);
                MoveVehicle(distance);
                
                if (vehicleTransform.position.z >= finishLinePos.position.z)
                {
                    OnFinishLinePassed();
                }
            }
            
            _gameController.UpdateRaceInfo(_vehicleModel.TotalDistance, _vehicleModel.CurrentSpeed * 3.6f, _vehicleModel.EngineRpm, _vehicleModel.CurrentGear + 1);
        }

        private void MoveVehicle(float distance)
        {
            if (_vehicleModel.CurrentSpeed <= 0) return;
            
            vehicleTransform.position += vehicleTransform.forward * distance;
        }

        private void UpdateWheelRotation(float distance)
        {
            var rotateAngle = distance * Mathf.Rad2Deg / _vehicleModel.TireDiameter;
            foreach (var wheel in wheels)
            {
                wheel.Rotate(Vector3.right, rotateAngle);
            }
        }
        
        public void OnCountdownStart()
        {
            _vehicleAudio.PlayEngineSound();
        }
        
        public void OnRaceStart()
        {
            _vehicleModel.SetDefaultAttributes();
        }

        private void OnFinishLinePassed()
        {
            _gameController.OnRaceEnd();
        }
        
        public void OnRaceEnd()
        {
            vehicleTransform.position = startPos.position;
            _vehicleModel.SetDefaultAttributes();
            _vehicleAudio.StopEngineSound();
            
            _throttleInputByButton = false;
            _brakeInputByButton = false;
        }

        #region VehicleInput

        public void ThrottleButtonDown()
        {
            _throttleInputByButton = true;
        }
        
        public void ThrottleButtonUp()
        {
            _throttleInputByButton = false;
        }
        
        public void BrakeButtonDown()
        {
            _brakeInputByButton = true;
        }
        
        public void BrakeButtonUp()
        {
            _brakeInputByButton = false;
        }
        
        private bool ThrottleInput => _throttleInputByButton || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        private bool BrakeInput => _brakeInputByButton || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        #endregion
    }
}