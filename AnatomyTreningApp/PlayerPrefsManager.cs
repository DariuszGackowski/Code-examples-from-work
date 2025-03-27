using IIT.UI;
using UnityEngine;
using HS = IIT.HeadsetSetup;
using CM = IIT.Managers.CameraManager;
using EM = IIT.EnvironmentManager;
using IIT.Anatomy;
using AOM = IIT.Anatomy.AnatomyObjectManager;

namespace IIT
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        private const string playModeKey = "PlayMode"; // GameManager.PlayModePC --> value: 1, // GameManager.PlayModeVR --> value: 2 
        private const string hintsKey = "Hints"; // GameManager.PromptControllerHintsAtStart: true --> value: 1, // false --> value: 2 
        private const string screenshotQualityKey = "ScreenshotQuality";
        private const string primaryLanguageKey = "PrimaryLanguage"; // LanguageManager.PrimaryLanguageCode: "pl" --> value: 1, // "en" --> value: 2
        private const string secondaryLanguageKey = "SecondaryLanguage"; // LanguageManager.SecondaryLanguageCode: "pl" --> value: 1, // "en" --> value: 2 // "ltn" --> value: 3
        private const string cameraModeKey = "CameraMode"; // Mode.LockedCamera --> value: 1, // CameraManager.Mode.FreeCamera --> value: 2 
        private const string controllersTypeKey = "ControllersType"; // HeadsetSetup.CurrentControllers.Oculus --> value: 1, // HeadsetSetup.CurrentControllers.Vive --> value: 2 // HeadsetSetup.CurrentControllers.Valve --> value: 3 
        private const string labelScaleKey = "LabelScale";
        private const string centerOfSceneKey = "CenterOfScene"; // EnvironmentManager.LensIsActivated: true --> value: 1, // false --> value: 2
        private const string dynamicObjectMovementKey = "DynamicObjectMovement"; // AnatomyObjectManager.IsDynamicObjectMovement: true --> value: 1, // false --> value: 2
        private const string mainFocusLockedPCKey = "MainFocusLockedPC"; // AOM.IsMainFocusLockedPC: true --> value: 1, // false --> value: 2
        private const string mainFocusLockedVRKey = "MainFocusLockedVR"; // AOM.IsMainFocusLockedVR: true --> value: 1, // false --> value: 2
        private const string zoomDiagramKey = "ZoomDiagram";
        private const string isRelativeRotationKey = "IsRelativeRotation"; // AOM.IsRelativeRotationKey: true --> value: 1, // false --> value: 2
        private const string xrayPreviewKey = "xrayPreviewKey"; // AOM.IsRelativeRotationKey: true --> value: 1, // false --> value: 2
        private const string diagramAutofocus = "diagramAutofocusKey"; // AOM.IsRelativeRotationKey: true --> value: 1, // false --> value: 2

        private const string polishLocaleIdentifier = "pl";
        private const string englishLocaleIdentifier = "en";
        private const string latinLocaleIdentifier = "ltn";

        #region Save Prefs Region
        public static void SavePlayMode()
        {
            SaveBool(playModeKey, GameManager.PlayModePC);
        }
        public static void SaveHints()
        {
            SaveBool(hintsKey, GameManager.PromptControllerHintsAtStart);
        }
        public static void SaveScreenshotQuality()
        {
            SaveFloat(screenshotQualityKey, ScreenshotManager.SuperScale);
        }
        public static void SavePrimaryLanguage()
        {
            SaveBool(primaryLanguageKey, LanguageManager.PrimaryLanguageCode == polishLocaleIdentifier);
        }
        public static void SaveSecondaryLanguage()
        {
            int languageValue;
            switch (LanguageManager.SecondaryLanguageCode)
            {
                case polishLocaleIdentifier:
                    languageValue = 1;
                    break;
                case englishLocaleIdentifier:
                    languageValue = 2;
                    break;
                case latinLocaleIdentifier:
                    languageValue = 3;
                    break;
                default:
                    languageValue = 1;

                    Debug.LogError($"Secondary language code: {LanguageManager.SecondaryLanguageCode} not exist");
                    break;
            }
            SaveInt(secondaryLanguageKey, languageValue);
        }
        public static void SaveCameraMode()
        {
            SaveBool(cameraModeKey, CM.CameraMode == CM.Mode.LockedCamera);
        }
        public static void SaveControlersType()
        {
            int headsetTypeValue;
            switch (HS.CurrentControllers)
            {
                case HS.ControllersType.Oculus:
                    headsetTypeValue = 1;
                    break;
                case HS.ControllersType.Vive:
                    headsetTypeValue = 2;
                    break;
                case HS.ControllersType.Valve:
                    headsetTypeValue = 3;
                    break;
                default:
                    headsetTypeValue = 1;

                    Debug.LogError($"Current controllers type code: {HS.CurrentControllers} not exist");
                    break;
            }
            SaveInt(controllersTypeKey, headsetTypeValue);
        }
        public static void SaveLabelScale()
        {
            SaveFloat(labelScaleKey, UIManager.LabelFontMultiplier);
        }
        public static void SaveCenterOfScene()
        {
            SaveBool(centerOfSceneKey, EM.IsLensActivated);
        }
        public static void SaveDynamicObjectMovement()
        {
            SaveBool(dynamicObjectMovementKey, AOM.IsDynamicObjectMovement);
        }
        public static void SaveMainFocusLockedPC()
        {
            SaveBool(mainFocusLockedPCKey, AOM.IsMainFocusLockedPC);
        }
        public static void SaveMainFocusLockedVR()
        {
            SaveBool(mainFocusLockedVRKey, AOM.IsMainFocusLockedVR);
        }
        public static void SaveZoomDiagram()
        {
            SaveFloat(zoomDiagramKey, UIManager.ZoomValueDiagram);
        }
        public static void SaveRelativeRotation()
        {
            SaveBool(isRelativeRotationKey, AOM.IsRelativeRotation);
        }
        public static void SaveXRAYPreview()
        {
            SaveBool(xrayPreviewKey, GameManager.XRAYDiagramPreviewEnabled);
        }
        public static void SaveDiagramAutofocus()
        {
            SaveBool(diagramAutofocus, XRUIPanelDiagram.AutofocusEnabled);
        }
        #endregion

        #region Load Prefs Region

        public static void LoadAllPlayerPrefs()
        {
            LoadPlayMode();
            LoadHints();
            LoadScreenshotQuality();
            LoadPrimaryLanguage();
            LoadSecondaryLanguage();
            LoadLabelScale();
            LoadCenterOfScene();
            LoadMainFocusLockedVR();
            LoadMainFocusLockedPC();
            LoadCameraMode();
            LoadControllersType();
            LoadDynamicObjectMovement();
            LoadZoomDiagram();
            LoadRelativeRotation();
            LoadXRAYPreview();
        }
        private static void LoadPlayMode()
        {
            if (!HasKey(playModeKey))
            {
                // If there is no key, create it
                SavePlayMode();

                Debug.LogWarning($"Created key: {playModeKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                PlayMode loadedPlayMode = GetBool(playModeKey) ? PlayMode.PC : PlayMode.VR;

                // If not equal loaded value, set up it to GameManager.PlayMode
                if (GameManager.PlayMode != loadedPlayMode) GameManager.PlayMode = loadedPlayMode;
            }
        }
        private static void LoadScreenshotQuality()
        {
            if (!HasKey(screenshotQualityKey))
            {
                // If there is no key, create it
                SaveScreenshotQuality();

                Debug.LogWarning($"Created key: {screenshotQualityKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                float loadedScreenshotQuality = GetFloat(screenshotQualityKey);

                // If not equal loaded value, set up it to ScreenshotManager.SuperScale
                if (ScreenshotManager.SuperScale != loadedScreenshotQuality) ScreenshotManager.SuperScale = loadedScreenshotQuality;
            }
        }
        private static void LoadHints()
        {
            if (!HasKey(hintsKey))
            {
                // If there is no key, create it
                SaveHints();

                Debug.LogWarning($"Created key: {hintsKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedHints = GetBool(hintsKey);

                // If not equal loaded value, set up it to GameManager.PromptControllerHintsAtStart
                if (GameManager.PromptControllerHintsAtStart != loadedHints) GameManager.PromptControllerHintsAtStart = loadedHints;
            }
        }
        private static void LoadPrimaryLanguage()
        {
            if (!HasKey(primaryLanguageKey))
            {
                // If there is no key, create it
                SavePrimaryLanguage();

                Debug.LogWarning($"Created key: {primaryLanguageKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                string loadedLocaleIdentifier = GetBool(primaryLanguageKey) ? polishLocaleIdentifier : englishLocaleIdentifier;

                // If not equal loaded value, set up it to LocalizationSettings.SelectedLocale
                if (LanguageManager.PrimaryLanguageCode != loadedLocaleIdentifier) LanguageManager.PrimaryLanguage = LanguageManager.ParseLanguageCode(loadedLocaleIdentifier);
            }
        }
        private static void LoadSecondaryLanguage()
        {
            if (!HasKey(secondaryLanguageKey))
            {
                // If there is no key, create it
                SaveSecondaryLanguage();

                Debug.LogWarning($"Created key: {secondaryLanguageKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                string loadedLocaleIdentifier;
                int loadedValue = GetInt(secondaryLanguageKey);

                switch (loadedValue)
                {
                    case 1:
                        loadedLocaleIdentifier = polishLocaleIdentifier;
                        break;
                    case 2:
                        loadedLocaleIdentifier = englishLocaleIdentifier;
                        break;
                    case 3:
                        loadedLocaleIdentifier = latinLocaleIdentifier;
                        break;
                    default:
                        loadedLocaleIdentifier = polishLocaleIdentifier;

                        Debug.LogError($"Loaded locale identifier: {loadedValue} not exist");
                        break;
                }

                // If not equal loaded value, set up it to LanguageManager.ChangeSecondaryLanguage 
                if (LanguageManager.SecondaryLanguageCode != loadedLocaleIdentifier) LanguageManager.SecondaryLanguage = LanguageManager.ParseLanguageCode(loadedLocaleIdentifier); ;
            }
        }
        private static void LoadCameraMode()
        {
            if (!HasKey(cameraModeKey))
            {
                // If there is no key, create it
                SaveCameraMode();

                Debug.LogWarning($"Created key: {cameraModeKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                CM.Mode loadedCameraMode = GetBool(cameraModeKey) ? CM.Mode.LockedCamera : CM.Mode.FreeCamera;

                // If not equal loaded value, set up it to CameraManager.CameraMode
                if (CM.CameraMode != loadedCameraMode) CM.CameraMode = loadedCameraMode;
            }
        }
        private static void LoadControllersType()
        {
            if (!HasKey(controllersTypeKey))
            {
                // If there is no key, create it
                SaveControlersType();

                Debug.LogWarning($"Created key: {controllersTypeKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                HS.ControllersType loadedControllersType;
                int loadedValue = GetInt(controllersTypeKey);
                switch (loadedValue)
                {
                    case 1:
                        loadedControllersType = HS.ControllersType.Oculus;
                        break;
                    case 2:
                        loadedControllersType = HS.ControllersType.Vive;
                        break;
                    case 3:
                        loadedControllersType = HS.ControllersType.Valve;
                        break;
                    default:
                        loadedControllersType = HS.ControllersType.Oculus;

                        Debug.LogError($"Loaded controllers type: {loadedControllersType} not exist");
                        break;
                }

                // If not equal loaded value, set up it to HS.CurrentControllers
                if (HS.CurrentControllers != loadedControllersType) HS.CurrentControllers = loadedControllersType;
            }
        }
        private static void LoadLabelScale()
        {
            if (!HasKey(labelScaleKey))
            {
                // If there is no key, create it
                SaveLabelScale();

                Debug.LogWarning($"Created key: {labelScaleKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                float loadedLabelScale = GetFloat(labelScaleKey);

                // If not equal loaded value, set up it to UIManager.LabelFontMultiplier
                if (UIManager.LabelFontMultiplier != loadedLabelScale) UIManager.LabelFontMultiplier = loadedLabelScale;
            }
        }
        private static void LoadCenterOfScene()
        {
            if (!HasKey(centerOfSceneKey))
            {
                // If there is no key, create it
                SaveCenterOfScene();

                Debug.LogWarning($"Created key: {centerOfSceneKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedCenterOfScene = GetBool(centerOfSceneKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (EM.IsLensActivated != loadedCenterOfScene) EM.IsLensActivated = loadedCenterOfScene;
            }
        }
        private static void LoadDynamicObjectMovement()
        {
            if (!HasKey(dynamicObjectMovementKey))
            {
                // If there is no key, create it
                SaveDynamicObjectMovement();

                Debug.LogWarning($"Created key: {dynamicObjectMovementKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedDynamicObjectMovement = GetBool(dynamicObjectMovementKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (AnatomyObjectManager.IsDynamicObjectMovement != loadedDynamicObjectMovement) AnatomyObjectManager.IsDynamicObjectMovement = loadedDynamicObjectMovement;
            }
        }
        private static void LoadMainFocusLockedVR()
        {
            if (!HasKey(mainFocusLockedVRKey))
            {
                // If there is no key, create it
                SaveMainFocusLockedVR();

                Debug.LogWarning($"Created key: {mainFocusLockedVRKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedIsMainFocusLockedVR = GetBool(mainFocusLockedVRKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (AOM.IsMainFocusLockedVR != loadedIsMainFocusLockedVR) AOM.IsMainFocusLockedVR = loadedIsMainFocusLockedVR;
            }
        }
        private static void LoadMainFocusLockedPC()
        {
            if (!HasKey(mainFocusLockedPCKey))
            {
                // If there is no key, create it
                SaveMainFocusLockedPC();

                Debug.LogWarning($"Created key: {mainFocusLockedPCKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedIsMainFocusLockedPC = GetBool(mainFocusLockedPCKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (AOM.IsMainFocusLockedPC != loadedIsMainFocusLockedPC) AOM.IsMainFocusLockedPC = loadedIsMainFocusLockedPC;
            }
        }
        private static void LoadZoomDiagram()
        {
            if (!HasKey(zoomDiagramKey))
            {
                // If there is no key, create it
                SaveZoomDiagram();

                Debug.LogWarning($"Created key: {zoomDiagramKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                float loadedZoomDiagram = GetFloat(zoomDiagramKey);

                // If not equal loaded value, set up it to UIManager.LabelFontMultiplier
                if (UIManager.ZoomValueDiagram != loadedZoomDiagram) UIManager.ZoomValueDiagram = loadedZoomDiagram;
            }
        }
        private static void LoadRelativeRotation()
        {
            if (!HasKey(isRelativeRotationKey))
            {
                // If there is no key, create it
                SaveRelativeRotation();

                Debug.LogWarning($"Created key: {isRelativeRotationKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedIsRelativeRotation = GetBool(isRelativeRotationKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (AOM.IsRelativeRotation != loadedIsRelativeRotation) AOM.IsRelativeRotation = loadedIsRelativeRotation;
            }
        }
        private static void LoadXRAYPreview()
        {
            if (!HasKey(xrayPreviewKey))
            {
                // If there is no key, create it
                SaveXRAYPreview();

                Debug.LogWarning($"Created key: {xrayPreviewKey} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedXRAYPreview = GetBool(xrayPreviewKey);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (GameManager.XRAYDiagramPreviewEnabled != loadedXRAYPreview) GameManager.XRAYDiagramPreviewEnabled = loadedXRAYPreview;
            }
        }
        private static void LoadDiagramAutofocus()
        {
            if (!HasKey(diagramAutofocus))
            {
                // If there is no key, create it
                SaveDiagramAutofocus();

                Debug.LogWarning($"Created key: {diagramAutofocus} for PlayerPrefs");
            }
            else
            {
                // If there is key, load it
                bool loadedXRAYPreview = GetBool(diagramAutofocus);

                // If not equal loaded value, set up it to EM.LensIsActivated
                if (XRUIPanelDiagram.AutofocusEnabled != loadedXRAYPreview) XRUIPanelDiagram.AutofocusEnabled = loadedXRAYPreview;
            }
        }
        #endregion

        #region Player Prefs Methods
        private static void SaveBool(string key, bool value)
        {
            SetBool(key, value);
            Save();
        }
        private static void SaveInt(string key, int value)
        {
            SetInt(key, value);
            Save();
        }
        private static void SaveFloat(string key, float value)
        {
            SetFloat(key, value);
            Save();
        }
        private static void SaveString(string key, string value)
        {
            SetString(key, value);
            Save();
        }

        // Method for setting a boolean value in PlayerPrefs
        private static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 2);
        }

        // Method for getting a boolean value from PlayerPrefs
        private static bool GetBool(string key)
        {
            int value = PlayerPrefs.GetInt(key);
            return value == 1;
        }

        // Method for setting an integer value in PlayerPrefs
        private static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        // Method for getting an integer value from PlayerPrefs
        private static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        // Method for setting a float value in PlayerPrefs
        private static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        // Method for getting a float value from PlayerPrefs
        private static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        // Method for setting a string value in PlayerPrefs
        private static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        // Method for getting a string value from PlayerPrefs
        private static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        // Method for checking if a given key exists in PlayerPrefs
        private static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        // Method for deleting a specific key-value pair from PlayerPrefs
        private static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        // Method for deleting all data from PlayerPrefs
        private static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
        // Method to save all changes made to PlayerPrefs
        private static void Save()
        {
            PlayerPrefs.Save();
        }
        #endregion
    }
}
