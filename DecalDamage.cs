using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class DecalDamage : AircraftDamage
    {
        [Header("Decal Damage Area")]
        public GameObject DecalObject;
        public Material DecalMaterial;

        private const string _hoveredPropertyName = "_IsHovered";
        public bool IsAvailable => DecalObject != null;
        public bool DecalMaterialIsAvailable => DecalMaterial != null;
        protected void SetDamagedDecal()
        {
            DecalObject.ActivateObject();
        }
        protected void UnsetDamagedDecal()
        {
            DecalObject.DeactivateObject();
        }
        protected void SetMaterialHover(bool value)
        {
            DecalMaterial.SetBoolProperty(_hoveredPropertyName, boolValue: value);
        }
        public override void ApplyDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Applying decal damage is not possible.", gameObject);
                return;
            }

            SetDamagedDecal();
        }
        public override void ResetDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Reset decal damage is not possible.", gameObject);
                return;
            }

            UnsetDamagedDecal();
        }

        public override void HoverDamageEnter()
        {
            if (!DecalMaterialIsAvailable)
            {
                Debug.LogError("Hover enter in decal damage is not possible.", gameObject);
                return;
            }

            SetMaterialHover(value: true);
        }

        public override void HoverDamageExit()
        {
            if (!DecalMaterialIsAvailable)
            {
                Debug.LogError("Hover exit in decal damage is not possible.", gameObject);
                return;
            }

            SetMaterialHover(value: false);
        }
    }
}
