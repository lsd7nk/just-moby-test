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
        void AddFigure(FigureModel model);
    }


    public sealed class FiguresBuilder : IFiguresBuilder
    {
        private const float PLACE_OFFSET = 20f;

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

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private void OnFigureDragStarted(DraggableObject draggable)
        {

        }

        private void OnFigureDragEnded(DraggableObject draggable)
        {
            bool firstPlace = _placedFigures.Count == 0;

            if (TryPlaceFigure(draggable, firstPlace))
            {
                // to do: place figure
            }


        }

        private void UnplaceFigure(DraggableObject draggable)
        {
            for (int i = 0; i < _figures.Count; ++i)
            {
                var figure = _figures[i];

                if (!figure.IsPlaced)
                {
                    continue;
                }

                if (figure.GetDraggable() != draggable)
                {
                    continue;
                }

                figure.IsPlaced = false;
                _placedFigures.Remove(figure);

                break;
            }
        }

        private bool TryPlaceFigure(DraggableObject draggable, bool firstPlace)
        {
            if (firstPlace)
            {
                PlaceFigure(draggable, true);
                return true;
            }

            var figure = GetFigure(draggable);

            if (figure == null)
            {
                return false;
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

        private void PlaceFigure(DraggableObject draggable, bool firstPlace)
        {
            var figure = GetFigure(draggable);

            if (figure == null)
            {
                return;
            }

            PlaceFigure(figure);
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