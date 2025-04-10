using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public class CancelationTrigger : MonoBehaviour
    {
        [Header(" Keep in mind that cancellation rect trigger can have a different un registering time")]
        [SerializeField, FilterByType(typeof(IUiRelatedEventsInvoker))]
        private Object _uiRelatedEventsInvokerObj;

        private RectTransform _mainRectTRansform; 
        private RectTransform MainRectTRansform
        {
            get
            {
                if (_mainRectTRansform == null)
                    _mainRectTRansform = GetComponent<RectTransform>();

                return _mainRectTRansform; 
            }
        }

        private IUiRelatedEventsInvoker _iUiRelatedEventsInvoker;
        private IUiRelatedEventsInvoker IUiRelatedEventsInvoker
        {
            get
            {
                if (_iUiRelatedEventsInvoker == null)
                    _iUiRelatedEventsInvoker = _uiRelatedEventsInvokerObj.GetComponent<IUiRelatedEventsInvoker>();
                return _iUiRelatedEventsInvoker; 
            }
        }

        private void OnEnable()
        {
            IUiRelatedEventsInvoker.RegisterCancellationRectTransform(MainRectTRansform); 
        }

        private void OnDisable()
        {
            IUiRelatedEventsInvoker.UnRegisterCancellationRect(MainRectTRansform); 
        }
    }
}
