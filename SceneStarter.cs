using System.Net;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Module7
{
    public class SceneStarter : MonoBehaviour, IScenarioReceiver
    {
        [Header("SceneStarter Setup")]
        [Space]
        public DamageManager DamageManager;
        public XRUIInputOverride XRUIInputOverride;
        public GameObject PathArrowsObject;
        public Transform StartingPositionTransform;
        public bool IsTraining;

        private QueryTriggerInteraction _initialRightXRRayInteractorTriggerInteraction;
        private QueryTriggerInteraction _initialLeftXRRayInteractorTriggerInteraction;
        private bool _initialLeftXRInteractorLineAutoAdjust;
        private bool _initialRightXRInteractorLineAutoAdjust;
        public void StartGame(byte[] scenarioFile = null, IPEndPoint serverToConnect = null)
        {
            SceneInitialization();
        }

        private void SceneInitialization()
        {
            if (IsTraining)
            {
                PathArrowsObject.ActivateObject();
            }
            else
            {
                PathArrowsObject.DeactivateObject();
            }

            DamageManager.InitializeDamage();
            XRUIInputOverride.Initialize();
            OverrideXRManagerElements();
            DamageManager.OnSummaryEnable.AddListener(SceneDenitialization, removeListenerBeforeAddNew: true);
        }

        private void SceneDenitialization()
        {
            XRUIInputOverride.Deinitialize();

            RestoreXRManagerElements();
        }
        private void OverrideXRManagerElements() 
        {
            _initialRightXRRayInteractorTriggerInteraction = XROriginSetupManager.RightXRRayInteractor.raycastTriggerInteraction;
            _initialLeftXRRayInteractorTriggerInteraction = XROriginSetupManager.LeftXRRayInteractor.raycastTriggerInteraction;
            _initialLeftXRInteractorLineAutoAdjust = XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength;
            _initialRightXRInteractorLineAutoAdjust = XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength;

            XROriginSetupManager.CompleteXROriginSetup.SetTransform(StartingPositionTransform, isScaleToSet: false);
            XROriginSetupManager.RightXRRayInteractor.raycastTriggerInteraction = QueryTriggerInteraction.Collide;
            XROriginSetupManager.LeftXRRayInteractor.raycastTriggerInteraction = QueryTriggerInteraction.Collide;
            XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength = false;
            XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength = false;
        }
        private void RestoreXRManagerElements()
        {
            XROriginSetupManager.RightXRRayInteractor.raycastTriggerInteraction = _initialRightXRRayInteractorTriggerInteraction;
            XROriginSetupManager.LeftXRRayInteractor.raycastTriggerInteraction = _initialLeftXRRayInteractorTriggerInteraction;
            XROriginSetupManager.LeftXRInteractorLineVisual.autoAdjustLineLength = _initialLeftXRInteractorLineAutoAdjust;
            XROriginSetupManager.RightXRInteractorLineVisual.autoAdjustLineLength = _initialRightXRInteractorLineAutoAdjust;
        }
    }
}
