using Scripts.BaseSystems;
using Scripts.BaseSystems.UiRelated;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class CollectionRectTransformAllignerRegister : MonoBehaviour
    {
        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<IRectTransformAligner>))]
        private Object _collectionRegisterObj;

        private ICollectionRegister<IRectTransformAligner> _icollectionRegisterObj;
        private ICollectionRegister<IRectTransformAligner> ICollectionRegisterObj
        {
            get
            {
                if (_icollectionRegisterObj == null)
                    _icollectionRegisterObj = _collectionRegisterObj.GetComponent<ICollectionRegister<IRectTransformAligner>>();

                return _icollectionRegisterObj;
            }
        }

        private int InstanceId { get; set; }
        private IRectTransformAligner _alligner;

        private void OnEnable()
        {
            _alligner = GetComponent<IRectTransformAligner>();

            if (_alligner == null) return;

            InstanceId = _alligner.InstanceId;
            ICollectionRegisterObj.Register(_alligner);
        }

        private void OnDisable()
        {
            ICollectionRegisterObj.Unregister(InstanceId);
        }
    }
}
