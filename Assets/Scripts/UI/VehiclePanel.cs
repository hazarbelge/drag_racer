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
        
        [SerializeField]  private GameObject pointPrefab;
        private const float StartAngle = 80f;
        private const float EndAngle = -80f;

        public override void Init(UIController uiController)
        {
            base.Init(uiController);
            UpdateUI(0, 0, 0);
            GenerateNumbers(rpmNeedle.transform.parent, 6, 6);
            GenerateNumbers(speedNeedle.transform.parent, 270, 9);
        }

        public void UpdateUI(float speedKmh, float engineRpm, int gear)
        {
            speedText.text = $"{Mathf.RoundToInt(speedKmh)} KM/H";
            rpmText.text = $"{Mathf.RoundToInt(engineRpm)}";
            gearText.text = gear == -1 ? "N" : gear.ToString();
            
            rpmNeedle.DORotate(new Vector3(0, 0, Mathf.Lerp(-5, -175, Mathf.InverseLerp(0, 6000, engineRpm))), 0.2f);
            speedNeedle.DORotate(new Vector3(0, 0, Mathf.Lerp(-5, -175, Mathf.InverseLerp(0, 270, speedKmh))), 0.2f);
        }
        
        private void GenerateNumbers(Transform parentTransform, int maxValue, int pointCount)
        {
            const float range = EndAngle - StartAngle;
            var increment = range / pointCount;

            for (var i = 0; i <= pointCount; i++)
            {
                var pointObj = Instantiate(pointPrefab, parentTransform);

                pointObj.GetComponentInChildren<TextMeshProUGUI>().text = (maxValue / pointCount * i).ToString();
                pointObj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, StartAngle + increment * i);

                pointObj.transform.SetSiblingIndex(0);
            }
        }
    }
}