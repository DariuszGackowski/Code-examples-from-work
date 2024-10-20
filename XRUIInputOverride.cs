using UnityEngine;
using UnityEngine.InputSystem;

namespace Module7
{
    public class XRUIInputOverride : MonoBehaviour
    {
        public InputActionAsset InputActionAsset;

        private string _originalLeftSelectBinding;
        private string _originalRightSelectBinding;

        private const string _rightActionMapName = "XRI RightHand Interaction";
        private const string _leftActionMapName = "XRI LeftHand Interaction";
        private const string _actionName = "Select";
        private const string _newRightTriggerBinding = "<XRController>{RightHand}/{TriggerButton}";
        private const string _newLeftTriggerBinding = "<XRController>{LefttHand}/{TriggerButton}";
        private const string _oldRightGripBinding = "<XRController>{RightHand}/{GripButton}";
        private const string _oldLeftGripBinding = "<XRController>{LeftHand}/{GripButton}";

        public void Initialize()
        {
            OverrideBinding(_leftActionMapName, _actionName, _newLeftTriggerBinding, _oldLeftGripBinding, ref _originalLeftSelectBinding);
            OverrideBinding(_rightActionMapName, _actionName, _newRightTriggerBinding, _oldRightGripBinding, ref _originalRightSelectBinding);
        }

        public void Deinitialize()
        {
            RestoreBinding(_leftActionMapName, _actionName, _originalLeftSelectBinding, _newLeftTriggerBinding);
            RestoreBinding(_rightActionMapName, _actionName, _originalRightSelectBinding, _newRightTriggerBinding);
        }

        private void OverrideBinding(string actionMapName, string actionName, string newBinding, string oldBinding, ref string originalBinding)
        {
            InputActionMap actionMap = InputActionAsset.FindActionMap(actionMapName);
            if (actionMap == null)
            {
                Debug.LogError($"Input action map: {actionMapName} is null");
                return;
            }

            InputAction action = actionMap.FindAction(actionName);
            if (action == null)
            {
                Debug.LogError($"Input action: {actionName} is null");
                return;
            }

            int bindingIndex = -1;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].effectivePath.Contains(oldBinding))
                {
                    bindingIndex = i;
                    break;
                }
            }

            if (bindingIndex == -1)
            {
                Debug.LogError($"Binding for {oldBinding} not found in action {actionName}");
                return;
            }

            originalBinding = action.bindings[bindingIndex].effectivePath;
            action.Disable();
            action.ApplyBindingOverride(newBinding);
            action.Enable();
        }

        private void RestoreBinding(string actionMapName, string actionName, string originalBinding, string currentBinding)
        {
            InputActionMap actionMap = InputActionAsset.FindActionMap(actionMapName);
            if (actionMap == null)
            {
                Debug.LogError($"Input action map: {actionMapName} is null");
                return;
            }

            InputAction action = actionMap.FindAction(actionName);
            if (action == null)
            {
                Debug.LogError($"Input action: {actionName} is null");
                return;
            }

            int bindingIndex = -1;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].effectivePath.Contains(currentBinding))
                {
                    bindingIndex = i;
                    break;
                }
            }

            if (bindingIndex == -1)
            {
                Debug.LogError($"Binding for {currentBinding} not found in action {actionName}");
                return;
            }

            action.Disable();
            action.ApplyBindingOverride(bindingIndex, originalBinding);
            action.Enable();
        }
    }
}
