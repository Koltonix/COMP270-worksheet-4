using UnityEngine;
using UnityEditor;
using VFX.Noise;

namespace VFX.Tools
{
    [CustomEditor(typeof(PerlinNoiseGenerator))]
    public class PerlinNoiseGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PerlinNoiseGenerator generator = (PerlinNoiseGenerator)target;

            if (GUILayout.Button("Randomise Seed"))
                generator.RandomiseSeed();

            if (GUILayout.Button("Save new Perlin Noise Generator"))
                generator.CreateNewPerlinNoiseTexture();
        }
    }
}