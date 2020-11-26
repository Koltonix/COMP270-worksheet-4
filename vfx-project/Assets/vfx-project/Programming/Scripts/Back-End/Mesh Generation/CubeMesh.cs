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


            vt.triangles[0] = v;
            vt.triangles[1] = vt.triangles[4] = v + 1;
            vt.triangles[2] = vt.triangles[3] = v + 2;
            vt.triangles[5] = v + 3;

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


            vt.vertices[v] = bottomLeft; // Bottom Left
            vt.vertices[v + 1] = topLeft; // Top Left
            vt.vertices[v + 2] = bottomRight; // Bottom Right
            vt.vertices[v + 3] = topRight; // Top Right

            vt.triangles = GetTriangleOrder(v);

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
    }
}