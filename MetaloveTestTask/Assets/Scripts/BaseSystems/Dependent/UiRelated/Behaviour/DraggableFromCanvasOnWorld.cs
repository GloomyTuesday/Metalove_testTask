using Scripts.BaseSystems.Core;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.BaseSystems.UiRelated
{
    /*
    [RequireComponent(typeof(ScrollableButton))]
    public class DraggableFromCanvasOnWorld : MonoBehaviour,
        IUiDraggable,
        IDragHandler,
        IPointerUpHandler
    {
        [SerializeField, Header("Only for information visualisation"), Space(10)]
        private LayerMask _layerMask;

        [SerializeField, Space(10)]
        private GroupAccessId[] _groupToActivateWhileDrag;

        [SerializeField, Space(10)]
        private int _worldCollider;
        [SerializeField]
        private CameraId _inputCameraId = CameraId.Main;
        [SerializeField]
        private GameObject[] _canvasGameObjects;

        [SerializeField, Space(10), FilterByType(typeof(IGroupAccessTools))]
        private UnityEngine.Object _groupAccesToolsObj;

        private IGroupAccessTools _iGroupAccessTools;
        private IGroupAccessTools IGroupAccessTools
        {
            get
            {
                if (_iGroupAccessTools == null)
                    _iGroupAccessTools = _groupAccesToolsObj.GetComponent<IGroupAccessTools>();
                return _iGroupAccessTools;
            }
        }

        [SerializeField]
        private RectTransform _activationDragModeBorder;

        [SerializeField]
        private GameObject _prefabToDrop;

        [SerializeField]
        private UiRelatedEventsSrc _uiRelatedEvents;
        [SerializeField]
        private RaycasterToolsSrc _raycasterTools;
        [SerializeField, FilterByType(typeof(ICameraBank))]
        private UnityEngine.Object _iCcameraBankObj;

        private ICameraBank _iICameraBank;
        private ICameraBank ICameraBank
        {
            get
            {
                if (_iICameraBank == null)
                    _iICameraBank = _iCcameraBankObj.GetComponent<ICameraBank>();
                return _iICameraBank;
            }
        }
        private Camera Camera { get; set; }
        private Transform UiRelatedTransform { get; set; }
        private bool DragMode { get; set; }
        private Action OnFinishedDraggableActivity;
        private ScrollableButton ScrollableButton { get; set; }

        private bool AccessGroupMemorized { get; set; }

#if UNITY_EDITOR
#endif

        private RectTransform[] CanvasObjectsRectTranform { get; set; }
        private Vector2[] CanvaseGameObjectInitLocalPos { get; set; }
        private GameObject PreviewObject { get; set; }


        private void OnValidate()
        {

        }

        private void OnEnable()
        {
            UiRelatedTransform = _uiRelatedEvents.GetUiHolderTransform();
            Camera = ICameraBank.CameraDictionary[_inputCameraId];
            CanvasObjectsRectTranform = new RectTransform[_canvasGameObjects.Length];
            CanvaseGameObjectInitLocalPos = new Vector2[_canvasGameObjects.Length];

            for (int i = 0; i < _canvasGameObjects.Length; i++)
            {
                CanvasObjectsRectTranform[i] = _canvasGameObjects[i].GetComponent<RectTransform>();
                CanvaseGameObjectInitLocalPos[i] = CanvasObjectsRectTranform[i].localPosition;
            }


            ScrollableButton = GetComponent<ScrollableButton>();

        }

        private void OnDisable()
        {
            if (OnFinishedDraggableActivity != null)
                OnFinishedDraggableActivity.Invoke();
        }

        public void SetFinishedDraggableActivityCallback(Action callback)
        {
            OnFinishedDraggableActivity = callback;
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragMode = !RectTransformUtility.RectangleContainsScreenPoint(_activationDragModeBorder, Input.mousePosition);

            if (DragMode)
            {
                ScrollableButton.enabled = false;
            }
            else
            {
                ScrollableButton.enabled = true;
                ScrollableButton.OnDrag(eventData);
                RessetObjects();
                return;
            }

            if (!AccessGroupMemorized)
            {
                IGroupAccessTools.MemorizeAllRegisteredObjectsStateFor(gameObject.GetInstanceID(), this);

                foreach (var item in _groupToActivateWhileDrag)
                    IGroupAccessTools.ChangeGroupActiveState(item, true, true);

                AccessGroupMemorized = true;
            }

            Transform parent;
            Vector3 position;
            TryToGetSpotToDrop(out parent, out position);

            for (int i = 0; i < CanvasObjectsRectTranform.Length; i++)
                CanvasObjectsRectTranform[i].position = Input.mousePosition;


            if (parent != null)
            {
                foreach (var item in _canvasGameObjects)
                    item.SetActive(false);

                if (PreviewObject == null)
                {
                    if (parent == null) return;

                    PreviewObject = Instantiate(_prefabToDrop);

                    PreviewObject.transform.SetParent(parent);
                    PreviewObject.transform.position = position;
                }

                PreviewObject.SetActive(true);
                PreviewObject.transform.SetParent(parent);
                PreviewObject.transform.position = position;

                _uiRelatedEvents.DropGameObjectByPointerDrag(PreviewObject, position);
                return;
            }

            foreach (var item in _canvasGameObjects)
                item.SetActive(true);

            if (PreviewObject == null) return;

            PreviewObject.SetActive(false);
            _uiRelatedEvents.CancelDropGameObjectByPointerDrag(PreviewObject);

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ScrollableButton.enabled = true;
            ScrollableButton.OnDrag(eventData);
            RessetObjects();
            DragMode = false;
            Drop();

            if (AccessGroupMemorized)
            {
                IGroupAccessTools.RestoreAllRegisteredObjectStateByMemory(gameObject.GetInstanceID(), true);
                AccessGroupMemorized = false;
            }
        }

        public void Drop()
        {
            Transform parent;
            Vector3 position;
            TryToGetSpotToDrop(out parent, out position);
            var requestResult = _uiRelatedEvents.IsPointerOverCancelationTrigger();

            if (requestResult != null && requestResult.Value)
                return;
            

            if (parent == null) return;

            _uiRelatedEvents.DropGameObjectByPointerUp(_prefabToDrop, position);
        }

        private void TryToGetSpotToDrop(out Transform parent, out Vector3 position)
        {
            var hitedCollider = _raycasterTools.CastCameraRayHit(Camera, _layerMask, position);
            parent = null;
            position = Vector3.zero;

            if (hitedCollider == null) return;

            parent = _uiRelatedEvents.GetWorldDropGameObjectSpace();

            if (parent == null)
            {
                Debug.Log("\t - DraggableFromCanvasOnWorld \t OnPointerDown() \t parent is null, preview object can't be droped.");
                return;
            }

            position = hitedCollider.transform.position;
        }

        private void RessetObjects()
        {
            if (PreviewObject != null)
                Destroy(PreviewObject);

            for (int i = 0; i < _canvasGameObjects.Length; i++)
            {
                _canvasGameObjects[i].SetActive(true);
                CanvasObjectsRectTranform[i].localPosition = CanvaseGameObjectInitLocalPos[i];
            }
        }

        private void ChangeCanvasObjActiveState(bool state)
        {
            foreach (var item in _canvasGameObjects)
                item.SetActive(state);
        }
    }
    */
}

