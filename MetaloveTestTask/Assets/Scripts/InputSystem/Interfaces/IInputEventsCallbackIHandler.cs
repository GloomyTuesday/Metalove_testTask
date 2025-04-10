using System;
using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsCallbackIHandler 
    {
        //  ----------------------------------------    Pointer 
        public event Action<Vector2> OnPointerDown;
        public event Action<Vector2> OnPointerDrag;
        public event Action<Vector2> OnPointerUp;

        //  ----------------------------------------    Touch 0
        public event Action<Vector2> OnTouch0Down;
        public event Action<Vector2> OnTouch0Drag;
        public event Action<Vector2> OnTouch0Up;

        //  ----------------------------------------    Touch 1
        public event Action<Vector2> OnTouch1Down;
        public event Action<Vector2> OnTouch1Drag;
        public event Action<Vector2> OnTouch1Up;

        //  ----------------------------------------    Mouse middle button
        public event Action<Vector2> OnMouseMiddleBtnDown;
        public event Action<Vector2> OnMouseMiddleBtnDrag;
        public event Action<Vector2> OnMouseMiddleBtnUp;

        //  ----------------------------------------    Mouse Y scroll
        public event Action<float> OnMouseScrollY;
        public event Action<float> OnMouseScrollYCanceled;


        //  ----------------------------------------    Keyboard input
        #region Keyboard input

        //  ----------------------------------------    Control
        public event Action OnKeyboardCtrlDown;
        public event Action OnKeyboardCtrlUp;

        #endregion
    }
}
