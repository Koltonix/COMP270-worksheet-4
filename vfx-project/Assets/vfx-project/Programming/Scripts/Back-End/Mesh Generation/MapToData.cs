using System.Collections.Generic;
using UnityEngine;

namespace VFX.MeshGeneration
{
    public class MapToData : MonoBehaviour
    {
        [Header("Tile Options")]
        [SerializeField]
        private float tileXSize = 1.0f;
        [SerializeField]
        private float tileYSize = 1.0f;
        [SerializeField]
        private float yHeightScalar = 1.0f;
        [SerializeField]
        private float roundedYPos = 5.0f;
        private Vector3[,] tilePositions = null;
        private List<Vector3> positions = new List<Vector3>();

        [Header("Noise")]
        [SerializeField]
        private Texture2D noiseMap = null;

        [SerializeField]
        private Material tileMaterial = null;

        [SerializeField]
        private bool drawDebug = false;

        private void Start()
        {
            tilePositions = GetPositionsFromTexture(noiseMap);
            CreateTiles();
            
        }

        // Source: https://catlikecoding.com/unity/tutorials/procedural-grid/
        private void CreateTiles()
        {

            GameObject parent = new GameObject("Map");
            foreach (Vector3 pos in tilePositions)
            {
                Mesh plane = CreateCube(pos);
                GameObject tile = new GameObject("Tile");

                tile.AddComponent<MeshFilter>().mesh = plane;
                tile.AddComponent<MeshRenderer>().material = tileMaterial;

                tile.transform.position = pos;
                tile.transform.SetParent(parent.transform);
            }
        }

        // Source: http://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html
        // Source: https://gist.github.com/prucha/866b9535d525adc984c4fe883e73a6c7
        public Mesh CreateCube(Vector3 centrePos)
        {
            Mesh plane = new Mesh();

            float width = tileXSize * .5f;
            float height = tileYSize * .5f;
            float length = tileXSize * .5f;

            Vector3[] coords =
            {
                new Vector3(-length, -width, height),
                new Vector3(length, -width, height),
                new Vector3(length, -width, -height),
                new Vector3(-length, -width, -height),

                new Vector3(-length, width, height),
                new Vector3(length, width, height),
                new Vector3(length, width, -height),
                new Vector3(-length, width, -height)
            };
            
            Vector3[] vertices =
            {
                coords[0], coords[1], coords[2], coords[3], // Bottom
	            coords[7], coords[4], coords[0], coords[3], // Left
	            coords[4], coords[5], coords[1], coords[0], // Front
	            coords[6], coords[7], coords[3], coords[2], // Back
	            coords[5], coords[6], coords[2], coords[1], // Right
	            coords[7], coords[6], coords[5], coords[4]  // Top
            };

            Vector3[] normals =
            {
                Vector3.down, Vector3.down, Vector3.down, Vector3.down,
                Vector3.left, Vector3.left, Vector3.left, Vector3.left,
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
                Vector3.back, Vector3.back, Vector3.back, Vector3.back,
                Vector3.right, Vector3.right, Vector3.right, Vector3.right,
                Vector3.up, Vector3.up, Vector3.up, Vector3.up
            };

            int[] tris =
            {
	            7, 5, 4,        7, 6, 5,        // Left
	            11, 9, 8,       11, 10, 9,      // Front
	            15, 13, 12,     15, 14, 13,     // Back
	            19, 17, 16,     19, 18, 17,	    // Right
	            23, 21, 20,     23, 22, 21,	    // Top
            };

            plane.vertices = vertices;
            plane.triangles = tris;
            plane.normals = normals;

            return plane;
        }

        // This is essentially the bounding box of the position
        private Vector3[] GetVerticesFromCentre(Vector3 pos)
        {
            Vector3[] corners = new Vector3[4];
            float halfXSize = tileXSize * .5f;
            float halfYSize = tileYSize * .5f;

            corners[0] = pos + new Vector3(-halfXSize, 0.0f, -halfYSize); // Bottom Left
            corners[1] = pos + new Vector3(-halfXSize, 0.0f, halfYSize); // Top Left
            corners[2] = pos + new Vector3(halfXSize, 0.0f, -halfYSize); // Bottom Right
            corners[3] = pos + new Vector3(halfXSize, 0.0f, halfYSize); // Top Right

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
                    position.y = Mathf.Round(position.y / roundedYPos) * roundedYPos;
                    // Offsetting the position by its max size - a tile halved
                    // This makes (0, 0, 0) the world centre of the object
                    position -= (maxWorldSize - new Vector3(tileXSize, 0.0f, tileYSize)) * .5f;
                    positions[x, y] = position;

                    //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = position;
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
        }
        #endregion


    }
}