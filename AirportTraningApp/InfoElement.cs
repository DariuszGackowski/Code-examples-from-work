using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Module9.SecurityThreatsSO;
using AllExtensions;
using UnityEngine.UI;

namespace Module9
{
    public class InfoElement : MonoBehaviour
    {
        public TextMeshProUGUI InfoText;
        public Button VolumeButton;

        public void Setup(List<Message> messageList, ThreatsAudio threatAudio)
        {
            string fullMessageText = messageList.GetFullMessageText();
            InfoText.SetText(fullMessageText);
            VolumeButton.onClick.AddListener(delegate { threatAudio.PlayMessagesSequentially(messageList); });
        }

        [ContextMenu("PlayMessage")]
        public void PlayMessage() 
        {
            VolumeButton.onClick.Invoke();
        }
    }
}
