using AllExtensions;
using CustomLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.CanvasManger;
using UnityEngine;
using UnityEngine.Events;

namespace Module7
{
    public class DamageCanvasManager : CanvasManagerBase
    {
        public static UnityEvent OnIssueMarked = new UnityEvent();

        public static DamageSegment CurrentElement;

        [Space]
        public GameObject FlashlightHintCanvas;
        public GameObject DamageElementPrefab;
        public GameObject MainContentHolderPCObject;
        [Space]
        public DamageManager DamageManager;
        [Space]
        public GameObject ExamTextObjectPC;
        public GameObject TutorialTextObjectPC;
        public GameObject ExamTextObjectVR;
        public GameObject TutorialTextObjectVR;

        #region Awake Methods
        public override void ResetCanvas()
        {
            base.ResetCanvas();

            ResetDamageElements(VRContent);
            ResetDamageElements(PCContent);

            FlashlightHintCanvas.DeactivateObject();
        }
        public override void SetupSummary()
        {
            DamageManager.PrepareSummary();
        }
        #endregion

        #region Buttons Methods
        [ContextMenu("StartModule")]
        public override void StartModule()
        {
            base.StartModule();

            SetupDamageElements();

            ActivateCanvasPC();
            DeactivateStartCanvas();

            PCMinimizeSegment.Deactivate();

            if (DamageSceneStarter.IsNight)
            {
                FlashlightHintCanvas.ActivateObject();
            }

            if (AirportData.ExamMode)
            {
                ExamTextObjectPC.ActivateObject();
                ExamTextObjectVR.ActivateObject();
                TutorialTextObjectPC.DeactivateObject();
                TutorialTextObjectVR.DeactivateObject();
            }
            else
            {
                ExamTextObjectPC.DeactivateObject();
                ExamTextObjectVR.DeactivateObject();
                TutorialTextObjectPC.ActivateObject();
                TutorialTextObjectVR.ActivateObject();
            }
        }
        private void ResetDamageElements(Transform contentHolder)
        {
            for (int i = contentHolder.childCount - 1; i >= 0; i--)
            {
                Transform child = contentHolder.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        private void ResetDamageCanvasManager()
        {
            IsStarted = false;

            ResetCanvasVRTransform();
            ResetCanvas();
        }
        private void ResetCanvasVRTransform()
        {
            CanvasVRTransform.SetParent(null);
            CanvasVRTransform.position = _startCanvasVRPosition;
            CanvasVRTransform.localEulerAngles = _startCanvasVRRotation;
            CanvasVRTransform.SetParent(this.transform);
        }
        #endregion

        #region DamageElements Setup
        private void SetupDamageElements()
        {
            //List<DamageSegment> orderedDamageSegments = DamageManager.CurrentDamageSegmentList.ToList();
            List<DamageSegment> disorderedDamageSegments = DamageManager.CurrentDamageSegmentList.OrderBy(_ => Guid.NewGuid()).ToList();
            List<DamageSegment> damageSegments = /*DamageSceneStarter.IsTraining ? orderedDamageSegments.ToList() :*/ disorderedDamageSegments.ToList();
            List<BaseDamage> baseDamages = damageSegments.SelectMany(damageSegment => damageSegment.BaseDamages).Where(damageBase => damageBase.IsAdded).ToList();

            foreach (DamageSegment damageSegment in damageSegments)
            {
                foreach (BaseDamage baseDamage in damageSegment.BaseDamages)
                {
                    string issueText = baseDamage switch
                    {
                        AdditionalDamage additionalDamage => CustomLocalisationSettings.Singleton.GetTranslation(additionalDamage.IssueNameID),
                        DisplacedDamage displacedDamage => CustomLocalisationSettings.Singleton.GetTranslation(displacedDamage.IssueNameID),
                        MarkedDecalDamage markedDecalDamage => CustomLocalisationSettings.Singleton.GetTranslation(markedDecalDamage.IssueNameID),
                        DecalDamage decalDamage => CustomLocalisationSettings.Singleton.GetTranslation(decalDamage.IssueNameID),
                        _ => string.Empty
                    };

                    SetupDamageCanvases(issueText, damageSegment);
                    break;
                }
            }

            Canvas.ForceUpdateCanvases();
        }
        private void SetupDamageCanvases(string issueName, DamageSegment damageSegment)
        {
            SetupDamages(issueName, damageSegment, PCContent);
            SetupDamages(issueName, damageSegment, VRContent);
        }
        public void SetupDamages(string issueName, DamageSegment damageSegment, Transform contentHolder)
        {
            GameObject damagePanelObject = Instantiate(DamageElementPrefab, contentHolder);
            DamageElement damagePanel = damagePanelObject.GetComponent<DamageElement>();
            damagePanel.Setup(issueName, damageSegment);
        }
        #endregion

        #region Other Methods
        public static void SetCurrentDamageSegment(DamageSegment damageSegment)
        {
            CurrentElement = damageSegment;
        }
        #endregion
    }
}