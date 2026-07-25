using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Health the object starts with and its maximum health.")]
        [SerializeField] private float baseHealth;

        // Maximum health of the object
        private float maxHealth;
        // Current health of the object
        protected float currentHealth;

        // Sets currentHealth and maxHealth to baseHealth on Start
        public virtual void Start() 
        {
            maxHealth = baseHealth;
            currentHealth = baseHealth;
        }

        // Returns currentHealth
        public float GetHealth() 
        {
            return currentHealth;
        }

        // Sets currentHealth
        public void SetHealth(float h) 
        {
            currentHealth = h;
        }

        // Increases currentHealth by h and makes sure that health does not increase past maxHealth
        public void Heal(float h) 
        {
            currentHealth += h;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        // Lowers currentHealth by damage
        public virtual void TakeDamage(float damage) 
        {
            currentHealth -= damage;
        }

        // Checks if the health is 0
        public bool CheckIfDead()
        {
            return currentHealth <= 0;
        }
    }
}
