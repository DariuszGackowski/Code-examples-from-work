using AllExtensions;
using Defective.JSON;
using SceneStarter;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;
using static Module7.DamageManager;

namespace Module7
{
    public class DamageSceneStarter : SceneStarterBase
    {
        public DamageManager Manager;
        //public static bool IsTraining = false;
        public static PlaneType ChosenPlaneType = PlaneType.CRJ900;
        public static Plane ChosenPlane;
        public static bool IsNight;

        [Header("SceneStarter Setup")]
        [Space]
        public List<Plane> Planes = new List<Plane>();

        public WeatherSettings WeatherSettings;

        public GameObject DamageFlashlight;
        public Material NightSkybox;
        public Light SceneLight;

        private bool _initialLeftXRInteractorLineAutoAdjust;
        private bool _initialRightXRInteractorLineAutoAdjust;
        private bool _initialRightXRInteractorLineHitClosestOnly;
        private bool _initialLefttXRInteractorLineHitClosestOnly;
        private float _initialRightMaxRaycastDistance;
        private float _initialLeftMaxRaycastDistance;

        private Vector3 _pokePosition;
        private Material _oldSkybox;
        public override void SceneInitialization()
        {

            base.SceneInitialization();

            PreparePlane();

            SwitchNightObject();

            OverrideXRManagerElements();
        }
        private void PreparePlane()
        {
            foreach (Plane plane in Planes)
            {
                if (plane.IsChosen)
                {
                    plane.PlaneObject.ActivateObject();
                    ChosenPlane = plane;

                    //if (IsTraining)
                    //    plane.PathArrowsObject.ActivateObject();
                    //else
                    //    plane.PathArrowsObject.DeactivateObject();
                }
                else
                    plane.PlaneObject.DeactivateObject();
            }
        }
        public override void SceneDeintialization()
        {
            RestoreXRManagerElements();

            DeinitializeNight();

            //Destroy(this.gameObject);
        }
        private void DeinitializeNight()
        {
            if (!IsNight) return;

            RenderSettings.skybox = _oldSkybox;
            DynamicGI.UpdateEnvironment();
        }
        public override void SceneSetup()
        {
            PreparePlane();

            XROrigin origin = FindFirstObjectByType<XROrigin>();
            if (origin != null)
            {
                WeatherSettings.GetComponent<PositionConstraint>().AddSource(new ConstraintSource() { sourceTransform = origin.transform, weight = 1 });
                WeatherSettings.GetComponent<PositionConstraint>().constraintActive = true;
            }

            base.SceneSetup();
        }
        private void OverrideXRManagerElements()
        {
            _initialRightXRInteractorLineHitClosestOnly = XROriginSetupManager.RightXRRayInteractor.hitClosestOnly;
            _initialLefttXRInteractorLineHitClosestOnly = XROriginSetupManager.LeftXRRayInteractor.hitClosestOnly;
            _initialRightMaxRaycastDistance = XROriginSetupManager.RightXRRayInteractor.maxRaycastDistance;
            _initialLeftMaxRaycastDistance = XROriginSetupManager.LeftXRRayInteractor.maxRaycastDistance;

            _initialLeftXRInteractorLineAutoAdjust = XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength;
            _initialRightXRInteractorLineAutoAdjust = XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength;

            XROriginSetupManager.RightXRRayInteractor.hitClosestOnly = true;
            XROriginSetupManager.LeftXRRayInteractor.hitClosestOnly = true;
            XROriginSetupManager.RightXRRayInteractor.maxRaycastDistance = 100f;
            XROriginSetupManager.LeftXRRayInteractor.maxRaycastDistance = 100f;

            XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength = false;
            XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength = false;
        }
        private void RestoreXRManagerElements()
        {
            XROriginSetupManager.RightXRRayInteractor.hitClosestOnly = _initialRightXRInteractorLineHitClosestOnly;
            XROriginSetupManager.LeftXRRayInteractor.hitClosestOnly = _initialLefttXRInteractorLineHitClosestOnly;
            XROriginSetupManager.RightXRRayInteractor.maxRaycastDistance = _initialRightMaxRaycastDistance;
            XROriginSetupManager.LeftXRRayInteractor.maxRaycastDistance = _initialLeftMaxRaycastDistance;

            XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength = _initialLeftXRInteractorLineAutoAdjust;
            XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength = _initialRightXRInteractorLineAutoAdjust;
            ResetInteractor(XROriginSetupManager.RightXRRayInteractor);
            ResetInteractor(XROriginSetupManager.LeftXRRayInteractor);
        }
        private void ResetInteractor(XRRayInteractor interactor)
        {
            if (interactor != null)
            {
                interactor.enabled = false;
                interactor.enabled = true;
            }
        }
        private void SwitchNightObject()
        {
            if (IsNight)      
                DamageFlashlight.ActivateObject();        
            else
                DamageFlashlight.DeactivateObject();
        }
        private void SetupNight()
        {
            IsNight = true;

            LightmapSettings.lightProbes = null;
            _oldSkybox = RenderSettings.skybox;
            RenderSettings.skybox = NightSkybox;
            DynamicGI.UpdateEnvironment();

            SceneLight.intensity = 0.05f;
            SceneLight.shadows = LightShadows.None;
        }

        public override void GetScenarioDataList(byte[] file, string title)
        {
            bool examMode = false;
            try
            {
                JSONObject json = new JSONObject(Encoding.UTF8.GetString(file));
                int mainDamageChance;
                int fodChance;
                int markerChance;
                int airplaneType;
                int weatherType;

                json.GetField(out mainDamageChance, "mainDamageChance", 0);
                json.GetField(out fodChance, "fodChance", 0);
                json.GetField(out markerChance, "markerChance", 0);
                json.GetField(out airplaneType, "airplaneType", 0);
                json.GetField(out weatherType, "weatherType", 0);
                json.GetField(out examMode, "examMode", false);

                Manager.MainDamageChance = mainDamageChance;
                Manager.FODChance = fodChance;
                Manager.MarkerChance = markerChance;

                if (weatherType == 1)
                {
                    WeatherSettings.FogObject.SetActive(true);
                }
                else if (weatherType == 2)
                {
                    WeatherSettings.RainObject.SetActive(true);
                }
                else if (weatherType == 3)
                {
                    WeatherSettings.SnowObject.SetActive(true);
                }
                else if (weatherType == 4)
                {
                    SetupNight();
                }

                ChosenPlaneType = (PlaneType)airplaneType;

                Debug.Log(Manager.MainDamageChance + " " + Manager.FODChance + " " + Manager.MarkerChance + " " + ChosenPlaneType);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to load scenario, {ex.Message}");
            }

            AirportData.ExamMode = examMode;
        }
    }
}