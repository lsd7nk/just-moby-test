using UnityEngine;
using System;

namespace App.Core
{
    public sealed class FigureModel : IDisposable
    {
        public float Width => _view.Width;
        public float Height => _view.Height;

        public bool IsPlaced { get; set; }

        public readonly Color Color;
        public readonly int Index;

        private FigureView _view;

        public FigureModel(Color color, int index)
        {
            Color = color;
            Index = index;
        }

        public void SetView(FigureView view)
        {
            _view = view;
        }

        public Vector3 GetPosition()
        {
            return _view.RectTransform.localPosition;
        }

        public RectTransform GetRectTransform()
        {
            return _view.RectTransform;
        }

        public DraggableObject GetDraggable()
        {
            return _view.GetDraggable();
        }

        public void Dispose()
        {
            GameObject.Destroy(_view.gameObject);
        }
    }
}