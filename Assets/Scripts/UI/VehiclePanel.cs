using System;
using DG.Tweening;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI
{
    public class VehiclePanel : BasePanel
    {
        [SerializeField] private TextMeshProUGUI rpmText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI gearText;
        [SerializeField] private RectTransform rpmNeedle;
        [SerializeField] private RectTransform speedNeedle;
        
        public override void Init(UIController uiController)
        {
            base.Init(uiController);
            UpdateUI(0, 0, 0);
        }

        public void UpdateUI(float speedKmh, float engineRpm, int gear)
        {
            speedText.text = $"{Mathf.RoundToInt(speedKmh)} KM/H";
            rpmText.text = $"{Mathf.RoundToInt(engineRpm)}";
            gearText.text = gear == -1 ? "N" : gear.ToString();
            
            rpmNeedle.DORotate(new Vector3(0, 0, Mathf.Lerp(-5, -175, Mathf.InverseLerp(1000, 7000, engineRpm))), 0.2f);
            speedNeedle.DORotate(new Vector3(0, 0, Mathf.Lerp(-5, -175, Mathf.InverseLerp(0, 260, speedKmh))), 0.2f);
        }
    }
}