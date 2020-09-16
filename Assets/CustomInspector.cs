using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TerrainGenerator terrainGenerator = (TerrainGenerator)target;
        if (GUILayout.Button("Generate Terrain"))
        {
            terrainGenerator.GenerateTerrain();
        }
    }
}
