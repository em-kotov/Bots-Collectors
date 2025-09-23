using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PearlSpawner))]
public class PearlGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        PearlSpawner pearlGenerator = (PearlSpawner)target;

        if (GUILayout.Button("Spawn 1 Pearl"))
        {
            pearlGenerator.SpawnSinglePearl();
        }
    }
}
