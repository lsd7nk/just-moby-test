using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

namespace App.Core
{
    public sealed class DraggableObject : MonoBehaviour,
        IBeginDragHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler,
        IEndDragHandler
    {
        private const float SCALE_UP_DURATION = 0.2f;
        private readonly Vector2 VECTOR_0_5 = new Vector2(0.5f, 0.5f);

        public static float DragOffset => 0.075f * Screen.width;

        [field: SerializeField] public bool Interactable { get; set; } = true;
        [field: SerializeField] public bool ApplyDragThreshold { get; set; } = true;
        [field: SerializeField] public float DragThreshold { get; set; } = 10;

        public event Action<DraggableObject> OnDragStartedEvent;
        public event Action<DraggableObject> OnDraggedEvent;
        public event Action<DraggableObject> OnDragEndedEvent;

        private RectTransform _dragContainer;
        private RectTransform _rectTransform;
        private Transform _startDragParent;

        private ScrollRect _containerScrollRect;
        private Camera _camera;

        private Vector3 _startDragPointerScreenPosition;
        private Vector3 _startDragLocalPosition;
        private Vector3 _startDragScale;

        private float _canvasPlaneDistance = 0.0f;
        private bool _dragging = false;
        private float _dragOffset;

        private Tween _scaleUpTween;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable)
            {
                _dragging = false;
                return;
            }

            _startDragLocalPosition = _rectTransform.localPosition;
            _startDragPointerScreenPosition = Input.mousePosition;

            if (_rectTransform.parent != _dragContainer)
            {
                // Forward to the scroll rect
                if (_containerScrollRect is IPointerDownHandler containerBeginDragHandler)
                {
                    containerBeginDragHandler.OnPointerDown(eventData);
                }
            }

            if (!ApplyDragThreshold)
            {
                BeginDragging();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Interactable || _dragging)
            {
                return;
            }

            _startDragLocalPosition = _rectTransform.localPosition;
            _startDragPointerScreenPosition = Input.mousePosition;

            if (_rectTransform.parent != _dragContainer)
            {
                // Forward to the scroll rect
                if (_containerScrollRect is IBeginDragHandler containerBeginDragHandler)
                {
                    containerBeginDragHandler.OnBeginDrag(eventData);
                }
            }
        }

        private void BeginDragging()
        {
            _dragging = true;
            _startDragScale = _rectTransform.localScale;

            _rectTransform.anchorMin = VECTOR_0_5;
            _rectTransform.anchorMax = VECTOR_0_5;
            _rectTransform.pivot = VECTOR_0_5;

            _startDragParent = _rectTransform.parent;

            _rectTransform.SetParent(_dragContainer, true);

            _dragOffset = 0.0f;
            var pointerOffset = _rectTransform.position - GetMousePosition(_dragOffset);

            _scaleUpTween = DOTween.To(() => _dragOffset, v =>
            {
                _dragOffset = v;
                var t = v / DragOffset;
                _rectTransform.position = GetMousePosition(_dragOffset) + Vector3.Lerp(pointerOffset, Vector3.zero, t);
            }, DragOffset, SCALE_UP_DURATION)
                .SetEase(Ease.InOutSine);

            OnDragStartedEvent?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Interactable)
            {
                return;
            }

            if (!_dragging)
            {
                if (ApplyDragThreshold)
                {
                    float y = Input.mousePosition.y - _startDragPointerScreenPosition.y;

                    if (y < DragThreshold)
                    {
                        if (_rectTransform.parent != _dragContainer)
                        {
                            // Forward to the scroll rect
                            if (_containerScrollRect is IDragHandler containerDragHandler)
                            {
                                containerDragHandler.OnDrag(eventData);
                            }
                        }

                        return;
                    }
                }

                BeginDragging();

                if (_rectTransform.parent != _dragContainer)
                {
                    if (_containerScrollRect is IEndDragHandler containerEndDragHandler)
                    {
                        containerEndDragHandler.OnEndDrag(eventData);
                    }
                }
            }

            _rectTransform.position = GetMousePosition(_dragOffset);

            OnDraggedEvent?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_dragging)
            {
                if (_rectTransform.parent != _dragContainer)
                {
                    if (_containerScrollRect is IPointerUpHandler containerEndDragHandler)
                    {
                        containerEndDragHandler.OnPointerUp(eventData);
                    }
                }

                return;
            }

            StopDragging();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragging)
            {
                if (_rectTransform.parent != _dragContainer)
                {
                    if (_containerScrollRect is IEndDragHandler containerEndDragHandler)
                    {
                        containerEndDragHandler.OnEndDrag(eventData);
                    }
                }

                return;
            }

            StopDragging();
        }

        public void CancelDrag()
        {
            StopDragging();
            MoveToStartDragPosition();
        }

        public void SetDragContainer(RectTransform container)
        {
            _dragContainer = container;
        }

        public void SetScrollRect(ScrollRect scrollRect)
        {
            _containerScrollRect = scrollRect;
        }

        private void StopDragging()
        {
            if (!_dragging)
            {
                return;
            }

            _dragging = false;

            KillScaleUpTween();

            _rectTransform.localScale = _startDragScale;

            OnDragEndedEvent?.Invoke(this);
        }

        private void KillScaleUpTween()
        {
            if (_scaleUpTween == null || !_scaleUpTween.active)
            {
                return;
            }

            _scaleUpTween.Kill();
            _scaleUpTween = null;
        }

        private void MoveToStartDragPosition()
        {
            _rectTransform.SetParent(_startDragParent, true);

            _rectTransform.localPosition = _startDragLocalPosition;
            _startDragParent = null;
        }

        private Vector3 GetMousePosition(float offset = 0)
        {
            var pointerPosition = Input.mousePosition;
            pointerPosition.y += offset;

            if (_camera != null)
            {
                pointerPosition.z = _canvasPlaneDistance;
                return _camera.ScreenToWorldPoint(pointerPosition);
            }

            pointerPosition.z = 0;
            return pointerPosition;
        }

        private void Start()
        {
            if (_rectTransform == null)
            {
                _rectTransform = this.transform as RectTransform;
            }

            var canvas = _rectTransform.root.GetComponent<Canvas>();

            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _camera = canvas.worldCamera;
                _canvasPlaneDistance = canvas.planeDistance;
            }
        }
    }
}
