using UnityEngine;
using UnityEngine.EventSystems;


namespace Scripts.BaseSystems.UiRelated
{
    public class ScrollableButton : MonoBehaviour,
                                    IPointerDownHandler,
                                    IBeginDragHandler,
                                    IDragHandler,
                                    IPointerUpHandler,
                                    IEndDragHandler
    {
        [SerializeField] 
        private GameObject _normalStateGroup;
        [SerializeField] 
        private GameObject _pressedStateGroup;

        private ScrollRectExtended _scrollRectExArray;

        private void OnEnable()
        {
            _scrollRectExArray = GetComponentInParent<ScrollRectExtended>(); 
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_scrollRectExArray == null) return;

            _scrollRectExArray.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_scrollRectExArray == null) return;

            _scrollRectExArray.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_scrollRectExArray == null) return;

            _scrollRectExArray.OnEndDrag(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_normalStateGroup !=null)
                _normalStateGroup.SetActive(false); 

            if(_pressedStateGroup!=null)
                _pressedStateGroup.SetActive(true); 

            if (_scrollRectExArray == null) return;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_normalStateGroup != null)
                _normalStateGroup.SetActive(true);

            if (_pressedStateGroup != null)
                _pressedStateGroup.SetActive(false);

            if (_scrollRectExArray == null) return;
        }

    }
}




