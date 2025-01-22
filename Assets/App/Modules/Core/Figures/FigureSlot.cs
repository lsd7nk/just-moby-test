using UnityEngine.EventSystems;
using UnityEngine;

namespace App.Core
{
    public sealed class FigureSlot : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler,
        IEndDragHandler
    {
        private DraggableObject _draggable;

        public void SetDraggableFigure(DraggableObject draggable)
        {
            _draggable = draggable;
        }

        public DraggableObject GetDraggable()
        {
            return _draggable;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _draggable.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _draggable.OnPointerUp(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _draggable.OnEndDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _draggable.OnDrag(eventData);
        }
    }
}
