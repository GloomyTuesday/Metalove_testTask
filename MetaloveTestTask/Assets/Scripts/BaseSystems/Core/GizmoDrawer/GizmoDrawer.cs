using UnityEngine;

namespace Scripts.BaseSystems
{
    public class GizmoDrawer : MonoBehaviour
    {
        private enum GizmoType 
        {
            Non = 0,
            Box = 1,
            Sphere = 2,
            Wird_Box = 3,
            Wird_Sphere = 4
        };

        [SerializeField]
        private GizmoType type = GizmoType.Box;

        [SerializeField]
        private float _sphereRadius = 1;
        [SerializeField]
        private Vector3 _boxSize = Vector3.one;
        [SerializeField, Space(10)]
        private Color _color = Color.white; 

        private void OnDrawGizmos()
        {
            if (type == GizmoType.Non) return;

            Gizmos.color = _color;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            switch (type)
            {
                case GizmoType.Box:
                    Gizmos.DrawCube(Vector3.zero, _boxSize);
                    break;
                case GizmoType.Sphere:
                    Gizmos.DrawSphere(Vector3.zero, _sphereRadius);
                    break;
                case GizmoType.Wird_Box:
                    Gizmos.DrawWireCube(Vector3.zero, _boxSize);
                    break;
                case GizmoType.Wird_Sphere:
                    Gizmos.DrawWireSphere(Vector3.zero, _sphereRadius);
                    break;
                default:
                    break;
            }
        }
    }
}
