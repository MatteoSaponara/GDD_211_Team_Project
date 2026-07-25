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

        // Determines if the grunt is walking
        private bool isWalking;

        // Cooldown between shots
        private float fireTimer;

<<<<<<< HEAD
        // Determines if the grunt should be walking towards or shooting at the player
=======
        // Moves grunt towards player and attacks them in range
>>>>>>> main
        public override void Update()
        {
            base.Update();
            if (Vector3.Distance(transform.position, player.position) >= chaseRange)
            {
<<<<<<< HEAD
                isWalking = true;
=======
                // INSERT: Walking sound effect
>>>>>>> main
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                isWalking = false;
                // Timing between firing
                fireTimer -= Time.deltaTime;

                if (fireTimer <= 0f)
                {
<<<<<<< HEAD
                    animator.SetTrigger("Shoot");
                    fireTimer = (1f / fireRate) - 0.5f + Random.value; // Resets timer with a bit of randomness
=======
                    BaseAttack();
                    // Resets timer with a bit of randomness
                    fireTimer = (1f / fireRate) - 0.5f + Random.value; 
>>>>>>> main
                }
            }
            animator.SetBool("IsWalking", isWalking);
            Debug.Log("This grunt is waling is a " + animator.GetBool("IsWalking") + " statement");
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

        public override void Death()
        {
            
            base.Death();
        }
    }
}
