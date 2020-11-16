using UnityEngine;
using UnityEngine.UI;

namespace VFX.Noise
{
    public class PerlinNoiseGenerator : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int resolution = new Vector2Int(64, 64);
        [SerializeField]
        private int seed = 0000;
        [SerializeField]
        private float scale = 20.0f;

        public RawImage debugImage = null;

        public void RandomiseSeed() => seed = Random.Range(0, 999999);

        private void Start()
        {
            RandomiseSeed();
            
        }
        private void Update()
        {
            Texture2D texture = GetRandomPerlinNoiseTexture();
            texture = MaxContrast(texture);
            debugImage.texture = texture;
        }

        // Source: https://www.youtube.com/watch?v=bG0uEXV6aHQ&t=563s
        public Texture2D GetRandomPerlinNoiseTexture()
        {
            Texture2D texture = new Texture2D(resolution.x, resolution.y);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color colour = GetPerlinColourAt(x, y);
                    texture.SetPixel(x, y, colour);
                }
            }

            texture.Apply();
            return texture;
        }

        public Texture2D MaxContrast(Texture2D texture)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color colour = texture.GetPixel(x, y);

                    colour.r = Mathf.RoundToInt(colour.r);
                    colour.g = Mathf.RoundToInt(colour.g);
                    colour.b = Mathf.RoundToInt(colour.b);

                    texture.SetPixel(x, y, colour);
                }
            }

            texture.Apply();
            return texture;
        }

        // Source: https://www.youtube.com/watch?v=bG0uEXV6aHQ&t=563s
        public Color GetPerlinColourAt(int x, int y)
        {
            float xCoord = (float)x / resolution.x * scale;
            float yCoord = (float)y / resolution.y * scale;

            float sample = Mathf.PerlinNoise(seed + xCoord, seed + yCoord);
            return new Color(sample, sample, sample);
        }
    }
}