using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsInvoker 
    {
        public void RegisterRaycastConstraint(IRaycastConstraint raycastConstraint);
        public void UnRegisterRaycastConstraint(IRaycastConstraint raycastConstraint);

        public bool IsRectConstraintHit(InputTypeId inputTypeIf);

        //  ----------------------------------------    Pointer 
        public Vector2 GetPointerPosition();
        public bool IsPointerDown();

        //  ----------------------------------------    Touch 0
        public Vector2 GetTouch0Position();
        public bool IsTouch0Down();

        //  ----------------------------------------    Touch 1
        public Vector2 GetTouch1Position();
        public bool IsTouch1Down();

        //  ----------------------------------------    Mouse middle button
        public Vector2 GetMouseMiddleBtnPointerPosition();
        public bool IsMouseMiddleBtnDown();

        //  ----------------------------------------    Keyboard input
        #region Keyboard input

        //  ----------------------------------------    Control
        public bool IsKeyboardCtrlDown();

        #endregion
    }
}
