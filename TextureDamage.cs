using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class TextureDamage : AircraftDamage
    {
        [Header("Texture Damage Area")]
        public Material DamageMaterial;
        public Material CustomMaterial;
        public MeshRenderer MeshRenderer;
        public bool IsAvailable => DamageMaterial != null && CustomMaterial != null && MeshRenderer != null;
        protected void SetDamagedMaterial()
        {
            MeshRenderer.SetMaterial(DamageMaterial);
        }
        protected void ResetDamagedMaterial()
        {
            MeshRenderer.SetMaterial(CustomMaterial);
        }
        public override void ApplyDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Applying texture damage is not possible.", gameObject);
                return;
            }

            SetDamagedMaterial();
        }

        public override void ResetDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Reset texture damage is not possible.", gameObject);
                return;
            }

            ResetDamagedMaterial();
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
