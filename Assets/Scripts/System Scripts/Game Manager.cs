using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace game
{
    public class GameManager : MonoBehaviour
    {
        // Singleton pattern
        public static GameManager Instance;

        public GameObject Player;

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
            if (Instance == null)
            {
                Instance = this;
                isPaused = false;
            }
            else
            {
                Debug.LogError("Two Instances! !");
            }
            DontDestroyOnLoad(gameObject);
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

    public class SoundManager
    {
        public void GunShotSound()
        {

        }
    }

    public class WaveManager
    {

    }

    public class ChallengeManager
    {

    }
}
