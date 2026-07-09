using UnityEngine; // Asked AI how to properly auto assign the player to Game Manager

namespace game
{
    // Contains members that will reference audio clips
    public enum SoundType 
    {
        GUNSHOT
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [Tooltip("List of sound effects.")]
        [SerializeField] private AudioClip[] soundList;

        // Reference to AudioSource
        private AudioSource audioSource;

        // Sets audioSource
        private void Start() 
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Plays sound effect
        public void PlaySound(SoundType sound, float volume = 1) 
        {
            Debug.Log(sound + "sound played");
            audioSource.PlayOneShot(soundList[(int) sound], volume);
        }
    }
}
