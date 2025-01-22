using Doozy.Engine.UI;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using App.Utils;
using App.Core;
using System;

namespace App.Common.Views
{
    public partial class GameView : AdsView
    {
        private const float DESTROY_ANIMATION_DURATION = 0.2f;

        [field: Space(10), SerializeField]
        public FiguresBuilderView FiguresBuilderView { get; private set; }

        [Header("Buttons")]
        [SerializeField] private UIButton _settingsButton;

        [Header("Prefabs")]
        [SerializeField] private FigureView _figurePrefab;
        [SerializeField] private FigureSlot _slotPrefab;

        [Header("Figures data")]
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private RectTransform _slotsParent;
        [SerializeField] private RectTransform _dragContainer;

        private FigureSlot[] _slots;

        public void Initialize(int slotsCount)
        {
            _slots = new FigureSlot[slotsCount];
        }

        public FigureView CreateFigureView(Color color, int index)
        {
            var figureSlot = Instantiate(_slotPrefab, _slotsParent, false);
            var figureView = Instantiate(_figurePrefab, figureSlot.transform, false);

            var draggable = figureView.GetDraggable();

            figureSlot.SetDraggableFigure(draggable);
            figureView.SetColor(color);

            draggable.SetDragContainer(_dragContainer);
            draggable.SetScrollRect(_scroll);

            _slots[index] = figureSlot;

            return figureView;
        }

        public FigureView CopyFigureView(Color color, int index)
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
            rectTransform.DOLocalJump(position, 500, 1, 0.6f)
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