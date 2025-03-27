using SceneStarter;
using System;
using static Module9.SecurityThreatsSO.Message;
using Defective.JSON;
using System.Text;
using UnityEngine;

namespace Module9
{

    public class ThreatsSceneStarter : SceneStarterBase
    {
        public static TypeInfo InfoType = TypeInfo.InformationFire; // potrzebne w GetScenarioDataList()
        public static bool ErrorChance = true; // potrzebne w GetScenarioDataList()

        public static TypePlane PlaneType = TypePlane.CRJ900;

        public ThreatsManager ThreatsManager;

        public override void SceneInitialization()
        {
            base.SceneInitialization();
        }
        public override void SceneDeintialization()
        {
            //TODO
        }

        public override void SceneSetup()
        {
            base.SceneSetup();
        }

        public override void GetScenarioDataList(byte[] file, string title)
        {
            switch (title)
            {
                case "Po¿ar lotniska":
                    InfoType = TypeInfo.InformationFire;
                    break;
                case "Awaryjne l¹dowanie":
                    InfoType = TypeInfo.InformationLanding;
                    break;
                case "Z³amane l¹dowanie":
                    InfoType = TypeInfo.InformationBrokenLanding;
                    break;
                case "Zdarzenia na pasie startowym":
                    InfoType = TypeInfo.InformationCrash;
                    break;
                case "Awaryjne procedury medyczne":
                    InfoType = TypeInfo.InformationMedical;
                    break;
                case "Zagro¿enia bezpieczeñstwa":
                    InfoType = TypeInfo.InformationSecurity;
                    break;
                case "P³yta w budowie":
                    InfoType = TypeInfo.InformationUnderConstruction;
                    break;
                default:
                    InfoType = TypeInfo.InformationFire;
                    break;
            }

            Debug.Log(title);

            bool examMode = false;

            if (file == null)
            {
                ErrorChance = true;
                return;
            }
            try
            {
                JSONObject json = new JSONObject(Encoding.UTF8.GetString(file));
                int errorChance;

                json.GetField(out errorChance, "errorChance", 0);
                json.GetField(out examMode, "examMode", false);

                ErrorChance = errorChance == 0 ? false : true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to load scenario, {ex.Message}");
            }

            AirportData.ExamMode = examMode;

            Debug.Log("ErrorChance " + ErrorChance + ", InfoType: " + InfoType + " " + title);
        }
    }
}