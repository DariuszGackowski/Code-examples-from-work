using AllExtensions;
using UnityEngine;

namespace Module7
{
    public class MinimizeSegment : MonoBehaviour
    {

        public GameObject MinimizeTextObject;
        public GameObject UnminimizeTextObject;
        public GameObject PCPanel;
        public bool IsUnminimized => PCPanel.activeSelf;

        public void Toggle()
        {
            if (IsUnminimized)
            {
                PCPanel.DeactivateObject();
                MinimizeTextObject.DeactivateObject();
                UnminimizeTextObject.ActivateObject();
            }
            else
            {
                PCPanel.ActivateObject();
                MinimizeTextObject.ActivateObject();
                UnminimizeTextObject.DeactivateObject();

                Canvas.ForceUpdateCanvases();
            }
        }

        public void Deactivate()
        {
            PCPanel.DeactivateObject();
            MinimizeTextObject.DeactivateObject();
            UnminimizeTextObject.ActivateObject();
        }
    }
}
