using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public class Icosphere
    {
        public const int s_smallestLodResolution = 2;
        //    public const int s_lodAmount = 3;
        private static readonly float s_2Pi = 2f * Mathf.PI;

        public Vector3 _northPole;
        public float _radius;

        public MeshGroupBs[] _meshPieces;

        public int _lodAmount;

        private bool _ready;
        public bool Ready => _ready;

        private int _readyPercent;
        public int ReadyPercent => _readyPercent;

        public volatile int _progress = 0;

        public Mesh CreateAsMesh(float radius, int resolution, bool tangents = false)
        {
            var mesh = new Mesh();

            var icosahedron = new Icosahedron(radius);
            var meshSubdivider = new MeshSubdivider();

            List<Vector3> vertices = new List<Vector3>(icosahedron._vertices);
            List<int> triangles = new List<int>(icosahedron._triangles);
            List<Vector3> normals = new List<Vector3>(icosahedron._normals);

            var subdivisions = resolution < 1 ? 1 : resolution - 1;

            if (subdivisions > 1)
                meshSubdivider.Subdivide(vertices, normals, triangles, subdivisions, radius);

            var uv = MeshSubdivider.GetUvs(triangles, vertices, normals, icosahedron._northPole);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv;

            return mesh; 
        }


        public async Task<IcosphereModel> CreateIcosphereModelAsync( IcosphereModel icosphere, int subdivivisionAmount = 1)
        {
            var icosphereModel = new IcosphereModel(); 

            await Task.Run(() =>
            {
                var meshSubdivider = new MeshSubdivider();

                List<Vector3> vertices = new List<Vector3>(icosphere._vertices);
                List<int> triangles = new List<int>(icosphere._triangles);
                List<Vector3> normals = new List<Vector3>(icosphere._normals);
                Vector2[] uv = null;

                meshSubdivider.Subdivide(vertices, normals, triangles, subdivivisionAmount, icosphere._radius);

                uv = MeshSubdivider.GetUvs(triangles, vertices, normals, icosphere._northPole);

                icosphereModel._vertices = vertices.ToArray();
                icosphereModel._normals = normals.ToArray();
                icosphereModel._triangles = triangles.ToArray();
                icosphereModel._uv = uv;

                icosphereModel._resolution = icosphere._resolution + subdivivisionAmount;
            });

            return icosphereModel;
        }

        public async Task<IcosphereModel> CreateIcosphereModelAsync(float radius, int resolution, bool tangents = false)
        {
            var icosphereModel = new IcosphereModel();

            await Task.Run(() =>
            {
                var icosahedron = new Icosahedron(radius);
                var meshSubdivider = new MeshSubdivider();

                List<Vector3> vertices = new List<Vector3>(icosahedron._vertices);
                List<int> triangles = new List<int>(icosahedron._triangles);
                List<Vector3> normals = new List<Vector3>(icosahedron._normals);
                Vector2[] uv = null;

                var subdivisions = resolution < 1 ? 1 : resolution - 1;

                if (subdivisions > 1)
                    meshSubdivider.Subdivide(vertices, normals, triangles, subdivisions, radius);

                uv = MeshSubdivider.GetUvs(triangles, vertices, normals, icosahedron._northPole);

                icosphereModel._vertices = vertices.ToArray();
                icosphereModel._triangles = triangles.ToArray();
                icosphereModel._normals = normals.ToArray();
                icosphereModel._uv = uv;
                icosphereModel._radius = radius;
            });

            return icosphereModel; 
        }

        public void Create(float radius, int smallestLodResolution = 1, int lodAmount = 1)
        {
            _lodAmount = lodAmount; 
            var meshSubdivider = new MeshSubdivider();
            _progress = 0;
            SyncCreation(meshSubdivider, radius, smallestLodResolution, lodAmount);
        }

        public Task CreateAsync(float radius, int smallestLodResolution = 1, int lodAmount = 1)
        {
            _lodAmount = lodAmount;
            var meshSubdivider = new MeshSubdivider();
            _progress = 0;

            return AsyncCreation(meshSubdivider, radius, smallestLodResolution, lodAmount);
        }

        private async Task AsyncCreation(MeshSubdivider meshSubdivider, float radius, int smallestLodResolution = 1, int lodAmount = 1)
        {
            await Task.Run(() =>
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

                timer.Start();

                var icosahedron = new Icosahedron();

                List<Vector3> vertices = new List<Vector3>(icosahedron._vertices);
                List<int> triangles = new List<int>(icosahedron._triangles);
                List<Vector3> normals = new List<Vector3>(icosahedron._normals);
                List<MeshGroupBs> meshPiecesList = new List<MeshGroupBs>();

                smallestLodResolution = smallestLodResolution < 1 ? 1 : smallestLodResolution;

                if (smallestLodResolution > 1)
                    meshSubdivider.Subdivide(vertices, normals, triangles, smallestLodResolution);

                //  This method will create first lod that is the lowest quality lod
                meshSubdivider.CutMeshOnMeshGroups(normals, triangles, meshPiecesList, icosahedron._northPole, radius);

                int maxIterationAmount = meshPiecesList.Count * (lodAmount-1);
                int count = 1;

                for (int i = 0; i < meshPiecesList.Count; i++)
                {
                    //  First lod is already ready when mesh was cut on pices
                    for (int j = 1; j < lodAmount; j++)
                    {
                        meshPiecesList[i] = meshSubdivider.SubdivideMeshPiece(meshPiecesList[i], icosahedron._northPole, radius);

                        if(maxIterationAmount!=0)
                            _progress = count * 100 / maxIterationAmount;
                        count++;
                    }
                }

                _meshPieces = new MeshGroupBs[meshPiecesList.Count];
                //  Right now mesh pices collection has all lods array inverted, the lod with highest quality is held in cell with highest index

                for (int i = 0; i < meshPiecesList.Count; i++)
                {
                    var lodBsList = new List<MeshBs>(meshPiecesList[i]._meshBsArray);
                    lodBsList.Reverse();
                    _meshPieces[i] = meshPiecesList[i];
                    _meshPieces[i]._meshBsArray = lodBsList.ToArray();
                }

                timer.Stop();

                Debug.Log("\t Icosphere creation time: " + timer.Elapsed);
            });

            _ready = true; 
        }

        private void SyncCreation(MeshSubdivider meshSubdivider, float radius, int smallestLodResolution = 1, int lodAmount = 1)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            var icosahedron = new Icosahedron();

            List<Vector3> vertices = new List<Vector3>(icosahedron._vertices);
            List<int> triangles = new List<int>(icosahedron._triangles);
            List<Vector3> normals = new List<Vector3>(icosahedron._normals);
            List<MeshGroupBs> meshPiecesList = new List<MeshGroupBs>();

            smallestLodResolution = smallestLodResolution < 1 ? 1 : smallestLodResolution;

            if (smallestLodResolution > 1)
                meshSubdivider.Subdivide(vertices, normals, triangles, smallestLodResolution);

            //  This method will create first lod that is the lowest quality lod
            meshSubdivider.CutMeshOnMeshGroups(normals, triangles, meshPiecesList, icosahedron._northPole, radius);

            for (int i = 0; i < meshPiecesList.Count; i++)
                //  First lod is already ready when mesh was cut on pices
                for (int j = 1; j < lodAmount; j++)
                    meshPiecesList[i] = meshSubdivider.SubdivideMeshPiece(meshPiecesList[i], icosahedron._northPole, radius);
                

            _meshPieces = new MeshGroupBs[meshPiecesList.Count];
            //  Right now mesh pices collection has all lods array inverted, the lod with highest quality is held in cell with highest index

            for (int i = 0; i < meshPiecesList.Count; i++)
            {
                var lodBsList = new List<MeshBs>(meshPiecesList[i]._meshBsArray);
                lodBsList.Reverse();
                _meshPieces[i] = meshPiecesList[i];
                _meshPieces[i]._meshBsArray = lodBsList.ToArray();
            }

            timer.Stop();

            Debug.Log("\t Icosphere creation time: " + timer.Elapsed);

            _ready = true;
        }

    }
}

