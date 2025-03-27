using UnityEngine;

namespace Module7
{
    public class CustomDamage : BaseDamage
    {
        public override void CheckDamageSetup()
        {
            if (IsAvailable) return;

            Debug.LogError($"Problems in Custom Damage setup for:{gameObject.name}.", gameObject);
        }
        public virtual void UnsetDamage() 
        {
            //Do nothing
        }

        public virtual void SetDamage() 
        {
            //Do nothing
        }
        public override void ApplyDamage()
        {
            base.ApplyDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Apply custom damage is not possible.", gameObject);
                return;
            }

            SetDamage();
        }
        public override void ResetDamage()
        {
            base.ResetDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Reset custom damage is not possible.", gameObject);
                return;
            }

            UnsetDamage();
        }
    }
}