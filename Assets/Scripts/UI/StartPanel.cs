using System;
using DG.Tweening;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartPanel : BasePanel
    { 
        private const int CountdownSeconds = 5;
        
        [SerializeField] private TMPro.TextMeshProUGUI startText;
        [SerializeField] private Image countdownImage;
        [SerializeField] private Sprite[] countdownSprites = new Sprite[CountdownSeconds];

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);
            startText.alpha = 0;
        }

        public void StartCountdown(Action onComplete)
        {
            UpdateCountdown(CountdownSeconds - 1);
            StartCountdown(CountdownSeconds - 1, onComplete);
        }
        
        private void StartCountdown(int index, Action onComplete)
        {
            if (index == -1)
            {
                StartTextFadeInOut();
                EndCountdown(onComplete);
                return;
            }
            
            DOTween.Sequence()
                .Append(countdownImage.DOFade(1, 0.5f))
                .AppendInterval(0.5f)
                .Append(countdownImage.DOFade(0, 0.5f))
                .OnComplete(() =>
                {
                    UpdateCountdown(index - 1);
                    StartCountdown(index - 1, onComplete);
                });
        }
        
        private void UpdateCountdown(int index)
        {
            if (index == -1)
            {
                return;
            }
            
            countdownImage.sprite = countdownSprites[index];
        }
        
        private void StartTextFadeInOut()
        {
            DOTween.Sequence()
                .AppendCallback(() => startText.gameObject.SetActive(true))
                .Append(startText.DOFade(1, 0.5f))
                .AppendInterval(1)
                .Append(startText.DOFade(0, 0.5f))
                .AppendCallback(() => startText.gameObject.SetActive(false))
                .OnComplete(() => Hide());
        }
        
        private static void EndCountdown(Action onComplete)
        {
            onComplete?.Invoke();
        }
    }
}