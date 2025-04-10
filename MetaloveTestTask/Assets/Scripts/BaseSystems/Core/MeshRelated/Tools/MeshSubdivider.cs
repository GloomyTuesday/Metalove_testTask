using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    public class MeshSubdivider 
    {
        private static readonly float s_2Pi = 2f * Mathf.PI;

        public void Subdivide(
            List<Vector3> vertices,
            List<Vector3> normals,
            List<int> triangles,
            int resolution,
            float radius = 1
            )
        {
            if (resolution < 2) return;

            Dictionary<long, int> vertexCache = new Dictionary<long, int>();

            var subdivisionAmount = resolution - 1;

            List<int> trianglesBuffer;

            for (int i = 0; i < subdivisionAmount; i++)
            {
                trianglesBuffer = new List<int>(triangles);
                triangles.Clear();

                for (int j = 0; j < trianglesBuffer.Count; j += 3)
                {
                    int vertexA = trianglesBuffer[j];
                    int vertexB = trianglesBuffer[j + 1];
                    int vertexC = trianglesBuffer[j + 2];

                    int abMidpoint = GetMidPointIndex(vertexCache, vertexA, vertexB, vertices, normals, radius);
                    int bcMidpoint = GetMidPointIndex(vertexCache, vertexB, vertexC, vertices, normals, radius);
                    int caMidpoint = GetMidPointIndex(vertexCache, vertexC, vertexA, vertices, normals, radius);

                    triangles.Add(vertexA);
                    triangles.Add(abMidpoint);
                    triangles.Add(caMidpoint);

                    triangles.Add(vertexB);
                    triangles.Add(bcMidpoint);
                    triangles.Add(abMidpoint);

                    triangles.Add(vertexC);
                    triangles.Add(caMidpoint);
                    triangles.Add(bcMidpoint);

                    triangles.Add(abMidpoint);
                    triangles.Add(bcMidpoint);
                    triangles.Add(caMidpoint);

                }
            }
        }

        public int GetMidPointIndex(Dictionary<long, int> vertexCache, int indexA, int indexB, List<Vector3> vertexList, List<Vector3> normals, float radius = 1)
        {
            long key;

            if (indexA < indexB)
            {
                key = indexA;
                key = (key << 32) + indexB;
            }
            else
            {
                key = indexB;
                key = (key << 32) + indexA;
            }

            if (vertexCache.ContainsKey(key)) return vertexCache[key];

            var midPoint = (vertexList[indexA] + vertexList[indexB]) / 2;

            //  Getting normal:
            normals.Add(midPoint.normalized);

        //    midPoint = normals[^1] * radius;

            vertexList.Add(midPoint);
            vertexCache.Add(key, vertexList.Count - 1);
            return vertexList.Count - 1;
        }


        public static Dictionary<int, int> FindAndFixeWarpedFaces(List<int> triangles, List<Vector3> vertices, List<Vector3> normals)
        {
            var checkedVert = new Dictionary<int, int>();

            float _minValue = .25f;

            // find warped faces
            for (int i = 0; i < triangles.Count; i += 3)
            {
                var triangleVertexId_A = triangles[i];
                var triangleVertexId_B = triangles[i + 1];
                var triangleVertexId_C = triangles[i + 2];

                Vector3 vertexUvNormal_A = GetUvCoordinate(normals[triangleVertexId_A]);
                Vector3 vertexUvNormal_B = GetUvCoordinate(normals[triangleVertexId_B]);
                Vector3 vertexUvNormal_C = GetUvCoordinate(normals[triangleVertexId_C]);

                Vector3 texNormal = Vector3.Cross((vertexUvNormal_B - vertexUvNormal_A), (vertexUvNormal_C - vertexUvNormal_A));

                if (texNormal.z <= 0) continue;

                if (vertexUvNormal_A.x < _minValue)
                {
                    int vertexId;
                    if (!checkedVert.TryGetValue(triangleVertexId_A, out vertexId))
                    {
                        vertices.Add(vertices[triangleVertexId_A]);
                        normals.Add(normals[triangleVertexId_A]);
                        vertexId = vertices.Count - 1;
                        checkedVert[triangleVertexId_A] = vertexId;

                    }
                    triangles[i] = vertexId;
                }

                if (vertexUvNormal_B.x < _minValue)
                {
                    int vertexId;
                    if (!checkedVert.TryGetValue(triangleVertexId_B, out vertexId))
                    {
                        vertices.Add(vertices[triangleVertexId_B]);
                        normals.Add(normals[triangleVertexId_B]);
                        vertexId = vertices.Count - 1;
                        checkedVert[triangleVertexId_B] = vertexId;
                    }
                    triangles[i + 1] = vertexId;
                }

                if (vertexUvNormal_C.x < _minValue)
                {
                    int vertexId;
                    if (!checkedVert.TryGetValue(triangleVertexId_C, out vertexId))
                    {
                        vertices.Add(vertices[triangles[i + 2]]);
                        normals.Add(normals[triangles[i + 2]]);
                        vertexId = vertices.Count - 1;
                        checkedVert[triangleVertexId_C] = vertexId;

                    }
                    triangles[i + 2] = vertexId;
                }
            }

            return checkedVert;
        }

        public static Vector2 GetUvCoordinate(Vector3 normal)
        {
            float u = (Mathf.Atan2(normal.z, normal.x) / s_2Pi);
            float v = (Mathf.Asin(normal.y) / Mathf.PI + 0.5f);

            return new Vector2(u, v);
        }

        public static Dictionary<int, float> FindAndFixPoleVertices(Vector3 northPole, List<int> triangles, List<Vector3> vertices, List<Vector3> normals)
        {
            List<int> poleVerticeInd = new List<int>();
            Dictionary<int, float> poleVertIndicesCorrectU = new Dictionary<int, float>();

            for (int i = 0; i < triangles.Count; i += 3)
            {
                if (vertices[triangles[i]] == northPole || vertices[triangles[i]] == -northPole)
                {
                    if (!poleVerticeInd.Contains(triangles[i]))
                    {
                        poleVerticeInd.Add(triangles[i]);
                    }
                    else
                    {
                        vertices.Add(vertices[triangles[i]] == northPole ? northPole : -northPole);
                        normals.Add(vertices[^1].normalized);
                        triangles[i] = (vertices.Count - 1);
                    }
                    float xCoordB = GetUvCoordinate(normals[triangles[i + 1]]).x;
                    float xCoordC = GetUvCoordinate(normals[triangles[i + 2]]).x;
                    float correctedU = (xCoordB + xCoordC) / 2f + 0.5f; // I am not sure why it is needed but it seems needed...

                    poleVertIndicesCorrectU[triangles[i]] = correctedU;
                }
            }
            return poleVertIndicesCorrectU;
        }

        public static Vector2[] GetUvs(List<int> triangles, List<Vector3> vertices, List<Vector3> normals, Vector3 northPole)
        {
            var vertWithWarpedUv = FindAndFixeWarpedFaces(triangles, vertices, normals);
            var poleVertIndicesCorrectU = FindAndFixPoleVertices(northPole, triangles, vertices, normals);
            var uv = new List<Vector2>();

            for (int i = 0; i < vertices.Count; i++)
            {
                float u = (Mathf.Atan2(normals[i].z, normals[i].x) / s_2Pi) + 0.5f;
                float v = (Mathf.Asin(normals[i].y) / Mathf.PI) + .5f;

                // correct uv issues
                if (poleVertIndicesCorrectU.ContainsKey(i))
                    u = poleVertIndicesCorrectU[i];

                if (vertWithWarpedUv.ContainsValue(i))
                    u += 1;

                if (vertWithWarpedUv.ContainsValue(i) && poleVertIndicesCorrectU.ContainsKey(i))
                    u -= 0.5f; // found through trial and error, it was working so I had to remove the 0.5 added when recalculating 

                uv.Add(new Vector2(u, v));
            }

            return uv.ToArray();
        }

        public MeshGroupBs SubdivideMeshPiece(MeshGroupBs meshPiece, Vector3 northPole, float radius = 1)
        {
            List<Vector3> vertices = new List<Vector3>(meshPiece._meshBsArray[^1]._vertices);
            List<Vector3> normals = new List<Vector3>(meshPiece._meshBsArray[^1]._normals);
            List<int> triangles = new List<int>(meshPiece._meshBsArray[^1]._triangles);

            Dictionary<long, int> vertexCache = new Dictionary<long, int>();
            List<int> trianglesBuffer = new List<int>(triangles);
            triangles.Clear();

            for (int j = 0; j < trianglesBuffer.Count; j += 3)
            {
                int vertexA = trianglesBuffer[j];
                int vertexB = trianglesBuffer[j + 1];
                int vertexC = trianglesBuffer[j + 2];

                int abMidpoint = GetMidPointIndex(vertexCache, vertexA, vertexB, vertices, normals, radius);
                int bcMidpoint = GetMidPointIndex(vertexCache, vertexB, vertexC, vertices, normals, radius);
                int caMidpoint = GetMidPointIndex(vertexCache, vertexC, vertexA, vertices, normals, radius);

                triangles.Add(vertexA);
                triangles.Add(abMidpoint);
                triangles.Add(caMidpoint);

                triangles.Add(vertexB);
                triangles.Add(bcMidpoint);
                triangles.Add(abMidpoint);

                triangles.Add(vertexC);
                triangles.Add(caMidpoint);
                triangles.Add(bcMidpoint);

                triangles.Add(abMidpoint);
                triangles.Add(bcMidpoint);
                triangles.Add(caMidpoint);

            }

            var newLod = new MeshBs();

            newLod._uv = GetUvs(triangles, vertices, normals, northPole);
            newLod._vertices = vertices.ToArray();
            newLod._triangles = triangles.ToArray();
            newLod._normals = normals.ToArray();

            var newLodsArray = new MeshBs[meshPiece._meshBsArray.Length + 1];

            Array.Copy(meshPiece._meshBsArray, newLodsArray, meshPiece._meshBsArray.Length);

            newLodsArray[^1] = newLod;
            meshPiece._meshBsArray = newLodsArray;

            return meshPiece;
        }

        public void CutMeshOnMeshGroups(
            List<Vector3> normals,
            List<int> triangles,
            List<MeshGroupBs> meshGroupList,
            Vector3 northPole,
            float radius = 1
            )
        {
            int meshPicesAmount = triangles.Count / 3;

            for (int i = 0; i < meshPicesAmount; i++)
            {
                var lod = new MeshBs();
                int triangleIndex = i * 3;

                var vertexIndex_A = triangles[triangleIndex];
                var vertexIndex_B = triangles[triangleIndex + 1];
                var vertexIndex_C = triangles[triangleIndex + 2];

                var lodVertices = new List<Vector3>();
                var lodNormals = new List<Vector3>() { normals[vertexIndex_A], normals[vertexIndex_B], normals[vertexIndex_C] };
                var lodTriangles = new List<int>() { 0, 1, 2 };
                var tangents = new List<Vector4>();

                for (int j = 0; j < lodNormals.Count; j++)
                    lodVertices.Add(lodNormals[j] * radius);

                lod._uv = GetUvs(lodTriangles, lodVertices, lodNormals, northPole);
                lod._vertices = lodVertices.ToArray();
                lod._normals = lodNormals.ToArray();
                lod._triangles = lodTriangles.ToArray();


                for (int j = 0; j < lodVertices.Count; j++)
                    tangents.Add(new Vector4(1f, 0f, 0f, -1f));

                lod._tangents = tangents.ToArray();

                var position = (lod._vertices[0] + lod._vertices[1] + lod._vertices[2]) / 3;

                var lodGroup = new MeshGroupBs();

                lodGroup._meshBsArray = new MeshBs[] { lod };
                lodGroup._position = position;

                meshGroupList.Add(lodGroup);
            }
        }
    }
}
