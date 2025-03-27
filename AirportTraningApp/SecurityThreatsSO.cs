using AllExtensions;
using CustomLocalisation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Module9.SecurityThreatsSO.Message;

namespace Module9
{
    [CreateAssetMenu(fileName = "NewSecurityThreat", menuName = "ScriptableObjects/SecurityThreats", order = 1)]
    public class SecurityThreatsSO : ScriptableObject
    {
        public class ThreatScheme
        {
            public TypeInfo InfoType;
            public string Entry { get; set; }
            public List<string> Plane { get; set; }
            public List<string> InfoID { get; set; }
            public List<string> Approaches { get; set; }
            public List<string> Passengers { get; set; }
            public List<string> Fuels { get; set; }
            public List<string> Materials { get; set; }
            public List<string> Winds { get; set; }
            public List<string> Alarms { get; set; }
        }

        [Serializable]
        public class Message
        {
            public enum TypeText
            {
                None,
                Entry,
                Passengers1,
                Passengers2,
                Passengers12,
                Fuel1,
                Fuel2,
                Fuel12,
                Materials,
                Wind,
                Alarm,
            }
            public enum TypeInfo
            {
                None,
                InformationFire,
                InformationMedical,
                InformationLanding,
                InformationBrokenLanding,
                InformationCrash,
                InformationSecurity,
                InformationUnderConstruction
            }
            public enum TypePlace
            {
                None,
                Approach,
                Apron
            }
            [Flags]
            public enum TypePlane
            {
                None = 0,
                CRJ900 = 1 << 0,
                A320Neo = 1 << 1,
                A320NeoAndCRJ900 = 1 << 2,
                B738 = 1 << 3
            }

            public TypeText TextType;
            public TypeInfo InfoType;
            public TypePlace PlaceType;
            public TypePlane PlaneType;
            public string Identifier;
            [TextArea] public string Text;
            public AudioClip Audio;
            public bool IsToAsk;
            public bool IsAsked;
            public bool IsCustom;
            public bool IsMarked;
            public bool IsEntry;

            public bool ExcessAsk => !IsToAsk && IsAsked;
            public bool LackAsk => IsToAsk && !IsAsked;
            public bool AskIssue => ExcessAsk || LackAsk;
            public bool ReportedIssue => !IsCustom && IsMarked;
            public bool NotReportedIssue => IsCustom && !IsMarked;
            public bool CorrectReportIssue => IsCustom && IsMarked;

            public string CorrectReportTextID => $"{CustomLocalisationSettings.ScoreStopSign}{_correctReportTextID}{CustomLocalisationSettings.ScoreStopSign}{Identifier}{CustomLocalisationSettings.ScoreStopSign}";
            public string ReportedIssueTextID => $"{CustomLocalisationSettings.ScoreStopSign}{_reportedIssueTextID}{CustomLocalisationSettings.ScoreStopSign}{Identifier}{CustomLocalisationSettings.ScoreStopSign}";
            public string NotReportedIssueTextID => $"{CustomLocalisationSettings.ScoreStopSign}{_notReportedIssueTextID}{CustomLocalisationSettings.ScoreStopSign}{Identifier}{CustomLocalisationSettings.ScoreStopSign}";
            public string ExcessAskTextID => $"{CustomLocalisationSettings.ScoreStopSign}{_excessAskTextID}{CustomLocalisationSettings.ScoreStopSign}{Identifier}{CustomLocalisationSettings.ScoreStopSign}";
            public string LackAskTextID => $"{CustomLocalisationSettings.ScoreStopSign}{_lackAskTextID}{CustomLocalisationSettings.ScoreStopSign}{Identifier}{CustomLocalisationSettings.ScoreStopSign}";

            private const string _correctReportTextID = "module9_correctReportText";
            private const string _reportedIssueTextID = "module9_reportedIssueText";
            private const string _notReportedIssueTextID = "module9_notReportedIssueText";
            private const string _excessAskTextID = "module9_excessAskText";
            private const string _lackAskTextID = "module9_lackAskText";

