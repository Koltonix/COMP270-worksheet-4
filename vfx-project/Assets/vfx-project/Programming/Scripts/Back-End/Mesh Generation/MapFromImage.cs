using System.Collections.Generic;
using UnityEngine;

namespace VFX.MeshGeneration
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
        private List<Vector3> positions = new List<Vector3>();

        [Header("Noise")]
        [SerializeField]
        private Texture2D noiseMap = null;

        [Header("Mesh GameObject")]
        private GameObject meshObject = null;
        private MeshFilter meshFilter = null;
        private Mesh mesh = null;

        [Header("Vertices & Triangles")]
        private Vector3[] vertices;
        private int[] triangles;

        [SerializeField]
        private bool drawDebug = false;

        private void Start()
        {
            meshObject = new GameObject("Generated Map");
            meshObject.AddComponent<MeshRenderer>();

            meshFilter = meshObject.AddComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;

            tilePositions = GetPositionsFromTexture(noiseMap);
            vertices = GetVerticesFromTilePositions(tilePositions);

            positions = new List<Vector3>(vertices);
            Debug.Log(positions.Count);
            CreateVerticesAndTriangles();

            mesh.vertices = positions.ToArray();
            mesh.triangles = triangles;

            //mesh.RecalculateNormals();
            
        }

        // Source: https://catlikecoding.com/unity/tutorials/procedural-grid/
        private void CreateVerticesAndTriangles()
        {
            int width = noiseMap.width * 4;
            int height = noiseMap.height * 4;

            Vector3 maxWorldSize = new Vector3(width * tileXSize, 0.0f, height * tileYSize);

            vertices = new Vector3[(width + 1) * (height + 1)];
            triangles = new int[height * width * 6];

            for (int i = 0, y = 0; y <=height; y++)
            {
                for (int x = 0; x <= width; x++, i++)
                {
                    if (x < tilePositions.GetLength(0) && y < tilePositions.GetLength(1))
                    {
                        Vector3 centre = tilePositions[x, y];
                        Vector3[] corners = GetVerticesFromCentre(centre);
                    }

                }
            }

            for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++)
            {
                for (int x = 0; x < width; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                    triangles[ti + 5] = vi + width + 2;
                }
            }
            

            Debug.Log(vertices.Length + " : " + triangles.Length);
        }

        private Vector3[] GetVerticesFromTilePositions(Vector3[,] tiles)
        {
            Stack<Vector3> vertices = new Stack<Vector3>();
            for (int y = 0; y <= tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    // Extra Row for the Top of the final row
                    if (y == tiles.GetLength(1))
                    {
                        Vector3[] corners = GetVerticesFromCentre(tiles[x, y - 1]);
                        vertices.Push(corners[1]);
                        vertices.Push(corners[2]);
                    }

                    // Rest of the rest
                    else
                    {
                        Vector3[] corners = GetVerticesFromCentre(tiles[x, y]);
                        vertices.Push(corners[0]);
                        vertices.Push(corners[3]);
                    }
                }
            }

            return vertices.ToArray();
        }

        // This is essentially the bounding box of the position
        private Vector3[] GetVerticesFromCentre(Vector3 pos)
        {
            Vector3[] corners = new Vector3[4];
            float halfXSize = tileXSize * .5f;
            float halfYSize = tileYSize * .5f;

            // Going clockwise from the bottom left point
            corners[0] = pos + new Vector3(-halfXSize, 0.0f, -halfYSize); // Bottom Left
            corners[1] = pos + new Vector3(-halfXSize, 0.0f, halfYSize); // Top Left
            corners[2] = pos + new Vector3(halfXSize, 0.0f, halfYSize); // Top Right
            corners[3] = pos + new Vector3(halfXSize, 0.0f, -halfYSize); // Bottom Right

            return corners;
        }

        #region Noise Map Reading
        private Vector3[,] GetPositionsFromTexture(Texture2D texture)
        {
            Vector3 maxWorldSize = new Vector3(texture.width * tileXSize, 0.0f, texture.height * tileYSize);
            Vector3[,] positions = new Vector3[texture.width, texture.height];
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
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
        #endregion

        #region Debug
        private void OnDrawGizmos()
        {
            if (tilePositions != null && drawDebug)
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

            if (positions != null)
            {
                foreach (Vector3 pos in positions)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(pos, .1f);
                }
            }
        }
        #endregion


    }
}