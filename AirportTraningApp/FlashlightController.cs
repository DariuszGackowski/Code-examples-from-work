using AllExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Module7
{
    public class FlashlightController : MonoBehaviour
    {
        public GameObject HintCanvas;
        public XRGrabInteractable GrabInteractable;
        public Light Flashlight;

        public InputActionReference LeftUseAction;
        public InputActionReference RightUseAction;

        private bool isGrabbed = false;

        private void Start()
        {
            if (GrabInteractable == null || HintCanvas == null || LeftUseAction == null || RightUseAction == null) return;

            LeftUseAction.action.started += UseActionPerformed;
            RightUseAction.action.started += UseActionPerformed;

            GrabInteractable.selectEntered.AddListener(OnFirstPickup);
        }
        private void UseActionPerformed(InputAction.CallbackContext obj)
        {
            if (!isGrabbed) return;
            Flashlight.enabled = !Flashlight.enabled;
        }
        private void OnFirstPickup(SelectEnterEventArgs args)
        {
            if (!DamageCanvasManager.IsStarted) return;

            HintCanvas.DeactivateObject();
            isGrabbed = true;

            GrabInteractable.selectEntered.RemoveAllListeners();
            GrabInteractable.selectEntered.AddListener(OnPickup);
            GrabInteractable.selectExited.AddListener(OnDrop);
        }
        private void OnPickup(SelectEnterEventArgs args)
        {
            isGrabbed = true;
        }
        private void OnDrop(SelectExitEventArgs args)
        {
            isGrabbed = false;
        }
    }
}