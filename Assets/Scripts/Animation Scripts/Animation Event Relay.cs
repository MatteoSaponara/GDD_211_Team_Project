using UnityEngine;

namespace game
{
    public class AnimationEventRelay : MonoBehaviour
    {
        // Calls DestroyEnemy from enemy script
        public void DestroyEnemy()
        {
            Enemy enemy = GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.DestroyEnemy();
            }
        }

        public void Shoot()
        {
            Enemy enemy = GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.BaseAttack();
            }
        }
    }
}