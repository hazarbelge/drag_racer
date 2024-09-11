using TMPro;
using UnityEngine;

namespace UI
{
    public class VehiclePanel : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI rpmText;
        [SerializeField] public TextMeshProUGUI speedText;
        [SerializeField] public TextMeshProUGUI gearText;
        
        private void Start()
        {
            UpdateUI(0, 0, 0);
        }

        private void UpdateUI(float speedKmh, float engineRpm, int gear)
        {
            speedText.text = $"{Mathf.RoundToInt(speedKmh)} KM/H";
            rpmText.text = $"{Mathf.RoundToInt(engineRpm)} RPM";
            gearText.text = $"{gear}";
        }
    }
}