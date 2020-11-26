using UnityEngine;

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

            vt.vertices[0] = bottomLeft;// Bottom Left
            vt.vertices[1] = topLeft; // Top Left
            vt.vertices[2] = bottomRight; // Bottom Right
            vt.vertices[3] = topRight; // Top Right

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

            vt.vertices[0] = bottomLeft;// Bottom Left
            vt.vertices[1] = topLeft; // Top Left
            vt.vertices[2] = bottomRight; // Bottom Right
            vt.vertices[3] = topRight; // Top Right

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

            vt.vertices[0] = bottomLeft; // Bottom Left
            vt.vertices[1] = topLeft; // Top Left
            vt.vertices[2] = bottomRight; // Bottom Right
            vt.vertices[3] = topRight; // Top Right

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

            vt.vertices[0] = bottomLeft; // Bottom Left
            vt.vertices[1] = topLeft; // Top Left
            vt.vertices[2] = bottomRight; // Bottom Right
            vt.vertices[3] = topRight; // Top Right

            vt.triangles = GetTriangleReversed(v);

            return vt;
        }

        private static int[] GetTriangleOrder(int v)
        {
            int[] triangles = new int[6];
            triangles[0] = v;
            triangles[1] = triangles[4] = v + 1;
            triangles[2] = triangles[3] = v + 2;
            triangles[5] = v + 3;

            return triangles;
        }

        private static int[] GetTriangleReversed(int v)
        {
            int[] triangles = new int[6];
            // 012 213
            //triangles[0] = 0;
            //triangles[1] = 1;
            //triangles[2] = 2;
            //triangles[3] = 2;
            //triangles[4] = 1;
            //triangles[5] = 3;

            //210 312
            //triangles[0] = 2;
            //triangles[1] = 1;
            //triangles[2] = 0;
            //triangles[3] = 3;
            //triangles[4] = 1;
            //triangles[5] = 2;

            triangles[0] = v + 2;
            triangles[1] = v + 1;
            triangles[2] = v;
            triangles[3] = v + 3;
            triangles[4] = v + 1;
            triangles[5] = v + 2;

            // v = 0
            // v + 1 = 1
            // v + 2 = 2
            // v + 3 = 3

            //triangles[0] = v;
            //triangles[1] = triangles[4] = v + 1;
            //triangles[2] = triangles[3] = v + 2;
            //triangles[5] = v + 3;


            return triangles;
        }
    }
}