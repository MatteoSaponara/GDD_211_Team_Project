using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class MonkeyKing : Enemy // Asked ChatGPT with help setting up the slam attack
    {
        [Header("References")]
        [Tooltip("Prefab of the slam's shockwave.")]
        [SerializeField] private GameObject shockwavePrefab;
        [Tooltip("Point of the slam attack.")]
        [SerializeField] private Transform slamPoint;
        [Tooltip("Layer of the player for the slam to hit them.")]
        [SerializeField] private LayerMask playerLayer;

        [Header("Attack")]
        [Tooltip("Radius of the initial slam.")]
        [SerializeField] private float impactRadius = 2.5f;
        [Tooltip("Damage of the initial slam.")]
        [SerializeField] private float impactDamage = 30f;
        [Tooltip("Cooldown time for the slam attack.")]
        [SerializeField] private float coolDownTime = 0.3f;
        [Tooltip("Length of the slam animation.")]
        [SerializeField] private float slamAnimationLength = 3f;
        [Tooltip("When the impact happens during the slam animation.")]
        [SerializeField] private float impactTime = 1.5f;

        // Animator of the Monkey King
        private Animator animator;

        // Determines if the Monkey King is attacking
        private bool isAttacking = false;
        // Determines if the Monkey King's slam is on cooldown
        private bool onCooldown = false; 

        // Gets Animator
        // NOTE: Check up on this when working on the Monkey King's Animator
        public override void Awake()
        {
            base.Awake();

            animator = GetComponentInChildren<Animator>();

            if (animator == null)
            {
                Debug.LogError("Animator not found on Monkey King!");
            }
        }
        // Determines whether the Monkey King should chase the player or do an attack
        public override void Update()
        {
            base.Update();

            if (player == null || isDead)
                return;

            float distance = Vector3.Distance(transform.position, player.position);

            bool shouldChase =
                distance >= chaseRange &&
                !isAttacking &&
                !onCooldown;

            // Single source of truth for animation state
            animator.SetBool("IsWalking", shouldChase && !isAttacking);

            if (shouldChase)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (!isAttacking && !onCooldown)
                    BaseAttack();
            }
        }

        // Initiates slam attack
        public override void BaseAttack()
        {
            if (!isAttacking && !onCooldown)
            {
                StartCoroutine(SlamAttack());
            }
        }

        // Slam attack process
        private IEnumerator SlamAttack()
        {
            isAttacking = true;

            animator.SetTrigger("SlamTrigger");

            Debug.Log("Windup... ");

            yield return new WaitForSeconds(impactTime);

            Impact();

            yield return new WaitForSeconds(slamAnimationLength - impactTime);

            isAttacking = false;
            onCooldown = true;

            yield return new WaitForSeconds(coolDownTime);

            onCooldown = false;
        }

        // Creates impact hit and a shockwave
        public void Impact()
        {
            // INSERT: Impact sound effect
            DoImpactHit();
            Instantiate(shockwavePrefab, slamPoint.position, Quaternion.identity);
        }

        // Impact hit
        private void DoImpactHit()
        {
            Collider[] hits = Physics.OverlapSphere(slamPoint.position, impactRadius, playerLayer);

            foreach (Collider hit in hits)
            {
                var health = hit.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    Debug.Log("King slam deals " + impactDamage + " damage to the player");
                    health.TakeDamage(impactDamage);
                }
                else
                {
                    Debug.Log("Impact could not find the player's health");
                }
            }
        }

        // As of now: initiates win state when the Monkey King dies
        public override void Death()
        {
            // INSERT: Death sound effect (maybe)
            SceneManager.LoadScene("WinScreen");
            base.Death();
        }

        // Makes the Monkey King moves towards the player
        private void MoveTowardsPlayer()
        {
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;

            rb.MovePosition(rb.position + direction.normalized * moveSpeed * Time.deltaTime);
        }

        // Gizmos for visualizing the impact hitbox
        void OnDrawGizmosSelected()
        {
            if (slamPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(slamPoint.position, impactRadius);
        }
    }
}