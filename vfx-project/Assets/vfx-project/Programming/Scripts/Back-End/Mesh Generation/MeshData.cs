//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using UnityEngine;

namespace VFX.MeshGeneration
{
    /// <summary>
    /// Stores the mesh data generated.
    /// </summary>
    public struct MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;

        public MeshData(Vector3[] v, int[] t)
        {
            vertices = v;
            triangles = t;
        }
    }
}
