using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class PlayerHealth : Health, IDamagable
    {
        [SerializeField] private UnityEvent death;
        [Tooltip("Toogle for player invincibility for debuging")]
        [SerializeField] private bool debugInvincibility = false;

        public override void TakeDamage(float damage)
        {
            if (debugInvincibility)
            {
                Debug.Log("The player is <TITLECARD>");
                return;
            }
            base.TakeDamage(damage);
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
    }
}
