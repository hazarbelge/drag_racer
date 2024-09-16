using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePanel : MonoBehaviour
    {
        protected UIController UIController;
        private CanvasGroup _canvasGroup;
        
        public virtual void Init(UIController uiController)
        {
            UIController = uiController;
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        private void FadeOut(Action onComplete = null, float duration = 0.5f)
        {
            DOTween.Kill(_canvasGroup.alpha, true);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        private void FadeIn(Action onComplete = null, float duration = 0.5f)
        {
            DOTween.Kill(_canvasGroup.alpha, true);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public virtual void Show(Action onComplete = null, float duration = 0.5f)
        {
            FadeIn(() =>
            {
                _canvasGroup.alpha = 1;
                gameObject.SetActive(true);
                onComplete?.Invoke();
            }, duration);
        }
        
        public virtual void Hide(Action onComplete = null, float duration = 0.5f)
        {
            FadeOut(() =>
            {
                _canvasGroup.alpha = 0;
                gameObject.SetActive(false);
                onComplete?.Invoke();
            }, duration);
        }
        
        public bool IsVisible() => _canvasGroup.alpha > 0 && gameObject.activeInHierarchy;
    }
}