using UnityEngine;

namespace Scripts.BaseSystems
{
    public class CubSphere 
    {

        private int _resolution;
        public int Resolution => _resolution; 

        private float _radius;
        public float Radius => _radius;

        private Vector3 _position; 
        public Vector3 Position => _position;

        private Vector3 _direction;
        public Vector3 Direction => _direction;

        public CubSphere(Vector3 position, Vector3 direction, int resolution = 1, float radius = 1)
        {
            _resolution = resolution;
            _radius = radius;
            _position = position;
            _direction = direction; 
        }

        public CubSphere(int resolution = 1, float radius = 1)
        {
            _resolution = resolution;
            _radius = radius;
            _position = Vector3.zero;
            _direction = Vector3.zero;
        }

        public CubSphere(CubSphereModel cubeSphere )
        {
            _resolution = cubeSphere._resolution;
            _radius = cubeSphere._radius;
        }

        public CubSphereModel GetCubSphereModel()
        {
            var cubeModel = new CubSphereModel();

            return cubeModel; 
        }

        public static void Subdevide()
        {

        }
    }
}
