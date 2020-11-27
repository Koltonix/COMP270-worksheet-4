//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
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
        private float noiseMapScalar = 10.0f;
        [SerializeField]
        private float roundedYPos = 5.0f;
        private Vector3[,] tilePositions = null;
        [Space]

        [Header("Noise and Material")]
        [SerializeField]
        private Texture2D noiseMap = null;
        [SerializeField]
        private Material tileMaterial = null;

        // Used to assign an even to a quad direction to draw
        delegate Quad GetQuadData(Vector3[] corners, int v, int tileHeight);

        private void Start()
        {
            tilePositions = GetPositionsFromTexture(noiseMap);
            GenerateMesh(tilePositions);
        }

        /// <summary>
        /// Creates a new GameObject and assigns the appropriate components to
        /// render and store the mesh. It then creates the mesh and assigns the
        /// data for the mesh.
        /// </summary>
        /// <param name="positions">The tile positions to be drawn.</param>
        private void GenerateMesh(Vector3[,] positions)
        {
            // GameObject to hold these components
            GameObject map = new GameObject("Generated Map");

            // Adding the Mesh Components
            Mesh mesh = null;
            map.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
            map.AddComponent<MeshRenderer>().material = tileMaterial;

            MeshData data = GetMeshData(positions);

            mesh.vertices = data.vertices;
            mesh.triangles = data.triangles;

            mesh.RecalculateNormals();
        }

        /// <summary>
        /// Calculates all of values of the mesh and returns them in a container
        /// named MeshData.
        /// </summary>
        /// <param name="tiles">All of the tiles for the mesh to generate.</param>
        /// <returns>All of the mesh data in a container.</returns>
        private MeshData GetMeshData(Vector3[,] tiles)
        {
            int xSize = tilePositions.GetLength(0);
            int ySize = tiles.GetLength(1);

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<GetQuadData> quadsToDraw = new List<GetQuadData>();

            int v = 0;
            int t = 0;

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    // New List of delegates to store the quads that need to be
                    // drawn on each tile.
                    quadsToDraw = new List<GetQuadData>();
                    Vector3[] corners = GetVerticesFromCentre(tilePositions[x, y]);

                    string adjacency = GetIfAdjacentTilesAreEqualHeight(x, y);
                    Debug.Log(adjacency);

                    // Add the top faces to the queue
                    quadsToDraw.Add(CubeMesh.AddTopFace);
                    quadsToDraw.Add(CubeMesh.AddBottomFace);

                    // 1 Refers to a tile being on a different height
                    // 0 Refers to a tile being on the same height
                    bool north = true;
                    bool east = true;
                    bool south = true;
                    bool west = true;

                    if (north)
                        quadsToDraw.Add(CubeMesh.AddNorthFace);

                    if (east)
                        quadsToDraw.Add(CubeMesh.AddEastFace);

                    if (south)
                        quadsToDraw.Add(CubeMesh.AddSouthFace);

                    if (west)
                        quadsToDraw.Add(CubeMesh.AddWestFace);


                    // Runs the queue of quads to be drawn
                    for (int i = 0; i < quadsToDraw.Count; i++)
                    {
                        Quad q = quadsToDraw[i].Invoke(corners, v, tileYSize);

                        // Inserting the new quad with its vertices and triangle
                        vertices.AddRange(q.vertices);
                        triangles.AddRange(q.triangles);

                        // Increasing the vertex and triangle amount of a quad
                        v += 4;
                        t += 6;
                    }    

                }
            }

            return new MeshData(vertices.ToArray(), triangles.ToArray());
        }

        

        /// <summary>
        /// Essentially the bounding box of the tile. Gets the corners which
        /// will act as the vertices for the quad on the mesh.
        /// </summary>
        /// <param name="centrePos">The centre position of the tile.</param>
        /// <returns>All four corners of the tile.</returns>
        private Vector3[] GetVerticesFromCentre(Vector3 centrePos)
        {
            Vector3[] corners = new Vector3[4];
            float halfXSize = tileXSize * .5f;
            float halfYSize = tileYSize * .5f;

            corners[0] = centrePos + new Vector3(-halfXSize, 0.0f, -halfYSize); // Bottom Left
            corners[1] = centrePos + new Vector3(-halfXSize, 0.0f, halfYSize); // Top Left
            corners[2] = centrePos + new Vector3(halfXSize, 0.0f, -halfYSize); // Bottom Right
            corners[3] = centrePos + new Vector3(halfXSize, 0.0f, halfYSize); // Top Right

            return corners;
        }

        /// <summary>
        /// Gets adjacent tiles from the North, East, South and West of the tile
        /// provided by the index. It then returns a string that designates if
        /// it needs to draw a face, or not.
        /// </summary>
        /// <param name="x">The x index for the tile.</param>
        /// <param name="y">The y index for the tile.</param>
        /// <returns>
        /// Designates if a face should be draw in the four directions.
        /// One denotes that a face should be draw in that direction and a zero
        /// denotes that a face should not be drawn as they are on the same height.
        /// </returns>
        private string GetIfAdjacentTilesAreEqualHeight(int x, int y)
        {
            int width = tilePositions.GetLength(0);
            int height = tilePositions.GetLength(1);

            // 1 Refers to a tile being on a different height and **should** draw the face
            // 0 Refers to a tile being on the same height and **shouldn't** draw the face
            // Goes North, East, South and West
            // Loop also goes in this order
            char[] adjacency = { '0', '0', '0', '0' };
            for (int i = -1, index = 0; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Making sure it is within the array bounds
                    // Making sure exclude corners with the abs check
                    if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height || Mathf.Abs(i) == Mathf.Abs(j))
                        continue;

                    // Tile adjacent is on a different height
                    if (tilePositions[x, y].y != tilePositions[x + i, y + j].y)
                    {
                        adjacency[index] = '1';
                        index++;
                    }

                    // Tile adjacent is equal height
                    else
                        adjacency[index] = '0';

                }
            }

            return new string(adjacency);
        }

        #region Noise Map Reading

        /// <summary>
        /// Gets the world positions for the tile positions which 
        /// will be the centre of each quad for the mesh generation.
        /// You can also use this data for the gameplay logic.
        /// </summary>
        /// <param name="texture">The texture to read from.</param>
        /// <returns>A 2D array of the centre points for each quad.</returns>
        private Vector3[,] GetPositionsFromTexture(Texture2D texture)
        {
            Vector3 maxWorldSize = new Vector3(texture.width * tileXSize, 0.0f, texture.height * tileYSize);
            Vector3[,] positions = new Vector3[texture.width, texture.height];
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    // Setting the initial position taking into account the tile size
                    Vector3 position = new Vector3(x * tileXSize, GetHeightFromPixel(x, y, texture, noiseMapScalar), y * tileYSize);
                    position.y = Mathf.Round(position.y / roundedYPos) * roundedYPos;

                    // Offsetting the position by its max size - a tile halved
                    // This makes (0, 0, 0) the world centre of the object
                    position -= (maxWorldSize - new Vector3(tileXSize, 0.0f, tileYSize)) * .5f;
                    positions[x, y] = position;
                }
            }
            return positions;
        }

        /// <summary>
        /// Gets the height of the pixel multiplied by a scalar.
        /// A black will be a 0 and 1 will be a white.
        /// </summary>
        /// <param name="x">Pixel Coordinate at X.</param>
        /// <param name="y">Pixel Coordinate at Y.</param>
        /// <param name="texture">Texture to read from.</param>
        /// <param name="scalar">Scalar to affect the height returned from the 0-1 scale.</param>
        /// <returns>The height of the pixel read from the texture with a scalar.</returns>
        private float GetHeightFromPixel(int x, int y, Texture2D texture, float scalar)
        {
            // The colour doesn't matter as it is greyscale, 
            // so R, G or B will all be the same values
            Color colour = texture.GetPixel(x, y);
            return colour.r * scalar;
        }
        #endregion
    }
}