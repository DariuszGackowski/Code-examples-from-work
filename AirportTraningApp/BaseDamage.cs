using UnityEngine;

namespace Module7
{
    public interface IDamageMarkedText
    {
        public string IssueOverMarkedInfoID { get; set; }
    }
    public interface IDamageName
    {
        public string IssueNameID { get; set; }
    }
    public interface DamageIssueText
    {
        public string IssueNotMarkedInfoID { get; set; }
        public string IssueMarkedInfoID { get; set; }
    }
    [System.Serializable]
    public abstract class BaseDamage : MonoBehaviour
    {
        [Header("Base Damage Area")]
        public GameObject DamageSegmentObject;
        public GameObject SourceObject;
        public bool IsAvailable => DamageSegmentObject != null && SourceObject != null;
        public bool DamageIsActive => gameObject.activeSelf;
        public bool IsAdded;
        public virtual void ApplyDamage()
        {
            IsAdded = true;
        }
        public virtual void ResetDamage()
        {
            IsAdded = false;
        }
        public abstract void CheckDamageSetup();
    }
}