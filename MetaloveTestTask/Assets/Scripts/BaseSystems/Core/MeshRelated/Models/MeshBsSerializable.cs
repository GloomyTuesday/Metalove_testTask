using System;
using UnityEngine; 

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public struct MeshBsSerializable
    {
        private float[] _vertices;  //  Vector3
        private int[] _triangles;
        private float[] _normals;  //  Vector3
        private float[] _uv;  //  Vector2
        private float[] _tangents;  //  Vector4

        public void Print()
        {
            var verticesAmount = _vertices.Length / 3;
            var trianglesAmount = _triangles.Length/3; 

            Debug.Log("\t Vertices: "+ verticesAmount);
            Debug.Log("\t Triangles: " + trianglesAmount);

            for (int i = 0; i < verticesAmount ; i++)
            {
                var vertexIndex = i * 3;
                var tangentIndex = i * 4;
                Debug.Log("\t\t [ " +vertexIndex+ " ] ");
                Debug.Log("\t\t\t vert: \t ( "+ _vertices[vertexIndex]+" , "+ _vertices[vertexIndex+1]+" , "+ _vertices[vertexIndex+2]+" )");
                Debug.Log("\t\t\t normal: ( " + _normals[vertexIndex] + " , " + _normals[vertexIndex + 1] + " , " + _normals[vertexIndex + 2] + " )");
                Debug.Log("\t\t\t uv: ( " + _uv[vertexIndex] + " , " + _uv[vertexIndex + 1]+" )");
                Debug.Log("\t\t\t tang: ( "+ _tangents[tangentIndex] + " , " + _tangents[tangentIndex+1] + " , "+ _tangents[tangentIndex+2] + " , "+ _tangents[tangentIndex+3] + " )");
            }
            
            for (int i = 0; i < _triangles.Length; i+=3)
            {
                Debug.Log("\t\t [ " + i + " ] ");
                Debug.Log("\t\t\t\t "+ _triangles[i]+"\t "+ _triangles[i+1]+"\t "+ _triangles[i+2]);
            }
        }


        public void SetData(MeshBs meshBs)
        {
            var tangentsFlag = _tangents == null? false : true;

            var arrayLength = meshBs._vertices.Length * 3;

            _vertices = new float[arrayLength];
            _normals = new float[arrayLength];
            _uv = new float[meshBs._vertices.Length * 3];
            _tangents = new float[meshBs._vertices.Length * 4];

            _triangles = new int[meshBs._triangles.Length];

            Array.Copy(meshBs._triangles, _triangles, _triangles.Length);

            for (int i = 0; i < meshBs._vertices.Length; i++)
            {
                var vertexIndex = i * 3;
                var uvIndex = i * 2;
                
                _vertices[vertexIndex] = meshBs._vertices[i].x;
                _vertices[vertexIndex + 1] = meshBs._vertices[i].y;
                _vertices[vertexIndex + 2] = meshBs._vertices[i].z;

                _normals[vertexIndex] = meshBs._normals[i].x;
                _normals[vertexIndex + 1] = meshBs._normals[i].y;
                _normals[vertexIndex + 2] = meshBs._normals[i].z;

                _uv[uvIndex] = meshBs._uv[i].x;
                _uv[uvIndex + 1] = meshBs._uv[i].y;

                if (!tangentsFlag) continue;

                var tangentIndex = i * 4;
                _tangents[tangentIndex] = meshBs._tangents[i].x;
                _tangents[tangentIndex + 1] = meshBs._tangents[i].y;
                _tangents[tangentIndex + 2] = meshBs._tangents[i].z;
                _tangents[tangentIndex + 3] = meshBs._tangents[i].w;
            }
        }

        public MeshBs GetMeshBs()
        {
            var tangentsFlag = _tangents == null ? false : true;

         //   Debug.Log("\t Get meshBs \t\t tangentsFlag: " + tangentsFlag); 

            var meshBs = new MeshBs();
            var arrayLength = _vertices.Length / 3;

            meshBs._vertices = new Vector3[arrayLength];
            meshBs._normals = new Vector3[arrayLength];
            meshBs._uv = new Vector2[arrayLength];

            meshBs._triangles = new int[_triangles.Length];
            Array.Copy(_triangles, meshBs._triangles, _triangles.Length);
            
            if(tangentsFlag)
                meshBs._tangents = new Vector4[arrayLength];

            for (int i = 0; i < arrayLength ; i++)
            {
                var vertexIndex = i * 3;
                var uvIndex = i * 2;

                meshBs._vertices[i] = new Vector3(_vertices[vertexIndex], _vertices[vertexIndex + 1], _vertices[vertexIndex + 2]);
                meshBs._normals[i] = new Vector3(_normals[vertexIndex], _normals[vertexIndex + 1], _normals[vertexIndex + 2]);
                meshBs._uv[i] = new Vector2(_uv[uvIndex], _uv[uvIndex + 1]);

                if (!tangentsFlag) continue;

                var tangentIndex = i * 4;

                meshBs._tangents[i] = new Vector4(_tangents[tangentIndex], _tangents[tangentIndex + 1], _tangents[tangentIndex + 2], _tangents[tangentIndex + 3]);
            }

            return meshBs; 
        }
    }
}
