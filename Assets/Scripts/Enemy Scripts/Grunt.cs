using UnityEngine; // Assited with AI

namespace game
{
    public class Grunt : Enemy 
    {
        [Header("References")]
        [Tooltip("Spawn point of projectiles.")]
        [SerializeField] private Transform firePoint;
        [Tooltip("Prefab of projectile.")]
        [SerializeField] private GameObject projectilePrefab;

        [Header("Shooting")]
        [Tooltip("Firerate of enemy projectiles.")]
        [SerializeField] private float fireRate = 1f;

        private float fireTimer;

        public override void Update()
        {
            base.Update();
            if (Vector3.Distance(transform.position, player.position) >= chaseRange)
            {
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Timing between firing
                fireTimer -= Time.deltaTime;

                if (fireTimer <= 0f)
                {
                    BaseAttack();
                    fireTimer = (1f / fireRate) - 0.5f + Random.value; // Resets timer with a bit of randomness
                }
            }
        }

        // Fires projectiles at the player
        public override void BaseAttack()
        {
            // Direction from fire point to player
            Vector3 direction = (player.position - firePoint.position).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            proj.GetComponent<Projectile>().Launch(direction);
        }
    }
}
