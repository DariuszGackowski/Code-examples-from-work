using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class MeshDamage : DamageBase
    {
        [Header("Mesh Damage Area")]
        public GameObject DamageObject;
        public GameObject CustomObject;
        public bool IsAvailable => DamageObject != null && CustomObject != null;
        protected void SetDamagedMesh()
        {
            CustomObject.DeactivateObject();
            DamageObject.ActivateObject();
        }
        public override void ApplyDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Applying mesh damage is not possible.", gameObject);
                return;
            }

            SetDamagedMesh();
        }
    }
}
