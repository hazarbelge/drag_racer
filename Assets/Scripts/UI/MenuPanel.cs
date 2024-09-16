using System;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI
{
    public class MenuPanel : BasePanel
    { 
        [SerializeField] private TextMeshProUGUI raceTimeText;
        [SerializeField] private TextMeshProUGUI distanceText;

        public override void Show(Action onComplete = null, float duration = 0.1f)
        {
            base.Show(onComplete, duration);
            UpdateUI(0, 0);
        }

        public void OnPlayClicked()
        {
            UIController.OnPlayClicked();
        }
        
        public void UpdateUI(float raceTime, float distance)
        {
            raceTimeText.gameObject.SetActive(raceTime > 0);
            distanceText.gameObject.SetActive(distance > 0);
            
            var timeSpan = TimeSpan.FromSeconds(raceTime);
            raceTimeText.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
            distanceText.text = $"{Mathf.RoundToInt(distance)} M";
        }
    }
}