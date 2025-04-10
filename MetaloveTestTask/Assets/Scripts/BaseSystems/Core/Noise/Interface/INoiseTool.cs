using UnityEngine;

namespace Scripts.BaseSystems
{
    public interface INoiseTool
    {
        public float[] Generate2dNoiseMapFloat(int wodth, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offset);
        public Color[] Generate2dNoiseMapColor(int wodth, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offset);
    }
}
