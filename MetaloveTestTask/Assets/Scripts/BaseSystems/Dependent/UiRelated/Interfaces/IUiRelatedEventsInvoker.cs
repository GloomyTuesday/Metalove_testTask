using UnityEngine;

namespace Scripts.BaseSystems
{
    public interface IUiRelatedEventsInvoker 
    {
        public void RegisterCancellationRectTransform(RectTransform rectTransform);
        public void UnRegisterCancellationRect(RectTransform rectTransform);
        public void UnRegisterCancellationRectImmediate(RectTransform rectTransform);

        public Transform GetUiHolderTransform();
        public bool CheckIsCancelationBtnWasHitted();
        public bool IsPointerOverCancelationRect(Vector2 position);
        public void DropGameObjectByPointerUp(GameObject gameObj, Vector3 position);
        public void DropGameObjectByPointerDrag(GameObject gameObj, Vector3 position);
        public void CancelDropGameObjectByPointerDrag(GameObject gameObj);
        public Transform GetWorldDropGameObjectSpace();
    }
}
