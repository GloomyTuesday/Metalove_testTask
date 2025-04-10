using System;
using UnityEngine;

namespace Scripts.BaseSystems.MeshRelated
{
    [Serializable]
    public class Icosahedron 
    {
        public Vector3[] _vertices;
        public int[] _triangles;
        public Vector3 _northPole;
        public Vector3[] _normals;
        public float _radius;

        public Icosahedron(float radius = 1 ) 
        {
            _radius = radius;
            float t = (1f + Mathf.Sqrt(5f)) / 2f;
            var interpolatedValue = t * _radius;
            _northPole = new Vector3(0, interpolatedValue, 0);
            
            _normals = new Vector3[]
            {
                    new Vector3(-1, t, 0 ) ,
                    new Vector3( 1,  t,  0) ,
                    new Vector3(-1, -t,  0) ,
                    new Vector3( 1, -t,  0) ,

                    new Vector3( 0, -1,  t) ,
                    new Vector3( 0,  1,  t) ,
                    new Vector3( 0, -1, -t) ,
                    new Vector3( 0,  1, -t) ,

                    new Vector3( t,  0, -1) ,
                    new Vector3( t,  0,  1) ,
                    new Vector3(-t,  0, -1) ,
                    new Vector3(-t,  0,  1)
            };

            _vertices =  new Vector3[]
            {
                    new Vector3(-_radius, interpolatedValue, 0 ) ,
                    new Vector3( _radius, interpolatedValue, 0 ) ,
                    new Vector3(-_radius, -interpolatedValue,  0) ,
                    new Vector3( _radius, -interpolatedValue,  0) ,

                    new Vector3( 0, -_radius,  interpolatedValue) ,
                    new Vector3( 0,  _radius,  interpolatedValue) ,
                    new Vector3( 0, -_radius, -interpolatedValue) ,
                    new Vector3( 0,  _radius, -interpolatedValue) ,

                    new Vector3( interpolatedValue,  0, -_radius) ,
                    new Vector3( interpolatedValue,  0,  _radius) ,
                    new Vector3(-interpolatedValue,  0, -_radius) ,
                    new Vector3(-interpolatedValue,  0,  _radius)
            };

            _triangles = new int[]
            {
                0, 11, 5,
                0, 5, 1,
                0, 1, 7,
                0, 7, 10,
                0, 10, 11,

                1, 5, 9,
                5, 11, 4,
                11, 10, 2,
                10, 7, 6,
                7, 1, 8,

                3, 9, 4,
                3, 4, 2,
                3, 2, 6,
                3, 6, 8,
                3, 8, 9,

                4, 9, 5,
                2, 4, 11,
                6, 2, 10,
                8, 6, 7,
                9, 8, 1
            };

        }
    }
}
