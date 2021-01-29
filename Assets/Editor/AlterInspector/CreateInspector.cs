using UnityEditor;

[CustomEditor(typeof(FreeCamera))]
public class CreateInspector : Editor
{
    private FreeCamera cam;
    
    private SerializedProperty type;
    private SerializedProperty _target;
    private SerializedProperty _smoothTime;
    private SerializedProperty _speed;
    private SerializedProperty _mouseActiveRect;
    private SerializedProperty _wheelSpeed;
    private SerializedProperty _yMaxLimit;
    private SerializedProperty _yMinLimit;
    private SerializedProperty _maxDistance;
    private SerializedProperty _minDistance;

    void OnEnable()
    {
        _mouseActiveRect = serializedObject.FindProperty("mouseActiveRect");
        _smoothTime = serializedObject.FindProperty("smoothTime");
        _speed = serializedObject.FindProperty("speed");
        _target = serializedObject.FindProperty("target");
        _wheelSpeed = serializedObject.FindProperty("wheelSpeed");
        _yMaxLimit = serializedObject.FindProperty("yMaxLimit");
        _yMinLimit = serializedObject.FindProperty("yMinLimit");
        _maxDistance = serializedObject.FindProperty("maxDistance");
        _minDistance = serializedObject.FindProperty("minDistance");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        cam = (FreeCamera) target;
        cam.targetType = (TargetType) EditorGUILayout.EnumPopup("Type-", cam.targetType);
        if (cam.targetType == TargetType.Exist)
        {
            EditorGUILayout.PropertyField(_target);
            CreateField();
        }
        else
        {
            CreateField();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    
    void CreateField()
    {
        EditorGUILayout.PropertyField(_mouseActiveRect);
        EditorGUILayout.PropertyField(_smoothTime);
        EditorGUILayout.PropertyField(_speed);
        EditorGUILayout.PropertyField(_wheelSpeed);
        EditorGUILayout.PropertyField(_yMaxLimit);
        EditorGUILayout.PropertyField(_yMinLimit);
        EditorGUILayout.PropertyField(_maxDistance);
        EditorGUILayout.PropertyField(_minDistance);
    }
}