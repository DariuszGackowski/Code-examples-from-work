using Module7;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DisplacedObjectSetup))]
public class DisplacedObjectSetupDrawer : PropertyDrawer
{
    private bool foldout = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return foldout ? EditorGUIUtility.singleLineHeight * 7 : EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldout, label, true);
        if (foldout)
        {
            EditorGUI.indentLevel++;

            SerializedProperty issueNameID = property.FindPropertyRelative("IssueNameID");
            SerializedProperty issueNotMarkedInfoID = property.FindPropertyRelative("IssueNotMarkedInfoID");
            SerializedProperty issueMarkedInfoID = property.FindPropertyRelative("IssueMarkedInfoID");
            SerializedProperty setupObject = property.FindPropertyRelative("SetupObject");
            SerializedProperty displacedMesh = property.FindPropertyRelative("DisplacedMesh");
            SerializedProperty additionalDisplacedObjects = property.FindPropertyRelative("AdditionalDispalcedObjects");
            SerializedProperty additionalCustomObjects = property.FindPropertyRelative("AdditionalCustomObjects");

            EditorGUILayout.PropertyField(issueNameID);
            EditorGUILayout.PropertyField(issueNotMarkedInfoID);
            EditorGUILayout.PropertyField(issueMarkedInfoID);
            EditorGUILayout.PropertyField(setupObject);
            EditorGUILayout.PropertyField(displacedMesh);
            EditorGUILayout.PropertyField(additionalDisplacedObjects, true);
            EditorGUILayout.PropertyField(additionalCustomObjects, true);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}