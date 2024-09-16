using System;
using Controller;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private MenuPanel menuPanel;
        [SerializeField] private StartPanel startPanel;
        [SerializeField] private RacePanel racePanel;
        [SerializeField] private VehiclePanel vehiclePanel;

        private GameController _gameController;

        private bool _isRaceStarted;
        private float _currentRaceTime;
        private float _totalDistance;
        
        public void Init(GameController gameController)
        {
            _gameController = gameController;

            menuPanel.Init(this);
            startPanel.Init(this);
            racePanel.Init(this);
            vehiclePanel.Init(this);
            
            menuPanel.Show();
            racePanel.Hide();
            vehiclePanel.Hide();
            startPanel.Hide();
            
            _currentRaceTime = 0f;
            _totalDistance = 0f;
        }

        public void OnPlayClicked()
        {
            _gameController.OnCountdownStart();
            menuPanel.Hide();
            vehiclePanel.Show();
            racePanel.Show();
            startPanel.Show(() => startPanel.StartCountdown(_gameController.OnRaceStart));
        }

        private void Update()
        {
            if (!_isRaceStarted)
            {
                return;
            }
            
            _currentRaceTime += Time.deltaTime;
        }

        public void OnRaceStart()
        {
            _isRaceStarted = true;
        }
        
        public void UpdateRaceInfo(float totalDistance, float speedKmh, float engineRpm, int gear)
        {
            _totalDistance = totalDistance;
            racePanel.UpdateUI(_currentRaceTime, _totalDistance);
            vehiclePanel.UpdateUI(speedKmh, engineRpm, !_isRaceStarted ? -1 : gear);
        }
        
        public void OnRaceEnd(Action callback)
        {
            _isRaceStarted = false;
            
            menuPanel.Show(callback, duration: 1.25f);
            menuPanel.UpdateUI(_currentRaceTime, _totalDistance);
            racePanel.Hide(duration: 1.25f);
            vehiclePanel.Hide();
            startPanel.Hide();
        }
    }
}