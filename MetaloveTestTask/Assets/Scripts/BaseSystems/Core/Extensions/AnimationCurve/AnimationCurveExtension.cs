using UnityEngine;

namespace Scripts.BaseSystems
{
    public static class AnimationCurveExtension 
    {
        public static SerializableKey[] GetSerializableKeys(this AnimationCurve animCurve)
        {
            var serializableKeys = new SerializableKey[animCurve.keys.Length];

            for ( int i = 0; i < serializableKeys.Length; i ++)
                serializableKeys[i] = new SerializableKey(animCurve.keys[i]);

            return serializableKeys; 
        }

        public static void SetSerializableKeys(this AnimationCurve animCurve, SerializableKey[] serializableKeys)
        {
            if(animCurve == null)
                animCurve = new AnimationCurve();

            foreach (var serializableKey in serializableKeys)
                animCurve.AddKey(serializableKey.GetKeyframe());
        }
    }
}
