using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IRaycastConstraint 
    {
        public bool ContactCheck(Vector2 position);
    }
}
