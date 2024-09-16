using System;
using UI.Base;
using UnityEngine;

namespace UI
{
    public class RacePanel : BasePanel
    { 
        [SerializeField] private TMPro.TextMeshProUGUI raceTimeText;
        
        public override void Show(Action onComplete = null, float duration = 0.5f)
        {
            base.Show(onComplete, duration);
            UpdateRaceTime(0);
        }
        
        public void UpdateRaceTime(float raceTime)
        {
            var timeSpan = TimeSpan.FromSeconds(raceTime);
            raceTimeText.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
        }
    }
}