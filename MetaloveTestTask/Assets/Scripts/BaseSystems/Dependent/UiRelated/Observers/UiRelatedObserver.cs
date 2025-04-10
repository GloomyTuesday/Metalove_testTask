using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public class UiRelatedObserver : MonoBehaviour
    {
        [SerializeField]
        private Transform _uiHolder; 

        [SerializeField,Space(10), FilterByType(typeof(IUiRelatedEventsHandler))]
        private Object _uiRelatedEventsObj;

        [SerializeField, Space(10), Header("Frame delay until unregistering")]
        private uint _unregisteringDelay = 1;

        //  As key is used instance id
        private Dictionary<int, RectTransform> CancellationRectTransforms = new Dictionary<int, RectTransform>();
        private HashSet<int> UnregisteringId { get; set; } = new HashSet<int>();

        private CancellationTokenSource CancellationTokenSource { get; set; }

        private IUiRelatedEventsHandler _iUiRelatedEventsHandler;
        private IUiRelatedEventsHandler IUiRelatedEventsHandler
        {
            get {
                if (_iUiRelatedEventsHandler == null)
                    _iUiRelatedEventsHandler = _uiRelatedEventsObj.GetComponent<IUiRelatedEventsHandler>();
                return _iUiRelatedEventsHandler; 
            }
        }

        private struct CancelationUnit
        {
            public int _instanceId;
            public Canvas _canvas; 
            public RectTransform _rectTransform;

            public CancelationUnit(RectTransform rectTransform)
            {
                _instanceId = rectTransform.GetInstanceID(); 
                _rectTransform = rectTransform;
                _canvas = rectTransform.GetComponentInParent<Canvas>();
            }
        }

        private void OnEnable()
        {
            CancellationTokenSource = new CancellationTokenSource(); 
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            IUiRelatedEventsHandler.OnRegisterCancellationRectTransform += RegisterCancellationRect;
            IUiRelatedEventsHandler.OnUnRegisterCancellationRect += UnRegisterCancellationRect;
            IUiRelatedEventsHandler.OnUnRegisterCancellationRectImmediate += UnRegisterCancellationRectImmediate;

            IUiRelatedEventsHandler.OnIsPointerOverCancelationRect += IsPointerOverCancelationRect;

            IUiRelatedEventsHandler.OnGetUiHolderTransform += GetUiHolderTransform;
            
        }

        private void Unsubscribe()
        {
            IUiRelatedEventsHandler.OnRegisterCancellationRectTransform -= RegisterCancellationRect;
            IUiRelatedEventsHandler.OnUnRegisterCancellationRect -= UnRegisterCancellationRect;
            IUiRelatedEventsHandler.OnUnRegisterCancellationRectImmediate -= UnRegisterCancellationRectImmediate;

            IUiRelatedEventsHandler.OnIsPointerOverCancelationRect -= IsPointerOverCancelationRect;

            IUiRelatedEventsHandler.OnGetUiHolderTransform -= GetUiHolderTransform;
            
        }

        private bool IsPointerOverCancelationRect(Vector2 position)
        {
            foreach (var item in CancellationRectTransforms)
                if (RectTransformUtility.RectangleContainsScreenPoint(item.Value, position))
                    return true; 

            return false;
        }

        private async void UnRegisterCancellationRect(RectTransform rectTransform)
        {
            var instanceId = rectTransform.GetInstanceID();
            if (!CancellationRectTransforms.ContainsKey(instanceId)) return;
            if (UnregisteringId.Contains(instanceId)) return;

            UnregisteringId.Add(instanceId); 
            int frameCount = 0;
            var token = CancellationTokenSource.Token;

            while (frameCount < _unregisteringDelay)
            {
                if(token.IsCancellationRequested)
                await Task.Yield();
                frameCount++;
            }

            if (!CancellationRectTransforms.ContainsKey(instanceId)) return; 

            CancellationRectTransforms.Remove(instanceId);
            UnregisteringId.Remove(instanceId); 
        }

        private void UnRegisterCancellationRectImmediate(RectTransform rectTransform)
        {
            var instanceId = rectTransform.GetInstanceID();
            if (!CancellationRectTransforms.ContainsKey(instanceId)) return;
            
            CancellationRectTransforms.Remove(instanceId);
        }

        private void RegisterCancellationRect(RectTransform rectTransform)
        {
            var instanceId = rectTransform.GetInstanceID();
            if (CancellationRectTransforms.ContainsKey(instanceId)) return;

            CancellationRectTransforms.Add(instanceId, rectTransform);
        }

        private Transform GetUiHolderTransform() => _uiHolder;
    }
}

