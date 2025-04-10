using System;
using UnityEngine;

namespace Scripts.BaseSystems.Space
{
    public interface ISpace3D: IBoundingBox, IEventDataUpdated
    {
        /// <summary>
        ///     Size of entire space, in cell units
        /// </summary>
        public Point3D CurrentSpaceSizeCells { get; }

        public void SetNewSpaceSize(Point3D newSize, Vector3 cellSize);
        public void SetNewSpaceSize(Point3D newSize);

        public Vector3 CellSizeAdapted { get; }
        public Vector3 CellSize { get; }

        public Vector3 GetGlobalPositionByPoint(Point3D point);
        public Vector3 GetGlobalPositionByPoint(int x, int y, int z);

        public Vector3 GetLocalPositionByPoint(Point3D point);
        public Vector3 GetLocalPositionByPoint(int x, int y, int z);

        public Point3D NormalizeCellPositionWithRepeat(Point3D point);

        public Point3D GetPointByGlobalPosition(Vector3 globalPosition);
        public Point3D GetPointByLocalPosition(Vector3 localPosition);

        public Point3D GetPointFromDirection(DirectionInSpace direction);

        public event Action<GameObject> OnRegisteredObjectIsOutOfRange;

        public void RegisterObject(GameObject obj);
        public void UnregisterObject(GameObject obj);
        public void UnregisterAll(); 

        public GameObject GetRegisteredObjectFromPoint(Point3D point);

        public GameObject[] GetRegisteredObjectArray();

        public int EncodPointPosition(Point3D point);
        public int EncodPointPosition(int x, int y, int z);
        public Point3D DecodePointPosition(int index);

    }
}
