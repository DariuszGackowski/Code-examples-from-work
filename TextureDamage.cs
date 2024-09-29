using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class TextureDamage : DamageBase
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
        public override void ApplyDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Applying texture damage is not possible.", gameObject);
                return;
            }

            SetDamagedMaterial();
        }
    }
}
