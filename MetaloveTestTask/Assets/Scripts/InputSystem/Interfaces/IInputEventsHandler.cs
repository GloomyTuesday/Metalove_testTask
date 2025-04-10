using System;
using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsHandler
    {
        public event Action<IRaycastConstraint> OnRegisterRaycastConstraint;
        public event Action<IRaycastConstraint> OnUnRegisterRaycastConstraint;

        public event Func<InputTypeId, bool> OnIsRectConstraintHit;

        //  ----------------------------------------    Pointer 
        public event Func<Vector2> OnGetPointerPosition;
        public event Func<bool> OnIsPointerDown;

        //  ----------------------------------------    Touch 0
        public event Func<Vector2> OnGetTouch0Position;
        public event Func<bool> OnIsTouch0Down;

        //  ----------------------------------------    Touch 1
        public event Func<Vector2> OnGetTouch1Position;
        public event Func<bool> OnIsTouch1Down;

        //  ----------------------------------------    Mouse middle button
        public event Func<Vector2> OnGetMouseMiddleBtnPointerPosition;
        public event Func<bool> OnIsMouseMiddleBtnDown;


        //  ----------------------------------------    Keyboard input
        #region Keyboard input

        //  ----------------------------------------    Control
        public event Func<bool> OnIsKeyboardCtrlDown;

        #endregion
    }
}
