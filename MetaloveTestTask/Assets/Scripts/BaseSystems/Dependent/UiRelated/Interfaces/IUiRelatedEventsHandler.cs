using System;
using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public interface IUiRelatedEventsHandler
    {
        public event Action<RectTransform> OnRegisterCancellationRectTransform;
        public event Action<RectTransform> OnUnRegisterCancellationRect;
        public event Action<RectTransform> OnUnRegisterCancellationRectImmediate;
        
        public event Func<Transform> OnGetUiHolderTransform;
        public event Func<bool> OnIsCancelationRectHit;

        public event Func<Vector2,bool> OnIsPointerOverCancelationRect;
        public event Action<GameObject, Vector3> OnDropGameObjectByPointerUp;
        public event Action<GameObject, Vector3> OnDropGameObjectByPointerDrag;
        public event Action<GameObject> OnCancelDropGameObjectByPointerDrag;
        public event Func<Transform> OnGetWorldDropGameObjectSpace;
    }
}
