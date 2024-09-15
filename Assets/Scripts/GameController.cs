using UI;
using UnityEngine;
using Vehicle;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private VehicleController vehicleController;
    
    private void Awake()
    {
        uiController.Init(this);
        vehicleController.Init(this);
    }
    
    public void OnRaceStart()
    {
        uiController.OnRaceStart();
        vehicleController.OnRaceStart();
    }
    
    public void UpdateRaceInfo(float totalDistance, float speedKmh, float engineRpm, int gear)
    {
        uiController.UpdateRaceInfo(totalDistance, speedKmh, engineRpm, gear);
    }
    
    public void OnRaceEnd()
    {
        vehicleController.OnRaceEnd();
        uiController.OnRaceEnd();
    }
}