using TMPro;
using UnityEngine; // Asked AI how to properly auto assign the player to Game Manager
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace game
{
    public class GameManager : MonoBehaviour
    {
        // Singleton pattern
        public static GameManager instance;

        [Header("References")]
        [Tooltip("Reference to the player object.")]
        public GameObject player;

        public SoundManager SoundManager => soundManager;
        public WaveManager WaveManager => waveManager;
        public ChallengeManager ChallengeManager => challengeManager;

        [Tooltip("Sound manager that plays all of the game's sounds.")]
        [SerializeField] private SoundManager soundManager;
        [Tooltip("Wave manager to handle enemy waves.")]
        [SerializeField] private WaveManager waveManager;
        [Tooltip("Challenge manager to handle enemy waves.")]
        [SerializeField] private ChallengeManager challengeManager;
        [Space]
        [Tooltip("Determines if the game is paused.")]
        public bool isPaused;

        // Sets GameManager instance and player reference
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                isPaused = false;
            }
            else
            {
                Debug.LogError("Two Instances! !");
            }
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
        }

        // Assigns player reference
        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player was not found");
            }
            else
            {
                Debug.Log("Player found");
            }
        }

        // Initiates the game over state
        public void GameOver()
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // to be replaced with an actual game over screen
            isPaused = false;
        }

        // Pauses the game
        // NOTE: Function either doesn't work, or it need to be reestablished in new scenes, either way need some touch ups
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("The player pressed pause");
                if (!isPaused)
                {
                    isPaused = true;
                    Time.timeScale = 0f;
                }
                else
                {
                    isPaused = false;
                    Time.timeScale = 1f;
                }
                if (isPaused)
                {
                    Debug.Log("Game is paused");
                }
                else
                {
                    Debug.Log("Game is unpaused");
                }
            }
        }
    }
}
    
