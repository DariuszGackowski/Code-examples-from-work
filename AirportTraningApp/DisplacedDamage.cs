using AllExtensions;
using System.Collections.Generic;
using UnityEngine;

namespace Module7
{
    public class DisplacedDamage : BaseDamage, DamageIssueText, IDamageName
    {
        [Header("Displaced Damage Area")]
        [SerializeField]
        private string _issueNameID;
        [SerializeField]
        private string _issueNotMarkedInfoID;
        [SerializeField]
        private string _issueMarkedInfoID;
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

        public List<GameObject> AdditionalDisplacedObjects;
        public List<GameObject> AdditionalCustomObjects;
        public  new bool IsAvailable => SourceObject != null && base.IsAvailable;
        public bool IsDisplaced => SourceObject.activeSelf;

        public override void CheckDamageSetup()
        {
            if (IsAvailable) return;

            Debug.LogError($"Problems in Displaced Damage setup for:{gameObject.name}.", gameObject);
        }
        public void UnsetDamage()
        {
            SourceObject.DeactivateObject();
            SourceObject.ActivateObjectRenderer();
            SourceObject.ActivateObjectCollider();
            AdditionalCustomObjects.ForEach(additionalCustomObject => { additionalCustomObject.ActivateObjectRenderer();});
        }
        public void SetDamage()
        {
            SourceObject.ActivateObject();
            SourceObject.DeactivateObjectRenderer();
            SourceObject.DeactivateObjectCollider();
            AdditionalCustomObjects.ForEach(additionalCustomObject => { additionalCustomObject.DeactivateObjectRenderer();});
        }
        public override void ApplyDamage()
        {
            base.ApplyDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Apply displaced damage is not possible.", gameObject);
                return;
            }

            SetDamage();
        }
        public override void ResetDamage()
        {
            base.ResetDamage();

            if (!IsAvailable)
            {
                Debug.LogError("Reset displaced damage is not possible.", gameObject);
                return;
            }

            UnsetDamage();
        }
    }
}