using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace game
{
    public class PlayerHealth : Health, IDamagable
    {
        [Tooltip("Unity Event for when the player reaches 0 health.")]
        [SerializeField] private UnityEvent death;
        [Tooltip("Health UI text.")]
        [SerializeField] private TextMeshProUGUI healthText;
        [Tooltip("Toggle for player invincibility for debuging.")]
        [SerializeField] private bool debugInvincibility = false;

        // Updates UI to starting health
        public override void Start()
        {
            base.Start();
            UpdateHealthUI();
        }

        // Applies damage to the player
        public override void TakeDamage(float damage)
        {
            if (debugInvincibility)
            {
                Debug.Log("The player is <TITLECARD>");
                return;
            }
            base.TakeDamage(damage);
            UpdateHealthUI();
            if (CheckIfDead())
            {
                Debug.Log("The player is dead!");
                Death();
            }
        }

        // Invokes death Unity Event
        public void Death()
        {
            death.Invoke();
        }

        // Updates the player health UI
        private void UpdateHealthUI()
        {
            healthText.text = currentHealth + " HP";
        }
    }
}
