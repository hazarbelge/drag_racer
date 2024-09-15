using UnityEngine;

namespace Vehicle
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private Transform vehicleTransform;
        [SerializeField] private Transform[] wheels = new Transform[4];
        [SerializeField] private AnimationCurve speedRpmCurve;
        private static bool ThrottleInput => Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        private static bool BrakeInput => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        
        private GameController _gameController;
        private VehicleModel _vehicleModel;
        
        private bool _canSpeedUp;
        
        public void Init(GameController gameController)
        {
            _gameController = gameController;
            
            InitVehicleModel();
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
                speedRpmCurve,
                shiftDownRpm,
                shiftUpRpm,
                maxRpm,
                tireDiameter
            );
        }

        private void Update()
        {
            _vehicleModel.ApplyBrake(BrakeInput);
            _vehicleModel.ApplyThrottle(ThrottleInput && !BrakeInput);
            _vehicleModel.ApplyDeceleration(!ThrottleInput && !BrakeInput);
            
            if (_canSpeedUp)
            {
                var deltaTime = Time.deltaTime;
            
                _vehicleModel.UpdateSpeed(deltaTime);
            
                var distance = _vehicleModel.CurrentSpeed * deltaTime;
            
                UpdateWheelRotation(distance);
                MoveVehicle(distance);
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
        
        public void OnRaceStart()
        {
            _vehicleModel.OnRaceStart();
            _canSpeedUp = true;
        }
        
        public void OnRaceEnd()
        {
            _canSpeedUp = false;
        }
    }
}