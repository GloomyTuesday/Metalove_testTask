using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scripts.BaseSystems
{
    public abstract class UiPickable : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        [SerializeField]
        private bool _hideContentWhenActivated;

        //  Object that will be turned off when drag mode is ON and will be active when drag mode is OFF
        [SerializeField]
        protected GameObject[] _content;
        [SerializeField]
        protected UnityEvent _activate;

        [SerializeField, Range(0, 100), Header("range from center that triggers the activate event, 100 - means that out of the rect")]
        private float _rangeToDrag;
        protected float DragDistance { get; set; }

        [NonSerialized]
        private bool _triggerDistanceReady;

        private float _triggerDistance;
        protected float TriggerDistanceDistance
        {
            get
            {
                if(!_triggerDistanceReady)
                    _triggerDistance = _rangeToDrag * (RectTransform.rect.width / 2) / 100;

                return _triggerDistance; 
            }
        }

        private RectTransform _rectTransform;
        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private bool _dragMode; 

        protected virtual bool DragMode 
        {
            get => _dragMode;
            set
            {
                ContentState(!value);
                _dragMode = value; 
            }
        }

        private bool ObjectIsTouched { get; set; }

        protected virtual void OnValidate()
        {
            _triggerDistanceReady = false; 
        }

        protected void ContentState(bool state)
        {
            foreach (var item in _content)
                item.SetActive(state);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ObjectIsTouched = true; 
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!ObjectIsTouched) return; 

            var distance = Vector2.Distance(Input.mousePosition, RectTransform.rect.center);

            if (distance >= TriggerDistanceDistance)
            {

                if (_hideContentWhenActivated)
                    ContentState(false);

                _activate?.Invoke(); 
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!ObjectIsTouched) return;

            ObjectIsTouched = false;

            if (_hideContentWhenActivated)
                ContentState(true);
        }
    }
}

