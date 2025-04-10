using UnityEngine;

namespace Scripts.BaseSystems.CameraSetup
{
    public class CameraBankRegister : MonoBehaviour
    {
        [SerializeField]
        private CameraId _cameraId;
        [SerializeField, FilterByType(typeof(ICameraBank)), Space(10)]
        private Object _iCcameraBankObj;
        [SerializeField, HideInInspector]
        private Camera _camera;

        private ICameraBank _iICameraBank;
        private ICameraBank ICameraBank
        {
            get
            {
                if (_iICameraBank == null)
                    _iICameraBank = _iCcameraBankObj.GetComponent<ICameraBank>();
                return _iICameraBank; 
            }
        }

        private void OnValidate()
        {
            _camera = GetComponent<Camera>(); 

            if(_camera==null)
                Debug.LogError("Object "+gameObject.name+"\t is not a camera"); 
        }

        private void Awake()
        {
            ICameraBank.RegisterCamera(_camera, _cameraId);
        }

        private void OnEnable()
        {
            ICameraBank.RegisterCamera(_camera, _cameraId) ;
        }

        private void OnDisable()
        {
            ICameraBank.UnRegisterCamera(_camera); 
        }
    }
}

