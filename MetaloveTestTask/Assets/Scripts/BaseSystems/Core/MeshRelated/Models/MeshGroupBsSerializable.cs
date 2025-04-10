using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public struct MeshGroupBsSerializable 
    {
        private MeshBsSerializable[] _meshBsSerializable;
        private float[] _position; 

        public void SetData(MeshGroupBs meshGroupBs)
        {
            _position = new float[3];
            _position[0] = meshGroupBs._position.x;
            _position[1] = meshGroupBs._position.y;
            _position[2] = meshGroupBs._position.z;

            _meshBsSerializable = new MeshBsSerializable[meshGroupBs._meshBsArray.Length];

            for (int i = 0; i < _meshBsSerializable.Length; i++)
            {
                _meshBsSerializable[i] = new MeshBsSerializable();
                _meshBsSerializable[i].SetData(meshGroupBs._meshBsArray[i]); 
            }
        }

        public MeshGroupBs GetMeshGroupBs()
        {
            var meshGroupBs = new MeshGroupBs();

            meshGroupBs._position = new Vector3(_position[0], _position[1], _position[2]);  

            meshGroupBs._meshBsArray = new MeshBs[_meshBsSerializable.Length]; 

            for (int i = 0; i < _meshBsSerializable.Length; i++)
            {
                meshGroupBs._meshBsArray[i] = new MeshBs();
                meshGroupBs._meshBsArray[i] = _meshBsSerializable[i].GetMeshBs(); 
            }

            return meshGroupBs; 
        }
    }
}
