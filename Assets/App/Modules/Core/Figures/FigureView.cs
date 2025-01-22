using UnityEngine.UI;
using UnityEngine;

namespace App.Core
{
    public sealed class FigureView : MonoBehaviour
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        [SerializeField] private DraggableObject _draggable;
        [SerializeField] private Image _image;

        public void SetColor(Color color)
        {
            _image.color = color;
        }

        public DraggableObject GetDraggable()
        {
            return _draggable;
        }

        private void Start()
        {
            Width = RectTransform.sizeDelta.x;
            Height = RectTransform.sizeDelta.y;
        }
    }
}