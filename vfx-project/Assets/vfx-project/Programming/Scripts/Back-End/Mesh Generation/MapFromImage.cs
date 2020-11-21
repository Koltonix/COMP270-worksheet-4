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
        private Vector3[] positions;

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
            vertices = new Vector3[tilePositions.Length];

            CreateVerticesAndTriangles();

           mesh.vertices = vertices;
           mesh.triangles = triangles;

            //mesh.RecalculateNormals();
            
        }

        // Source: https://catlikecoding.com/unity/tutorials/procedural-grid/
        private void CreateVerticesAndTriangles()
        {
            int width = noiseMap.width;
            int height = noiseMap.height;

            Vector3 maxWorldSize = new Vector3(width * tileXSize, 0.0f, height * tileYSize);

            vertices = new Vector3[(width + 1) * (height + 1)];
            triangles = new int[height * width * 6];

            for (int i = 0, y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++, i++)
                {
                    // Setting the initial position taking into account the tile size
                    Vector3 position = new Vector3(x * tileXSize, GetYHeight(x, y, noiseMap), y * tileYSize);

                    // Offsetting the position by its max size - a tile halved
                    // This makes (0, 0, 0) the world centre of the object
                    position -= (maxWorldSize - new Vector3(tileXSize, 0.0f, tileYSize)) * .5f;
                    vertices[i] = position;
        
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

            // Going clockwise from the bottom left point
            corners[0] = pos + new Vector3(-halfXSize, 0.0f, -halfYSize); // Bottom Left
            corners[1] = pos + new Vector3(-halfXSize, 0.0f, halfYSize); // Top Left
            corners[2] = pos + new Vector3(halfXSize, 0.0f, halfYSize); // Top Right
            corners[3] = pos + new Vector3(halfXSize, 0.0f, -halfYSize); // Bottom Right

            return corners;
        }

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
        }


    }
}