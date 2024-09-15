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
        
        private void FadeOut(Action onComplete = null)
        {
            DOTween.Kill(_canvasGroup.alpha, true);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 0.5f).OnComplete(() => onComplete?.Invoke());
        }
        
        private void FadeIn(Action onComplete = null)
        {
            DOTween.Kill(_canvasGroup.alpha, true);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, 0.5f).OnComplete(() => onComplete?.Invoke());
        }
        
        public virtual void Show(Action onComplete = null)
        {
            FadeIn(() =>
            {
                _canvasGroup.alpha = 1;
                gameObject.SetActive(true);
                onComplete?.Invoke();
            });
        }
        
        public virtual void Hide(Action onComplete = null)
        {
            FadeOut(() =>
            {
                _canvasGroup.alpha = 0;
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
        
        public bool IsVisible() => _canvasGroup.alpha > 0 && gameObject.activeInHierarchy;
    }
}