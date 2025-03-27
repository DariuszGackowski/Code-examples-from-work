using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Module7;

[CustomPropertyDrawer(typeof(PlaneDataToSetup))]
public class PlaneDataToSetupDrawer : PropertyDrawer
{
    private bool foldout = true;
    private Dictionary<int, bool> displacedObjectFoldouts = new Dictionary<int, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldout, label, true);
        if (foldout)
        {
            EditorGUI.indentLevel++;

            SerializedProperty newSourceObject = property.FindPropertyRelative("NewSourceObject");
            SerializedProperty damageSegmentClearErrorID = property.FindPropertyRelative("DamageSegmentClearErrorID");
            SerializedProperty damageSegmentAreaID = property.FindPropertyRelative("DamageSegmentAreaID");
            SerializedProperty damageSegmentDescriptionAreaID = property.FindPropertyRelative("DamageSegmentDescriptionAreaID");
            SerializedProperty customDamageList = property.FindPropertyRelative("CustomDamageList");
            SerializedProperty displacedObjectSetups = property.FindPropertyRelative("DisplacedObjectSetups");

            EditorGUILayout.PropertyField(newSourceObject);
            EditorGUILayout.PropertyField(damageSegmentClearErrorID);
            EditorGUILayout.PropertyField(damageSegmentAreaID);
            EditorGUILayout.PropertyField(damageSegmentDescriptionAreaID);
            EditorGUILayout.PropertyField(customDamageList, true);

            EditorGUILayout.LabelField("Displaced Object Setups", EditorStyles.boldLabel);

            if (displacedObjectSetups.isArray)
            {
                if (displacedObjectSetups.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("Brak elementów w liœcie!", MessageType.Info);
                }

                for (int i = 0; i < displacedObjectSetups.arraySize; i++)
                {
                    SerializedProperty element = displacedObjectSetups.GetArrayElementAtIndex(i);

                    if (!displacedObjectFoldouts.ContainsKey(i))
                        displacedObjectFoldouts[i] = true;  // Domyœlnie rozwiniête

                    displacedObjectFoldouts[i] = EditorGUILayout.Foldout(displacedObjectFoldouts[i], $"Displaced Object {i + 1}", true);

                    if (displacedObjectFoldouts[i])
                    {
                        EditorGUI.indentLevel++;

                        SerializedProperty issueNameID = element.FindPropertyRelative("IssueNameID");
                        SerializedProperty issueNotMarkedInfoID = element.FindPropertyRelative("IssueNotMarkedInfoID");
                        SerializedProperty issueMarkedInfoID = element.FindPropertyRelative("IssueMarkedInfoID");
                        SerializedProperty setupObject = element.FindPropertyRelative("SetupObject");
                        SerializedProperty displacedMesh = element.FindPropertyRelative("DisplacedMesh");
                        SerializedProperty additionalDisplacedObjects = element.FindPropertyRelative("AdditionalDispalcedObjects");
                        SerializedProperty additionalCustomObjects = element.FindPropertyRelative("AdditionalCustomObjects");

                        EditorGUILayout.PropertyField(issueNameID);
                        EditorGUILayout.PropertyField(issueNotMarkedInfoID);
                        EditorGUILayout.PropertyField(issueMarkedInfoID);
                        EditorGUILayout.PropertyField(setupObject);
                        EditorGUILayout.PropertyField(displacedMesh);
                        EditorGUILayout.PropertyField(additionalDisplacedObjects, true);
                        EditorGUILayout.PropertyField(additionalCustomObjects, true);

                        EditorGUI.indentLevel--;
                    }

                    if (GUILayout.Button($"Remove Displaced Object {i + 1}"))
                    {
                        displacedObjectSetups.DeleteArrayElementAtIndex(i);
                    }
                }

                if (GUILayout.Button("Add Displaced Object"))
                {
                    displacedObjectSetups.InsertArrayElementAtIndex(displacedObjectSetups.arraySize);
                }
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}