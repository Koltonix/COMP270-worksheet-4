using UnityEngine;

// Source for the Triangles and Vertex assignment:
// https://www.youtube.com/watch?v=8PlpCbxB6tY
namespace VFX.MeshGeneration
{
    public class Quad
    {
        public Vector3[] vertices = new Vector3[4];
        public int[] triangles = new int[6];
    }

    public static class CubeMesh
    {
        public static Quad AddTopFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            for (int i = 0; i < vt.vertices.Length; i++)
                vt.vertices[i] = corners[i];

            vt.triangles = GetTriangleOrder(v);

            return vt;
        }

        public static Quad AddBottomFace(Vector3[] corners, int v, int tileHeight)
        {
            Quad vt = new Quad();

            for (int i = 0; i < vt.vertices.Length; i++)
            {
                Vector3 corner = corners[i];
                corner.y -= tileHeight;
                vt.vertices[i] = corner;
            }

            vt.triangles = GetTriangleReversed(v);

            return vt;
        }

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

            vt.triangles = GetTriangleReversed(v);

            return vt;
        }

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

            vt.triangles = GetTriangleOrder(v);

            return vt;
        }

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

            vt.triangles = GetTriangleOrder(v);

            return vt;
        }

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

            vt.triangles = GetTriangleReversed(v);

            return vt;
        }

        private static int[] GetTriangleOrder(int v)
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

        private static int[] GetTriangleReversed(int v)
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