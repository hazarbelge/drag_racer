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
        
        private bool _initialBlockRaycasts;
        private bool _initialInteractable;
        
        public virtual void Init(UIController uiController)
        {
            UIController = uiController;
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _initialBlockRaycasts = _canvasGroup.blocksRaycasts;
            _initialInteractable = _canvasGroup.interactable;
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
                _canvasGroup.interactable = _initialInteractable;
                _canvasGroup.blocksRaycasts = _initialBlockRaycasts;
                gameObject.SetActive(true);
                onComplete?.Invoke();
            }, duration);
        }
        
        public virtual void Hide(Action onComplete = null, float duration = 0.5f)
        {
            FadeOut(() =>
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                gameObject.SetActive(false);
                onComplete?.Invoke();
            }, duration);
        }
        
        public bool IsVisible() => _canvasGroup.alpha > 0 && gameObject.activeInHierarchy;
    }
}