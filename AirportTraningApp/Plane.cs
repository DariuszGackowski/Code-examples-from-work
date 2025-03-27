using System.Collections.Generic;
using UnityEngine;
using static Module7.DamageManager;

namespace Module7
{
    public class Plane : MonoBehaviour
    {
        [Header("Damage Segment List")]
        [Space]
        public List<DamageSegment> DamageSegmentList = new List<DamageSegment>();
        [Header("Plane Setup")]
        [Space]
        public PlaneType PlaneType;
        public GameObject PlaneObject;

        public bool IsChosen => PlaneType == DamageSceneStarter.ChosenPlaneType;

        [ContextMenu("CheckDamageSegmentList")]
        public void CheckDamageSegmentList()
        {
            DamageSegmentList.ForEach(damageSegment =>
            {
                if (string.IsNullOrEmpty(damageSegment.AreaTextID) ||
                    string.IsNullOrEmpty(damageSegment.ClearErrorTextID) ||
                    string.IsNullOrEmpty(damageSegment.DescriptionAreaTextID))
                {
                    Debug.LogError("Error here", damageSegment as Object);
                }

                damageSegment.BaseDamages.ForEach(baseDamage =>
                {

                    if (baseDamage is AdditionalDamage)
                    {
                        AdditionalDamage additionalDamage = baseDamage as AdditionalDamage;

                        if (string.IsNullOrEmpty(additionalDamage.IssueMarkedInfoID) || string.IsNullOrEmpty(additionalDamage.IssueNotMarkedInfoID) || string.IsNullOrEmpty(additionalDamage.IssueNameID))
                        {
                            Debug.LogError("Error here", additionalDamage as Object);
                        }
                    }

                    if (baseDamage is DecalDamage && baseDamage is not MarkedDecalDamage)
                    {
                        DecalDamage decalDamage = baseDamage as DecalDamage;

                        if (string.IsNullOrEmpty(decalDamage.IssueMarkedInfoID) || string.IsNullOrEmpty(decalDamage.IssueNotMarkedInfoID))
                        {
                            Debug.LogError("Error here", decalDamage as Object);
                        }
                    }

                    if (baseDamage is MarkedDecalDamage)
                    {
                        MarkedDecalDamage markedDecalDamage = baseDamage as MarkedDecalDamage;

                        if (string.IsNullOrEmpty(markedDecalDamage.IssueMarkedInfoID) || string.IsNullOrEmpty(markedDecalDamage.IssueNotMarkedInfoID) || string.IsNullOrEmpty(markedDecalDamage.IssueOverMarkedInfoID))
                        {
                            Debug.LogError("Error here", markedDecalDamage as Object);
                        }
                    }

                    if (baseDamage is DisplacedDamage)
                    {
                        DisplacedDamage additionalDamage = baseDamage as DisplacedDamage;

                        if (string.IsNullOrEmpty(additionalDamage.IssueMarkedInfoID) || string.IsNullOrEmpty(additionalDamage.IssueNotMarkedInfoID) || string.IsNullOrEmpty(additionalDamage.IssueNameID))
                        {
                            Debug.LogError("Error here", additionalDamage as Object);
                        }
                    }

                });

                //foreach (BaseDamage baseDamage in damageSegment.BaseDamages)
                //{
                //    baseDamage.DamageSegmentObject = damageSegment.gameObject;
                //    Debug.Log("Set up", baseDamage.DamageSegmentObject);
                //}

                if (damageSegment.XRSimpleInteractable.colliders.Count == 0)
                {
                    Debug.LogError("Error here in collider count", damageSegment as Object);
                }

                foreach (Collider collider in damageSegment.XRSimpleInteractable.colliders)
                {
                    if (collider == null)
                    {
                        Debug.LogError("Error here in collider null", damageSegment as Object);
                    }
                }
            });

            Debug.Log("Plane checked", this.gameObject);
        }
    }
}