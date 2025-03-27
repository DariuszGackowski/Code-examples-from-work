using System.Collections;
using TMPro;
using UI.CanvasManger;
using UnityEngine;

namespace XR.Watch
{
    public class Watch : MonoBehaviour
    {
        public Transform WatchTransform;
        public TextMeshProUGUI TimeText;
        public TextMeshProUGUI ModuleNameText;

        private const string _startModuleName = "Lobby startowe";
        private const string _startModuleTime = "00:00";

        private float lastClickTime = 0;

        public bool DisableWatch = false;

        public void DisableWatchAction()
        {
            StartCoroutine(DisableWatchCoroutine());
        }

        private IEnumerator DisableWatchCoroutine()
        {
            DisableWatch = true;
            yield return new WaitForSeconds(2f);
            DisableWatch = false;
        }

        public void Setup(string moduleName)
        {
            ModuleNameText.SetText(moduleName);

            CanvasManagerBase.OnDeinitializationScene.AddListener(ResetWatch);
            CanvasManagerBase.OnTimeChanged.AddListener(SetTime);
        }
        private void ResetWatch()
        {
            ModuleNameText.SetText(_startModuleName);
            TimeText.SetText(_startModuleTime);
        }
        public void ToggleCanvasVRVisibility()
        {
            if (!CanvasManagerBase.IsStarted || PlayerSitBehaviour.Instance.ActiveSeat != null) return;

            if (DisableWatch) return;

            if(Time.time > lastClickTime + 1f)
            {
                lastClickTime = Time.time;
                CanvasManagerBase.OnToggleCanvasVRVisibility.Invoke();
            }

            
        }
        private void SetTime(string timeText)
        {
            TimeText.SetText(timeText);
        }
    }
}
