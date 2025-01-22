using UnityEngine.UI;
using UnityEngine;

namespace App.Core
{
    public sealed class FigureView : MonoBehaviour
    {
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
    }
}