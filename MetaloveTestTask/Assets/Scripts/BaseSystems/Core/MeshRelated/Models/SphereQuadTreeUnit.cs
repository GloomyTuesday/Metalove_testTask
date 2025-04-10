using UnityEngine;

namespace Scripts.BaseSystems
{
    public class SphereQuadTreeUnit 
    {
        private const float PiDouble = 2.0f * Mathf.PI; 

        /*
        public Vector3[] Vertice { get; set; }
        public Vector3[] Normal { get; set; }
        public Vector2[] UV { get; set; }
        */

        public Mesh Mesh { get; set; }
        public SphereQuadTreeUnit[] Childs { get; set; }
        public float Radius { get; set; }
        public int Resolution { get; set; }
        public Vector3 Position { get; set; }

        public SphereQuadTreeUnit(float radius, Vector3[] vertices, int[] triangles, Vector3[] normals = null, Vector2[] uvs = null)
        {
            Radius = radius;
            Resolution = vertices.Length/4;

            Mesh = new Mesh();

            Mesh.vertices = vertices;

            normals = new Vector3[vertices.Length];

            /*
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 p = vertices[i].normalized;
                float theta = Mathf.Atan2(p.z, p.x);
                float phi = Mathf.Acos(p.y);
                vertices[i] = new Vector3(
                    radius * Mathf.Sin(phi) * Mathf.Cos(theta),
                    radius * Mathf.Cos(phi),
                    radius * Mathf.Sin(phi) * Mathf.Sin(theta)
                );
                normals[i] = vertices[i].normalized;
            }
            */

            if (uvs==null)
            {
                uvs = new Vector2[vertices.Length];

                for (int i = 0; i < normals.Length; i++)
                {
                    uvs[i] = new Vector2(
                        .5f + (Mathf.Atan2(normals[i].z, normals[i].x) / PiDouble),
                        .5f - (Mathf.Asin(normals[i].y) / Mathf.PI)
                        ); 
                }
            }

            Mesh.vertices = vertices;
            Mesh.triangles = triangles;
            Mesh.normals = normals;
            Mesh.uv = uvs;
            
        }
    }
}
