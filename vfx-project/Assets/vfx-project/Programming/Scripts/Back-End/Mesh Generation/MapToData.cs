using System.Collections.Generic;
using UnityEngine;

namespace VFX.MeshGeneration
{
    public class Quad
    {
        public Vector3[] vertices = new Vector3[4];
        public int[] triangles = new int[6];
    }


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
            //Generate(, noiseMap.width, noiseMap.height);
            GetAllVertices(tilePositions);


        }

        // Source: https://catlikecoding.com/unity/tutorials/procedural-grid/
        public void Generate(Vector3[] vertices, int xSize, int ySize)
        {
        //    Debug.Log(vertices.Length);

        //    GameObject map = new GameObject("Generated Map");

        //    Mesh mesh = null;
        //    map.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
        //    map.AddComponent<MeshRenderer>().material = tileMaterial;

        //    mesh.vertices = vertices;

        //    Debug.Log(triangles.Length);
        //    mesh.triangles = triangles;
        //    mesh.RecalculateNormals();
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
            //Vector3[] normals = new Vector3[vertices.Length];
            List<int> triangles = new List<int>();

            int v = 0;
            int t = 0;

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {

                    Vector3[] corners = GetVerticesFromCentre(tilePositions[x, y]);
                    Quad top = AddTopFace(corners, v);

                    vertices.AddRange(top.vertices);
                    triangles.AddRange(top.triangles);

                    v += 4;
                    t += 6;

                    bool north = false;
                    bool east = false;
                    bool south = true;
                    bool west = false;

                    if (east)
                    {
                        Vector3 bottomLeft = corners[2];
                        Vector3 topLeft = corners[3];
                        Vector3 bottomRight = new Vector3(bottomLeft.x, bottomLeft.y - tileYSize, bottomLeft.z);
                        Vector3 topRight = new Vector3(topLeft.x, topLeft.y - tileYSize, topLeft.z);


                        vertices[v] = bottomLeft; // Bottom Left
                        vertices[v + 1] = topLeft; // Top Left
                        vertices[v + 2] = bottomRight; // Bottom Right
                        vertices[v + 3] = topRight; // Top Right


                        triangles[t] = v;
                        triangles[t + 1] = triangles[t + 4] = v + 1;
                        triangles[t + 2] = triangles[t + 3] = v + 2;
                        triangles[t + 5] = v + 3;

                        v += 4;
                        t += 6;
                    }

                    if (south)
                    {
                        Quad quad = AddSouthFace(corners, v);

                        vertices.AddRange(quad.vertices);
                        triangles.AddRange(quad.triangles);
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

        private Quad AddTopFace(Vector3[] corners, int v)
        {
            Quad vt = new Quad();

            for (int i = 0; i < vt.vertices.Length; i++)
                vt.vertices[i] = corners[i];


            vt.triangles[0] = v;
            vt.triangles[1] = vt.triangles[4] = v + 1;
            vt.triangles[2] = vt.triangles[3] = v + 2;
            vt.triangles[5] = v + 3;

            return vt;
        }



        private Quad AddSouthFace(Vector3[] corners, int v)
        {
            Quad vt = new Quad();

            Vector3 topLeft = corners[0];
            Vector3 topRight = corners[2];
            Vector3 bottomLeft = new Vector3(topLeft.x, topLeft.y - tileYSize, topLeft.z);
            Vector3 bottomRight = new Vector3(topRight.x, topRight.y - tileYSize, topRight.z);

            vt.vertices[0] = bottomLeft;// Bottom Left
            vt.vertices[1] = topLeft; // Top Left
            vt.vertices[2] = bottomRight; // Bottom Right
            vt.vertices[3] = topRight; // Top Right

            vt.triangles[0] = v;
            vt.triangles[1] = vt.triangles[4] = v + 1;
            vt.triangles[2] = vt.triangles[3] = v + 2;
            vt.triangles[5] = v + 3;

            return vt;
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