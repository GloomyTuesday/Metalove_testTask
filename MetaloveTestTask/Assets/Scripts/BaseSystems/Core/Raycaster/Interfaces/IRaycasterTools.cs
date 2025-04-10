using UnityEngine;

namespace Scripts.BaseSystems
{
    public interface IRaycasterTools 
    {
        public bool ActiveState { get; set; }

        public Collider CastRayHit(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask);
        public Collider[] CastRayHitAll(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask);

        public Collider CastCameraRayHit(LayerMask layerMask, Vector2 position);
        public Collider[] CastCameraRayHitAll(LayerMask layerMask, Vector2 position);

        public Collider CastCameraRayHit(Camera cam, LayerMask layerMask, Vector2 position);
        public Collider[] CastCameraRayHitAll(Camera cam, LayerMask layerMask, Vector2 position);
    }
}
