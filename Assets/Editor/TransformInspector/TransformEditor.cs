using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private Transform transform;

    private void OnEnable()
    {
        transform = target as Transform;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        transform.localPosition = EditorGUILayout.Vector3Field("Position", transform.localPosition);
        transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", transform.localEulerAngles);
        transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
        
        Undo.RecordObject(transform,"transform.position"); //记录object属性
        transform.position = EditorGUILayout.Vector3Field("World Position", transform.position);
        EditorGUILayout.EndVertical();
    }
}