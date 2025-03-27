using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Module7
{
    [System.Serializable]
    public class DisplacedObjectSetup : PropertyDrawer
    {
        public string IssueNameID;
        public string IssueNotMarkedInfoID;
        public string IssueMarkedInfoID;

        public GameObject SetupObject;
        public Mesh DisplacedMesh;
        public List<GameObject> AdditionalDispalcedObjects = new List<GameObject>();
        public List<GameObject> AdditionalCustomObjects = new List<GameObject>();
    }
    [System.Serializable]
    public class PlaneDataToSetup
    {
        public GameObject NewSourceObject;
        public string DamageSegmentClearErrorID;
        public string DamageSegmentAreaID;
        public string DamageSegmentDescriptionAreaID;

        public List<GameObject> CustomDamageList = new List<GameObject>();
        public List<DisplacedObjectSetup> DisplacedObjectSetups = new List<DisplacedObjectSetup>();
    }

    public class BaseSetup : MonoBehaviour
    {
        [Space]
        public GameObject LineRenderer;
        public GameObject IssueCanvas;
        [Space]
        public DamageManager DamageManager;

        protected DamageSegment DamageSegmentSetup(PlaneDataToSetup planeDataToSetup)
        {
            GameObject gameobjectToSetupAsDamageSegment = planeDataToSetup.NewSourceObject;

            GameObject createdLineRendererObject = PrefabUtility.InstantiatePrefab(LineRenderer) as GameObject;
            GameObject createdIssueCanvas = PrefabUtility.InstantiatePrefab(IssueCanvas) as GameObject;
            createdLineRendererObject.transform.SetParent(gameobjectToSetupAsDamageSegment.transform);
            createdIssueCanvas.transform.SetParent(gameobjectToSetupAsDamageSegment.transform);
            gameobjectToSetupAsDamageSegment.name = $"{gameobjectToSetupAsDamageSegment.name}_DamageSegment";
            XRSimpleInteractable xrSimpleInteractable = gameobjectToSetupAsDamageSegment.AddComponent<XRSimpleInteractable>();

            DamageSegment damageSegment = gameobjectToSetupAsDamageSegment.AddComponent<DamageSegment>();
            damageSegment.SourceObject = gameobjectToSetupAsDamageSegment;
            damageSegment.DamageManager = DamageManager;
            damageSegment.LineRenderer = createdLineRendererObject.GetComponent<LineRenderer>();
            damageSegment.IssueCanvasObject = createdIssueCanvas;
            damageSegment.StartPoint = createdIssueCanvas.transform.GetChild(0).transform.GetChild(0).transform;
            damageSegment.EndPoint = LineRenderer.transform.GetChild(0).transform;
            damageSegment.XRSimpleInteractable = xrSimpleInteractable;
            damageSegment.ClearErrorTextID = planeDataToSetup.DamageSegmentClearErrorID;
            damageSegment.AreaTextID = planeDataToSetup.DamageSegmentAreaID;
            damageSegment.DescriptionAreaTextID = planeDataToSetup.DamageSegmentDescriptionAreaID;

            return damageSegment;
        }
        protected void CustomSetup(GameObject gameObjectToSetupAsCustomDamage, GameObject damageSegemntObject, out BaseDamage baseDamage)
        {
            gameObjectToSetupAsCustomDamage.name = $"{gameObjectToSetupAsCustomDamage.name}_CustomDamage";

            CustomDamage customDamage = gameObjectToSetupAsCustomDamage.AddComponent<CustomDamage>();
            customDamage.SourceObject = gameObjectToSetupAsCustomDamage;
            customDamage.DamageSegmentObject = damageSegemntObject;

            baseDamage = customDamage;
        }
        protected void DisplacedSetup(DisplacedObjectSetup displacedObjectSetup, GameObject damageSegemntObject,  Mesh mesh, out DisplacedDamage displacedDamage, out GameObject createdDisplacedObject)
        {
            GameObject gameObjectToSetupAsDisplacedDamage = displacedObjectSetup.SetupObject;
            string displacedObjectName = $"{gameObjectToSetupAsDisplacedDamage.name}_DispalcedDamage";
            GameObject existingObject = GameObject.Find(displacedObjectName);

            if (existingObject == null)
            {
                // creating a displace object
                createdDisplacedObject = Instantiate(gameObjectToSetupAsDisplacedDamage);
                createdDisplacedObject.name = $"{gameObjectToSetupAsDisplacedDamage.name}_DispalcedDamage";

                Transform createdDisplacedObjectTransform = createdDisplacedObject.transform;
                createdDisplacedObjectTransform.SetParent(gameObjectToSetupAsDisplacedDamage.transform);
                createdDisplacedObjectTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                createdDisplacedObjectTransform.localPosition = new Vector3(0f, 0f, 0f);
                createdDisplacedObjectTransform.localScale = new Vector3(1f, 1f, 1f);
                createdDisplacedObject.AddComponent<MeshCollider>();

                MeshFilter meshFilter = createdDisplacedObject.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                displacedDamage = createdDisplacedObject.AddComponent<DisplacedDamage>();
                displacedDamage.DamageSegmentObject = damageSegemntObject;
                displacedDamage.SourceObject = createdDisplacedObject;
                displacedDamage.AdditionalCustomObjects = displacedObjectSetup.AdditionalCustomObjects;
                displacedDamage.AdditionalDisplacedObjects = displacedObjectSetup.AdditionalDispalcedObjects;
                displacedDamage.IssueNameID = displacedObjectSetup.IssueNameID;
                displacedDamage.IssueMarkedInfoID = displacedObjectSetup.IssueMarkedInfoID;
                displacedDamage.IssueNotMarkedInfoID = displacedObjectSetup.IssueNotMarkedInfoID;
            }
            else
            {
                displacedDamage = existingObject.GetComponent<DisplacedDamage>();
                createdDisplacedObject = existingObject;
                Debug.Log("The displaced object already exist in the scene.", existingObject);
            }

        }
        protected bool DamageSegmentExist(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out DamageSegment damageSegment) && damageSegment != null && damageSegment.SourceObject == gameObject) return true;

            return false;
        }
        protected bool BaseDamageExist(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out BaseDamage baseDamage) && baseDamage != null && baseDamage.SourceObject == gameObject) return true;

            return false;
        }
    }
}