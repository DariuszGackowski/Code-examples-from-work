using UnityEditor;
using UnityEngine;

namespace Module7
{
    [CustomEditor(typeof(DamageHelper))]
    public class DamageHelperEditor : Editor
    {
        private const float _mediumButtonHeight = 25f;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DamageHelper damageHelper = (DamageHelper)target;

            EditorGUILayout.Space();

            #region Check damages
            if (GUILayout.Button("Check damage", GUILayout.Height(_mediumButtonHeight)))
            {
                damageHelper.CheckDamage();

                Debug.Log($"Damage is checked");
            }
            #endregion

            EditorGUILayout.Space();
        }
    }
}