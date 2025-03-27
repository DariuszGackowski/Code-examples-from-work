using AllExtensions;
using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public class DecalDamage : BaseDamage, DamageIssueText, IDamageName
    {
        [Header("Decal Damage Area")]
        public GameObject DecalObject;

        [SerializeField] private string _issueNameID;
        [SerializeField] private string _issueNotMarkedInfoID;
        [SerializeField] private string _issueMarkedInfoID;
        public new bool IsAvailable => DecalObject != null &&  base.IsAvailable;

        public string IssueNameID
        {
            get => _issueNameID;
            set => _issueNameID = value;
        }
        public string IssueNotMarkedInfoID
        {
            get => _issueNotMarkedInfoID;
            set => _issueNotMarkedInfoID = value;
        }
        public string IssueMarkedInfoID
        {
            get => _issueMarkedInfoID;
            set => _issueMarkedInfoID = value;
        }

        public override void CheckDamageSetup()
        {
            if (IsAvailable) return;

            Debug.LogError($"Problems in Decal Damage setup for:{DecalObject.name}.", DecalObject);
        }
        protected void SetDamaged()
        {
            DecalObject.ActivateObject();
        }
        protected void UnsetDamaged()
        {
            DecalObject.DeactivateObject();
        }
        public override void ApplyDamage()
        {
            base.ApplyDamage();

            if (!IsAvailable )
            {
                Debug.LogError("Applying decal damage is not possible.", DecalObject);
                return;
            }

            SetDamaged();
        }
        public override void ResetDamage()
        {
            base.ResetDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Reset decal damage is not possible.", DecalObject);
                return;
            }

            UnsetDamaged();
        }
    }
}