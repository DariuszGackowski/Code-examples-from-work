using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module7
{
    public class AdditionalDamage : DamageBase
    {
        [Header("Additional Damage Area")]
        public GameObject CorrectObejct;
        public GameObject IncorrectObject;
        public Material IncorrectObjectMaterial;

        private const string _hoveredPropertyName = "_IsHovered";

        public bool IncorrectObjectMaterialIsAvailable => IncorrectObjectMaterial != null;
        public bool IsAvailable => IncorrectObject != null && CorrectObejct != null;

        protected void SetDamaged()
        {
            CorrectObejct.ActivateObject();
            IncorrectObject.DeactivateObject();
        }
        protected void UnsetDamaged()
        {
            CorrectObejct.DeactivateObject();
            IncorrectObject.ActivateObject();
        }
        protected void SetMaterialHover(bool value)
        {
            IncorrectObjectMaterial.SetBoolProperty(_hoveredPropertyName, boolValue: value);
        }
        public override void ApplyDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Applying additional damage is not possible.", gameObject);
                return;
            }

            SetDamaged();
        }
        public override void ResetDamage()
        {
            if (!IsAvailable)
            {
                Debug.LogError("Reset additional damage is not possible.", gameObject);
                return;
            }

            UnsetDamaged();
        }
        public override void HoverDamageEnter()
        {
            if (!IncorrectObjectMaterialIsAvailable)
            {
                Debug.LogError("Hover enter in additional damage is not possible.", gameObject);
                return;
            }

            SetMaterialHover(value: true);
        }

        public override void HoverDamageExit()
        {
            if (!IncorrectObjectMaterialIsAvailable)
            {
                Debug.LogError("Hover exit in additional damage is not possible.", gameObject);
                return;
            }

            SetMaterialHover(value: false);
        }
    }
}
