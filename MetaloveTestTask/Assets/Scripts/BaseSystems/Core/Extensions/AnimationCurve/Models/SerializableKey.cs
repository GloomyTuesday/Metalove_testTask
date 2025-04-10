using System;
using UnityEngine;

namespace Scripts.BaseSystems
{
    [Serializable]
    public struct SerializableKey
    {
        public float _time;
        public float _value;
        public float _inTangent;
        public float _outTangent;
        public float _inWeight;
        public float _outWeight;

        public SerializableKey(Keyframe keyframe)
        {
            _time = keyframe.time;
            _value = keyframe.value;
            _inTangent = keyframe.inTangent;
            _outTangent = keyframe.outTangent;
            _inWeight = keyframe.inWeight;
            _outWeight = keyframe.outWeight;
        }

        public SerializableKey
            (
            float time,
            float value,
            float inTangent,
            float outTangetn,
            float inWeight,
            float outWeight
            )
        {
            _time = time;
            _value = value;
            _inTangent = inTangent;
            _outTangent = outTangetn;
            _inWeight = inWeight;
            _outWeight = outWeight;
        }

        public Keyframe GetKeyframe() => new Keyframe(_time, _value, _inTangent, _outTangent, _inWeight, _outWeight);

        public static float Evaluate(float time, SerializableKey[] keys)
        {
            if (keys.Length == 1)
                return keys[0]._value;

            // Find the surrounding keys
            SerializableKey leftKey = keys[0];
            SerializableKey rightKey = keys[keys.Length - 1];

            for (int i = 0; i < keys.Length; i++)
            {
                if (time < keys[i]._time)
                {
                    rightKey = keys[i];
                    leftKey = keys[Math.Max(0, i - 1)];
                    break;
                }
            }

            // If time is before the first keyframe, return the first value
            if (time <= leftKey._time)
                return leftKey._value;

            // If time is after the last keyframe, return the last value
            if (time >= rightKey._time)
                return rightKey._value;

            // Perform Hermite interpolation between leftKey and rightKey
            float t = (time - leftKey._time) / (rightKey._time - leftKey._time);
            float t2 = t * t;
            float t3 = t2 * t;

            float h00 = 2 * t3 - 3 * t2 + 1;
            float h10 = t3 - 2 * t2 + t;
            float h01 = -2 * t3 + 3 * t2;
            float h11 = t3 - t2;

            float delta = rightKey._time - leftKey._time;

            float value = h00 * leftKey._value +
                          h10 * leftKey._outTangent * delta +
                          h01 * rightKey._value +
                          h11 * rightKey._inTangent * delta;

            return value;
        }
    }

}
