using Scripts.BaseSystems;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class RectTransformRegisterer : MonoBehaviour
    {
        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<RectTransform>))]
        private Object _collectionRegisterObj;

        private ICollectionRegister<RectTransform> _icollectionRegisterObj;
        private ICollectionRegister<RectTransform> ICollectionRegisterObj
        {
            get
            {
                if(_icollectionRegisterObj == null)
                    _icollectionRegisterObj = _collectionRegisterObj.GetComponent<ICollectionRegister<RectTransform>>();

                return _icollectionRegisterObj;
            }
        }

        private int InstanceId { get; set; }
        private RectTransform _rectTransform; 

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;

            InstanceId = _rectTransform.GetInstanceID();
            ICollectionRegisterObj.Register(_rectTransform); 
        }

        private void OnDisable()
        {
            if (_rectTransform == null) return;

            ICollectionRegisterObj.Unregister(InstanceId); 
        }
    }
}
