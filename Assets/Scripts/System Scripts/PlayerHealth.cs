using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class PlayerHealth : Health, IDamagable
    {
        [SerializeField] private UnityEvent death;

        public override void TakeDamage(float damage)
        {
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
