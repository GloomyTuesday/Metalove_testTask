using UnityEngine;

namespace Scripts.BaseSystems.Space
{
    public interface ISpaceCollider
    {
        public Transform Transform {get;}
        public void SetSpaceHolder(ISpace3D iSpace3D);
        public void SetActiveLayerYColliders(bool activeFlag, int? layerY=null);
    }
}

