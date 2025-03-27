using Module7;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneSetup))]
public class PlaneSetupEditor : Editor
{
    private SerializedProperty planeDataToSetupList;
    private SerializedProperty lineRenderer;
    private SerializedProperty issueCanvas;
    private SerializedProperty damageManager;

    private bool foldout = true;

    private void OnEnable()
    {
        planeDataToSetupList = serializedObject.FindProperty("PlaneDataToSetupList");
        lineRenderer = serializedObject.FindProperty("LineRenderer");
        issueCanvas = serializedObject.FindProperty("IssueCanvas");
        damageManager = serializedObject.FindProperty("DamageManager");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Plane Setup", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Damage Segment Elements for Plane Setup", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(lineRenderer);
        EditorGUILayout.PropertyField(issueCanvas);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Managers for Base Setup", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(damageManager);

        foldout = EditorGUILayout.Foldout(foldout, "Plane Data List", true);
        if (foldout)
        {
            EditorGUI.indentLevel++;

            if (planeDataToSetupList.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No Plane Data added!", MessageType.Warning);
            }

            for (int i = 0; i < planeDataToSetupList.arraySize; i++)
            {
                SerializedProperty element = planeDataToSetupList.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent($"Plane Data {i + 1}"), true);

                if (GUILayout.Button("Remove Plane Data"))
                {
                    planeDataToSetupList.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add New Plane Data"))
            {
                planeDataToSetupList.InsertArrayElementAtIndex(planeDataToSetupList.arraySize);
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
