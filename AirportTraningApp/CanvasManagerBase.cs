using AllExtensions;
using CustomLocalisation;
using Module7;
using UnityEngine;
using UnityEngine.Events;

namespace UI.CanvasManger
{
    public abstract class CanvasManagerBase : MonoBehaviour
    {
        public static UnityEvent OnSegmentsInitialization = new UnityEvent(); // Only for ModeInfoSegment and ScrollViewSegment.
        public static UnityEvent<Vector2> OnScrollPositionChanged = new UnityEvent<Vector2>(); // Only for scroll element synchronization.
        public static UnityEvent OnToggleCanvasPCVisibility = new UnityEvent(); // Only for watch's ToggleCanvasVRVisibility method.
        public static UnityEvent OnToggleCanvasVRVisibility = new UnityEvent(); // Only for watch's ToggleCanvasVRVisibility method.
        public static UnityEvent<string> OnTimeChanged = new UnityEvent<string>(); // Only for Watch and ModeInfoSegment.
        public static UnityEvent OnCanvasVRPositionChanged = new UnityEvent(); //Only for watch when Player teleports to add new position for CanvasVR.
        public static UnityEvent OnSceneExit = new UnityEvent(); // Only for calling a LoadNewScene method.
        public static UnityEvent OnOtherSceneStart = new UnityEvent(); // Only for activate transition object from SceneStarterBase.
        public static UnityEvent OnSceneReload = new UnityEvent(); // Add here methods which are called when scene is reloaded but not initialized.
        public static UnityEvent OnInitializationScene = new UnityEvent(); // Add here methods which are called when scene is reloded or initialized.
        public static UnityEvent OnDeinitializationScene = new UnityEvent(); // Add here methods which are called when scene is changed to other scene.

        public static bool IsStarted;
        public static Vector2 SegementCanvasScrollsPosition;

        [Space]
        public Transform VRContent;
        public Transform PCContent;
        [Space]
        public GameObject CanvasVR;
        public GameObject CanvasPC;

        [Header("If you don't use StartCanvas does not assign this here")]
        public GameObject StartCanvas;
        [Space]
        public string ModuleNameID;
        public float ModuleTime;
        public Transform CanvasVRTransform => CanvasVR.transform;
        public Transform CanvasPCTransform => CanvasPC.transform;
        public Transform StartCanvasTransform => StartCanvas.transform;

        public MinimizeSegment PCMinimizeSegment;

        public bool NonMovableCanvasVR;
        public bool IsCanvasVRActive = false;
        public bool IsCanvasPCActive => CanvasPC.activeSelf;
        public bool IsStartCanvasActive => StartCanvas.activeSelf;

        protected Vector3 _startCanvasVRPosition = new(0f, 1.6f, 0f);
        protected Vector3 _startCanvasVRRotation = new(0f, 180f, 0f);

        public void SetModuleName(string text)
        {
            ModuleNameID = text;
        }

        private void OnDestroy()
        {
            OnSegmentsInitialization.RemoveAllListeners();
            OnScrollPositionChanged.RemoveAllListeners();
            OnToggleCanvasVRVisibility.RemoveAllListeners();
            OnTimeChanged.RemoveAllListeners();
            OnCanvasVRPositionChanged.RemoveAllListeners();
            OnSceneExit.RemoveAllListeners();
            OnOtherSceneStart.RemoveAllListeners();
            OnSceneReload.RemoveAllListeners();
            OnInitializationScene.RemoveAllListeners();
            OnDeinitializationScene.RemoveAllListeners();
        }
        private void Awake() => SetupCanvasManager();

