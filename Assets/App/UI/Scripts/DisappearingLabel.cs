using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

namespace App.UI
{
    public sealed class DisappearingLabel : MonoBehaviour
    {
        private const float SCALE_ANIMATION_DURATION = 0.15f;
        private const float VISIBLE_DELAY = 0.35f;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _label;

        private Tween _moveTween;
        private bool _isVisible;

        public void SetText(string value)
        {
            if (_isVisible)
            {
                ShowWithDelay(() =>
                {
                    _label.text = value;
                }).Forget();

                return;
            }

            _label.text = value;
            Show();
        }

        private void Show()
        {
            if (_isVisible)
            {
                return;
            }

            KillMoveTween();

            _isVisible = true;
            _moveTween = _rect.DOScale(Vector3.one, SCALE_ANIMATION_DURATION)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    Hide(VISIBLE_DELAY);
                });
        }

        private void Hide(float delay = 0)
        {
            if (!_isVisible)
            {
                return;
            }

            KillMoveTween();

            _moveTween = _rect.DOScale(Vector3.zero, SCALE_ANIMATION_DURATION)
                .SetEase(Ease.InOutCubic)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    _isVisible = false;
                });
        }

        private async UniTaskVoid ShowWithDelay(Action callback = null)
        {
            await UniTask.WaitUntil(() => !_isVisible);

            callback?.Invoke();
            Show();
        }

        private void KillMoveTween()
        {
            if (_moveTween == null)
            {
                return;
            }

            _moveTween.Kill();
            _moveTween = null;
        }

        private void OnDisable()
        {
            KillMoveTween();
        }
    }
}