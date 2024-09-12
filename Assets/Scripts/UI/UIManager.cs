using System;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MenuPanel menuPanel;
        [SerializeField] private VehiclePanel vehiclePanel;

        private void Awake()
        {
            menuPanel.Init(this);
            vehiclePanel.Init(this);
        }

        public void OnPlayClicked()
        {
            menuPanel.Hide();
            vehiclePanel.Show();
        }
        
        public void OnRaceEnd()
        {
            menuPanel.Show();
            vehiclePanel.Hide();
        }
    }
}