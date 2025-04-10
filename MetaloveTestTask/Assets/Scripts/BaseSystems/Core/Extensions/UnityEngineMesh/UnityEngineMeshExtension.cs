using UnityEngine;

namespace Scripts.BaseSystems
{
    public static class UnityEngineMeshExtension 
    {
        public static Mesh Clone(this Mesh origin )
        {
            Mesh newMesh = new Mesh();

            newMesh.vertices = (Vector3[])origin.vertices.Clone();

            if (origin.normals.Length > 0)
                newMesh.normals = (Vector3[])origin.normals.Clone();
            

            if (origin.tangents.Length > 0)
                newMesh.tangents = (Vector4[])origin.tangents.Clone();
            
            if (origin.uv.Length > 0)
                newMesh.uv = (Vector2[])origin.uv.Clone();

            if (origin.uv2.Length > 0)
                newMesh.uv2 = (Vector2[])origin.uv2.Clone();

            if (origin.uv3.Length > 0)
                newMesh.uv3 = (Vector2[])origin.uv3.Clone();

            if (origin.uv4.Length > 0)
                newMesh.uv4 = (Vector2[])origin.uv4.Clone();

            if (origin.uv5.Length > 0)
                newMesh.uv5 = (Vector2[])origin.uv5.Clone();

            if (origin.uv6.Length > 0)
                newMesh.uv6 = (Vector2[])origin.uv6.Clone();

            if (origin.uv7.Length > 0)
                newMesh.uv7 = (Vector2[])origin.uv7.Clone();

            if (origin.uv8.Length > 0)
                newMesh.uv8 = (Vector2[])origin.uv8.Clone();

            if (origin.colors.Length > 0)
                newMesh.colors = (Color[])origin.colors.Clone();

            if (origin.boneWeights.Length > 0)
                newMesh.boneWeights = (BoneWeight[])origin.boneWeights.Clone();

            if (origin.bindposes.Length > 0)
                newMesh.bindposes = (Matrix4x4[])origin.bindposes.Clone();

            newMesh.subMeshCount = origin.subMeshCount;
            for (int i = 0; i < origin.subMeshCount; i++)
                newMesh.SetTriangles((int[])origin.GetTriangles(i).Clone(), i);

            for (int i = 0; i < origin.blendShapeCount; i++)
            {
                string blendShapeName = origin.GetBlendShapeName(i);
                int frameCount = origin.GetBlendShapeFrameCount(i);
                for (int j = 0; j < frameCount; j++)
                {
                    float frameWeight = origin.GetBlendShapeFrameWeight(i, j);
                    Vector3[] deltaVertices = new Vector3[origin.vertexCount];
                    Vector3[] deltaNormals = new Vector3[origin.vertexCount];
                    Vector3[] deltaTangents = new Vector3[origin.vertexCount];
                    origin.GetBlendShapeFrameVertices(i, j, deltaVertices, deltaNormals, deltaTangents);
                    newMesh.AddBlendShapeFrame(blendShapeName, frameWeight, deltaVertices, deltaNormals, deltaTangents);
                }
            }

            newMesh.RecalculateBounds();

            return newMesh;
        }
    }
}
