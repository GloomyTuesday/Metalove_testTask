using UnityEngine;

namespace Scripts.BaseSystems
{
    public static class QuaternionExtension 
    {
        public static Quaternion CalcAverageQuaternion(this Quaternion[] quaternionArray)
        {
            float w = 0;
            float x = 0;
            float y = 0;
            float z = 0;

            foreach (var quaternion in quaternionArray)
            {
                x += quaternion.x;
                y += quaternion.y;
                z += quaternion.z;
                w += quaternion.w;
            }

            x /= quaternionArray.Length;
            y /= quaternionArray.Length;
            z /= quaternionArray.Length;
            w /= quaternionArray.Length;

            Quaternion averageQuaternion = new Quaternion(x, y, z, w);

            return Quaternion.Normalize(averageQuaternion);
        }
    }
}
