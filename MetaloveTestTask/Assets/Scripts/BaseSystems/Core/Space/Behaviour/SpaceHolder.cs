using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.BaseSystems.Space
{
    public enum DirectionInSpace { Non = 0, X_up = 1, X_down = 2, Y_up = 3, Y_down = 4, Z_up = 5, Z_down = 6 }

    public class SpaceHolder : MonoBehaviour, ISpace3D
    {

        [SerializeField]
        private bool _printFlag;
        [SerializeField, Space(5)]
        private bool _drawBaseGrid;
        [SerializeField]
        private Color _BaseGridColor = Color.black;

        [SerializeField]
        private Vector3 _spaceSize;

        [SerializeField]
        private int _sectionAmountX = 1;
        [SerializeField]
        private int _sectionAmountY = 1;
        [SerializeField]
        private int _sectionAmountZ = 1;

        [SerializeField, Space(10)]
        private UnityEvent<GameObject> _registeredObjIsOutOfRangeUnEve;

        [SerializeField, HideInInspector]
        private Vector3 _cellSize = Vector3.zero;
        [SerializeField, HideInInspector]
        private Vector3 _cellQuarterSize;
        [SerializeField, HideInInspector]
        private Vector3 _anchorMin;

        [SerializeField, HideInInspector]
        private Vector3 _cellSizeAdapted = Vector3.zero;
        [SerializeField, HideInInspector]
        private Vector3 _cellQuarterSizeAdapted;
        [SerializeField, HideInInspector]
        private Vector3 _anchorMinAdapted;

        [SerializeField, HideInInspector]
        private Vector3 _spaceSizeAdapted;

        [SerializeField, HideInInspector]
        private Point3D _currentSize;

        private ISpace3D _iSpace3d; 
        private ISpace3D ISpace3D
        {
            get
            {
                if (_iSpace3d == null)
                    _iSpace3d = gameObject.GetComponent<ISpace3D>();
                return _iSpace3d; 
            }
        }

        Vector3 ISpace3D.CellSizeAdapted => _cellSizeAdapted;
        Vector3 ISpace3D.CellSize => _cellSize;
        Point3D ISpace3D.CurrentSpaceSizeCells => _currentSize;

        private Action onDataUpdated;
        public event Action OnDataUpdated
        {
            add => onDataUpdated += value;
            remove => onDataUpdated -= value;
        }

        private Action<GameObject> onRegisteredObjectIsOutOfRange;
        event Action<GameObject> ISpace3D.OnRegisteredObjectIsOutOfRange
        {
            add => onRegisteredObjectIsOutOfRange += value;
            remove => onRegisteredObjectIsOutOfRange -= value;
        }

        //  As key should be used unique Id
        private Dictionary<int, RegisteredUnit> RegisteredObjects { get; set; } =
            new Dictionary<int, RegisteredUnit>();

        //  Dictionary is directin on the RegisteredObjects dictionary,
        //  As key are used point position in space
        //  As value is used unique Id ( that is key for RegisteredObjects dictionary )
        private Dictionary<Point3D, int> RegisteredObjectPosition { get; set; } =
            new Dictionary<Point3D, int>();

        private Bounds _bounds;
        public Bounds Bounds => _bounds;
        public Quaternion BoundingBoxOrientation => transform.rotation;

        private class RegisteredUnit
        {
            public GameObject GameObject { get; set; }
            public Point3D Point3D { get; set; }
            public int Id { get; set; }

            public RegisteredUnit(GameObject obj, Point3D point)
            {
                GameObject = obj;
                Point3D = point;
                Id = obj.GetInstanceID();
            }
        }

        private void OnValidate()
        {
            UpdateData();
        }

        private void OnEnable()
        {
            UpdateData();
        }

        private void OnDisable()
        {
        }

        void UpdateData()
        {
            if (_sectionAmountX < 1)
                _sectionAmountX = 1;

            if (_sectionAmountY < 1)
                _sectionAmountY = 1;

            if (_sectionAmountZ < 1)
                _sectionAmountZ = 1;

            Vector3 losyScale = transform.lossyScale;

            //  spaceSizeAdapted
            _spaceSizeAdapted = new Vector3(
                                            _spaceSize.x * losyScale.x,
                                            _spaceSize.y * losyScale.y,
                                            _spaceSize.z * losyScale.z
                                            );
            //  cellSize
            _cellSize = new Vector3(
                                    _spaceSize.x / (float)_sectionAmountX,
                                    _spaceSize.y / (float)_sectionAmountY,
                                    _spaceSize.z / (float)_sectionAmountZ
                                   );

            _cellSizeAdapted = new Vector3(
                                            _cellSize.x * losyScale.x,
                                            _cellSize.y * losyScale.y,
                                            _cellSize.z * losyScale.z
                                           );

            _anchorMin = _spaceSize / 2;
            _anchorMinAdapted = _spaceSizeAdapted / 2;

            _cellQuarterSize = _cellSize / 2;
            _cellQuarterSizeAdapted = _cellSizeAdapted / 2;

            _currentSize.X = _sectionAmountX;
            _currentSize.Y = _sectionAmountY;
            _currentSize.Z = _sectionAmountZ;

            _bounds = new Bounds(transform.position, _spaceSizeAdapted);
            
            onDataUpdated?.Invoke();
        }

        private void CalcValues(
            Point3D pointSpaceSize,
            Vector3 cellSize,
            out Vector3 spaceSize,
            out Vector3 spaceSizeAdapted,
            out Vector3 cellSizeAdapted,
            out Vector3 anchorMin,
            out Vector3 anchorMinAdapted,
            out Vector3 cellQuarterSize,
            out Vector3 cellQuarterSizeAdapted

            )
        {
            if (_sectionAmountX < 1)
                _sectionAmountX = 1;

            if (_sectionAmountY < 1)
                _sectionAmountY = 1;

            if (_sectionAmountZ < 1)
                _sectionAmountZ = 1;

            Vector3 losyScale = transform.lossyScale;
            spaceSize = new Vector3(
                pointSpaceSize.X * cellSize.x,
                pointSpaceSize.Y * cellSize.y,
                pointSpaceSize.Z * cellSize.z
                );


            //  spaceSizeAdapted
            spaceSizeAdapted = new Vector3(
                                            spaceSize.x * losyScale.x,
                                            spaceSize.y * losyScale.y,
                                            spaceSize.z * losyScale.z
                                            );

            cellSizeAdapted = new Vector3(
                                            cellSize.x * losyScale.x,
                                            cellSize.y * losyScale.y,
                                            cellSize.z * losyScale.z
                                           );

            anchorMin = spaceSize / 2;
            anchorMinAdapted = spaceSizeAdapted / 2;

            cellQuarterSize = cellSize / 2;
            cellQuarterSizeAdapted = cellSizeAdapted / 2;
        }


        Vector3 lastPosition = new Vector3();

        Vector3 ISpace3D.GetGlobalPositionByPoint(Point3D point)
        {
            var position = GetGlobalPositionByPointInternal(
                point,
                transform.position,
                _cellSizeAdapted,
                _anchorMinAdapted,
                _cellQuarterSizeAdapted
                );
            return position;
        }

        private Vector3 GetGlobalPositionByPointInternal(
            Point3D point,
            Vector3 spacePosition,
            Vector3 cellSizeAdapted,
            Vector3 anchorMinAdapted,
            Vector3 cellQuarterSizeAdapted
            )
        {
            Vector3 positionRelativeToZero = new Vector3(
                                                            cellSizeAdapted.x * point.X,
                                                            cellSizeAdapted.y * point.Y,
                                                            cellSizeAdapted.z * point.Z
                                                        );

            Vector3 position = positionRelativeToZero - anchorMinAdapted + cellQuarterSizeAdapted + spacePosition;

            if (point.X == 0 && point.Y == 0 && point.Z == 0)
            {
                if (lastPosition != position)
                    lastPosition = position;
            }

            return position;
        }

        Vector3 ISpace3D.GetGlobalPositionByPoint(int x, int y, int z) => ISpace3D.GetGlobalPositionByPoint(new Point3D(x, y, z));

        Vector3 ISpace3D.GetLocalPositionByPoint(Point3D point)
        {
            Vector3 positionRelativeToZero = new Vector3(
                                                            _cellSize.x * point.X,
                                                            _cellSize.y * point.Y,
                                                            _cellSize.z * point.Z
                                                        );

            Vector3 position = positionRelativeToZero - _anchorMin + _cellQuarterSize;

            if (point.X == 0 && point.Y == 0 && point.Z == 0)
            {
                if (lastPosition != position)
                    lastPosition = position;

            }

            return position;
        }

        Vector3 ISpace3D.GetLocalPositionByPoint(int x, int y, int z) => ISpace3D.GetLocalPositionByPoint(new Point3D(x, y, z));

        Point3D ISpace3D.NormalizeCellPositionWithRepeat(Point3D cell)
        {
            if (cell.X < 0) cell.X = _currentSize.X - 1;
            if (cell.Y < 0) cell.Y = _currentSize.Y - 1;
            if (cell.Z < 0) cell.Z = _currentSize.Z - 1;

            cell.X = cell.X % _currentSize.X;
            cell.Y = cell.Y % _currentSize.Y;
            cell.Z = cell.Z % _currentSize.Z;

            return cell;
        }

        private Point3D NormalizeCellPositionWith(Point3D cell)
        {
            if (cell.X < 0) cell.X = 0;
            if (cell.Y < 0) cell.Y = 0;
            if (cell.Z < 0) cell.Z = 0;

            if (cell.X >= _currentSize.X) cell.X = _currentSize.X - 1;
            if (cell.Y >= _currentSize.Y) cell.Y = _currentSize.Y - 1;
            if (cell.Z >= _currentSize.Z) cell.Z = _currentSize.Z - 1;

            return cell;
        }

        Point3D ISpace3D.GetPointByGlobalPosition(Vector3 globalPosition)
        {
            var point = GetPointByGlobalPositionInternal(globalPosition, _anchorMinAdapted, _cellSizeAdapted);
            return point;
        }

        private Point3D GetPointByGlobalPositionInternal(
            Vector3 globalPosition,
            Vector3 anchorMinAdapted,
            Vector3 cellSizeAdapted
            )
        {
            Vector3 positionInsideSpace;

            positionInsideSpace = globalPosition + anchorMinAdapted;


            Point3D point = new Point3D(
                                        (int)(positionInsideSpace.x / cellSizeAdapted.x),
                                        (int)(positionInsideSpace.y / cellSizeAdapted.y),
                                        (int)(positionInsideSpace.z / cellSizeAdapted.z)
                                       );

            return point;
        }

        Point3D ISpace3D.GetPointByLocalPosition(Vector3 localPosition)
        {
            var point = GetPointByLocalPositionInternal(localPosition, _anchorMin, _cellSize);
            return point;
        }

        private Point3D GetPointByLocalPositionInternal(
            Vector3 globalPosition,
            Vector3 anchorMin,
            Vector3 cellSize
            )
        {
            Vector3 positionInsideSpace;

            positionInsideSpace = globalPosition + anchorMin;


            Point3D point = new Point3D(
                                        (int)(positionInsideSpace.x / cellSize.x),
                                        (int)(positionInsideSpace.y / cellSize.y),
                                        (int)(positionInsideSpace.z / cellSize.z)
                                       );

            return point;
        }

        Point3D ISpace3D.GetPointFromDirection(DirectionInSpace direction)
        {
            Point3D directionPoint = new Point3D(0, 0, 0);

            switch (direction)
            {
                case DirectionInSpace.X_up: return new Point3D(1, 0, 0);
                case DirectionInSpace.X_down: return new Point3D(-1, 0, 0);
                case DirectionInSpace.Y_up: return new Point3D(0, 1, 0);
                case DirectionInSpace.Y_down: return new Point3D(0, -1, 0);
                case DirectionInSpace.Z_up: return new Point3D(0, 0, 1);
                case DirectionInSpace.Z_down: return new Point3D(0, 0, -1);
            }

            return directionPoint;
        }
                
        private void OnDrawGizmos()
        {
            bool flag = _printFlag;
            _printFlag = false;

            Color initColor = Gizmos.color;

            Gizmos.matrix = Matrix4x4.TRS(transform.localPosition, transform.rotation, Vector3.one);

            #region _drawBaseGrid
            if (_drawBaseGrid)
            {
                Gizmos.color = _BaseGridColor;

                for (int x = 0; x < _sectionAmountX + 1; x++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, 0)) - transform.position ;
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, _sectionAmountY, 0)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    endPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, 0)) - transform.position; 
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, _sectionAmountZ)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    endPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    Gizmos.DrawLine(startPoint, endPoint);
                }
                
                for (int y = 0; y < _sectionAmountY + 1; y++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, 0)) - transform.position;
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(_sectionAmountX, y, 0)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    endPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, 0)) - transform.position;
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, _sectionAmountZ)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    endPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    Gizmos.DrawLine(startPoint, endPoint);
                }

                for (int z = 0; z < _sectionAmountZ + 1; z++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, 0, z)) - transform.position;
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(_sectionAmountX, 0, z)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    endPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, 0, z)) - transform.position;
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, _sectionAmountY, z)) - transform.position;
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    endPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    Gizmos.DrawLine(startPoint, endPoint);
                }
               
                #endregion
            }

            _printFlag = flag;
        }

        /*
        private void OnDrawGizmos()
        {
            bool flag = _printFlag;
            _printFlag = false;

            Color initColor = Gizmos.color;

            Gizmos.matrix = Matrix4x4.TRS(transform.localPosition, transform.rotation, Vector3.one);

            #region _drawBaseGrid
            if (_drawBaseGrid)
            {
                Gizmos.color = _BaseGridColor;

                for (int x = 0; x < _sectionAmountX + 1; x++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, 0));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, _sectionAmountY, 0));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    endPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, 0));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(x, 0, _sectionAmountZ));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    endPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    Gizmos.DrawLine(startPoint, endPoint);
                }

                for (int y = 0; y < _sectionAmountY + 1; y++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, 0));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(_sectionAmountX, y, 0));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    endPoint.z += _cellSizeAdapted.z * _sectionAmountZ;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, 0));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, y, _sectionAmountZ));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    endPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    Gizmos.DrawLine(startPoint, endPoint);
                }

                for (int z = 0; z < _sectionAmountZ + 1; z++)
                {
                    Vector3 startPoint;
                    Vector3 endPoint;
                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, 0, z));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(_sectionAmountX, 0, z));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    endPoint.y += _cellSizeAdapted.y * _sectionAmountY;
                    Gizmos.DrawLine(startPoint, endPoint);

                    startPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, 0, z));
                    endPoint = ISpace3D.GetGlobalPositionByPoint(new Point3D(0, _sectionAmountY, z));
                    startPoint -= _cellSizeAdapted / 2;
                    endPoint -= _cellSizeAdapted / 2;
                    Gizmos.DrawLine(startPoint, endPoint);
                    startPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    endPoint.x += _cellSizeAdapted.x * _sectionAmountX;
                    Gizmos.DrawLine(startPoint, endPoint);
                }
                #endregion
            }

            _printFlag = flag;
            Gizmos.color = initColor;
        }
        */
        
        void ISpace3D.SetNewSpaceSize(Point3D newSize)
        {
            var oldSize = ISpace3D.CurrentSpaceSizeCells;
            _spaceSize = new Vector3(
                                    newSize.X * _cellSize.x,
                                    newSize.Y * _cellSize.y,
                                    newSize.Z * _cellSize.z
                                    );

            _sectionAmountX = newSize.X;
            _sectionAmountY = newSize.Y;
            _sectionAmountZ = newSize.Z;

            RecalcRegisteredObjPosition(oldSize, newSize, RegisteredObjects.GetEnumerator());
            UpdateData();
        }

        void ISpace3D.SetNewSpaceSize(Point3D newSize, Vector3 cellSize)
        {
            _cellSize = cellSize;
            _spaceSize = new Vector3(
                                    newSize.X * _cellSize.x,
                                    newSize.Y * _cellSize.y,
                                    newSize.Z * _cellSize.z
                                    );

            _sectionAmountX = newSize.X;
            _sectionAmountY = newSize.Y;
            _sectionAmountZ = newSize.Z;

            UpdateData();
        }

        void ISpace3D.RegisterObject(GameObject obj)
        {
            var uniqueId = obj.GetInstanceID();

            if (RegisteredObjects.ContainsKey(uniqueId))
            {
                ISpace3D.UnregisterObject(obj);
            }

            var point = ISpace3D.GetPointByGlobalPosition(obj.transform.position);

            var RegisteredUnit = new RegisteredUnit(obj, point);

            RegisteredObjects.Add(uniqueId, RegisteredUnit);

            if (RegisteredObjectPosition.ContainsKey(point))
                RegisteredObjectPosition[point] = uniqueId;
            else
                RegisteredObjectPosition.Add(point, uniqueId);

        }

        void ISpace3D.UnregisterObject(GameObject obj)
        {
            RemoveRegisteredGameObject(obj.GetInstanceID());
        }

        private void RecalcRegisteredObjPosition(
            Point3D oldSizCells,
            Point3D newSizeCells,
            IEnumerator<KeyValuePair<int, RegisteredUnit>> enumerator)
        {
            var spaceGlobalPosition = transform.position;

            CalcValues(
                oldSizCells,
                _cellSize,
                out Vector3 oldSpaceSize,
                out Vector3 oldSpaceSizeAdapted,
                out Vector3 oldCellSizeAdapted,
                out Vector3 oldAnchorMin,
                out Vector3 oldAnchorMinAdapted,
                out Vector3 oldCellQuarterSize,
                out Vector3 oldCellQuarterSizeAdapted
                );

            CalcValues(
                newSizeCells,
                _cellSize,
                out Vector3 newSpaceSize,
                out Vector3 newSpaceSizeAdapted,
                out Vector3 newCellSizeAdapted,
                out Vector3 newAnchorMin,
                out Vector3 newAnchorMinAdapted,
                out Vector3 newCellQuarterSize,
                out Vector3 newCellQuarterSizeAdapted
                );

            enumerator.Reset();

            Debug.Log(" ");
            Debug.Log(" Space old size cells: "+ oldSizCells);
            Debug.Log(" Space new size cells: "+ newSizeCells);
            while (enumerator.MoveNext())
            {
                var target = enumerator.Current.Value.GameObject;

                if (target == null) continue;

                var transform = target.transform;
                var oldPoint = GetPointByGlobalPositionInternal(transform.position, oldAnchorMinAdapted, oldCellSizeAdapted);
                var verificationResult = CellIsInsideSpaceVerification(oldPoint, newSizeCells);

                Debug.Log("\t\t Old point: "+ oldPoint+"\t "+transform.gameObject.name);
                Debug.Log("\t\t New point: " + ISpace3D.GetPointByGlobalPosition(transform.position) + "\t " );

                var newPoint = GetPointByGlobalPositionInternal(transform.position, newAnchorMinAdapted, newCellSizeAdapted);

                if (!verificationResult)
                {
                    onRegisteredObjectIsOutOfRange?.Invoke(target);
                    _registeredObjIsOutOfRangeUnEve?.Invoke(target); 
                    continue;
                }

                var newPos = GetGlobalPositionByPointInternal(
                                                                oldPoint,
                                                                spaceGlobalPosition,
                                                                newCellSizeAdapted,
                                                                newAnchorMinAdapted,
                                                                newCellQuarterSizeAdapted
                                                                );
                transform.position = newPos;
            }
        }

        private bool CellIsInsideSpaceVerification(Point3D point, Point3D spaceSize)
        {
            if (point.X < 0 || (point.X >= spaceSize.X && point.X != 0)) return false;
            if (point.Y < 0 || (point.Y >= spaceSize.Y && point.Y != 0)) return false;
            if (point.Z < 0 || (point.Z >= spaceSize.Z && point.Z != 0)) return false;

            return true;
        }

        GameObject ISpace3D.GetRegisteredObjectFromPoint(Point3D point)
        {
            if (!RegisteredObjectPosition.ContainsKey(point)) return null;

            var uniqueId = RegisteredObjectPosition[point];
            var registeredUnit = RegisteredObjects[uniqueId];

            return registeredUnit.GameObject;
        }

        private void RemoveRegisteredGameObject(int uniqueId)
        {
            if (!RegisteredObjects.ContainsKey(uniqueId)) return;

            RegisteredObjectPosition.Remove(RegisteredObjects[uniqueId].Point3D);
            RegisteredObjects.Remove(uniqueId);
        }

        public void UnregisterAll()
        {
            RegisteredObjectPosition.Clear();
            RegisteredObjects.Clear(); 
        }

        private void ClearFromNullls()
        {
            var uniqueIdToRemoveList = new List<int>();
            
            foreach (var item in RegisteredObjects)
            {
                if (item.Value.GameObject == null)
                    uniqueIdToRemoveList.Add(item.Key);
            }

            for (int i = 0; i < uniqueIdToRemoveList.Count; i++)
            {
                var uniqueId = uniqueIdToRemoveList[i];
                var point3D = RegisteredObjects[uniqueId].Point3D;
                
                RegisteredObjectPosition.Remove(point3D);
                RegisteredObjects.Remove(uniqueId);
            }
        }

        GameObject[] ISpace3D.GetRegisteredObjectArray()
        {
            var array = new List<GameObject>();
            var objectsUniqueIdToRemove = new List<int>(); 
            foreach (var item in RegisteredObjects)
            {
                if (item.Value.GameObject != null)
                    array.Add(item.Value.GameObject);
                else
                    objectsUniqueIdToRemove.Add(item.Key); 
            }

            for (int i = 0; i < objectsUniqueIdToRemove.Count; i++)
                RemoveRegisteredGameObject(objectsUniqueIdToRemove[i]); 
            
            return array.ToArray(); 
        }

        int ISpace3D.EncodPointPosition(Point3D point)
        {
            return point.X + point.Y * ISpace3D.CurrentSpaceSizeCells.Z + point.Z * ISpace3D.CurrentSpaceSizeCells.Z * ISpace3D.CurrentSpaceSizeCells.Y;
        }

        int ISpace3D.EncodPointPosition(int x, int y, int z)
        {
            return x + y * ISpace3D.CurrentSpaceSizeCells.Z + z * ISpace3D.CurrentSpaceSizeCells.Z * ISpace3D.CurrentSpaceSizeCells.Y;
        }

        Point3D ISpace3D.DecodePointPosition(int index)
        {
            int multiplication = (ISpace3D.CurrentSpaceSizeCells.Z * ISpace3D.CurrentSpaceSizeCells.Y);
            int z = index / multiplication;
            int remainder = index % multiplication;
            int y = remainder / ISpace3D.CurrentSpaceSizeCells.Z;
            int x = remainder % ISpace3D.CurrentSpaceSizeCells.Z;
            return new Point3D(x,y,z); 
        }
    }
}
