using System;
using UnityEngine; 

namespace Scripts.BaseSystems
{
    [Serializable]
    public class TriangleStack 
    {
        public Vector3[] _vertices;
        public Vector3[] _normals;
        public Vector4[] _tangents;
        public Vector2[] _uv;

        //  Those are ID from main mesh
        public int[] _verticesId;

        public TriangleStack[] _triangleStackArray; 

        public TriangleStack(
            Vector3[] vertices,
            Vector3[] normals, 
            Vector2[] uv,
            Vector4[] tangents,
            int[] verticesId, 
            TriangleStack[] triangleStackArray=null
            )
        {
            _vertices = new Vector3[ vertices.Length ];
            Array.Copy(vertices, _vertices, vertices.Length);

            _normals = new Vector3[normals.Length];
            Array.Copy(normals, _normals, normals.Length);

            _uv = new Vector2[uv.Length];
            Array.Copy(uv, _uv, uv.Length);

            _tangents = new Vector4[tangents.Length];
            Array.Copy(tangents, _tangents, tangents.Length);

            _verticesId = new int[verticesId.Length];
            Array.Copy(verticesId, _verticesId, verticesId.Length);

            if (triangleStackArray == null) return;

            _triangleStackArray = new TriangleStack[triangleStackArray.Length];
            Array.Copy(triangleStackArray, _triangleStackArray, triangleStackArray.Length);
        }
    }
}
