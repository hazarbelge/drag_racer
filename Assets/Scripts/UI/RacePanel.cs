using System;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI
{
    public class RacePanel : BasePanel
    { 
        [SerializeField] private TextMeshProUGUI raceTimeText;
        [SerializeField] private TextMeshProUGUI distanceText;
        
        public override void Show(Action onComplete = null, float duration = 0.1f)
        {
            base.Show(onComplete, duration);
            UpdateUI(0f, 0f);
        }
        
        public void UpdateUI(float raceTime, float distance)
        {
            var timeSpan = TimeSpan.FromSeconds(raceTime);
            raceTimeText.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
            distanceText.text = $"{Mathf.RoundToInt(distance)} M";
        }
    }
}