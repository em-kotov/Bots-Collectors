using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fort))]
public class FortEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        Fort fort = (Fort)target;

        if (GUILayout.Button("Spawn 1 Bot"))
        {
            fort.SpawnSingleBot();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Remove Flag"))
        {
            fort.RemoveFlag();
        }
    }
}
