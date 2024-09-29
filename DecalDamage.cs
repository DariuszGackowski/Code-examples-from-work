using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class DecalDamage : DamageBase
    {
        [Header("Decal Damage Area")]
        public GameObject DecalObject;
        public bool IsAvailable => DecalObject != null;
        protected void SetDamagedDecal()
        {
            DecalObject.ActivateObject();
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
    }
}
