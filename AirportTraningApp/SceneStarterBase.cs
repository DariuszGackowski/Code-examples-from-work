using AllExtensions;
using System.Collections;
using System.Net;
using UI.CanvasManger;
using Unity.Netcode;
using UnityEngine;

namespace SceneStarter
{
    public abstract class SceneStarterBase : MonoBehaviour, IScenarioReceiver
    {
        public Transform StartingPositionTransform;
        public GameObject TransitionObject;

        public void StartGame(byte[] scenarioFile = null, string title = "", IPEndPoint serverToConnect = null)
        {
            GetScenarioDataList(scenarioFile, title);
            SceneInitialization();
        }
        // Only called in StartGame method. Override if you want extend a initialization
        public virtual void SceneInitialization()
        {
            SceneSetup();

            CanvasManagerBase.OnDeinitializationScene.AddListener(SceneDeintialization);
            CanvasManagerBase.OnSceneReload.AddListener(SceneSetup);
            CanvasManagerBase.OnOtherSceneStart.AddListener(SetActiveTransition);
        }

        public void StopGame()
        {
            NetworkManager.Singleton.Shutdown();
        }

        public abstract void GetScenarioDataList(byte[] file, string title);
        // Only called on reload and initialize scene. Add methods here that need to be called on these two case
        public virtual void SceneSetup()
        {
            SetStartPlayerPosition();

            CanvasManagerBase.OnInitializationScene.Invoke();
        }
        // Only called in the OnDeinitializationScene event. Add methods here that have to be called before scene changes
        public abstract void SceneDeintialization();

        // Only called in SceneSetup method te set staritng postion for player.
        private void SetStartPlayerPosition()
        {
            SetPlayerPosition(StartingPositionTransform);
        }
        // Only called in teleportation method for teleport button.
        public static void SetNewPlayerPosition(Transform transform)
        {
            SetPlayerPosition(transform);
        }
        // Only for setting new player postion
        public static void SetPlayerPosition(Transform transform)
        {
            XROriginSetupManager.XROriginTransform.SetTransform(transform);
        }
        // Only for activating Transition game object that loads a new scene
        public virtual void SetActiveTransition() 
        {
            StartCoroutine(SetScoreDataCoroutine());
        }

        private IEnumerator SetScoreDataCoroutine()
        {
            FindFirstObjectByType<CanvasManagerBase>().DeactivateCanvasVR();

            yield return null;

            TransitionObject.ActivateObject();
        }
    }
}