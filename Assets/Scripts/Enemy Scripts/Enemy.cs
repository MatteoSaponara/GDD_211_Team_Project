using Game;
using UnityEngine; // Assited with AI

namespace game
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IDamagable
    {
        [Header("References")]
        [Tooltip("Position of the player")]
        [SerializeField] private Transform player;
        [Tooltip("Spawn point of projectiles")]
        [SerializeField] private Transform firePoint;
        [Tooltip("Prefab of projectile")]
        [SerializeField] private GameObject projectilePrefab;

        [Header("Shooting")]
        [SerializeField] private float fireRate = 1f;

        private Health health; // Health of enemy
        
        private float fireTimer;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            // Checks for player
            if (player == null)
            {
                return;
            }

            // Makes enemy always face the player
            Debug.Log("Rotating toward: " + player.position);
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0; // prevents tilting
            transform.rotation = Quaternion.LookRotation(lookPos);

            // Timing between firing
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = 1f / fireRate;
            }
        }

        // Firing of projectile
        void Shoot()
        {
            // Direction from fire point to player
            Vector3 direction = (player.position - firePoint.position).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            proj.GetComponent<Projectile>().Launch(direction);
        }

        //Lowers enemy health by damage and calls death
        public void TakeDamage(float damage)
        {
            health.TakeDamage(damage);
            if (health.CheckIfDead())
            {
                Death();
            }
        }

        public void Death()
        {
            Destroy(gameObject);
        }
    }
}
