using AllExtensions;
using CustomLocalisation;
using CustomXR;
using Module7;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Module9.SecurityThreatsSO;

namespace Module9
{
    public class ReportElement : MonoBehaviour
    {
        public Message Message;

        public TextMeshProUGUI MessageText;

        public GameObject CorerectMarker;
        public GameObject IncorectMarker;

        public Toggle MarkToggle;

        public bool IsMarked;
        public void Setup(Message message)
        {
            Message = message;

            gameObject.name = gameObject.name + "_ReportMessage";

            SetupTMPs(CustomLocalisationSettings.Singleton.GetTranslation(message.Identifier));
            SetupButtons();
            ResetMarkers();

            Canvas.ForceUpdateCanvases();
        }
        private void ResetMarkers()
        {
            CorerectMarker.DeactivateObject();
            IncorectMarker.ActivateObject();
        }
        private void SetupTMPs(string messageText)
        {
            MessageText.SetText(messageText);
        }
        private void SetupButtons()
        {
            MarkToggle.onValueChanged.AddListener(MarkObject);
        }
        [ContextMenu("MarkMessege")]
        public void MarkMessege() 
        {
            MarkToggle.onValueChanged.Invoke(!IsMarked);
        }
        public void MarkObject(bool value)
        {
            if (!AirportData.ExamMode && !Message.IsCustom)
            {
                ErrorInfoBoard.Singleton.TryShow();
            }
            else
            {
                ToggleMarker();

                Debug.Log("Mark changed " + value.ToString());

                IsMarked = value;
                Message.IsMarked = value;
            }
        }
        public void ToggleMarker()
        {
            if (IsMarked)
            {
                CorerectMarker.DeactivateObject();
                IncorectMarker.ActivateObject();
            }
            else
            {
                CorerectMarker.ActivateObject();
                IncorectMarker.DeactivateObject();
            }
        }
    }
}
