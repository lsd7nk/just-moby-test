using Random = UnityEngine.Random;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using App.Common;
using App.Utils;
using System;

namespace App.Core
{
    public interface IFiguresBuilder : IViewService<FiguresBuilderView>
    {
        public event Action<FigureModel, Vector3> OnFigurePlacedCorrectlyEvent;
        public event Action<FigureModel> OnFigurePlacedUncorrectlyEvent;
        public event Action<FigureModel> OnFigureTakeFromScrollEvent;

        void AddFigure(FigureModel model);
        void Dispose();
    }


    public sealed class FiguresBuilder : IFiguresBuilder
    {
        private const float PLACE_OFFSET = 10f;
        private const float MULTIPLIER_0_5 = 0.5f;

        public event Action<FigureModel, Vector3> OnFigurePlacedCorrectlyEvent;
        public event Action<FigureModel> OnFigurePlacedUncorrectlyEvent;
        public event Action<FigureModel> OnFigureTakeFromScrollEvent;

        private FiguresBuilderView _view;
        private List<FigureModel> _figures;
        private List<FigureModel> _placedFigures;

        private Vector3 _startBuildPosition;

        public void SetView(FiguresBuilderView view)
        {
            _view = view;
        }

        public void AddFigure(FigureModel model)
        {
            _figures.Add(model);

            var draggable = model.GetDraggable();

            draggable.OnDragStartedEvent += OnFigureDragStarted;
            draggable.OnDragEndedEvent += OnFigureDragEnded;
        }

        public void Initialize()
        {
            _placedFigures = new List<FigureModel>();
            _figures = new List<FigureModel>();
        }

        public void Dispose()
        {
            for (int i = 0; i < _figures.Count; ++i)
            {
                var draggable = _figures[i].GetDraggable();

                draggable.OnDragStartedEvent -= OnFigureDragStarted;
                draggable.OnDragEndedEvent -= OnFigureDragEnded;
            }

            _placedFigures.Clear();
            _figures.Clear();
        }

        public async UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            var placedFigureRects = new RectTransform[_placedFigures.Count];

            for (int i = 0; i < placedFigureRects.Length; ++i)
            {
                placedFigureRects[i] = _placedFigures[i].GetRectTransform();
            }

            await _view.HideAsync(placedFigureRects, cancellationToken);
        }

        private void OnFigureDragStarted(DraggableObject draggable)
        {
            if (!TryUnplaceFigure(draggable, out var figure))
            {
                OnFigureTakeFromScrollEvent?.Invoke(figure);
            }
        }

        private void OnFigureDragEnded(DraggableObject draggable)
        {
            bool firstPlace = _placedFigures.Count == 0;

            if (!TryPlaceFigure(draggable, firstPlace, out var figure))
            {
                _figures.Remove(figure);
                OnFigurePlacedUncorrectlyEvent?.Invoke(figure);

                return;
            }

            if (firstPlace)
            {
                return;
            }

            OnFigurePlacedCorrectlyEvent?.Invoke(figure, GetPlacePosition(figure));
        }

        private bool TryUnplaceFigure(DraggableObject draggable, out FigureModel figure)
        {
            figure = GetFigure(draggable);

            if (figure == null)
            {
                return false;
            }

            if (figure.IsPlaced)
            {
                UnplaceFigure(figure);
                return true;
            }

            return false;
        }

        private bool TryPlaceFigure(DraggableObject draggable, bool firstPlace, out FigureModel figure)
        {
            figure = GetFigure(draggable);

            if (figure == null)
            {
                return false;
            }

            var figureRect = figure.GetRectTransform();

            if (!_view.CheckFigureInField(figureRect))
            {
                return false;
            }

            if (firstPlace)
            {
                PlaceFigure(figure);
                return true;
            }

            var lastPlacedFigure = _placedFigures[^1];

            if (RectUtils.CheckRectsTouching(figure.GetRectTransform(),
                lastPlacedFigure.GetRectTransform(), PLACE_OFFSET))
            {
                PlaceFigure(figure);
                return true;
            }

            return false;
        }

        private void UnplaceFigure(FigureModel figure)
        {
            figure.IsPlaced = false;
            _placedFigures.Remove(figure);
        }

        private void PlaceFigure(FigureModel figure)
        {
            figure.IsPlaced = true;
            _placedFigures.Add(figure);
        }

        private Vector3 GetPlacePosition(FigureModel figure)
        {
            var lastPlacedFigure = _placedFigures[^2];
            var lastPlacedPosition = lastPlacedFigure.GetPosition();

            int xMultiplier = Random.Range(-1, 2);

            float xMax = figure.Width * MULTIPLIER_0_5;
            float xMin = 0f;

            Debug.Log($"min: {xMin}, max: {xMax}, mult: {xMultiplier}");

            lastPlacedPosition.y += figure.Height + PLACE_OFFSET * MULTIPLIER_0_5;
            lastPlacedPosition.x += Random.Range(xMin, xMax) * xMultiplier;

            return lastPlacedPosition;
        }

        private FigureModel GetFigure(DraggableObject draggable)
        {
            FigureModel figure = null;

            for (int i = 0; i < _figures.Count; ++i)
            {
                figure = _figures[i];

                if (figure.GetDraggable() != draggable)
                {
                    continue;
                }

                break;
            }

            return figure;
        }
    }
}