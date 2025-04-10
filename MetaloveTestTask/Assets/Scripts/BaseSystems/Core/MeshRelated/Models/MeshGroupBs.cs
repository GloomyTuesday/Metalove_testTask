using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public struct MeshGroupBs 
    {
        //  lod with index 0 is the lowes quality lod, the highest quality has the last index
        public MeshBs[] _meshBsArray;

        public Vector3 _position;
    }
}
