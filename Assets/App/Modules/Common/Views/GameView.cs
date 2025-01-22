using Doozy.Engine.UI;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using App.Utils;
using App.Core;
using System;
using App.UI;

namespace App.Common.Views
{
    public partial class GameView : AdsView
    {
        private const float DESTROY_ANIMATION_DURATION = 0.2f;

        private const float JUMP_ANIMATION_DURATION = 0.6f;
        private const int JUMP_ANIMATION_FORCE = 500;

        public int LastSlotIndex { get; private set; }

        [field: Space(10), SerializeField] public FiguresBuilderView FiguresBuilderView { get; private set; }

        [Header("Buttons")]
        [SerializeField] private UIButton _settingsButton;

        [Header("Prefabs")]
        [SerializeField] private FigureView _figurePrefab;

        [Header("Figures data")]
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private RectTransform _dragContainer;

        [Space(10), SerializeField] private FigureSlot[] _slots;

        [Header("Other")]
        [SerializeField] private DisappearingLabel _disappearingLabel;

        public void Initialize(int slotsCount)
        {
            int availableSlotsCount = slotsCount > _slots.Length
                ? _slots.Length
                : slotsCount;

            for (int i = 0; i < availableSlotsCount; ++i)
            {
                _slots[i].gameObject.SetActive(true);
            }

            LastSlotIndex = availableSlotsCount - 1;
        }

        public void SetDisappearingText(string value)
        {
            _disappearingLabel.SetText(value);
        }

        public FigureView CreateFigureView(Color color, int index)
        {
            var figureSlot = _slots[index];
            var figureView = Instantiate(_figurePrefab, figureSlot.transform, false);
            var draggable = figureView.GetDraggable();

            figureSlot.SetDraggableFigure(draggable);
            figureView.SetColor(color);

            draggable.SetDragContainer(_dragContainer);
            draggable.SetScrollRect(_scroll);

            return figureView;
        }

        public void PlayDestroyAnimation(RectTransform rectTransform, Action completeCallback = null)
        {
            rectTransform.DOScale(Vector3.zero, DESTROY_ANIMATION_DURATION)
                .SetLink(rectTransform.gameObject)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    completeCallback?.Invoke();
                });
        }

        public void PlayJumpAnimation(RectTransform rectTransform, Vector3 position, Action completeCallback = null)
        {
            rectTransform.DOLocalJump(position, JUMP_ANIMATION_FORCE, 1, JUMP_ANIMATION_DURATION)
                .SetLink(rectTransform.gameObject)
                .OnComplete(() =>
                {
                    completeCallback?.Invoke();
                });
        }
    }


    public partial class GameView
    {
        public void AddSettingsButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_settingsButton, handler);
        }
    }
}