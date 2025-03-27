using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Module9.SecurityThreatsSO;

namespace Module9
{
    public class QuestionElement : MonoBehaviour
    {
        public ThreatsManager ThreatsManager;
        public RevelantTextType TextType;

        [ContextMenu("Ask about message")]
        public void AskAboutMessage()
        {
            List<Message> relevantMessages = ThreatsManager.SecurityThreatsSO.GetRelevantMessages(TextType);

            var message = relevantMessages.FirstOrDefault(message => message.IsCustom);

            if (message == null)
            {
                Debug.LogError($"Nie znaleziono pasuj¹cej wiadomoœci dla typu: {RevelantTextType.Place}");
                return;
            }

            ThreatsManager.ThreatsAudio.PlayMessagesSequentially(new List<Message> { message });

            if (message.IsAsked)
            {
                Debug.LogWarning($"Wiadomoœæ dla typu {RevelantTextType.Place} ju¿ zosta³a zapytana.");
                return;
            }

            if (message.IsToAsk)
            {
                message.IsAsked = true;
                ThreatsCanvasManager.OnStartThreatMessages.Invoke(new List<Message> { message });
            }
        }
    }
}