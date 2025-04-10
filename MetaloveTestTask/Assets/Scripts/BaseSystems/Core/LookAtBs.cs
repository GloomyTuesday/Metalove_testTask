using UnityEngine;

namespace Scripts.BaseSystems
{
    public class LookAtBs : MonoBehaviour
    {
        [SerializeField]
        private bool _apply; 

        [SerializeField]
        private Transform _target;
        [SerializeField]
        private Transform _upVectorSource;

        [SerializeField, FilterByType(typeof(ITimeUtilitiesEventsCallbackHandler))]
        private UnityEngine.Object _timeUtilitiesEventsCallbackHandlerObj;

        private ITimeUtilitiesEventsCallbackHandler _iTimeUtilitiesEventsCallbackHandler;
        private ITimeUtilitiesEventsCallbackHandler ITimeUtilitiesEventsCallbackHandler
        {
            get
            {
                if (_iTimeUtilitiesEventsCallbackHandler == null)
                    _iTimeUtilitiesEventsCallbackHandler = _timeUtilitiesEventsCallbackHandlerObj.GetComponent<ITimeUtilitiesEventsCallbackHandler>();

                return _iTimeUtilitiesEventsCallbackHandler;
            }
        }

        private void OnValidate()
        {
            if(_apply)
            {
                _apply = false;
                transform.LookAt(_target, _upVectorSource.up);
            }
        }

        private void OnEnable()
        {
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            ITimeUtilitiesEventsCallbackHandler.OnUnityFixedUpdate += UnityCallback;
        }

        private void Unsubscribe()
        {
            ITimeUtilitiesEventsCallbackHandler.OnUnityFixedUpdate -= UnityCallback;
        }

        private void UnityCallback()
        {
            transform.LookAt(_target, _upVectorSource.up);
        }

        public void ApplyLookAt()
        {

        }
    }
}
