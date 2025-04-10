using System;
using UnityEngine;

namespace Scripts.BaseSystems
{
    [Serializable]
    public struct CameraSetupModel
    {
        [SerializeField]
        private Vector3 _position;

        [SerializeField]
        private Vector3 _rotation;

        [SerializeField]
        private Vector2 _viewRectSize;

        [SerializeField]
        private float _depth;

        [SerializeField]
        private float _animTime;

        [SerializeField]
        private bool _applyDepth; // 1 byte, but will get padded

        [SerializeField]
        private AnimationCurve _animCurve; // reference (8 bytes on 64-bit systems)

        // Properties (unchanged)
        public Vector3 Position { get => _position; set => _position = value; }

        public Quaternion Rotation { get => Quaternion.Euler(_rotation.x, _rotation.y, _rotation.z); set => _rotation = value.eulerAngles; }

        public Vector2 ViewRectSize { get => _viewRectSize; set => _viewRectSize = value; }

        public bool ApplyDepth { get => _applyDepth; set => _applyDepth = value; }

        public float Depth { get => _depth; set => _depth = value; }

        public AnimationCurve AnimCurve { get => _animCurve; set => _animCurve = value; }

        public float AnimTime
        {
            get => _animTime;
            set
            {
                if (value < 0)
                    value = 0;

                _animTime = value;
            }
        }
    }
}
