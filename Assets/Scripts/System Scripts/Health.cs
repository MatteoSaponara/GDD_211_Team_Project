using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class Health : MonoBehaviour
    {
        [Tooltip("The health the object starts with and its maximum health")]
        [SerializeField] private float baseHealth;

        private float maxHealth; // Maximum health of the object
        protected float currentHealth; // Current health of the object

        public virtual void Start() // Sets currentHealth and maxHealth to baseHealth on Start
        {
            maxHealth = baseHealth;
            currentHealth = baseHealth;
        }

        public float GetHealth() // Returns currentHealth
        {
            return currentHealth;
        }

        public void SetHealth(float h) // Sets currentHealth
        {
            currentHealth = h;
        }

        public void Heal(float h) // Increases currentHealth by h and makes sure that health does not increase past maxHealth
        {
            currentHealth += h;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public virtual void TakeDamage(float damage) // Lowers currentHealth by damage
        {
            currentHealth -= damage;
        }

        public bool CheckIfDead()
        {
            return currentHealth <= 0;
        }
    }
}
