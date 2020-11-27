//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using UnityEngine;

// Source for the Triangles and Vertex assignment:
// https://www.youtube.com/watch?v=8PlpCbxB6tY
namespace VFX.MeshGeneration
{
    /// <summary>
    /// Handles all of the functionality for drawing a cube base on the corners provided
    /// and the order of the vertices which are used to calculate the triangles.
    /// **NOTE:** 
    /// - The centre point (pivot) of the cube in this scenario is the centre of the top
    /// face.
    /// - North, East, South and West refer to Vector3.Foward, Vector3.Right, 
    /// Vector3.Backwards, and Vector3.Left respectively in world space.
    /// - Up and Down refers to Vector.up and Vector3.down.
    /// </summary>
    public static class CubeMesh
    {
        /// <summary>
        /// Calculates the vertices and triangles of the top quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddTopFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            vt.vertices = corners;
            vt.triangles = GetTrianglesUp(v);

            return vt;
        }

        /// <summary>
        /// Calculates the vertices and triangles of the bottom quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddBottomFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            // Moves the corners down by the height
            for (int i = 0; i < vt.vertices.Length; i++)
            {
                Vector3 corner = corners[i];
                corner.y -= tileHeight;
                vt.vertices[i] = corner;
            }

            vt.triangles = GetTrianglesDown(v);

            return vt;
        }

        /// <summary>
        /// Calculates the vertices and triangles of the north quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddNorthFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            Vector3 topLeft = corners[1];
            Vector3 topRight = corners[3];
            Vector3 bottomLeft = new Vector3(topLeft.x, topLeft.y - tileHeight, topLeft.z);
            Vector3 bottomRight = new Vector3(topRight.x, topRight.y - tileHeight, topRight.z);

            vt.vertices[0] = bottomLeft;
            vt.vertices[1] = topLeft;
            vt.vertices[2] = bottomRight;
            vt.vertices[3] = topRight;

            vt.triangles = GetTrianglesDown(v);

            return vt;
        }

        /// <summary>
        /// Calculates the vertices and triangles of the south quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddSouthFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            Vector3 topLeft = corners[0];
            Vector3 topRight = corners[2];
            Vector3 bottomLeft = new Vector3(topLeft.x, topLeft.y - tileHeight, topLeft.z);
            Vector3 bottomRight = new Vector3(topRight.x, topRight.y - tileHeight, topRight.z);

            vt.vertices[0] = bottomLeft;
            vt.vertices[1] = topLeft;
            vt.vertices[2] = bottomRight;
            vt.vertices[3] = topRight;

            vt.triangles = GetTrianglesUp(v);

            return vt;
        }

        /// <summary>
        /// Calculates the vertices and triangles of the east quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddEastFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            Vector3 bottomLeft = corners[2];
            Vector3 topLeft = corners[3];
            Vector3 bottomRight = new Vector3(bottomLeft.x, bottomLeft.y - tileHeight, bottomLeft.z);
            Vector3 topRight = new Vector3(topLeft.x, topLeft.y - tileHeight, topLeft.z);

            vt.vertices[0] = bottomLeft;
            vt.vertices[1] = topLeft;
            vt.vertices[2] = bottomRight;
            vt.vertices[3] = topRight;

            vt.triangles = GetTrianglesUp(v);

            return vt;
        }

        /// <summary>
        /// Calculates the vertices and triangles of the west quad depending on the
        /// position of it and the current iteration of the total mesh.
        /// </summary>
        /// <param name="corners">The corners of the tile which act as the vertices.</param>
        /// <param name="v">The current vertex position in the entire grid.</param>
        /// <param name="tileHeight">The height of the tile.</param>
        /// <returns>The Quad data for the top face.</returns>
        public static Quad AddWestFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            Vector3 bottomLeft = corners[0];
            Vector3 topLeft = corners[1];
            Vector3 bottomRight = new Vector3(bottomLeft.x, bottomLeft.y - tileHeight, bottomLeft.z);
            Vector3 topRight = new Vector3(topLeft.x, topLeft.y - tileHeight, topLeft.z);

            vt.vertices[0] = bottomLeft;
            vt.vertices[1] = topLeft;
            vt.vertices[2] = bottomRight;
            vt.vertices[3] = topRight;

            vt.triangles = GetTrianglesDown(v);

            return vt;
        }

        /// <summary>
        /// Calculates the triangles facing upwards based on the vertex position.
        /// </summary>
        /// <param name="v">The current vertex position of the mesh grid.</param>
        /// <returns>
        /// The triangle order for two triangles facing upwards to
        /// make a quad.
        /// </returns>
        private static int[] GetTrianglesUp(int v)
        {
            int[] triangles = new int[6];

            // Regular Triangle Order
            // Faces upwards
            triangles[0] = v;
            triangles[1] = triangles[4] = v + 1;
            triangles[2] = triangles[3] = v + 2;
            triangles[5] = v + 3;

            return triangles;
        }

        /// <summary>
        /// Calculates the triangles facing downwards based on the vertex position.
        /// </summary>
        /// <param name="v">The current vertex position of the mesh grid.</param>
        /// <returns>
        /// The triangle order for two triangles facing downwards to
        /// make a quad.
        /// </returns>
        private static int[] GetTrianglesDown(int v)
        {
            int[] triangles = new int[6];

            // Reversed Triangle Order
            // Faces Downwards
            triangles[0] = v + 2;
            triangles[1] = v + 1;
            triangles[2] = v;
            triangles[3] = v + 3;
            triangles[4] = v + 1;
            triangles[5] = v + 2;

            return triangles;
        }
    }
}