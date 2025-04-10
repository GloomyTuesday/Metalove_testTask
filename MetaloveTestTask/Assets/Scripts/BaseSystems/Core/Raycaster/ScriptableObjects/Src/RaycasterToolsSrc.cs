using Scripts.BaseSystems.CameraSetup;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Raycaster
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Raycaster/Raycaster tools")]
    public class RaycasterToolsSrc : ScriptableObject, IRaycasterTools, IActiveStateAccessible
    {
        [SerializeField, Header("This field is used only to visualize active state")]
        private bool _active;

        [SerializeField, FilterByType(typeof(ICameraBank))]
        private UnityEngine.Object _cameraBankObj;

        private List<GameObject> LineRendererGameObject { get; set; } = new List<GameObject>(); 
        private List<LineRenderer> LineRendererList { get; set; } = new List<LineRenderer>();

        private GameObject _holder;

        private ICameraBank _iCameraBank;
        private ICameraBank ICameraBank
        {
            get
            {
                if (_iCameraBank == null)
                    _iCameraBank = _cameraBankObj.GetComponent<ICameraBank>();

                return _iCameraBank;
            }
        }

        [NonSerialized]
        private bool _activeState = true; 
        public bool ActiveState { 
            get => _activeState;
            set
            {
                _activeState = value;
                _active = _activeState;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            _active = true;
            _activeState = true; 
        }

        //  Area of effect
        //  Single ray cast
        public Collider CastRayHit(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask)
        {
            if (!ActiveState) return null; 

            RaycastHit hit; 
            var hitData = Physics.Raycast(origin , direction ,out hit, rayLength , layerMask); 
            return hit.collider; 
        }

        public Collider[] CastRayHitAll(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask)
        {
            if (!ActiveState) return new Collider[0];

            RaycastHit[] hitResults = Physics.RaycastAll(origin, direction, rayLength, layerMask);
            var collidersHit = new Collider[hitResults.Length];

            for (int i = 0; i < hitResults.Length; i++)
                collidersHit[i] = hitResults[i].collider;

            return collidersHit;
        }

        public Collider CastCameraRayHit(Camera cam, LayerMask layerMask, Vector2 position)
        {
            if (!ActiveState) return null;

            Ray cameraRay = cam.ScreenPointToRay(position);
            var hitResult = Physics.Raycast(cameraRay, out RaycastHit raycastHit, float.MaxValue, layerMask );
            return raycastHit.collider; 
        }

        public Collider[] CastCameraRayHitAll(Camera cam, LayerMask layerMask, Vector2 position)
        {
            if (!ActiveState) return new Collider[0];

            Ray cameraRay = cam.ScreenPointToRay(position);
            RaycastHit[] hitResults = Physics.RaycastAll(cameraRay, float.MaxValue, layerMask);
            var collidersHit = new Collider[hitResults.Length];

            for (int i = 0; i < hitResults.Length; i++)
                collidersHit[i] = hitResults[i].collider;

            return collidersHit;
        }

        public Collider CastCameraRayHit( LayerMask layerMask, Vector2 position)
        {
            if (!ActiveState) return null;

            var cam = ICameraBank.GetCamera(CameraId.Main);
            Ray cameraRay = cam.ScreenPointToRay(position);
            Physics.Raycast(cameraRay, out RaycastHit raycastHit, float.MaxValue, layerMask);
            return raycastHit.collider;
        }

        public Collider[] CastCameraRayHitAll(LayerMask layerMask, Vector2 position)
        {
            if (!ActiveState) return new Collider[0];

            var cam = ICameraBank.GetCamera(CameraId.Main);
            return CastCameraRayHitAll(cam, layerMask, position); 
        }
    }
}

