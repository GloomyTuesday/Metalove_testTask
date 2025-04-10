using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public class IcosphereThroughTriangleStack
    {
        public const int s_smallestLodResolution = 2;
        public const int s_lodAmount = 3;

        private readonly Vector4[] s_tangents = new Vector4[3]
                                                    {
                                                        new Vector4(1f, 0f, 0f, -1f),
                                                        new Vector4(1f, 0f, 0f, -1f),
                                                        new Vector4(1f, 0f, 0f, -1f)
                                                    };

        public Vector3 _northPole;
        public float _radius;
        public int _resolution;

        private static readonly float s_2Pi = 2f * Mathf.PI;

        private TriangleStack[] _triangleStacks;
        public TriangleStack[] TriangleStacks => _triangleStacks;

        private bool _ready;
        public bool Ready => _ready;

        private int _readyPercent;
        public int ReadyPercent => _readyPercent;


        private Dictionary<long, int> VertexIdCache { get; set; } = new Dictionary<long, int>();
        private List<Vector3> VerticesCacheList { get; set; } = new List<Vector3>();
        private List<Vector3> NormalsCacheList { get; set; } = new List<Vector3>();

        public IcosphereThroughTriangleStack(float radius, int resolution = 1 )
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            var icosahedron = new Icosahedron();

            var triangleStacks = new List<TriangleStack>();

            for (int i = 0; i < icosahedron._triangles.Length; i += 3)
            {
                var vertexA = icosahedron._vertices[ icosahedron._triangles [i] ];
                var vertexB = icosahedron._vertices[ icosahedron._triangles [i+1] ];
                var vertexC = icosahedron._vertices[ icosahedron._triangles [i+2] ];

                var triangleVertices_1 = new List<Vector3>() { vertexA, vertexB, vertexC };
                var triangleNormals_1 = new List<Vector3>() { triangleVertices_1[0].normalized, triangleVertices_1[1].normalized, triangleVertices_1[2].normalized };
                var localVertexId_1 = new List<int>() { 0, 1, 2 };
                //  For UV are used local vertices Id, that means that every new TriangleStack will have  _verticesId = new int[] {0,1,2};
                var triangleUV_1 = GetUvs(localVertexId_1, triangleVertices_1, triangleNormals_1, icosahedron._northPole);

                var triangleStack = new TriangleStack(
                    triangleVertices_1.ToArray(),
                    triangleNormals_1.ToArray(),
                    triangleUV_1,
                    s_tangents,
                    new int[] { 0, 1, 2 },
                    null
                    );

                triangleStacks.Add(triangleStack); 
            }


            if (resolution >1)
            {
                for (int i = 0; i < triangleStacks.Count; i++)
                {
                    Subdivide(
                        triangleStacks[i],
                        VertexIdCache,
                        VerticesCacheList,
                        NormalsCacheList,
                        resolution,
                        icosahedron._northPole,
                        radius
                        );
                }
            }
            
            

            //  Right now mesh pices collection has all lods array inverted, the lod with highest quality is held in cell with highest index

            _triangleStacks = triangleStacks.ToArray(); 

            timer.Stop();

            Debug.Log("\t Icosphere creation time: " + timer.Elapsed);
        }

        private void Subdivide(
            TriangleStack triangleStack,
            Dictionary<long, int> vertexCache,
            List<Vector3> vertices,
            List<Vector3> normals,
            int resolution,
            Vector3 northPole,
            float radius = 1
            )
        {
            var vertexA = vertices[triangleStack._verticesId[0]];
            var vertexB = vertices[triangleStack._verticesId[1]];
            var vertexC = vertices[triangleStack._verticesId[2]];

            int abMidpoint = GetMidPointIndex(vertexCache, triangleStack._verticesId[0], triangleStack._verticesId[1], vertices, normals, radius);
            int bcMidpoint = GetMidPointIndex(vertexCache, triangleStack._verticesId[1], triangleStack._verticesId[2], vertices, normals, radius);
            int caMidpoint = GetMidPointIndex(vertexCache, triangleStack._verticesId[2], triangleStack._verticesId[0], vertices, normals, radius);

            var vertexAB = vertices[abMidpoint];
            var vertexBC = vertices[bcMidpoint];
            var vertexCA = vertices[caMidpoint];

            triangleStack._triangleStackArray = new TriangleStack[4];

            #region New TriangleStack object data

            //  TriangleStack   1
            var triangleVertices_1 = new List<Vector3>() { vertexA, vertexAB, vertexCA };
            var triangleNormals_1 = new List<Vector3>() { triangleVertices_1[0].normalized, triangleVertices_1[1].normalized, triangleVertices_1[2].normalized };
            var localVertexId_1 = new List<int>() { 0, 1, 2 };
            //  For UV are used local vertices Id, that means that every new TriangleStack will have  _verticesId = new int[] {0,1,2};
            var triangleUV_1 = GetUvs(localVertexId_1, triangleVertices_1, triangleNormals_1, northPole);

            //  TriangleStack   2
            var triangleVertices_2 = new List<Vector3>() { vertexB, vertexBC, vertexAB };
            var triangleNormals_2 = new List<Vector3>() { triangleVertices_2[0].normalized, triangleVertices_2[1].normalized, triangleVertices_2[2].normalized };
            var localVertexId_2 = new List<int>() { 0, 1, 2 };
            //  For UV are used local vertices Id, that means that every new TriangleStack will have  _verticesId = new int[] {0,1,2};
            var triangleUV_2 = GetUvs(localVertexId_2, triangleVertices_2, triangleNormals_1, northPole);

            //  TriangleStack   3
            var triangleVertices_3 = new List<Vector3>() { vertexC, vertexCA, vertexBC };
            var triangleNormals_3 = new List<Vector3>() { triangleVertices_3[0].normalized, triangleVertices_3[1].normalized, triangleVertices_3[2].normalized };
            var localVertexId_3 = new List<int>() { 0, 1, 2 };
            //  For UV are used local vertices Id, that means that every new TriangleStack will have  _verticesId = new int[] {0,1,2};
            var triangleUV_3 = GetUvs(localVertexId_3, triangleVertices_3, triangleNormals_3, northPole);

            //  TriangleStack   4
            var triangleVertices_4 = new List<Vector3>() { vertexAB, vertexBC, vertexCA };
            var triangleNormals_4 = new List<Vector3>() { triangleVertices_4[0].normalized, triangleVertices_4[1].normalized, triangleVertices_4[2].normalized };
            var localVertexId_4 = new List<int>() { 0, 1, 2 };
            //  For UV are used local vertices Id, that means that every new TriangleStack will have  _verticesId = new int[] {0,1,2};
            var triangleUV_4 = GetUvs(localVertexId_4, triangleVertices_4, triangleNormals_4, northPole);

            #endregion

            triangleStack._triangleStackArray[0] = new TriangleStack(
                triangleVertices_1.ToArray() ,
                triangleNormals_1.ToArray() ,
                triangleUV_1,
                s_tangents,
                new int[] {0,1,2}
                );

            triangleStack._triangleStackArray[1] = new TriangleStack(
                triangleVertices_2.ToArray(),
                triangleNormals_2.ToArray(),
                triangleUV_2,
                s_tangents,
                new int[] { 0, 1, 2 }
                );

            triangleStack._triangleStackArray[2] = new TriangleStack(
                triangleVertices_3.ToArray(),
                triangleNormals_3.ToArray(),
                triangleUV_3,
                s_tangents,
                new int[] { 0, 1, 2 }
                );

            triangleStack._triangleStackArray[3] = new TriangleStack(
                triangleVertices_4.ToArray(),
                triangleNormals_4.ToArray(),
                triangleUV_4,
                s_tangents,
                new int[] { 0, 1, 2 }
                );


            if (resolution < 2) return;
            resolution--;

            for (int i = 0; i < triangleStack._triangleStackArray.Length; i++)
            {
                Subdivide(
                    triangleStack._triangleStackArray[i],
                    vertexCache,
                    vertices,
                    normals,
                    resolution,
                    northPole,
                    radius
                    ); 
            }
        }

        private int GetMidPointIndex(Dictionary<long, int> vertexCache, int indexA, int indexB, List<Vector3> vertexList, List<Vector3> normals, float radius = 1)
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

            normals.Add(midPoint.normalized);

            midPoint = normals[^1] * radius;

            vertexList.Add(midPoint);
            vertexCache.Add(key, vertexList.Count - 1);
            return vertexList.Count - 1;
        }

        private Dictionary<int, int> FindAndFixeWarpedFaces(List<int> triangles, List<Vector3> vertices, List<Vector3> normals)
        {
            var checkedVert = new Dictionary<int, int>();

            float _minValue = .25f;

            // find warped faces
            for (int i = 0; i < triangles.Count; i += 3)
            {
                var triangleVertexId_A = triangles[i];
                var triangleVertexId_B = triangles[i + 1];
                var triangleVertexId_C = triangles[i + 2];

                Vector3 vertexUvNormal_A = GetUvCoordinates(normals[triangleVertexId_A]);
                Vector3 vertexUvNormal_B = GetUvCoordinates(normals[triangleVertexId_B]);
                Vector3 vertexUvNormal_C = GetUvCoordinates(normals[triangleVertexId_C]);

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

        private Vector2 GetUvCoordinates(Vector3 normal)
        {
            float u = (Mathf.Atan2(normal.z, normal.x) / s_2Pi);
            float v = (Mathf.Asin(normal.y) / Mathf.PI + 0.5f);

            return new Vector2(u, v);
        }

        private Dictionary<int, float> FindAndFixPoleVertices(Vector3 northPole, List<int> triangles, List<Vector3> vertices, List<Vector3> normals)
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
                    float xCoordB = GetUvCoordinates(normals[triangles[i + 1]]).x;
                    float xCoordC = GetUvCoordinates(normals[triangles[i + 2]]).x;
                    float correctedU = (xCoordB + xCoordC) / 2f + 0.5f; // I am not sure why it is needed but it seems needed...

                    poleVertIndicesCorrectU[triangles[i]] = correctedU;
                }
            }

            return poleVertIndicesCorrectU;
        }

        private Vector2[] GetUvs(List<int> triangles, List<Vector3> vertices, List<Vector3> normals, Vector3 northPole)
        {

            var vertWithWarpedUv = FindAndFixeWarpedFaces(triangles, vertices, normals);
            var poleVertIndicesCorrectU = FindAndFixPoleVertices(northPole, triangles, vertices, normals);
            var uv = new List<Vector2>();

            for (int i = 0; i < vertices.Count; i++)
            {
                float u = (Mathf.Atan2(normals[i].z, normals[i].x) / s_2Pi) + 0.5f; // remove the 0.5f
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

    }
}
