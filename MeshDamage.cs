using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class MeshDamage : AircraftDamage
    {
        [Header("Mesh Damage Area")]
        public GameObject DamageObject;
        public GameObject CustomObject;
        public bool IsAvailable => DamageObject != null && CustomObject != null;
        protected void SetDamagedMesh()
        {
            DamageObject.ActivateObject();
        }
        protected void ResetDamagedMesh()
        {
            CustomObject.DeactivateObject();
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

        public override void ResetDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Reset mesh damage is not possible.", gameObject);
                return;
            }

            ResetDamagedMesh();
        }

        public override void HoverDamageEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void HoverDamageExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
