using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunController))]
public class GunControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GunController gunController = (GunController)target;
        if (GUILayout.Button("Fire")) gunController.Fire();
    }
}
