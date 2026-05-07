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

        public GameObject player;

        public bool isPaused;
        public SoundManager SoundManager => SoundManager;
        public WaveManager WaveManager => WaveManager;
        public ChallengeManager ChallengeManager => ChallengeManager;

        [Tooltip("Sound manager that plays all of the game's sounds")]
        [SerializeField] private SoundManager soundManager;
        [Tooltip("Wave manager to handle enemy waves")]
        [SerializeField] private WaveManager waveManager;
        [Tooltip("Challenge manager to handle enemy waves")]
        [SerializeField] private ChallengeManager challengeManager;

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


        public void GameOver() 
        {
            // Game Over Screen

            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // to be replaced with an actual game over screen
            isPaused = false;
        }

        public void OnPause(InputAction.CallbackContext context) // Pauses the game
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

    public class SoundManager : MonoBehaviour
    {
        public void GunShotSound()
        {

        }
    }

    public class ChallengeManager : MonoBehaviour
    {

    }
}