            public Message(TypeText textType, TypeInfo infoType, TypePlace placeType, TypePlane planeType, string identifier, string text)
            {
                TextType = textType;
                InfoType = infoType;
                PlaceType = placeType;
                PlaneType = planeType;
                Identifier = identifier;
                Text = text;
                Audio = CatchAudio(identifier);
            }
            public void Clear()
            {
                IsToAsk = false;
                IsAsked = false;
                IsCustom = false;
                IsMarked = false;
                IsEntry = false;
            }
        }
        public static readonly string FilePath = Application.dataPath + "/XLSXs/";
        public static readonly string FileName = "Module9.xlsx";
        public static readonly string AudioPath = Application.dataPath + "/Audio/Module9/";
        public static readonly string RelativePath = "Assets/Audio/Module9/";

        public List<Message> Messages = new List<Message>();

        public static List<ThreatScheme> ThreatSchemes = new List<ThreatScheme>()
        {
            new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry2",
                Plane = new(){ "plane1" },
                InfoID = new(){ "security1" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms =new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry2",
                Plane =new() {"plane2", "plane3" },
                InfoID = new() { "security1" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry4",
                Plane = new(){ "plane1" },
                InfoID = new() { "security2" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry4",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "security2" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry5",
                Plane = new(){ "plane1" },
                InfoID = new() { "security3" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
           new()
            {
                InfoType = TypeInfo.InformationSecurity,
                Entry = "entry5",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "security3" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationCrash,
                Entry = "entry1",
                Plane = new(){ "plane12" },
                InfoID = new() { "crash1" },
                Approaches = new() { "approach1" },
                Passengers = new() { "passengers7" },
                Fuels = new() { "fuel7" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationLanding,
                Entry = "entry1",
                Plane = new(){ "plane1" },
                InfoID = new() { "landing1", "landing2" },
                Approaches = new() { "approach1", "approach2" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationLanding,
                Entry = "entry1",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "landing1", "landing2" },
                Approaches = new() { "approach1", "approach2" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },            new()
            {
                InfoType = TypeInfo.InformationUnderConstruction,
                Entry = "entry1",
                Plane = new(){ "plane1" },
                InfoID = new() { "construction1" },
                Approaches = new() { "approach1"},
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationUnderConstruction,
                Entry = "entry1",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "construction1"},
                Approaches = new() { "approach1"},
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationMedical,
                Entry = "entry3",
                Plane = new(){ "plane1" },
                InfoID = new() { "medical1", "medical2" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationMedical,
                Entry = "entry3",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "medical1", "medical2" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationFire,
                Entry = "entry1",
                Plane = new(){ "plane1" },
                InfoID = new() { "fire1", "fire2", "fire3" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationFire,
                Entry = "entry1",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "fire1", "fire2", "fire3" },
                Approaches = new() { "approach3", "approach4", "approach5" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationBrokenLanding,
                Entry = "entry1",
                Plane = new(){ "plane1" },
                InfoID = new() { "brokenLanding1" },
                Approaches = new() {"approach1", "approach2" },
                Passengers = new() { "passengers4", "passengers5", "passengers6" },
                Fuels = new() { "fuel1", "fuel2", "fuel3" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
            new()
            {
                InfoType = TypeInfo.InformationBrokenLanding,
                Entry = "entry1",
                Plane = new() {"plane2", "plane3" },
                InfoID = new() { "brokenLanding1" },
                Approaches = new() { "approach1", "approach2" },
                Passengers = new() { "passengers1", "passengers2", "passengers3" },
                Fuels = new() { "fuel4", "fuel5", "fuel6" },
                Materials = new() { "materials1", "materials2", "materials3", "materials4" },
                Winds = new() { "wind1", "wind2", "wind3", "wind4" },
                Alarms = new() { "alarm1", "alarm2", "alarm3", "alarm4" }
            },
        };

        public List<Message> EntryMessages
        {
            get
            {
                return Messages.Where(message => message.TextType == TypeText.Entry).ToList();
            }
        }
        public List<Message> PlaneMessages
        {
            get
            {
                var planeTypes = new List<TypePlane>{
                    TypePlane.CRJ900,
                    TypePlane.A320Neo,
                    TypePlane.A320NeoAndCRJ900,
                    TypePlane.B738
                };

                return Messages.Where(message => planeTypes.Contains(message.PlaneType)).ToList();
            }
        }
        public List<Message> ApproachMessages
        {
            get
            {
                var apronTypes = new HashSet<TypeInfo>{
                    TypeInfo.InformationFire,
                    TypeInfo.InformationMedical,
                    TypeInfo.InformationSecurity};

                var approachTypes = new HashSet<TypeInfo>{
                    TypeInfo.InformationLanding,
                    TypeInfo.InformationUnderConstruction,
                    TypeInfo.InformationBrokenLanding,
                    TypeInfo.InformationCrash};

                return Messages.Where(message =>
                    (apronTypes.Contains(ThreatsSceneStarter.InfoType) && message.PlaceType == TypePlace.Apron) ||
                    (approachTypes.Contains(ThreatsSceneStarter.InfoType) && message.PlaceType == TypePlace.Approach)).ToList();
            }
        }
        public List<Message> PassengerMessages
        {
            get
            {
                TypeText passengerText = ThreatsSceneStarter.PlaneType switch
                {
                    TypePlane.CRJ900 => TypeText.Passengers1,
                    TypePlane.A320Neo => TypeText.Passengers2,
                    TypePlane.A320NeoAndCRJ900 => TypeText.Passengers12,
                    TypePlane.B738 => TypeText.Passengers2,
                    _ => TypeText.Passengers1
                };
                return Messages.Where(message => message.TextType == passengerText).ToList();
            }
        }
        public List<Message> FuelMessages
        {
            get
            {
                TypeText fuelText = ThreatsSceneStarter.PlaneType switch
                {
                    TypePlane.CRJ900 => TypeText.Fuel1,
                    TypePlane.A320Neo => TypeText.Fuel2,
                    TypePlane.A320NeoAndCRJ900 => TypeText.Fuel12,
                    TypePlane.B738 => TypeText.Fuel2,
                    _ => TypeText.Fuel1
                };
                return Messages.Where(message => message.TextType == fuelText).ToList();
            }
        }
        public List<Message> AlarmMessages
        {
            get
            {
                return Messages.Where(message => message.TextType == TypeText.Alarm).ToList();
            }
        }
        public List<Message> InformationMessages
        {
            get
            {
                var informationTypes = new List<TypeInfo>{
                    TypeInfo.InformationCrash,
                    TypeInfo.InformationFire,
                    TypeInfo.InformationMedical,
                    TypeInfo.InformationLanding,
                    TypeInfo.InformationSecurity,
                    TypeInfo.InformationUnderConstruction,
                    TypeInfo.InformationBrokenLanding};

                return Messages.Where(message => informationTypes.Contains(message.InfoType)).ToList();
            }
        }
        public List<Message> MaterialMessages
        {
            get
            {
                return Messages.Where(message => message.TextType == TypeText.Materials).ToList();
            }
        }
        public List<Message> WindMessages
        {
            get
            {
                return Messages.Where(message => message.TextType == TypeText.Wind).ToList();
            }
        }
        public enum RevelantTextType
        {
            Place,
            Passengers,
            Fuel,
            Materials,
            Wind,
            Plane,
            Information,
            Alarm
        }

        public List<Message> GetRelevantMessages(RevelantTextType mainTextType)
        {
            return mainTextType switch
            {
                RevelantTextType.Place => ApproachMessages,
                RevelantTextType.Passengers => PassengerMessages,
                RevelantTextType.Fuel => FuelMessages,
                RevelantTextType.Materials => MaterialMessages,
                RevelantTextType.Wind => WindMessages,
                RevelantTextType.Information => InformationMessages,
                RevelantTextType.Alarm => AlarmMessages,
                RevelantTextType.Plane => PlaneMessages,
                _ => new List<Message>()
            };
        }
        private static readonly Dictionary<string, TypePlane> PlaneNameToType = new()
        {
           { "plane1", TypePlane.CRJ900 },
           { "plane2", TypePlane.A320Neo },
           { "plane3", TypePlane.B738 },
           { "plane12", TypePlane.A320NeoAndCRJ900 },
        };
        public List<Message> GetRandomThreat()
        {

            Messages.ForEach(message => message.Clear());

            List<Message> allMessages = new List<Message>();
            List<Message> askList = new List<Message>();
            List<Message> excessList = new List<Message>();

            Debug.Log(ThreatsSceneStarter.InfoType);

            string planeName = ThreatSchemes.GetRandomPlane(ThreatsSceneStarter.InfoType);
            if (!PlaneNameToType.TryGetValue(planeName, out TypePlane planeType))
            {
                Debug.LogError($"Nieznana nazwa samolotu: {planeName}");
            }
            ThreatsSceneStarter.PlaneType = planeType;
            ThreatScheme selectedScheme = ThreatSchemes.GetRandomSchemeByPlane(ThreatsSceneStarter.InfoType, planeName);

            Message entry = EntryMessages.GetRandomMessageById(selectedScheme.Entry);
            Message information = InformationMessages.GetRandomMessageByInfoTypeAndId(selectedScheme.InfoType, selectedScheme.InfoID);
            Message plane = PlaneMessages.GetRandomMessageByType(planeType);
            Message place = ApproachMessages.GetRandomMessageByApproaches(selectedScheme.Approaches, selectedScheme.InfoType);
            Message passenger = PassengerMessages.GetRandomMessageById(selectedScheme.Passengers);
            Message fuel = FuelMessages.GetRandomMessageById(selectedScheme.Fuels);
            Message materials = MaterialMessages.GetRandomMessageById(selectedScheme.Materials);
            Message wind = WindMessages.GetRandomMessageById(selectedScheme.Winds);
            Message alarm = AlarmMessages.GetRandomMessageById(selectedScheme.Alarms);

            if (entry != null) { entry.IsEntry = true; allMessages.Add(entry); }
            if (information != null) { information.IsCustom = true; allMessages.Add(information); }
            if (plane != null) { plane.IsCustom = true; allMessages.Add(plane); }
            if (place != null) { place.IsCustom = true; allMessages.Add(place); }
            if (passenger != null) { passenger.IsCustom = true; allMessages.Add(passenger); }
            if (fuel != null) { fuel.IsCustom = true; allMessages.Add(fuel); }
            if (materials != null) { materials.IsCustom = true; allMessages.Add(materials); }
            if (wind != null) { wind.IsCustom = true; allMessages.Add(wind); }
            if (alarm != null) { alarm.IsCustom = true; allMessages.Add(alarm); }

            allMessages.SetRandomAskList();
            excessList = Messages.GetRandomExcessList(allMessages);
            allMessages.AddRange(excessList);

            return allMessages;
        }
        public void BuildDatabase()
        {
            if (!ExcelImporter.LoadRawData(FilePath, FileName))
            {
                Debug.LogWarning($"Could not build database - Could not get raw data from '{FilePath}'.");
                return;
            }

            ConvertRawDataToMessages();
        }
        public void ConvertRawDataToMessages()
        {
            Messages.Clear();

            foreach (Tuple<TypeText, TypeInfo, TypePlace, TypePlane, string, string> tuple in ExcelImporter.RawData)
            {
                Message message = new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
                Messages.Add(message);
            }
        }

        public static AudioClip CatchAudio(string identifier)
        {
            string path = Path.Combine(AudioPath, identifier + ".mp3");
            string relativePath = Path.Combine(RelativePath, identifier + ".mp3");

            if (!File.Exists(path))
            {
                Debug.LogError("Nie znaleziono œcie¿ki do audio: " + path);
                return null;
            }
            AudioClip clip = null;
#if UNITY_EDITOR
            clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

            if (clip == null)
            {
                Debug.LogError("Nie znaleziono pliku audio w œcie¿ce: " + path);
            }
#endif
            return clip;
        }
    }
}