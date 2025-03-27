#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Module9
{
    [CustomEditor(typeof(SecurityThreatsSO))]

    public class SecurityThreatsEditor : Editor
    {
        private readonly float MediumButtonHeight = 30f;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Refresh table", GUILayout.Height(MediumButtonHeight)))
            {
                ((SecurityThreatsSO)target).BuildDatabase();
            }

            if (GUILayout.Button("Save", GUILayout.Height(MediumButtonHeight)))
            {
                EditorUtility.SetDirty((SecurityThreatsSO)target);
            }
        }
    }
}
#endif