using Scripts.BaseSystems;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class CameraSetupObj : MonoBehaviour
    {
        [SerializeField]
        private bool _applyOnEnable = true;

        [SerializeField]
        private bool _draw = true;

        [Space(15)]
        [SerializeField]
        private Vector2 _viewRectSize = new Vector2(Screen.width, Screen.height);
        public Vector2 ViewRectSize { get => _viewRectSize; set => _viewRectSize = value; }

        [SerializeField]
        private float _depth;
        public float Depth { get => _depth; set => _depth = value; }

        [SerializeField]
        private float _animTime;

        [Space(15)]
        [Header("This field can be empty.")]
        [SerializeField]
        private AnimationCurve _animCurve;

        

        [Space(25)]
        [SerializeField]
        private Color _color = Color.red;

        [Space(15)]
        [Header("Will apply depth value to the cam. ")]
        [SerializeField]
        private bool _setCamPosition = true;

        [SerializeField]
        private float _gizmoSize = 1;

        [Space(15)]
        [SerializeField, FilterByType(typeof(ICameraEventsInvoker))]
        private UnityEngine.Object _cameraEventsInvokerObj;

        private ICameraEventsInvoker _ICameraEventsInvoker;
        private ICameraEventsInvoker ICameraEventsInvoker
        {
            get
            {
                if (_ICameraEventsInvoker == null)
                    _ICameraEventsInvoker = _cameraEventsInvokerObj.GetComponent<ICameraEventsInvoker>();

                return _ICameraEventsInvoker;
            }
        }

        private void OnDrawGizmos()
        {
            if (_gizmoSize < 0)
                _gizmoSize = 0;

            if (!_draw) return;

            Gizmos.color = new Color(_color.r, _color.g, _color.b, _color.a / 20);

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, new Vector3(ViewRectSize.x, ViewRectSize.y, 0));

            Gizmos.color = _color;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(ViewRectSize.x, ViewRectSize.y, 0));

            var frustrumHalfedW = ViewRectSize.x / 2;
            var frustrumHalfedH = ViewRectSize.y / 2;

            if (!_setCamPosition) return;

            var displace = new Vector3(0, 0, Depth);

            Gizmos.DrawLine(Vector3.zero, displace);

            if (_gizmoSize <= 0) return;

            Gizmos.DrawWireCube(new Vector3(0, 0, Depth), new Vector3(_gizmoSize, _gizmoSize, _gizmoSize));
        }

        private void OnValidate()
        {
            if (_animTime < 0)
                _animTime = 0; 
        }

        private void OnEnable()
        {
            if (_applyOnEnable)
                ICameraEventsInvoker.ApplyCameraSetup(GetCameraSetupModel()); 
        }

        public CameraSetupModel GetCameraSetupModel()
        {
            var cameraSetupModel = new CameraSetupModel();

            cameraSetupModel.Position = transform.position; 
            cameraSetupModel.Rotation = transform.rotation;
            cameraSetupModel.ViewRectSize = ViewRectSize;
            cameraSetupModel.Depth = Depth;
            cameraSetupModel.AnimCurve = _animCurve;
            cameraSetupModel.AnimTime = _animTime;

            return cameraSetupModel; 
        }
    }
}
