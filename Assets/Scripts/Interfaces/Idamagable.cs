using UnityEngine;

namespace game
{
    public interface IDamagable
    {
        public void TakeDamage(float damage);
        public void Death();
    }
}
