using UnityEngine;

namespace VFX.Mesh
{
    public class MapFromImage : MonoBehaviour
    {
        [Header("Tile Options")]
        [SerializeField]
        private float tileXSize = 1.0f;
        [SerializeField]
        private float tileYSize = 1.0f;
        [SerializeField]
        private float yHeightScalar = 1.0f;
        private Vector3[,] tilePositions = null;

        [Header("Noise")]
        [SerializeField]
        private Texture2D noiseMap = null;

        [Header("Mesh GameObject")]
        private GameObject meshObject = null;
        private MeshFilter mesh = null;

        [Header("Vertices & Triangles")]
        private Vector3[] vertices;
        private int[] triangles;

        private void Start()
        {
            meshObject = new GameObject("Generated Map");
            mesh = meshObject.AddComponent<MeshFilter>();

            tilePositions = GetPositionsFromTexture(noiseMap);
        }

        private Vector3[,] GetPositionsFromTexture(Texture2D texture)
        {
            Vector3 maxWorldSize = new Vector3(texture.width * tileXSize, 0.0f, texture.height * tileYSize);
            Vector3[,] positions = new Vector3[texture.width, texture.height];
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    // Setting the initial position taking into account the tile size
                    Vector3 position = new Vector3(x * tileXSize, GetYHeight(x, y, texture), y * tileYSize);

                    // Offsetting the position by its max size - a tile halved
                    // This makes (0, 0, 0) the world centre of the object
                    position -= (maxWorldSize - new Vector3(tileXSize, 0.0f, tileYSize)) * .5f;
                    positions[x, y] = position;
                }
            }

            return positions;
        }

        private float GetYHeight(int x, int y, Texture2D texture)
        {
            Color colour = texture.GetPixel(x, y);
            return colour.r * yHeightScalar;
        }

        // This is essentially the bounding box of the position
        private Vector3[] GetVerticesFromCentre(Vector3 pos)
        {
            Vector3[] corners = new Vector3[4];
            float halfXSize = tileXSize * .5f;
            float halfYSize = tileYSize * .5f;

            // Going clockwise from the top left point
            corners[0] = pos + new Vector3(-halfXSize, 0.0f, halfYSize); // Top Left
            corners[1] = pos + new Vector3(halfXSize, 0.0f, halfYSize); // Top Right
            corners[2] = pos + new Vector3(halfXSize, 0.0f, -halfYSize); // Bottom Right
            corners[3] = pos + new Vector3(-halfXSize, 0.0f, -halfYSize); // Bottom Left

            return corners;
        }

        private void OnDrawGizmos()
        {
            if (tilePositions != null)
            {
                foreach (Vector3 pos in tilePositions)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(pos - (Vector3.up * tileYSize * .5f), new Vector3(tileXSize, 1.0f, tileYSize));
                    Gizmos.DrawSphere(pos, .1f);

                    Vector3[] corners = GetVerticesFromCentre(pos);
                    foreach (Vector3 e in corners)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(e, .1f);
                    }
                }
            }
        }


    }
}