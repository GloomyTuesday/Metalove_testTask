using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsCallbackInvoker 
    {
        //  ----------------------------------------    Pointer 
        public void PointerDown(Vector2 position);
        public void PointerDrag(Vector2 position);
        public void PointerUp(Vector2 position);

        //  ----------------------------------------    Touch 0
        public void Touch0Down(Vector2 position);
        public void Touch0Drag(Vector2 position);
        public void Touch0Up(Vector2 position);

        //  ----------------------------------------    Touch 1
        public void Touch1Down(Vector2 position);
        public void Touch1Drag(Vector2 position);
        public void Touch1Up(Vector2 position);

        //  ----------------------------------------    Mouse middle button
        public void MouseMiddleBtnDown(Vector2 position);
        public void MouseMiddleBtnDrag(Vector2 position);
        public void MouseMiddleBtnUp(Vector2 position);

        //  ----------------------------------------    Mouse Y scroll
        public void MouseScrollY(float scrollDirection);
        public void MouseScrollYCanceled(float scrollDirection);


        //  ----------------------------------------    Keyboard input
        #region Keyboard input

        //  ----------------------------------------    Control
        public void KeyboardControlDown();
        public void KeyboardCtrlUp();

        #endregion
    }
}
