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

        // Moves grunt towards player and attacks them in range
        public override void Update()
        {
            base.Update();
            if (Vector3.Distance(transform.position, player.position) >= chaseRange)
            {
                // INSERT: Walking sound effect
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Timing between firing
                fireTimer -= Time.deltaTime;

                if (fireTimer <= 0f)
                {
                    BaseAttack();
                    // Resets timer with a bit of randomness
                    fireTimer = (1f / fireRate) - 0.5f + Random.value; 
                }
            }
        }

        // Fires projectiles at the player
        public override void BaseAttack()
        {
            //INSERT: Grunt firing sound effect

            // Direction from fire point to player
            Vector3 direction = (player.position - firePoint.position).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            proj.GetComponent<Projectile>().Launch(direction);
        }
    }
}
