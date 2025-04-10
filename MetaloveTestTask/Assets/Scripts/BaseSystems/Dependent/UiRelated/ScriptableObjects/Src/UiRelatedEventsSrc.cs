using System;
using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    [CreateAssetMenu (menuName ="Scriptable Obj/BaseSystems/Ui related/Ui related events")]
    public class UiRelatedEventsSrc : ScriptableObject, IUiRelatedEventsInvoker, IUiRelatedEventsHandler
    {
        private Action<RectTransform> _onRegisterCancellationRectTransform;
        event Action<RectTransform> IUiRelatedEventsHandler.OnRegisterCancellationRectTransform
        {
            add => _onRegisterCancellationRectTransform = value;
            remove => _onRegisterCancellationRectTransform = value;
        }

        public void RegisterCancellationRectTransform(RectTransform rectTransform) => 
            _onRegisterCancellationRectTransform?.Invoke(rectTransform);


        private Action<RectTransform> _onUnRegisterCancellationRect;
        event Action<RectTransform> IUiRelatedEventsHandler.OnUnRegisterCancellationRect
        {
            add => _onUnRegisterCancellationRect = value;
            remove => _onUnRegisterCancellationRect = value;
        }
        public void UnRegisterCancellationRect(RectTransform rectTransform) => 
            _onUnRegisterCancellationRect?.Invoke(rectTransform);
        

        private Action<RectTransform> _onUnRegisterCancellationRectImmediate;
        event Action<RectTransform> IUiRelatedEventsHandler.OnUnRegisterCancellationRectImmediate
        {
            add => _onUnRegisterCancellationRectImmediate = value;
            remove => _onUnRegisterCancellationRectImmediate = value;
        }
        public void UnRegisterCancellationRectImmediate(RectTransform rectTransform) => 
            _onUnRegisterCancellationRectImmediate?.Invoke(rectTransform);


        private Func<Transform>_onGetUiHolderTransform;
        event Func<Transform> IUiRelatedEventsHandler.OnGetUiHolderTransform
        {
            add =>_onGetUiHolderTransform += value;
            remove =>_onGetUiHolderTransform -= value; 
        }
        public Transform GetUiHolderTransform() =>_onGetUiHolderTransform?.Invoke();

        //  Cancelation buttons have to subscribe for this event
        private Func<bool> _onCheckIsCancelationBtnWasHitted;
        event Func<bool> IUiRelatedEventsHandler.OnIsCancelationRectHit
        {
            add => _onCheckIsCancelationBtnWasHitted += value;
            remove => _onCheckIsCancelationBtnWasHitted -= value; 
        }
        public bool CheckIsCancelationBtnWasHitted()
        {
            var request = _onCheckIsCancelationBtnWasHitted?.Invoke();

            if(request == null ) return false;

            return request.Value; 
        }

        public Action<GameObject[]> OnChangeActiveState;

        private Func<Vector2,bool> _onIsPointerOverCancelationRect;
        event Func<Vector2, bool> IUiRelatedEventsHandler.OnIsPointerOverCancelationRect
        {
            add => _onIsPointerOverCancelationRect += value;
            remove => _onIsPointerOverCancelationRect -= value;
        }
        public bool IsPointerOverCancelationRect(Vector2 position)
        {
            var request = _onIsPointerOverCancelationRect?.Invoke(position);

            if (request == null) return false;

            return request.Value;
        }

        private Action<GameObject, Vector3> _onDropGameObjectByPointerUp;
        event Action<GameObject,Vector3> IUiRelatedEventsHandler.OnDropGameObjectByPointerUp
        {
            add => _onDropGameObjectByPointerUp += value;
            remove => _onDropGameObjectByPointerUp -= value; 
        }
        public void DropGameObjectByPointerUp(GameObject gameObj, Vector3 position) =>
            _onDropGameObjectByPointerUp?.Invoke(gameObj, position);

        private Action<GameObject, Vector3>_onDropGameObjectByPointerDrag;
        event Action<GameObject, Vector3> IUiRelatedEventsHandler.OnDropGameObjectByPointerDrag
        {
            add =>_onDropGameObjectByPointerDrag += value;
            remove =>_onDropGameObjectByPointerDrag -= value;
        }
        public void DropGameObjectByPointerDrag(GameObject gameObj, Vector3 position) =>
           _onDropGameObjectByPointerDrag?.Invoke(gameObj, position);

        private Action<GameObject>_onCancelDropGameObjectByPointerDrag;
        event Action<GameObject> IUiRelatedEventsHandler.OnCancelDropGameObjectByPointerDrag
        {
            add =>_onCancelDropGameObjectByPointerDrag += value;
            remove =>_onCancelDropGameObjectByPointerDrag -= value; 
        }
        public void CancelDropGameObjectByPointerDrag(GameObject gameObj) =>
           _onCancelDropGameObjectByPointerDrag?.Invoke(gameObj);

        public Func<Transform>_onGetWorldDropGameObjectSpace;
        event Func<Transform> IUiRelatedEventsHandler.OnGetWorldDropGameObjectSpace
        {
            add =>_onGetWorldDropGameObjectSpace += value;
            remove =>_onGetWorldDropGameObjectSpace -= value; 
        }
        public Transform GetWorldDropGameObjectSpace() =>_onGetWorldDropGameObjectSpace?.Invoke();
    }
}

