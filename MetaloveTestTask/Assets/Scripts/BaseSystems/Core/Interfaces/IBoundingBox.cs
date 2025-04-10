using UnityEngine;

namespace Scripts.BaseSystems
{
    public interface IBoundingBox
    {
        public Bounds Bounds { get; }
        public Quaternion BoundingBoxOrientation { get; }
        public static bool Contains(IBoundingBox boudingBox, Vector3 point)
        {
            Quaternion inverseRotation = Quaternion.Inverse(boudingBox.BoundingBoxOrientation);
            Vector3 localPoint = inverseRotation * (point - boudingBox.Bounds.center);
            return boudingBox.Bounds.Contains(localPoint);
        }
    }
}
