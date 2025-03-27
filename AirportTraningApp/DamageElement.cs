using AllExtensions;
using CustomLocalisation;
using CustomXR;
using SceneStarter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.CanvasManger;
using UnityEngine;
using UnityEngine.UI;

namespace Module7
{
    public class DamageElement : MonoBehaviour
    {
        public List<VerticalLayoutGroup> Group;
        public List<ContentSizeFitter> Fitter;

        public TextMeshProUGUI AreaDescriptionText;
        public TextMeshProUGUI AreaNameText;
        public TextMeshProUGUI IssueNameText;

        public GameObject CorerectMarker;
        public GameObject IncorectMarker;

        public Toggle MarkToggle;

        public DamageSegment DamageSegment;

        public GameObject IssueNameObject => IssueNameText.gameObject;

        public bool IsMarkedCorrect => CorerectMarker.activeSelf;
        public void Setup(string issueName, DamageSegment damageSegment)
        {
            DamageSegment = damageSegment;

            DamageCanvasManager.OnIssueMarked.AddListener(Toggle);

            gameObject.name = damageSegment.gameObject.name + "_DamegeElement";

            SetupTMPs(issueName, CustomLocalisationSettings.Singleton.GetTranslation(damageSegment.DescriptionAreaTextID), CustomLocalisationSettings.Singleton.GetTranslation(damageSegment.AreaTextID));
            SetupButtons();
            //SetupHint();
            ResetMarkers();

            XROriginSetupManager.XRWatch.StartCoroutine(DisableAfter());
        }

        private IEnumerator DisableAfter()
        {
            yield return new WaitForSeconds(1f);

            for(int i=0;i< Group.Count; i++)
            {
                Group[i].enabled = false;
            }

            for (int i = 0; i < Fitter.Count; i++)
            {
                Fitter[i].enabled = false;
            }
        }

        private void ResetMarkers()
        {
            CorerectMarker.DeactivateObject();
            IncorectMarker.ActivateObject();
        }
        //private void SetupHint() 
        //{
        //    if (DamageSceneStarter.IsTraining)
        //    {
        //        IssueNameObject.ActivateObject();
        //    }
        //    else
        //    {
        //        IssueNameObject.DeactivateObject();
        //    }
        //}
        private void SetupTMPs(string issueName, string areaDescription, string areaName)
        {
            //IssueNameText.SetText(issueName);
            //IssueNameText.gameObject.DeactivateObject();
            AreaDescriptionText.SetText(areaDescription);
            AreaNameText.SetText(areaName);
        }
        private void SetupButtons()
        {
            MarkToggle.onValueChanged.AddListener(MarkObject);
        }
        private void MarkObject(bool value)
        {
            if (!AirportData.ExamMode && (!DamageSegment.IsAdded || DamageSegment.IsReported))
            {
                ErrorInfoBoard.Singleton.TryShow();
            }
            else
            {
                DamageCanvasManager.SetCurrentDamageSegment(DamageSegment);
                DamageCanvasManager.OnIssueMarked.Invoke();
                DamageCanvasManager.CurrentElement.ToggleIssue();
            }
        }
        private void Toggle()
        {
            if (DamageSegment != DamageCanvasManager.CurrentElement) return;

            ToggleMarker();    
        }
        public void ToggleMarker()
        {
            if (IsMarkedCorrect)
            {
                CorerectMarker.DeactivateObject();
                IncorectMarker.ActivateObject();
            }
            else
            {
                CorerectMarker.ActivateObject();
                IncorectMarker.DeactivateObject();
            }
        }
    }
}