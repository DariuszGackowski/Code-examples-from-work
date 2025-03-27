using CustomLocalisation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.CanvasManger;
using UnityEngine;
using XR.Watch;
using static Module9.SecurityThreatsSO;

namespace Module9
{
    [Serializable]
    public class Threat
    {
        public List<Message> GeneratedConversationList = new List<Message>();
        public List<Message> GeneratedCustomConversationList => GeneratedConversationList.Where(message => message.IsCustom && !message.IsToAsk && !message.IsEntry).ToList();
        public List<Message> GeneratedReportConversationList => GeneratedConversationList.Where(message => !message.IsEntry).ToList();

        public Threat(List<Message> threat)
        {
            GeneratedConversationList = threat.ToList();
        }
    }
    public class ThreatsManager : MonoBehaviour
    {
        public static Threat Threat;

        public ThreatsAudio ThreatsAudio;

        public SecurityThreatsSO SecurityThreatsSO;

        public Threat ThreatToCheck;

        private const string _scenarioDescShortID = "module9_name";
        private const string _criticalErrorsDescriptionID = "module9_errors";
        private const string _detailsHeaderID = "module9_detailsHeader";
        private const string _scenarioDescriptionID = "module9_scenarioDescription";

        private void Awake() => Setup();
        private void Setup()
        {
            ThreatsCanvasManager.OnStartThreat.AddListener(InitializeThreat);
        }
        public void InitializeThreat()
        {
            if (SecurityThreatsSO.Messages.Count == 0)
            {
                Debug.LogWarning("No threats available in SecurityThreats!");
            }

            GenerateRandomThreat();

            ThreatsCanvasManager.OnStartThreatMessages.Invoke(Threat.GeneratedCustomConversationList);
            ThreatsCanvasManager.OnStarReporting.Invoke();
            ThreadGenerator.OnSetupThread.Invoke();
        }

        private void GenerateRandomThreat()
        {
            List<Message> threat = SecurityThreatsSO.GetRandomThreat();

            Threat = new(threat);
            ThreatToCheck = Threat;
        }
        public static void PrepareSummary()
        {
            ScoreData score = new()
            {
                ScenarioDescription = $"{_scenarioDescriptionID}{CustomLocalisationSettings.ScoreStopSign}{Timer.CurrentTimeInMinutes}",
                DetailsHeader = _detailsHeaderID,
                CriticalErrorsDescription = _criticalErrorsDescriptionID,
                Date = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString(),
                ScenarioDescShort = _scenarioDescShortID
            };
            ThreatsSummary(ref score);

            AirportData.LastScoreData = score;
        }
        private static void ThreatsSummary(ref ScoreData score)
        {
            foreach (Message message in Threat.GeneratedConversationList)
            {
                if (message.IsEntry) continue;

                if (message.CorrectReportIssue)
                {
                    score.DetailsDescription += $"{message.CorrectReportTextID}\n";
                }

                if (message.ReportedIssue)
                {
                    score.CriticalErrors += $"{message.ReportedIssueTextID}\n";
                }
                if (message.NotReportedIssue)
                {
                    score.CriticalErrors += $"{message.NotReportedIssueTextID}\n";
                }
                if (message.AskIssue)
                {
                    if (message.LackAsk)
                    {
                        score.CriticalErrors += $"{message.LackAskTextID}\n";
                    }
                    else if (message.ExcessAsk)
                    {
                        score.CriticalErrors += $"{message.ExcessAskTextID}\n";
                    }
                }
            }
        }
    }
}