using UnityEngine;
using TMPro;

namespace game
{
    public class ChallengeManager : MonoBehaviour // Assisted with AI
    {
        [Header("Challenge UI")]
        [Tooltip("Displays current challenge.")]
        [SerializeField] private TextMeshProUGUI challengeText;

        [Header("Challenge Settings")]
        [Tooltip("Total time player has to complete challenge.")]
        [SerializeField] private float challengeDuration = 10f;

        [Tooltip("How long the player must stand still.")]
        [SerializeField] private float requiredStillTime = 3f;

        [Tooltip("How much movement counts as moving.")]
        [SerializeField] private float movementThreshold = 0f;

        // Determines if the challenge is active
        private bool challengeActive;

        // Time left the player needs to stand still for
        private float currentStillTime;
        // Time left for the player to complete the challenge
        private float challengeTimer;

        private Vector3 lastPosition;
        private Transform player;

        private void Update()
        {
            if (!challengeActive || player == null)
            {
                return;
            }

            challengeTimer -= Time.deltaTime;

            // Detect movement
            float distanceMoved = Vector3.Distance(player.position, lastPosition);

            if (distanceMoved <= movementThreshold)
            {
                currentStillTime += Time.deltaTime;

                challengeText.text =
                    $"- STAND STILL: {currentStillTime:F1}/{requiredStillTime:F1}";
            }
            else
            {
                currentStillTime = 0f;

                challengeText.text =
                    $"STAND STILL WITHOUT MOVING!";
            }

            lastPosition = player.position;

            // Success
            if (currentStillTime >= requiredStillTime)
            {
                ChallengeSuccess();
            }

            // Failure
            if (challengeTimer <= 0f)
            {
                ChallengeFailed();
            }
        }

        public void StartChallenge()
        {
            if (challengeActive)
            {
                return;
            }

            player = GameManager.instance.player.transform;

            challengeActive = true;

            currentStillTime = 0f;
            challengeTimer = challengeDuration;

            lastPosition = player.position;

            challengeText.text =
                $"- STAND STILL FOR {requiredStillTime} SECONDS!";
        }

        private void ChallengeSuccess()
        {
            challengeActive = false;

            challengeText.text = "- I AM APPEASED";

            Debug.Log("Challenge completed!");
        }

        private void ChallengeFailed()
        {
            challengeActive = false;

            challengeText.text = "- YOU DISSAPOINTMENT ME\n  I FEEL SLUGGISH";

            Debug.Log("Challenge failed!");

            ApplyPunishment();
        }

        // Apply's punishment to player
        private void ApplyPunishment()
        {
            Debug.Log("Apply fire rate penalty");

            GameManager.instance.player.GetComponent<Shooting>().ChangeFireRate();
        }
    }
}
