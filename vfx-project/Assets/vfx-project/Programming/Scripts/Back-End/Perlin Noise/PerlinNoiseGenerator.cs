using UnityEngine;

namespace VFX.Noise
{
    /// <summary>
    /// A Scriptable Object that generates a random perlin noise based on the
    /// provided parameters and exports it as an image.
    /// </summary>
    [CreateAssetMenu(fileName = "Perlin Noise Generator", menuName = "ScriptableObjects/Tools/PerlinNoiseGenerator")]
    public class PerlinNoiseGenerator : ScriptableObject
    {
        [SerializeField]
        private Vector2Int resolution = new Vector2Int(64, 64);
        [SerializeField]
        [Range(0, 999999)]
        private int seed = 0000;
        [SerializeField]
        private float scale = 20.0f;
        [SerializeField]
        private bool enableContrast = false;

        [Tooltip("Relative to assets folder (Application.DataPath.")]
        public string directory = "/vfx-project/Engine-Assets/Maps/PerlinNoise";

        /// <summary>
        /// Randomly generates a seed from the maximum ranges.
        /// </summary>
        public void RandomiseSeed() => seed = Random.Range(0, 999999);

        /// <summary>
        /// Creates a new perlin noise and saves it to the drive.
        /// </summary>
        public void CreateNewPerlinNoiseTexture()
        {
            Texture2D texture = GetRandomPerlinNoiseTexture();
            if (enableContrast) texture = MaxContrast(texture);
            SaveTexture2D(texture);
        }

        // Source: https://www.youtube.com/watch?v=bG0uEXV6aHQ&t=563s
        /// <summary>
        /// Creates a Texture of perlin noise based on the provided resolution.
        /// </summary>
        /// <returns>A perlin noise texture.</returns>
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

        /// <summary>
        /// Converts the gradient based perlin noise to have each pixel value
        /// to either be 1 or 0 based on what it rounds closest to. This gives
        /// a contrast effect. Good for cave generation.
        /// </summary>
        /// <param name="texture"> A new texture of the contrasted texture provided.</param>
        /// <returns></returns>
        public Texture2D MaxContrast(Texture2D texture)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color colour = texture.GetPixel(x, y);

                    // Sets the pixel to be either black (0) or white (1)
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
        /// <summary>
        /// Gets the shade at the current pixel from the range of 0 (black) to 1 (white)
        /// with an added seed for a unique offset and a scale for higher and lower
        /// resolutions of the perlin noise.
        /// </summary>
        /// <param name="x">The x position of the perlin noise.</param>
        /// <param name="y">The y position of the perlin noise.</param>
        /// <returns>
        /// The colour of the pixel of the perlin noies at thecoordinate.
        /// </returns>
        public Color GetPerlinColourAt(int x, int y)
        {
            float xCoord = (float)x / resolution.x * scale;
            float yCoord = (float)y / resolution.y * scale;

            float sample = Mathf.PerlinNoise(seed + xCoord, seed + yCoord);
            return new Color(sample, sample, sample);
        }

        // Source: https://answers.unity.com/questions/1331297/how-to-save-a-texture2d-into-a-png.html
        /// <summary>
        /// Saves the provided texture to the location specified.
        /// </summary>
        /// <param name="texture">The texture to save as an image.</param>
        public void SaveTexture2D(Texture2D texture)
        {
            string fileInfo = string.Format("[{0}][{1}]({2}){3}", resolution.x, resolution.y, scale, seed);
            string filePath = Application.dataPath + directory + fileInfo + ".PNG";

            byte[] bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, bytes);
 
            Debug.Log("Saved at: " + filePath);
        }
    }
}