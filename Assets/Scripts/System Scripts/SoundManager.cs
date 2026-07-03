using UnityEngine; // Asked AI how to properly auto assign the player to Game Manager

namespace game
{

    public enum SoundType // Contains memebers that will reference audio clips
    {
        GUNSHOT
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] soundList; // List of AudioClips
        private AudioSource audioSource; // Reference to AudioSource

        private void Start() // Sets audioSource
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundType sound, float volume = 1) // Plays sound effect
        {
            Debug.Log(sound + "sound played");
            audioSource.PlayOneShot(soundList[(int) sound], volume);
        }
    }
}
