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
        public event Action<FigureModel> OnFigurePlacedUncorrectlyEvent;
        public event Action<FigureModel> OnFigureTakeFromScrollEvent;

        void AddFigure(FigureModel model);
        void Dispose();
    }


    public sealed class FiguresBuilder : IFiguresBuilder
    {
        private const float PLACE_OFFSET = 15f;

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

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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
            }
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