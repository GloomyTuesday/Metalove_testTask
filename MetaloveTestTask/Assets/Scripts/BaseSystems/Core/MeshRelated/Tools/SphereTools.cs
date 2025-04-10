using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    public static class SphereTools 
    {
        //  Method returns values for position, normal and uv for vertex projected on the sphere
        //  Method is treating vertices as if they are part of the plane that has size of a 2 x radius each.
        public static (Vector3, Vector3) SpherifyData(Vector3 vertexPosition, float radius)
        {
            var adaptedVertex = vertexPosition / radius;

            var squareX = adaptedVertex.x * adaptedVertex.x;
            var squareY = adaptedVertex.y * adaptedVertex.y;
            var squareZ = adaptedVertex.z * adaptedVertex.z;

            var factorX = 1f - squareY / 2f - squareZ / 2f + squareY * squareZ / 3f;
            var factorY = 1f - squareX / 2f - squareZ / 2f + squareX * squareZ / 3f;
            var factorZ = 1f - squareX / 2f - squareY / 2f + squareX * squareY / 3f;

            var normal = new Vector3
            (
               adaptedVertex.x * Mathf.Sqrt(factorX),
               adaptedVertex.y * Mathf.Sqrt(factorY),
               adaptedVertex.z * Mathf.Sqrt(factorZ)
            );
            vertexPosition = normal * radius;

            return (vertexPosition, normal);
        }

        public static Vector3[] RecalculateNormals(Vector3[] vertices, int[] triangles)
        {
            // Initialize normal vectors
            Vector3[] normals = new Vector3[vertices.Length];

            // Build the vertex to triangle mapping
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int i1 = triangles[i];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];

                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];
                Vector3 v3 = vertices[i3];

                Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

                // Add normal to each vertex of the triangle
                normals[i1] += normal;
                normals[i2] += normal;
                normals[i3] += normal;
            }

            // Normalize all the normal vectors
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].normalized;
            }

            return normals;
        }
    }
}
