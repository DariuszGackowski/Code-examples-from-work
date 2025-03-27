using AllExtensions;
using CustomXR;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Module7
{
    public enum DamageArea
    {
        AirportApron,
        Nose,
        LeftFrontArea,
        LeftWingArea,
        LeftEngineArea,
        LeftLandingGear,
        LeftBackArea,
        PlaneTail,
        RightBackArea,
        RightLandingGear,
        RightEngineArea,
        RightWingArea,
        RightFrontArea,
        FrontLandingGear
    }
    [System.Serializable]
    public class HoveredDisplacedPair
    {
        public DisplacedDamage DisplacedDamage;
        public GameObject SourceObject;
        public GameObject DisplacedObject;

        public HoveredDisplacedPair(GameObject sourceObject, GameObject displacedObject, DisplacedDamage displacedDamage) 
        {
            SourceObject = sourceObject;
            DisplacedObject = displacedObject;
            DisplacedDamage = displacedDamage;
        }
    }
    public class DamageSegment : MonoBehaviour
    {
        [Header("Damage Segment Setup")]
        public XRSimpleInteractable XRSimpleInteractable;
        public DamageManager DamageManager;
        public GameObject SourceObject;
        public GameObject IssueCanvasObject;
        public LineRenderer LineRenderer;
        public Transform StartPoint;
        public Transform EndPoint;
        public List<GameObject> HoveredObjects = new List<GameObject>();
        public List<GameObject> HoveredFODs = new List<GameObject>();
        public List<HoveredDisplacedPair> HoveredDisplacedPairObjects = new List<HoveredDisplacedPair>();
        public List<BaseDamage> BaseDamages = new List<BaseDamage>();
        public List<DecalDamage> DecalDamages => BaseDamages.OfType<DecalDamage>().ToList();
        public List<DisplacedDamage> DisplacedDamages => BaseDamages.OfType<DisplacedDamage>().ToList();
        public List<MarkedDecalDamage> MarkedDecalDamages => DecalDamages.OfType<MarkedDecalDamage>().ToList();
        //public List<AdditionalDamage> AdditionalDamages => this.IsFOD ? BaseDamages.OfType<AdditionalDamage>().ToList() : new List<AdditionalDamage>();

        [Header("Damage Segment Text Area")]
        public string ClearErrorTextID;
        public string AreaTextID;
        public string DescriptionAreaTextID;
        public bool IssueIsActive => IssueCanvasObject.activeSelf;
        public bool LineRendererIsAvailable => LineRenderer != null;
        public bool LineRendererIsActive => LineRenderer.gameObject.activeSelf;
        public bool LineRendererPointsAreAvailable => StartPoint != null && EndPoint != null;
        public bool IssueIsAvailable => IssueCanvasObject != null;
        public bool HoveredObjectIsAvailable => HoveredObjects.Count != 0 || HoveredDisplacedPairObjects.Count != 0;
        public bool IsAvailable => HoveredObjectIsAvailable && IssueIsAvailable && LineRendererIsAvailable && IssueIsActive && LineRendererPointsAreAvailable;
        public bool IsAdded => BaseDamages.Any(baseDamage => baseDamage.IsAdded);
        public bool IsReported => MarkedDecalDamages.Any(markedDamage => markedDamage.IsReported);

        private void Update()
        {
            bool isHovered = XRSimpleInteractable.isHovered;

            if (XRSimpleInteractable.isHovered)
            {
                if (XRSimpleInteractable.interactorsHovering[0].transform.GetComponent<XRBaseControllerInteractor>().xrController.uiPressInteractionState.activatedThisFrame)
                    SetIssueEvents();
            }
        }
        private void Awake()
        {
            XRSimpleInteractable.hoverEntered.AddListener(delegate { HoverDamageEnter(); });
            XRSimpleInteractable.hoverExited.AddListener(delegate { HoverDamageExit(); });
        }
        public void CheckDamageSetup()
        {
            if (IsAvailable) return;

            Debug.LogError($"Problems in Damage Segment setup for:{gameObject.name}.", gameObject);
        }
        public void ResetDamageSegment()
        {
            SetupLineRenderer();
            DisableIssue();
            RestartAllLists();
        }
        public void EnableIssue()
        {
            if (!IssueIsAvailable)
            {
                Debug.LogError("Enabling issue is not possible.", gameObject);
                return;
            }

            SetIssueObject();
            EnableLineRenderer();
        }
        private void DisableIssue()
        {
            if (!IssueIsAvailable)
            {
                Debug.LogError("Disabling issue is not possible.", gameObject);
                return;
            }

            UnsetIssueObject();
            DisableLineRenderer();
        }
        [ContextMenu("MarkIssue")]
        public void SetIssueEvents()
        {
            if (!AirportData.ExamMode && (!IsAdded || IsReported))
            {
                ErrorInfoBoard.Singleton.TryShow();
            }
            else
            {
                ToggleIssue();
                DamageCanvasManager.SetCurrentDamageSegment(this);

                DamageCanvasManager.OnIssueMarked.Invoke();
            }
        }
        public void ToggleIssue()
        {
            if (!IssueIsAvailable) return;

            if (IssueIsActive)
                DisableIssue();
            else
                EnableIssue();
        }
        private void EnableLineRenderer()
        {
            if (!LineRendererIsAvailable)
            {
                Debug.LogError("Enabling lineRenderer is not possible.", gameObject);
                return;
            }

            SetLineRendererObject();
        }
        public void DisableLineRenderer()
        {
            if (!LineRendererIsAvailable)
            {
                Debug.LogError("Disabling lineRenderer is not possible.", gameObject);
                return;
            }

            UnsetLineRendererObject();
        }
        private void SetupLineRenderer()
        {
            if (!LineRendererPointsAreAvailable)
            {
                Debug.LogError("Setup of lineRenderer points is not possible.", gameObject);
                return;
            }
            LineRenderer.SetPosition(0, StartPoint.position);
            LineRenderer.SetPosition(1, EndPoint.position);
        }
        protected void SetIssueObject()
        {
            IssueCanvasObject.ActivateObject();
        }
        protected void UnsetIssueObject()
        {
            IssueCanvasObject.DeactivateObject();
        }
        protected void SetLineRendererObject()
        {
            LineRenderer.gameObject.ActivateObject();
        }
        protected void UnsetLineRendererObject()
        {
            LineRenderer.gameObject.DeactivateObject();
        }
        public void HoverDamageEnter()
        {
            if (!HoveredObjectIsAvailable)
            {
                Debug.LogError("Hover enter in aircraft displaced damage is not possible.", gameObject);
                return;
            }

            SetHovered();
        }
        public void HoverDamageExit()
        {
            if (!HoveredObjectIsAvailable)
            {
                Debug.LogError("Hover exit in aircraft displaced damage is not possible.", gameObject);
                return;
            }

            UnsetHovered();
        }
        protected void SetHovered()
        {
            HoveredObjects.ForEach(hoveredObject => hoveredObject.ActivateHighlight());
            HoveredFODs.ForEach(hoveredObject => hoveredObject.ActivateHighlight());

            DecalDamages.Where(decalDamage => decalDamage.IsAdded && decalDamage is MarkedDecalDamage).ToList().ForEach(decalDamage => decalDamage.gameObject.DeactivateObject());

            HoveredDisplacedPairObjects.ForEach(pair =>
            {
                if (pair.DisplacedDamage.IsAdded)
                {
                    pair.DisplacedObject.ActivateHighlight();
                    DisplacedDamage displacedDamage = pair.DisplacedDamage;
                    displacedDamage.AdditionalDisplacedObjects.ForEach(displacedObject => displacedObject.DeactivateObjectRenderer());
                }
                else
                {
                    pair.SourceObject.ActivateHighlight();
                    DisplacedDamage displacedDamage = pair.DisplacedDamage;
                    displacedDamage.AdditionalCustomObjects.ForEach(customObject => customObject.DeactivateObjectRenderer());
                }
            });
        }
        protected void UnsetHovered()
        {
            HoveredObjects.ForEach(hoveredObject => hoveredObject.DeactivateHighlight());
            HoveredFODs.ForEach(hoveredObject => hoveredObject.DeactivateHighlight());

            DecalDamages.Where(decalDamage => decalDamage.IsAdded && decalDamage is MarkedDecalDamage).ToList().ForEach(decalDamage => decalDamage.gameObject.ActivateObject());
            HoveredDisplacedPairObjects.ForEach(pair =>
            {
                if (pair.DisplacedDamage.IsAdded)
                {
                    pair.DisplacedObject.DeactivateHighlight();
                    DisplacedDamage displacedDamage = pair.DisplacedDamage;
                    displacedDamage.AdditionalDisplacedObjects.ForEach(displacedObject => displacedObject.ActivateObjectRenderer());
                }
                else
                {
                    pair.SourceObject.DeactivateHighlight();
                    DisplacedDamage displacedDamage = pair.DisplacedDamage;
                    displacedDamage.AdditionalCustomObjects.ForEach(customObject => customObject.ActivateObjectRenderer());
                }
            });
        }
        protected void RestartAllLists()
        {
            HoveredObjects.ForEach(hoveredObject => hoveredObject.DeactivateHighlight());
            DecalDamages.ForEach(decalDamage => decalDamage.gameObject.DeactivateObject());

            HoveredDisplacedPairObjects.ForEach(pair =>
            {
                pair.DisplacedObject.DeactivateHighlight();
                pair.SourceObject.DeactivateHighlight();
            });

            DisplacedDamages.OfType<DisplacedDamage>().ToList().ForEach(displacedDamage => displacedDamage.SourceObject.DeactivateHighlight());
            //AdditionalDamages.ForEach(additionalDamage => additionalDamage.SourceObject.DeactivateObject());
        }
    }
}