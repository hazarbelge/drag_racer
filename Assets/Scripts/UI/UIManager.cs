using System;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MenuPanel menuPanel;
        [SerializeField] private StartPanel startPanel;
        [SerializeField] private VehiclePanel vehiclePanel;

        private void Awake()
        {
            menuPanel.Init(this);
            startPanel.Init(this);
            vehiclePanel.Init(this);
        }

        public void OnPlayClicked()
        {
            menuPanel.Hide();
            startPanel.Show(() => startPanel.StartCountdown(OnRaceStart));
            vehiclePanel.Show();
        }
        
        private void OnRaceStart()
        {
            //TODO: Enable vehicle controls
        }
        
        public void OnRaceEnd()
        {
            menuPanel.Show();
            vehiclePanel.Hide();
        }
    }
}