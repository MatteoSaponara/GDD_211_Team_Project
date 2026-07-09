using UnityEngine;

namespace game
{
    // Interface for objects that can take damage and be killed
    public interface IDamagable
    {
        public void TakeDamage(float damage);
        public void Death();
    }
}
