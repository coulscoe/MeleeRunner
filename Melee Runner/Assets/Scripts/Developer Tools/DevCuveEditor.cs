using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevCubeTool))]
public class DevCuveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DevCubeTool devCube = (DevCubeTool)target;

        if (GUILayout.Button("Generate Cube"))
        {
            devCube.GenerateCube();
        }
    }
}
