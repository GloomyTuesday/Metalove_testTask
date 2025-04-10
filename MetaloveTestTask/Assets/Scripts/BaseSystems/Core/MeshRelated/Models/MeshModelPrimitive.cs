using System;
using UnityEngine;

namespace Scripts.BaseSystems
{
    [Serializable]
    public class MeshModelPrimitive
    {

        /// <summary>
        ///     [0] - vertex 0 x
        ///     [1] - vertex 0 y
        ///     [2] - vertex 0 z
        ///     [3] - vertex 1 x
        ///     ...
        /// </summary>
        public float[] _vertices;
        public float[] _normals;
        public float[] _uv;
        public int[] _triangles;

        public MeshModelPrimitive(Mesh unityMeshObj)
        {
            _vertices = GetFloatArrayFromVector3(unityMeshObj.vertices);
            _normals = GetFloatArrayFromVector3(unityMeshObj.normals);
            _uv = GetFloatArrayFromVector2(unityMeshObj.uv);
            _triangles = unityMeshObj.triangles; 
        }

        public MeshModelPrimitive(MeshModel meshModel)
        {
            _vertices = new float[meshModel._vertices.Length];
            System.Array.Copy(meshModel._vertices, _vertices, meshModel._vertices.Length);

            _normals = new float[meshModel._normals.Length];
            System.Array.Copy(meshModel._normals, _normals, meshModel._normals.Length);

            _uv = new float[meshModel._uv.Length];
            System.Array.Copy(meshModel._uv, _uv, meshModel._uv.Length);

            _triangles = new int[meshModel._triangles.Length];
            System.Array.Copy(meshModel._triangles, _triangles, meshModel._triangles.Length);
        }

        public Mesh GetUnityMesh()
        {
            var unityMesh = new Mesh();

            unityMesh.vertices = GetVector3ArrayFromFloat(_vertices);
            unityMesh.normals = GetVector3ArrayFromFloat(_normals);
            unityMesh.uv = GetVector2ArrayFromFloat(_uv);
            unityMesh.triangles = _triangles;

            return unityMesh; 
        }

        public Vector3[] GetVector3ArrayFromFloat(float[] floatArray)
        {
            var array = new Vector3[floatArray.Length/2];

            for (int i = 0; i < floatArray.Length; i+=3)
            {
                array[i] = new Vector3(
                    floatArray[i],
                    floatArray[i+1],
                    floatArray[i+2]
                    ); 
            }

            return array; 
        }

        public Vector2[] GetVector2ArrayFromFloat(float[] floatArray)
        {
            var array = new Vector2[floatArray.Length / 2];

            for (int i = 0; i < floatArray.Length; i += 2)
            {
                array[i] = new Vector2(
                    floatArray[i],
                    floatArray[i + 1]
                    );
            }

            return array;
        }

        public float[] GetFloatArrayFromVector3(Vector3[] vectorArray)
        {
            float[] array = new float[vectorArray.Length * 3];

            for (int i = 0; i < array.Length; i+=3)
            {
                array[i] = vectorArray[i].x;
                array[i+1] = vectorArray[i].y;
                array[i+2] = vectorArray[i].z;
            }

            return array; 
        }

        public float[] GetFloatArrayFromVector2(Vector2[] vectorArray)
        {
            float[] array = new float[vectorArray.Length * 2];

            for (int i = 0; i < array.Length; i += 2)
            {
                array[i] = vectorArray[i].x;
                array[i + 1] = vectorArray[i].y;
            }

            return array;
        }
    }
}
