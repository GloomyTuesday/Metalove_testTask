using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public class IcosphereSerializable 
    {
        private float[] _northPole;
        private float _radius;
        private int _lodAmount;
        private MeshGroupBsSerializable[] _meshPieces; 

        public void SetData(Icosphere icosphere)
        {
            _northPole = new float[3];
            _northPole[0] = icosphere._northPole.x;
            _northPole[1] = icosphere._northPole.y;
            _northPole[2] = icosphere._northPole.z;
            _lodAmount = icosphere._lodAmount; 
            _radius = icosphere._radius;

            _meshPieces = new MeshGroupBsSerializable[ icosphere._meshPieces.Length ];

            for (int i = 0; i < _meshPieces.Length; i++)
            {
                _meshPieces[i] = new MeshGroupBsSerializable();
                _meshPieces[i].SetData(icosphere._meshPieces[i]); 
            }

        }

        public Icosphere GetIcosphere()
        {
            var icosphere = new Icosphere();

            icosphere._lodAmount = _lodAmount;
            icosphere._northPole = new Vector3(_northPole[0], _northPole[1], _northPole[2]);
            icosphere._radius = _radius;

            icosphere._meshPieces = new MeshGroupBs[ _meshPieces.Length ];

            for (int i = 0; i < _meshPieces.Length; i++)
            {
                icosphere._meshPieces[i] = new MeshGroupBs();
                icosphere._meshPieces[i] = _meshPieces[i].GetMeshGroupBs(); 
            }

            return icosphere; 
        }
    }
}
