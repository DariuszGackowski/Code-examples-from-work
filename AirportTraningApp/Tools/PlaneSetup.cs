using AllExtensions;
using System.Collections.Generic;
using UnityEngine;

namespace Module7
{
    public class PlaneSetup : BaseSetup
    {
        [Header("Plane Setup")]
        [Space]
        public List<PlaneDataToSetup> PlaneDataToSetupList;

        [ContextMenu("Make plane setup")]
        private void Setup()
        {
            foreach (PlaneDataToSetup planeDataToSetup in PlaneDataToSetupList)
            {
                DamageSegmentsSetup(planeDataToSetup);
                CustomDamageSetup(planeDataToSetup);
                DisplacedDamageSetup(planeDataToSetup);
            }
        }
        private void GetAllChildrenRecursive(Transform parent, List<Transform> childrenList)
        {
            foreach (Transform child in parent)
            {
                childrenList.Add(child);
                GetAllChildrenRecursive(child, childrenList);
            }
        }
        protected void DamageSegmentsSetup(PlaneDataToSetup planeDataToSetup)
        {
            if (planeDataToSetup == null)
            {
                Debug.LogError($"DamageSegment is null");
                return;
            }

            if (DamageSegmentExist(planeDataToSetup.NewSourceObject))
            {
                Debug.LogWarning($"{planeDataToSetup.NewSourceObject.name} exist", planeDataToSetup.NewSourceObject);
                return;
            }
            DamageSegmentSetup(planeDataToSetup);

            Debug.Log($"Added {planeDataToSetup.NewSourceObject.name} damageSegment");
        }
        private void CustomDamageSetup(PlaneDataToSetup planeDataToSetup)
        {
            foreach (GameObject gameObjectToSetupAsCustomDamage in planeDataToSetup.CustomDamageList)
            {
                bool allValid = planeDataToSetup.CustomDamageList.Count > 0 && planeDataToSetup.NewSourceObject != null;

                if (!allValid)
                {
                    Debug.LogError($"Some element is not added correct or CustomDamageList is empty or DamageSegment is null");
                    return;
                }

                if (BaseDamageExist(gameObjectToSetupAsCustomDamage))
                {
                    Debug.LogWarning($"{gameObjectToSetupAsCustomDamage.name} exist", gameObjectToSetupAsCustomDamage);
                    continue;
                }
                CustomSetup(gameObjectToSetupAsCustomDamage, planeDataToSetup.NewSourceObject, out BaseDamage baseDamage);

                DamageSegment damageSegment = planeDataToSetup.NewSourceObject.GetComponent<DamageSegment>();
                if (!damageSegment.BaseDamages.Contains(baseDamage))
                    damageSegment.BaseDamages.Add(baseDamage);

                if (!gameObjectToSetupAsCustomDamage.TryGetComponent(out MeshCollider mainObjectCollider))
                {
                    mainObjectCollider = gameObjectToSetupAsCustomDamage.AddComponent<MeshCollider>();
                }
                damageSegment.XRSimpleInteractable.AddCollider(mainObjectCollider);
                damageSegment.AddHoverObject(gameObjectToSetupAsCustomDamage);

                Debug.Log($"Added {gameObjectToSetupAsCustomDamage.name} as CustomDamage");
            }
        }
        private void DisplacedDamageSetup(PlaneDataToSetup planeDataToSetup)
        {
            foreach (DisplacedObjectSetup displacedObjectSetup in planeDataToSetup.DisplacedObjectSetups)
            {
                bool allValid = displacedObjectSetup.SetupObject != null && displacedObjectSetup.DisplacedMesh != null;

                if (!allValid)
                {
                    Debug.LogError($"Some element is not added correct or DisplacedDamageList is empty");
                    return;
                }

                if (BaseDamageExist(displacedObjectSetup.SetupObject))
                {
                    Debug.LogWarning($"{displacedObjectSetup.SetupObject.name} exist", displacedObjectSetup.SetupObject);
                    continue;
                }

                DisplacedSetup(displacedObjectSetup, planeDataToSetup.NewSourceObject, displacedObjectSetup.DisplacedMesh, out DisplacedDamage displacedDamage,out GameObject createdDisplacedObject);
                CustomSetup(displacedObjectSetup.SetupObject, planeDataToSetup.NewSourceObject, out BaseDamage baseDamage);

                DamageSegment damageSegment = planeDataToSetup.NewSourceObject.GetComponent<DamageSegment>();

                if (!damageSegment.BaseDamages.Contains(baseDamage))
                    damageSegment.BaseDamages.Add(baseDamage);

                if (!damageSegment.BaseDamages.Contains(displacedDamage))
                    damageSegment.BaseDamages.Add(displacedDamage);

                if (!displacedObjectSetup.SetupObject.TryGetComponent(out MeshCollider mainObjectCollider))
                {
                    mainObjectCollider = displacedObjectSetup.SetupObject.AddComponent<MeshCollider>();
                }
                damageSegment.XRSimpleInteractable.AddCollider(mainObjectCollider);

                if (!createdDisplacedObject.TryGetComponent(out MeshCollider createdDisplacedObjectCollider))
                {
                    createdDisplacedObjectCollider = createdDisplacedObject.AddComponent<MeshCollider>();
                }
                damageSegment.XRSimpleInteractable.AddCollider(createdDisplacedObjectCollider);

                HoveredDisplacedPair hoveredDisplacedPair = new HoveredDisplacedPair(displacedObjectSetup.SetupObject, displacedDamage.SourceObject, displacedDamage);

                if (!damageSegment.HoveredDisplacedPairObjects.Contains(hoveredDisplacedPair))
                {
                    damageSegment.AddHoveredDisplacedPair(hoveredDisplacedPair);
                }

                Debug.Log($"Added {displacedObjectSetup.SetupObject.name} as Displaced Damage", displacedObjectSetup.SetupObject);
            }
        }
    }
}