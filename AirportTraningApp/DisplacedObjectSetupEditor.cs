#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Module7
{
    [CustomPropertyDrawer(typeof(DisplacedObjectSetup))]
    public class DisplacedObjectSetupEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty setupObject = property.FindPropertyRelative("SetupObject");
            SerializedProperty displacedMesh = property.FindPropertyRelative("DisplacedMesh");

            EditorGUI.BeginProperty(position, label, property);

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect mainSetupObjectRect = new Rect(position.x, position.y, position.width, singleLineHeight);
            Rect selectedTypeRect = new Rect(position.x, position.y + singleLineHeight + spacing, position.width, singleLineHeight);

            EditorGUI.PropertyField(mainSetupObjectRect, setupObject, new GUIContent("Setup Object"));
            //EditorGUI.PropertyField(selectedTypeRect, selectedType, new GUIContent("Displaced Changes Type"));

            //    ObjectType type = (ObjectType)selectedType.enumValueIndex;
            //    Rect displacedSetupRect = new Rect(position.x, selectedTypeRect.y + singleLineHeight + spacing, position.width, singleLineHeight);

            //    if (type == ObjectType.Mesh)
            //    {
            //        EditorGUI.PropertyField(displacedSetupRect, displacedSetupMesh, new GUIContent("Displaced Setup Mesh"));
            //    }
            //    else if (type == ObjectType.Material)
            //    {
            //        EditorGUI.PropertyField(displacedSetupRect, displacedSetupMaterial, new GUIContent("Displaced Setup Material"));
            //    }
            //    else if (type == ObjectType.MeshAndMaterial)
            //    {
            //        Rect displacedSetupMeshRect = new Rect(position.x, displacedSetupRect.y, position.width, singleLineHeight);
            //        Rect displacedSetupMaterialRect = new Rect(position.x, displacedSetupMeshRect.y + singleLineHeight + spacing, position.width, singleLineHeight);

            //        EditorGUI.PropertyField(displacedSetupMeshRect, displacedSetupMesh, new GUIContent("Displaced Setup Mesh"));
            //        EditorGUI.PropertyField(displacedSetupMaterialRect, displacedSetupMaterial, new GUIContent("Displaced Setup Material"));

            //        displacedSetupRect = displacedSetupMaterialRect; // Update for consistent positioning
            //    }

            //    Rect damageSegmentRect = new Rect(position.x, displacedSetupRect.y + singleLineHeight + spacing, position.width, singleLineHeight);
            //    EditorGUI.PropertyField(damageSegmentRect, damageSegment, new GUIContent("Damage Segment"));

            //    EditorGUI.EndProperty();
            //}
            //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            //{
            //    SerializedProperty displacedChangeType = property.FindPropertyRelative("DisplacedChangesType");
            //    ObjectType type = (ObjectType)displacedChangeType.enumValueIndex;

            //    if (type == ObjectType.MeshAndMaterial)
            //    {
            //        return EditorGUIUtility.singleLineHeight * 5 + EditorGUIUtility.standardVerticalSpacing * 4;
            //    }
            //    else
            //    {
            //        return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
            //    }
            //}
        }
    }
}
#endif