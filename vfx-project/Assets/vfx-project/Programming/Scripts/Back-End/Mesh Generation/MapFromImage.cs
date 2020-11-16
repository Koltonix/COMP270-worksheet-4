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
                    Vector3 position = new Vector3(x * tileXSize, 0.0f,y * tileYSize);

                    // Offsetting the position by its max size - a tile halved
                    // This makes (0, 0, 0) the world centre of the object
                    position -= (maxWorldSize - new Vector3(tileXSize, 0.0f, tileYSize)) * .5f;
                    positions[x, y] = position;
                }
            }

            return positions;
        }

        private void OnDrawGizmos()
        {
            if (tilePositions != null)
            {
                Gizmos.color = Color.green;
                foreach (Vector3 pos in tilePositions)
                    Gizmos.DrawWireCube(pos, new Vector3(tileXSize, 1.0f, tileYSize));
            }
        }


    }
}