using UnityEngine;
using System;

namespace Scripts.InputSystem
{
    [CreateAssetMenu(fileName = "InputEvents", menuName = "Scriptable Obj/Input system/Input events")]
    public class InputEventsSrc : ScriptableObject,
        IInputEventsCallbackIHandler,
        IInputEventsCallbackInvoker,
        IInputEventsHandler,
        IInputEventsInvoker
    {

        private Action<IRaycastConstraint> _onRegisterRaycastConstraint; 
        event Action<IRaycastConstraint> IInputEventsHandler.OnRegisterRaycastConstraint
        {
            add => _onRegisterRaycastConstraint += value;
            remove => _onRegisterRaycastConstraint -= value;
        }
        void IInputEventsInvoker.RegisterRaycastConstraint(IRaycastConstraint raycastConstraint) => 
            _onRegisterRaycastConstraint?.Invoke(raycastConstraint);


        private Action<IRaycastConstraint> _onUnRegisterRaycastConstraint;
        event Action<IRaycastConstraint> IInputEventsHandler.OnUnRegisterRaycastConstraint
        {
            add => _onUnRegisterRaycastConstraint += value;
            remove => _onUnRegisterRaycastConstraint -= value;
        }
        void IInputEventsInvoker.UnRegisterRaycastConstraint(IRaycastConstraint raycastConstraint) =>
            _onUnRegisterRaycastConstraint?.Invoke(raycastConstraint);


        private Func<InputTypeId, bool> _onIsRectConstraintHit;
        event Func<InputTypeId, bool> IInputEventsHandler.OnIsRectConstraintHit
        {
            add => _onIsRectConstraintHit += value;
            remove => _onIsRectConstraintHit -= value;
        }
        bool IInputEventsInvoker.IsRectConstraintHit(InputTypeId inputTypeId)
        {
            var result = _onIsRectConstraintHit?.Invoke(inputTypeId);

            if (result == null) return false;

            return result.Value; 
        }


        //  ----------------------------------------    Pointer 0
        #region Pointer
        private Func<Vector2> _onGetPointerPosition;
        event Func<Vector2> IInputEventsHandler.OnGetPointerPosition
        {
            add => _onGetPointerPosition += value;
            remove => _onGetPointerPosition -= value;
        }
        Vector2 IInputEventsInvoker.GetPointerPosition() => _onGetPointerPosition();
        public Vector2 GetPointerPosition() => _onGetPointerPosition();


        private Action<Vector2> _onPointerDown;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerDown
        {
            add => _onPointerDown += value;
            remove => _onPointerDown -= value;
        }
        void IInputEventsCallbackInvoker.PointerDown(Vector2 position) => _onPointerDown?.Invoke(position);
        public void PointerDown()
        {
            var requestResult = _onGetPointerPosition(); 
            _onPointerDown?.Invoke(requestResult);
        }


        private Action<Vector2> _onPointerDrag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerDrag
        {
            add => _onPointerDrag += value;
            remove => _onPointerDrag -= value;
        }
        void IInputEventsCallbackInvoker.PointerDrag(Vector2 position) => _onPointerDrag?.Invoke(position);
        public void PointerDrag()
        {
            var requestResult = _onGetPointerPosition();
            _onPointerDrag?.Invoke(requestResult);
        }


        private Action<Vector2> _onPointerUp;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerUp
        {
            add => _onPointerUp += value;
            remove => _onPointerUp -= value;
        }
        void IInputEventsCallbackInvoker.PointerUp(Vector2 position) => _onPointerUp?.Invoke(position);
        public void PointerUp()
        {
            var requestResult = _onGetPointerPosition();
            _onPointerUp?.Invoke(requestResult);
        }


        private Func<bool> _onIsPointerDown;
        event Func<bool> IInputEventsHandler.OnIsPointerDown
        {
            add => _onIsPointerDown += value;
            remove => _onIsPointerDown -= value;
        }
        bool IInputEventsInvoker.IsPointerDown()
        {
            var requestResult = _onIsPointerDown?.Invoke();
            if (requestResult == null) return false;

            return requestResult.Value;
        }
        #endregion


        //  ----------------------------------------    Touch 0
        #region Touch 0
        private Func<Vector2> _onGetTouch0Position;
        event Func<Vector2> IInputEventsHandler.OnGetTouch0Position
        {
            add => _onGetTouch0Position += value;
            remove => _onGetTouch0Position -= value;
        }
        Vector2 IInputEventsInvoker.GetTouch0Position() => _onGetTouch0Position();
        public Vector2 GetTouch0Position() => _onGetTouch0Position();


        private Action<Vector2> _onTouch0Down;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch0Down
        {
            add => _onTouch0Down += value;
            remove => _onTouch0Down -= value;
        }
        void IInputEventsCallbackInvoker.Touch0Down(Vector2 position) => _onTouch0Down?.Invoke(position);
        public void Touch0Down()
        {
            var requestResult = _onGetTouch0Position();
            _onTouch0Down?.Invoke(requestResult);
        }


        private Action<Vector2> _onTouch0Drag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch0Drag
        {
            add => _onTouch0Drag += value;
            remove => _onTouch0Drag -= value;
        }
        void IInputEventsCallbackInvoker.Touch0Drag(Vector2 position) => _onTouch0Drag?.Invoke(position);
        public void Touch0Drag()
        {
            var requestResult = _onGetTouch0Position();
            _onTouch0Drag?.Invoke(requestResult);
        }


        private Action<Vector2> _onTouch0Up;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch0Up
        {
            add => _onTouch0Up += value;
            remove => _onTouch0Up -= value;
        }
        void IInputEventsCallbackInvoker.Touch0Up(Vector2 position) => _onTouch0Up?.Invoke(position);
        public void Touch0Up()
        {
            var requestResult = _onGetTouch0Position();
            _onTouch0Up?.Invoke(requestResult);
        }


        private Func<bool> _onIsTouch0Down;
        event Func<bool> IInputEventsHandler.OnIsTouch0Down
        {
            add => _onIsTouch0Down += value;
            remove => _onIsTouch0Down -= value;
        }
        bool IInputEventsInvoker.IsTouch0Down()
        {
            var requestResult = _onIsTouch0Down?.Invoke();
            if (requestResult == null) return false;

            return requestResult.Value;
        }
        #endregion

        //  ----------------------------------------    Touch 1
        #region Touch 1
        private Func<Vector2> _onGetTouch1Position;
        event Func<Vector2> IInputEventsHandler.OnGetTouch1Position
        {
            add => _onGetTouch1Position += value;
            remove => _onGetTouch1Position -= value;
        }
        Vector2 IInputEventsInvoker.GetTouch1Position() => _onGetTouch1Position();
        public Vector2 GetTouch1Position() => _onGetTouch1Position();


        private Action<Vector2> _onTouch1Down;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch1Down
        {
            add => _onTouch1Down += value;
            remove => _onTouch1Down -= value;
        }
        void IInputEventsCallbackInvoker.Touch1Down(Vector2 position) => _onTouch1Down?.Invoke(position);
        public void Touch1Down()
        {
            var requestResult = _onGetTouch1Position();
            _onTouch1Down?.Invoke(requestResult);
        }


        private Action<Vector2> _onTouch1Drag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch1Drag
        {
            add => _onTouch1Drag += value;
            remove => _onTouch1Drag -= value;
        }
        void IInputEventsCallbackInvoker.Touch1Drag(Vector2 position) => _onTouch1Drag?.Invoke(position);
        public void Touch1Drag()
        {
            var requestResult = _onGetTouch1Position();
            _onTouch1Drag?.Invoke(requestResult);
        }


        private Action<Vector2> _onTouch1Up;
        event Action<Vector2> IInputEventsCallbackIHandler.OnTouch1Up
        {
            add => _onTouch1Up += value;
            remove => _onTouch1Up -= value;
        }
        void IInputEventsCallbackInvoker.Touch1Up(Vector2 position) => _onTouch1Up?.Invoke(position);
        public void Touch1Up()
        {
            var requestResult = _onGetTouch1Position();
            _onTouch1Up?.Invoke(requestResult);
        }


        private Func<bool> _onIsTouch1Down;
        event Func<bool> IInputEventsHandler.OnIsTouch1Down
        {
            add => _onIsTouch1Down += value;
            remove => _onIsTouch1Down -= value;
        }
        bool IInputEventsInvoker.IsTouch1Down()
        {
            var requestResult = _onIsTouch1Down?.Invoke();
            if (requestResult == null) return false;

            return requestResult.Value;
        }
        #endregion


        //  ----------------------------------------    Mouse middle button
        #region Mouse middle button
        private Func<Vector2> _onGetMouseMiddleBtnPointerPosition;
        event Func<Vector2> IInputEventsHandler.OnGetMouseMiddleBtnPointerPosition
        {
            add => _onGetMouseMiddleBtnPointerPosition += value;
            remove => _onGetMouseMiddleBtnPointerPosition -= value;
        }
        Vector2 IInputEventsInvoker.GetMouseMiddleBtnPointerPosition() => _onGetMouseMiddleBtnPointerPosition();
        public Vector2 GetMouseMiddleBtnPointerPosition() => _onGetMouseMiddleBtnPointerPosition();


        private Action<Vector2> _onMouseMiddleBtnDown;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnDown
        {
            add => _onMouseMiddleBtnDown += value;
            remove => _onMouseMiddleBtnDown -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnDown(Vector2 position)=> _onMouseMiddleBtnDown?.Invoke(position);

        public void MouseMiddleBtnDown()
        {
            var requestResult = _onGetTouch1Position();
            _onMouseMiddleBtnDown?.Invoke(requestResult);
        }


        private Action<Vector2> _onMouseMiddleBtnDrag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnDrag
        {
            add => _onMouseMiddleBtnDrag += value;
            remove => _onMouseMiddleBtnDrag -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnDrag(Vector2 position) => _onMouseMiddleBtnDrag?.Invoke(position);
        public void MouseMiddleBtnDrag()
        {
            var requestResult = _onGetMouseMiddleBtnPointerPosition();
            _onMouseMiddleBtnDrag?.Invoke(requestResult);
        }


        private Action<Vector2> _onMouseMiddleBtnUp;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnUp
        {
            add => _onMouseMiddleBtnUp += value;
            remove => _onMouseMiddleBtnUp -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnUp(Vector2 position) => _onMouseMiddleBtnUp?.Invoke(position);
        public void MouseMiddleBtnUp()
        {
            var requestResult = _onGetMouseMiddleBtnPointerPosition();
            _onMouseMiddleBtnUp?.Invoke(requestResult);
        }


        private Func<bool> _onIsMouseMiddleBtnDown;
        event Func<bool> IInputEventsHandler.OnIsMouseMiddleBtnDown
        {
            add => _onIsMouseMiddleBtnDown += value;
            remove => _onIsMouseMiddleBtnDown -= value;
        }
        bool IInputEventsInvoker.IsMouseMiddleBtnDown()
        {
            var requestResult = _onIsMouseMiddleBtnDown?.Invoke();
            if (requestResult == null) return false;

            return requestResult.Value;
        }
        #endregion


        //  ----------------------------------------    Mouse Y scroll
        #region Mouse Y scroll
        private Action<float> _onMouseScrollY;
        event Action<float> IInputEventsCallbackIHandler.OnMouseScrollY
        {
            add => _onMouseScrollY += value;
            remove => _onMouseScrollY -= value;
        }
        void IInputEventsCallbackInvoker.MouseScrollY(float scrollValue) => _onMouseScrollY?.Invoke(scrollValue);
        public void MouseScrollY(float scrollValue = 0)
        {
            _onMouseScrollY?.Invoke(scrollValue);
        }


        private Action<float> _onMouseScrollYCanceled;
        event Action<float> IInputEventsCallbackIHandler.OnMouseScrollYCanceled
        {
            add => _onMouseScrollYCanceled += value;
            remove => _onMouseScrollYCanceled -= value;
        }
        void IInputEventsCallbackInvoker.MouseScrollYCanceled(float scrollValue) => _onMouseScrollYCanceled?.Invoke(scrollValue);
        public void MouseScrollYCanceled(float scrollValue)
        {
            var requestResult = _onGetMouseMiddleBtnPointerPosition();
            _onMouseScrollYCanceled?.Invoke(scrollValue);
        }
        #endregion


        //  ----------------------------------------    Keyboard input
        #region Keyboard input

        //  ----------------------------------------    Control

        private Action _onKeyboardCtrlDown;
        event Action IInputEventsCallbackIHandler.OnKeyboardCtrlDown
        {
            add => _onKeyboardCtrlDown += value; 
            remove => _onKeyboardCtrlDown -= value;
        }
        void IInputEventsCallbackInvoker.KeyboardControlDown()=> _onKeyboardCtrlDown?.Invoke();


        private Action _onKeyboardCtrlUp;
        event Action IInputEventsCallbackIHandler.OnKeyboardCtrlUp
        {
            add => _onKeyboardCtrlUp += value;
            remove => _onKeyboardCtrlUp -= value;
        }
        void IInputEventsCallbackInvoker.KeyboardCtrlUp() => _onKeyboardCtrlUp?.Invoke();


        private Func<bool> _onIsKeyboardCtrlDown; 
        event Func<bool> IInputEventsHandler.OnIsKeyboardCtrlDown
        {
            add=> _onIsKeyboardCtrlDown += value;
            remove => _onIsKeyboardCtrlDown -= value;
        }
        bool IInputEventsInvoker.IsKeyboardCtrlDown()
        {
            var requestResult = _onIsKeyboardCtrlDown?.Invoke(); 
            if(requestResult==null) return false;

            return requestResult.Value; 
        }

        #endregion
    }
}

