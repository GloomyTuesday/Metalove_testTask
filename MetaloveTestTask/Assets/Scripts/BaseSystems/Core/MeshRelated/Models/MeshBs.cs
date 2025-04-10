using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public struct MeshBs 
    {
        public Vector3[] _vertices;
        public int[] _triangles;
        public Vector3[] _normals;
        public Vector2[] _uv;
        public Vector4[] _tangents; 
    }
}
