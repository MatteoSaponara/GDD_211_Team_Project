using System.Collections;
using TMPro;
using UnityEngine;

namespace game
{
    public class EnemyDialogue : MonoBehaviour // Assisted with AI
    {
        [Header("References")]
        [Tooltip("Reference to text panel.")]
        [SerializeField] private GameObject dialoguePanel;
        [Tooltip("Text component of the dialogue.")]
        [SerializeField] private TMP_Text dialogueText;

        [Header("Typewriter Effect")]
        [Tooltip("Time between each letter being typed.")]
        [SerializeField] private float letterDelay = 0.1f;

        // Reference to TypeDialogue coroutine
        private Coroutine currentDialogue;

        // Sets texts (and panel)
        public void Speak(string text, float duration)
        {
            if (currentDialogue != null)
                StopCoroutine(currentDialogue);

            currentDialogue = StartCoroutine(TypeDialogue(text, duration));
        }

        private IEnumerator TypeDialogue(string text, float duration)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "";

            foreach (char letter in text)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(letterDelay);
            }

            yield return new WaitForSeconds(duration);

            dialoguePanel.SetActive(false);
        }
    }
}
