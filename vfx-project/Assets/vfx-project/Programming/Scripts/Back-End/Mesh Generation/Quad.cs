//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using UnityEngine;

namespace VFX.MeshGeneration
{
    /// <summary>
    /// Stores the vertices and triangles of a quad.
    /// </summary>
    public class Quad
    {
        public Vector3[] vertices = new Vector3[4];
        public int[] triangles = new int[6];
    }
}