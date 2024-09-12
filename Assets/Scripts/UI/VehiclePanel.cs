using System;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI
{
    public class VehiclePanel : BasePanel
    {
        [SerializeField] public TextMeshProUGUI rpmText;
        [SerializeField] public TextMeshProUGUI speedText;
        [SerializeField] public TextMeshProUGUI gearText;
        
        private UIManager _uiManager;
        
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            UpdateUI(0, 0, 0);
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);
            UpdateUI(0, 0, 0);
        }
        
        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
            UpdateUI(0, 0, 0);
        }

        private void UpdateUI(float speedKmh, float engineRpm, int gear)
        {
            speedText.text = $"{Mathf.RoundToInt(speedKmh)} KM/H";
            rpmText.text = $"{Mathf.RoundToInt(engineRpm)} RPM";
            gearText.text = gear == 0 ? "N" : gear.ToString();
        }
    }
}