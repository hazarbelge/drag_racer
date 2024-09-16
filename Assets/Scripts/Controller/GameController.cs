using UI;
using UnityEngine;
using Vehicle;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private CameraController cameraController;
    
        private void Awake()
        {
            uiController.Init(this);
            vehicleController.Init(this);
        }
    
        public void OnRaceStart()
        {
            cameraController.OnRaceStart();
            uiController.OnRaceStart();
            vehicleController.OnRaceStart();
        }
    
        public void UpdateRaceInfo(float totalDistance, float speedKmh, float engineRpm, int gear)
        {
            uiController.UpdateRaceInfo(totalDistance, speedKmh, engineRpm, gear);
        }
    
        public void OnRaceEnd()
        {
            cameraController.OnRaceEnd();
            uiController.OnRaceEnd(() =>
            {
                vehicleController.OnRaceEnd();
            });
        }
    }
}