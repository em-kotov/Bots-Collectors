using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PearlGenerator))]
public class PearlGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        PearlGenerator pearlGenerator = (PearlGenerator)target;

        if (GUILayout.Button("Spawn 1 Pearl"))
        {
            pearlGenerator.SpawnSinglePearl();
        }
    }
}
