using UnityEngine;

namespace game
{
    public class AnimationRandomFrameStart : MonoBehaviour
    {
        // Animator of model
        private Animator animator;
        
        // Plays the models idle animation at a random frame
        void Start()
        {
            animator = GetComponent<Animator>();
            var state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
        }
    }
}
