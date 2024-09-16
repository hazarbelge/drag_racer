using DG.Tweening;
using UI;
using UnityEngine;
using Vehicle;
using static Enums;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private CameraController cameraController;

        public GameState GameState { get; private set; } = GameState.Menu;
    
        private void Awake()
        {
            Application.targetFrameRate = -1;
            DOTween.SetTweensCapacity(500, 50);
        }
        
        private void Start()
        {
            uiController.Init(this);
            vehicleController.Init(this);
            cameraController.Init();
        }
    
        public void OnCountdownStart()
        {
            GameState = GameState.Countdown;
            cameraController.OnCountdownStart();
            vehicleController.OnCountdownStart();
        }
    
        public void OnRaceStart()
        {
            GameState = GameState.Racing;
            vehicleController.OnRaceStart();
        }
    
        public void UpdateRaceInfo(float totalDistance, float speedKmh, float engineRpm, int gear)
        {
            uiController.UpdateRaceInfo(totalDistance, speedKmh, engineRpm, gear);
        }
    
        public void OnRaceEnd()
        {
            GameState = GameState.PostRace;
            cameraController.OnRaceEnd();
            uiController.OnRaceEnd(() =>
            {
                vehicleController.OnRaceEnd();
                GameState = GameState.Menu;
            });
        }
    }
}