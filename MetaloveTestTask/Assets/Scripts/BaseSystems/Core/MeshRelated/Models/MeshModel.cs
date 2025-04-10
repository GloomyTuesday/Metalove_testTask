using System; 
using UnityEngine;

namespace Scripts.BaseSystems
{
    [Serializable]
    public class MeshModel
    {
        public Vector3[] _vertices;
        public Vector3[] _normals;
        public Vector2[] _uv;
        public int[] _triangles;

        public MeshModel(
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] uv,
            int[] triangles
            
            )
        {
            _vertices = new Vector3[vertices.Length];
            Array.Copy(vertices, _vertices, vertices.Length);

            _normals = new Vector3[normals.Length];
            Array.Copy(normals, _normals, normals.Length);

            _uv = new Vector2[uv.Length];
            Array.Copy(uv, _uv, uv.Length);

            _triangles = new int[triangles.Length];
            Array.Copy(triangles, _triangles, triangles.Length);
        }

        public MeshModel(Mesh unityMeshObj)
        {
            _vertices = unityMeshObj.vertices;
            _normals = unityMeshObj.normals;
            _uv = unityMeshObj.uv;
            _triangles = unityMeshObj.triangles; 
        }

        public MeshModel(MeshModel meshModel)
        {
            _vertices = new Vector3[meshModel._vertices.Length];
            Array.Copy(meshModel._vertices, _vertices, meshModel._vertices.Length);

            _normals = new Vector3[meshModel._normals.Length];
            Array.Copy(meshModel._normals, _normals, meshModel._normals.Length);

            _uv = new Vector2[meshModel._uv.Length];
            Array.Copy(meshModel._uv, _uv, meshModel._uv.Length);

            _triangles = new int[meshModel._triangles.Length];
            Array.Copy(meshModel._triangles, _triangles, meshModel._triangles.Length);
        }

        public Mesh GetUnityMesh()
        {
            /*
            var unityMesh = new Mesh();
            unityMesh.vertices = _vertices;
            unityMesh.normals = _normals;
            unityMesh.uv = _uv;
            unityMesh.triangles = _triangles;
            return unityMesh; 
            */

            /*
            var vertices = new Vector3[_vertices.Length];
            var normals = new Vector3[_vertices.Length];
            var uv = new Vector3[_vertices.Length];
            var triangles = new int[_triangles.Length];
*/
         //   var unityMesh = new Mesh();
         //   unityMesh.vertices = new Vector3[_vertices.Length];
         //   unityMesh.normals = new Vector3[_normals.Length];
        //    unityMesh.uv = new Vector2[_uv.Length];
        //    unityMesh.triangles = new int[_triangles.Length];
        /*

            for (int i = 0; i < _vertices.Length; i++)
            {
                vertices[i] = _vertices[i];
                normals[i] = _normals[i];
                uv[i] = _uv[i];
            }

            Array.Copy(_triangles, triangles, _triangles.Length);
        */

            var unityMesh = new Mesh();
            unityMesh.Clear();
            unityMesh.vertices = _vertices;
            unityMesh.normals = _normals;
            unityMesh.uv = _uv;
            unityMesh.triangles = _triangles;
            unityMesh.Optimize();
            unityMesh.MarkDynamic();
        //    unityMesh.RecalculateBounds();
        //    unityMesh.RecalculateNormals();
        //    unityMesh.RecalculateTangents();

            return unityMesh; 
            
        }
    }
}
