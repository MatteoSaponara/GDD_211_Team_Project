using System;
using System.Collections;
using UnityEngine; // Assited with AI

namespace game
{
    [RequireComponent(typeof(Health))]
    public abstract class Enemy : MonoBehaviour, IDamagable
    {
        public static event Action<Enemy> OnEnemyKilled;
        [Header("References")]
        [Tooltip("Animator of the enemy.")]
        [SerializeField] protected Animator animator;

        [Header("Stats")]
        [Tooltip("The speed of the enemy.")]
        [SerializeField] protected float moveSpeed;
        [Tooltip("The range at which the enemy will chase the player if they are not within.")]
        [SerializeField] protected float chaseRange;

        [Header("Damage Flash")]
        [Tooltip("Color of damage flash.")]
        [SerializeField] private Color flashColor = Color.red;
        [Tooltip("Length of damage flash.")]
        [SerializeField] private float flashDuration = 0.1f;

        protected Rigidbody rb; // Rigidbody of enemy

        // Mesh renderer of GameObject
        private Renderer rend;

        // Original color of GameObject
        private Color originalColor;

        protected Transform player;

        // Health of enemy
        private Health health;

        // Determines if the enemy is dead
        protected bool isDead = false;

        public virtual void Awake()
        {
            health = GetComponent<Health>(); // Sets health to the enemy's health component
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Debug.Log("Enemy start");
            // Get player reference
            player = GameManager.instance.player.transform;
            if (player != null)
            {
                Debug.Log("Player found by enemy");
            }

            // Get mesh renderer reference
            rend = GetComponent<MeshRenderer>();
            if (rend == null)
            {
                
                Debug.Log("Mesh renderer was not found");
            }
            else
            {
                Debug.Log("Mesh renderer found");
                originalColor = rend.material.color;
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {
            // Checks for player
            if (player == null)
            {
                return;
            }

            // Makes enemy always face the player. Might want to remove and only put into subclasses depending on enemy
            //Debug.Log("Rotating toward: " + player.position);
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0; // prevents tilting
            transform.rotation = Quaternion.LookRotation(lookPos);
        }

        // Basic attack of the enemy
        public virtual void BaseAttack()
        {

        }

        // Lowers enemy health by damage and calls death
        public virtual void TakeDamage(float damage)
        {
            health.TakeDamage(damage);
            Flash();
            if (health.CheckIfDead())
            {
                Death();
            }
        }

        // Kills the enemy
        public virtual void Death()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;
            animator.SetBool("Die", isDead);
            OnEnemyKilled?.Invoke(this);
            // Stops movement
            rb.linearVelocity = Vector3.zero;
        }

        // Destroys self
        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        // Makes the GameObject flash a color
        private IEnumerator DoFlash()
        {
            rend.material.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            rend.material.color = originalColor;
        }

        // Calls DoFlash courtine
        public void Flash()
        {
            StopCoroutine("DoFlash");
            StartCoroutine(DoFlash());
        }
    }
}
