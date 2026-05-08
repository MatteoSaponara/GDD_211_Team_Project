using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace game
{
    public class PlayerHealth : Health, IDamagable
    {
        [SerializeField] private UnityEvent death;
        [Tooltip("Toogle for player invincibility for debuging")]
        [SerializeField] private bool debugInvincibility = false;
        [Tooltip("Health UI Text")]
        [SerializeField] private TextMeshProUGUI healthText;

        public override void Start()
        {
            base.Start();
            UpdateHealthUI();
        }

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

        public void Death()
        {
            death.Invoke();
        }
        private void UpdateHealthUI()
        {
            healthText.text = currentHealth + " HP";
        }
    }
}
