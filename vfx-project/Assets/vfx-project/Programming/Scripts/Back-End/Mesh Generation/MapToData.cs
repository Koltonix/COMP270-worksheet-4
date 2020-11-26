using System.Collections.Generic;
using UnityEngine;

namespace VFX.MeshGeneration
{
    public class MapToData : MonoBehaviour
    {
        [Header("Tile Options")]
        [SerializeField]
        private int tileXSize = 1;
        [SerializeField]
        private int tileYSize = 1;
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

        delegate Quad GetQuadData(Vector3[] corners, int v, int tileHeight);
        

        private void Start()
        {
            tilePositions = GetPositionsFromTexture(noiseMap);
            //Generate(, noiseMap.width, noiseMap.height);
            GetAllVertices(tilePositions);


        }

        // Source: https://www.youtube.com/watch?v=8PlpCbxB6tY
        private Vector3[] GetAllVertices(Vector3[,] tilePosition)
        {

            GameObject map = new GameObject("Generated Map");

            Mesh mesh = null;
            map.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
            map.AddComponent<MeshRenderer>().material = tileMaterial;


            int xSize = tilePositions.GetLength(0);
            int ySize = tilePosition.GetLength(1);

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<GetQuadData> quadsToDraw = new List<GetQuadData>();

            int v = 0;
            int t = 0;

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    quadsToDraw = new List<GetQuadData>();
                    Vector3[] corners = GetVerticesFromCentre(tilePositions[x, y]);

                    // Add the top faces to the queue
                    quadsToDraw.Add(CubeMesh.AddTopFace);
  
                    bool north = false;
                    bool east = false;
                    bool south = true;
                    bool west = false;

                    if (south)
                        quadsToDraw.Add(CubeMesh.AddSouthFace);

                    if (east)
                        quadsToDraw.Add(CubeMesh.AddEastFace);

                    // Runs the queue of quads to be drawn
                    for (int i = 0; i < quadsToDraw.Count; i++)
                    {
                        Quad q = quadsToDraw[i].Invoke(corners, v, tileYSize);

                        vertices.AddRange(q.vertices);
                        triangles.AddRange(q.triangles);

                        v += 4;
                        t += 6;
                    }    

                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();

            return vertices.ToArray();
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