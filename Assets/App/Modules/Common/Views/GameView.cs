using Doozy.Engine.UI;
using UnityEngine.UI;
using UnityEngine;
using App.Utils;
using App.Core;
using System;

namespace App.Common.Views
{
    public partial class GameView : AdsView
    {
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

        public FigureView CreateFigureView(Color color)
        {
            var figureSlot = Instantiate(_slotPrefab, _slotsParent, false);
            var figureView = Instantiate(_figurePrefab, figureSlot.transform, false);

            var draggable = figureView.GetDraggable();

            figureSlot.SetDraggableFigure(draggable);
            figureView.SetColor(color);

            draggable.SetDragContainer(_dragContainer);
            draggable.SetScrollRect(_scroll);

            return figureView;
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