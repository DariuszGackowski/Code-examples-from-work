using AllExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.CanvasManger;
using UnityEngine;
using UnityEngine.Events;
using static Module9.SecurityThreatsSO;

namespace Module9
{
    public class ThreatsCanvasManager : CanvasManagerBase
    {
        public static UnityEvent<List<Message>> OnStartThreatMessages = new UnityEvent<List<Message>>();
        public static UnityEvent OnStarReporting = new UnityEvent();
        public static UnityEvent OnStartThreat = new UnityEvent();

        [Space]
        public ThreatsManager ThreatsManager;
        [Space]
        public GameObject AdditionalCanvas;
        public GameObject InfoCanvas;
        public GameObject ExitCanvas;
        [Space]
        public Transform InfoContent;
        [Space]
        public GameObject ExamTextObject;
        public GameObject TutorialTextObject;
        [Space]
        public List<ReportElement> ReportElementList = new List<ReportElement>();
        [Space]
        public GameObject InfoElementPrefab;
        public override void SetupCanvasManager()
        {
            base.SetupCanvasManager();

            OnStartThreatMessages.AddListener(SetupThreatMesseges);
            OnStarReporting.AddListener(SetupReportElements);
        }
        public override void ResetCanvas()
        {
            InfoCanvas.DeactivateObject();
            AdditionalCanvas.DeactivateObject();
            CanvasVR.DeactivateObject();
            CanvasPC.DeactivateObject();
            ExitCanvas.DeactivateObject();
            StartCanvas.ActivateObject();

            Canvas.ForceUpdateCanvases();
        }
        public override void SetupSummary()
        {
            ThreatsManager.PrepareSummary();
        }
        [ContextMenu("StartModule")]
        public override void StartModule()
        {
            base .StartModule();

            OnStartThreat.Invoke();

            InfoCanvas.ActivateObject();
            AdditionalCanvas.ActivateObject();
            CanvasVR.ActivateObject();
            StartCanvas.DeactivateObject();

            if (AirportData.ExamMode)
            {
                ExamTextObject.ActivateObject();
                TutorialTextObject.DeactivateObject();
            }
            else
            {
                ExamTextObject.DeactivateObject();
                TutorialTextObject.ActivateObject();
            }
            Canvas.ForceUpdateCanvases();
        }

        #region Buttons Methods
        [ContextMenu("ReportMesseges")]
        public void ReportMesseges()
        {
            SetMarkedStateToAllMessges();

            ThreadGenerator.OnStartThread.Invoke();

            DeactivateCanvasVR();
            InfoCanvas.DeactivateObject();
            AdditionalCanvas.DeactivateObject();
            ExitCanvas.ActivateObject();
        }

        #endregion

        #region ThreatElements Setup
        private void SetupThreatMesseges(List<Message> messageList)
        {
            GameObject InfoElementObject = Instantiate(InfoElementPrefab, InfoContent);
            InfoElement infoElement = InfoElementObject.GetComponent<InfoElement>();
            infoElement.Setup(messageList, ThreatsManager.ThreatsAudio);

            ThreatsManager.ThreatsAudio.PlayMessagesSequentially(messageList);

            ForceUpdateCanvases(InfoCanvas);
        }
        private void SetupReportElements()
        {
            List<Message> messageList = ThreatsManager.Threat.GeneratedReportConversationList.OrderBy(_ => Guid.NewGuid()).ToList();
            int count = Math.Min(messageList.Count, ReportElementList.Count);

            for (int i = 0; i < count; i++)
            {
                ReportElementList[i].Setup(messageList[i]);

                ForceUpdateCanvases(CanvasVR);
            }

            ForceUpdateCanvases(CanvasVR);
        }
        #endregion
        #region Other Methods
        private void SetMarkedStateToAllMessges()
        {
            var messageDictionary = ThreatsManager.Threat.GeneratedConversationList.ToDictionary(message => message.Identifier);

            foreach (var reportElement in ReportElementList)
            {
                if (messageDictionary.TryGetValue(reportElement.Message.Identifier, out var message))
                {
                    message.IsMarked = reportElement.IsMarked;
                }
            }
        }
        private void ForceUpdateCanvases(GameObject gameObject)
        {
            gameObject.DeactivateObject();
            gameObject.ActivateObject();
            Canvas.ForceUpdateCanvases();
        }
        #endregion
    }
}
