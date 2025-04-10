using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public class IcosphereModel 
    {
        public Vector3[] _vertices;
        public int[] _triangles;
        public Vector3[] _normals; 
        public Vector2[] _uv;

        public Vector3 _northPole;
        public float _radius;
        public int _resolution;

        public IcosphereModel()
        {
        }

        public IcosphereModel(IcosphereModel icosphere)
        {
            _vertices = new Vector3[icosphere._vertices.Length];
            _triangles = new int[icosphere._triangles.Length];
            _normals = new Vector3[icosphere._normals.Length];
            _uv = new Vector2[icosphere._uv.Length];

            Array.Copy(icosphere._vertices, _vertices, _vertices.Length);
            Array.Copy(icosphere._triangles, _triangles, _triangles.Length);
            Array.Copy(icosphere._normals, _normals, _normals.Length);
            Array.Copy(icosphere._uv, _uv, _uv.Length);

            _northPole = icosphere._northPole;
            _radius = icosphere._radius;
            _resolution = icosphere._resolution; 
        }
    }
}
