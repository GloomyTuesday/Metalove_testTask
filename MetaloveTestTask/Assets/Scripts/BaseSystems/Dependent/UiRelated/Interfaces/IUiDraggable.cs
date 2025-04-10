using System;

namespace Scripts.BaseSystems
{
    public interface IUiDraggable
    {
        public void SetFinishedDraggableActivityCallback(Action callback);
    }
}

