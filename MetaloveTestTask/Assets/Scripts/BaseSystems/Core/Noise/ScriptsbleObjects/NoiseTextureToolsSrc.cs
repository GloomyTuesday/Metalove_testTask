using UnityEngine;

namespace Scripts.BaseSystems
{
    [CreateAssetMenu(fileName = "NoiseTextureTools", menuName = "Scriptable Obj/Base systems/Core/Noise/Noise texture tools")]
    public class NoiseTextureToolsSrc : ScriptableObject, INoiseTool
    {

        private Noise _noise; 
        private Noise Noise
        {
            get
            {
                if(_noise == null)
                    _noise = new Noise();
                return _noise;
            }
        }

        public float[,] Generate2dNoiseMapFloat(int wodth, int height, float scale)
        {
            float[,] array = new float[wodth, height];

            if(scale <=0 )
                scale = .0001f;

            for (int y = 0; y < wodth; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    float perlinNoise = Mathf.PerlinNoise(
                        x / scale,
                        y / scale
                        );

                    array[x, y] = perlinNoise;
                }
            }

            return array; 
        }

        /*
        public float[] Generate2dNoiseMapFloat(
                int width,
                int height,
                float scale,
                int octaves,
                float persistance,
                float lacunarity,
                Vector2 offset
            )
        {
            float[] noiseMap = new float[width * height];

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            System.Random prng = new System.Random();
            Vector2[] octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[y * width + x] = noiseHeight;
                }
            }

            // Normalize the noise map values to be within the range [0, 1]
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseMap[y * width + x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y * width + x]);
                }
            }

            return noiseMap;
        }
        */

        /// <summary>
        ///     Does not working right now
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="scale"></param>
        /// <param name="octaves"></param>
        /// <param name="persistance"></param>
        /// <param name="lacunarity"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float[] Generate2dNoiseMapFloat(
                int width,
                int height,
                float scale,
                int octaves,
                float persistance,
                float lacunarity,
                Vector2 offset
            )
        {
            float[] noiseMap = new float[width * height];

           

            return noiseMap;
        }

        public Color[] Generate2dNoiseMapColor(
            int width,
            int height,
            float scale,
            int octaves,
            float persistance,
            float lacunarity,
            Vector2 offset
            )
        {
            var colorArray = new Color[width * height];

            if (scale <= 0)
                scale = 0.0001f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x + offset.x) / scale * frequency;
                        float sampleY = (y + offset.y) / scale * frequency;

                        // Make noise tileable
                        float x1 = Mathf.PerlinNoise(sampleX, sampleY);
                        float x2 = Mathf.PerlinNoise(sampleX - width / scale, sampleY);
                        float y1 = Mathf.PerlinNoise(sampleX, sampleY - height / scale);
                        float y2 = Mathf.PerlinNoise(sampleX - width / scale, sampleY - height / scale);

                        float noiseValue = Mathf.Lerp(
                            Mathf.Lerp(x1, x2, (float)x / width),
                            Mathf.Lerp(y1, y2, (float)x / width),
                            (float)y / height);

                        noiseHeight += noiseValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    colorArray[y * width + x] = new Color(noiseHeight, noiseHeight, noiseHeight);
                }
            }

            // Normalize the noise map to values between 0 and 1
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float normalizedHeight = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, colorArray[y * width + x].r);
                    colorArray[y * width + x] = new Color(normalizedHeight, normalizedHeight, normalizedHeight);
                }
            }

            return colorArray;
        }

        /*
        public Color[] Generate2dNoiseMapColor(
            int width,
            int height,
            float scale,
            int octaves,
            float persistance,
            float lacunarity
            )
        {
            var array = new Color[width*height];

            if (scale <= 0)
                scale = .0001f;

            int index = 0;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency;
                        float sampleY = (y - halfHeight) / scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    array[y * width + x] = new Color(noiseHeight, noiseHeight, noiseHeight);
                }
            }

            return array;
        }
        */

        public Color[,] GenerateColorArray(int width, int height)
        {
            var colorArray = new Color[width, height];



            return colorArray; 
        }
    }
}
