using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Module7
{
    public class XRUIInputOverride : MonoBehaviour
    {
        public InputActionAsset InputActionAsset;

        private const string _rightActionMapName = "XRI RightHand Interaction";
        private const string _leftActionMapName = "XRI LeftHand Interaction";
        private const string _actionName = "Select";
        private const string _rightTriggerBinding = "<XRController>{RightHand}/{TriggerButton}";
        private const string _leftTriggerBinding = "<XRController>{LeftHand}/{TriggerButton}";
        private const string _rightGripBinding = "<XRController>{RightHand}/{GripButton}";
        private const string _leftGripBinding = "<XRController>{LeftHand}/{GripButton}";

        public void Initialize()
        {
            ApplyBindingOverride(_leftActionMapName, _actionName, _leftGripBinding, _leftTriggerBinding);
            ApplyBindingOverride(_rightActionMapName, _actionName, _rightGripBinding, _rightTriggerBinding);
        }

        public void Deinitialize()
        {
            RemoveBindingOverride(_leftActionMapName, _actionName, _leftTriggerBinding);
            RemoveBindingOverride(_rightActionMapName, _actionName, _rightTriggerBinding);
        }

        private void ApplyBindingOverride(string actionMapName, string actionName, string oldBinding, string newBinding)
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

            action.Disable();
            int bindingIndex = -1;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].path == oldBinding)
                {
                    bindingIndex = i;
                    break;
                }
            }
            if (bindingIndex == -1)
            {
                bool newBindingExist = false;
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    if (action.bindings[i].path == newBinding)
                    {
                        newBindingExist = true;
                        break;
                    }
                }
                if (!newBindingExist)
                {
                    action.AddBinding(newBinding);
                }
            }
            else
            {
                action.ApplyBindingOverride(bindingIndex, newBinding);
            }

            action.Enable();
        }

        private void RemoveBindingOverride(string actionMapName, string actionName, string newBinding)
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

            action.Disable();
            action.RemoveAllBindingOverrides();

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].path == newBinding)
                {
                    action.ApplyBindingOverride(i, string.Empty);
                    break;
                }
            }
            action.Enable();
        }
    }
}