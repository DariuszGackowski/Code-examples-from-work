using AllExtensions;
using UnityEngine;

namespace Module7
{
    public class AdditionalDamage : CustomDamage, IDamageName, DamageIssueText
    {
        [Header("Additional Damage Area")]
        [SerializeField] private string _issueNameID;
        [SerializeField] private string _issueNotMarkedInfoID;
        [SerializeField] private string _issueMarkedInfoID;

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
        public string IssueNameID
        {
            get => _issueNameID;
            set => _issueNameID = value;
        }

        public new bool IsAvailable => base.IsAvailable;

        public override void CheckDamageSetup()
        {
            base.CheckDamageSetup();
        }
        protected void SetDamaged()
        {
            SourceObject.ActivateObject();
        }
        protected void UnsetDamaged()
        {
            SourceObject.DeactivateObject();
        }
        public override void ApplyDamage()
        {
            base.ApplyDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Applying additional damage is not possible.", gameObject);
                return;
            }

            SetDamaged();
        }
        public override void ResetDamage()
        {
            base.ResetDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Reset additional damage is not possible.", gameObject);
                return;
            }

            UnsetDamaged();
        }
    }
}