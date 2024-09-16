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
            float[] maxSpeedsInMs = { 14.69f, 24.96f, 35.33f, 48.37f, 67.13f};
            const float shiftDownRpm = 1000f;
            const float shiftUpRpm = 5800f;
            const float maxRpm = 6000f;
            const float tireDiameter = 0.33f;

            _vehicleModel = new VehicleModel(
                maxSpeedsInMs,
                shiftDownRpm,
                shiftUpRpm,
                maxRpm,
                tireDiameter
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
            
            if (_gameController.GameState != GameState.PostRace)
            {
                _vehicleModel.ApplyBrake(BrakeInput);
                _vehicleModel.ApplyThrottle(ThrottleInput && !BrakeInput);
                _vehicleModel.ApplyDeceleration(!ThrottleInput && !BrakeInput);
            } 
            else
            {
                _vehicleModel.ApplyBrake(true);
            }
            
            _vehicleAudio.UpdateEngineSound();
            
            if (_gameController.GameState == GameState.Racing)
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

        private void OnFinishLinePassed()
        {
            _gameController.OnRaceEnd();
        }
        
        public void OnRaceEnd()
        {
            vehicleTransform.position = startPos.position;
            _vehicleModel.SetDefaultAttributes();
            _vehicleAudio.StopEngineSound();
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