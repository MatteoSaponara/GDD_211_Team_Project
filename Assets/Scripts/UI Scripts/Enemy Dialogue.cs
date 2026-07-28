using System.Collections;
using TMPro;
using UnityEngine;

namespace game
{
    public class EnemyDialogue : MonoBehaviour // Assisted with AI
    {
        [Tooltip("Reference to text panel.")]
        [SerializeField] private GameObject dialoguePanel;
        [Tooltip("Text component of the dialogue.")]
        [SerializeField] private TMP_Text dialogueText;

        // Sets texts (and panel)
        public void Speak(string text, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(SpeakRoutine(text, duration));
        }

        private IEnumerator SpeakRoutine(string text, float duration)
        {
            dialogueText.text = text;
            dialoguePanel.SetActive(true);

            yield return new WaitForSeconds(duration);

            dialoguePanel.SetActive(false);
        }
    }
}
