using UnityEngine;

namespace Scripts.BaseSystems.Space
{


    public class SetterToPoint3D : MonoBehaviour
    {

        [SerializeField] 
        private Point3D _initPosition = new Point3D();
        [SerializeField]
        private GameObject _spaceHolderGameObject;

        private ISpace3D ISpace3d { get; set; }

        private void OnEnable()
        {
            ISpace3d = _spaceHolderGameObject.GetComponent<ISpace3D>(); 
            Vector3 newLocalPosition = ISpace3d.GetLocalPositionByPoint(_initPosition);
            transform.localPosition = newLocalPosition;

            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        void Subscribe()
        {
        }
        
        void Unsubscribe()
        {
        }

        void RessetPositionToInit()
        {
            Vector3 newLocalPosition = ISpace3d.GetLocalPositionByPoint(_initPosition);
            transform.localPosition = newLocalPosition;
        }
    }

}
