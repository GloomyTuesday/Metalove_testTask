using UnityEngine;

namespace Scripts.InputSystem
{
    public class RaycastConstraint : MonoBehaviour, IRaycastConstraint
    {
        [SerializeField]
        private RectTransform[] _constraintRectTransforms;

        [SerializeField, Space(10), Header("InputEventsSrc")]
        private ScriptableObject _inputSystemEvent;

        private IInputEventsInvoker _iInputEventsInvoker; 
        private IInputEventsInvoker IInputEventsInvoker
        {
            get
            {
                if (_iInputEventsInvoker == null)
                    _iInputEventsInvoker = (IInputEventsInvoker)_inputSystemEvent;

                return _iInputEventsInvoker; 
            }
        }

        private void OnEnable()
        {
            IInputEventsInvoker.RegisterRaycastConstraint(this); 
        }

        private void OnDisable()
        {
            IInputEventsInvoker.UnRegisterRaycastConstraint(this);
        }

        public bool ContactCheck(Vector2 position)
        {
            foreach (var item in _constraintRectTransforms)
                if (RectTransformUtility.RectangleContainsScreenPoint(item, position)) return true;

            return false;
        }

    }
}
