using UnityEngine;

namespace App.Core
{
    public sealed class FigureModel
    {
        public bool IsPlaced { get; set; }

        public readonly Color Color;
        private FigureView _view;

        public FigureModel(Color color)
        {
            Color = color;
        }

        public void SetView(FigureView view)
        {
            _view = view;
        }

        public Vector3 GetPosition()
        {
            return _view.transform.position;
        }

        public RectTransform GetRectTransform()
        {
            return _view.RectTransform;
        }

        public DraggableObject GetDraggable()
        {
            return _view.GetDraggable();
        }
    }
}