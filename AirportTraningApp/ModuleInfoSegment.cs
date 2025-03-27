using TMPro;
using UI.CanvasManger;
using UnityEngine;

namespace UI.Segments
{
    public class ModuleInfoSegment : MonoBehaviour
    {
        public TextMeshProUGUI TimeText;

        private void Awake() => Initialize();
        public virtual void Initialize()
        {
            CanvasManagerBase.OnTimeChanged.AddListener(SetTimeText);
        }
        private void SetTimeText(string timeText) 
        {
            TimeText.SetText(timeText);
        }
    }
}