        // Only called on Awake.
        public virtual void SetupCanvasManager()
        {
            XROriginSetupManager.XRWatch.Setup(CustomLocalisationSettings.Singleton.GetTranslation(ModuleNameID));
            XROriginSetupManager.XRTimer.Setup(ModuleTime);

            OnSegmentsInitialization.Invoke();

            OnSceneExit.AddListener(LoadNewScene);
            OnSceneReload.AddListener(ResetCanvasManager);
            if (!NonMovableCanvasVR)
            {
                OnCanvasVRPositionChanged.AddListener(SetCanvasVRPosition);
                OnToggleCanvasVRVisibility.AddListener(delegate { SetCanvasVRPosition(); ToggleCanvasVRVisibility(); });
                CanvasVRTransform.SetParent(null);
            }
            OnOtherSceneStart.RemoveAllListeners();

            ResetCanvas();
        }
        // Only called on StartButton in StartCanvas. You can add this to SetupCanvasManager when you override SetupCanvasManager method to call on Awake.
        public virtual void StartModule()
        {
            IsStarted = true;
        }
        // Only called on OnSceneReload event. Method used to reset canvas manager when is reloded.
        public virtual void ResetCanvasManager()
        {
            IsStarted = false;

            ResetCanvas();
        }
        // Called in ResetCanvasManager method and SetupCanvasManager method. Method used to reset any canvas object.
        public virtual void ResetCanvas()
        {
            DeactivateCanvasPC();
            DeactivateCanvasVR();
            ActivateStartCanvas();

            ResetCanvasVRPosition();
        }
        // Only called in ResetCanvas method. Method used to reset canvasVR position to position at start scene.
        public virtual void ResetCanvasVRPosition()
        {
            CanvasVRTransform.position = _startCanvasVRPosition;
            CanvasVRTransform.localEulerAngles = _startCanvasVRRotation;
        }
        // Only called in OnWatchPositionChanged event.Method which set new position for CanvasVR. 
        public virtual void SetCanvasVRPosition()
        {
            
            Transform parent = CanvasVRTransform.parent;

            Vector3 pos = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * 2f, _startCanvasVRPosition.y, Camera.main.transform.position.z + Camera.main.transform.forward.z * 2f);

            CanvasVRTransform.position = pos;

            Vector3 directionToCamera = Camera.main.transform.position - CanvasVRTransform.position;
            directionToCamera.y = 0;
            directionToCamera.Normalize();
            float angle = Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;

            CanvasVRTransform.rotation = Quaternion.Euler(0, angle, 0);

            CanvasVRTransform.transform.SetParent(parent);

            Debug.Log("SetCanvasVRPosition " + pos.ToString());
        }
        // Only called on OnSceneExit event. Method used to prepare summary by SetupSummary method and used to calling OnDeinitializationScene event and OnOtherSceneStart event.
        public virtual void LoadNewScene()
        {
            SetupSummary();

            OnDeinitializationScene.Invoke();
            OnOtherSceneStart.Invoke();
        }
        // Only called in LoadNewScene method. Method used to prepare summary.
        public abstract void SetupSummary();

        // Only called on ExitButton in CanvasVR and CanvasPC. Only for call OnSceneExit event.
        [ContextMenu("Exit")]
        public virtual void Exit()
        {
            OnSceneExit.Invoke();
        }
        // Only called on ResetButton in in CanvasVR and CanvasPC. Only for call OnSceneReload event.
        public virtual void ReloadScene()
        {
            OnSceneReload.Invoke();
        }
        //Method which activate CanvasPC gameObject
        public virtual void ActivateCanvasPC()
        {
            CanvasPC.ActivateObject();
            OnToggleCanvasPCVisibility.Invoke();
        }
        //Method which activate CanvasVR gameObject
        public virtual void ActivateCanvasVR()
        {
            CanvasVR.ActivateObject();
        }
        //Method which activate StartCanvas gameObject when StartCanvas gameObject is attached to script 
        public virtual void ActivateStartCanvas()
        {
            if (StartCanvas == null) return;

            StartCanvas.ActivateObject();
        }
        //Method which deactivate CanvasPC gameObject
        public virtual void DeactivateCanvasPC()
        {
            CanvasPC.DeactivateObject();
        }
        //Method which deactivate CanvasVR gameObject
        public virtual void DeactivateCanvasVR()
        {
            //CanvasVR.transform.position = new Vector3(0, -10000, 0);
            CanvasVR.DeactivateObject();
        }
        //Method which deactivate StartCanvas gameObject when StartCanvas gameObject is attached to script 
        public virtual void DeactivateStartCanvas()
        {
            if (StartCanvas == null) return;

            StartCanvas.DeactivateObject();
        }
        //Only called in watch's ToggleCanvasVRVisibility method. Method that turns CanvasVR on or off. Method calls OnCanvasVRPositionChanged event to change position of CanvasVR.
        public virtual void ToggleCanvasVRVisibility()
        {
            IsCanvasVRActive = !IsCanvasVRActive;

            Debug.Log("ToggleCanvasVRVisibility");
            if (IsCanvasVRActive)
            {
                OnCanvasVRPositionChanged.Invoke();
                CanvasVR.ActivateObject();
            }
            else
            {
                //CanvasVR.transform.position = new Vector3(0, -10000, 0);
                CanvasVR.DeactivateObject();
               
            }
        }
    }
}