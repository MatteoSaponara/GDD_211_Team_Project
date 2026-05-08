using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class MonkeyKing : Enemy // Asked ChatGPT with help setting up the slam attack
    {
        [Header("References")]
        [Tooltip("Prefab of the slam shockwave.")]
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

        private Animator animator; // Animator of the Monkey King

        private bool isAttacking = false; // Determines if the Monkey King is attacking
        private bool onCooldown = false; // Determines if the Monkey King's slam is on cooldown

        public override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

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

            if (shouldChase)
            {
                MoveTowardsPlayer();
            }
            else
            {
                animator.SetBool("IsWalking", false);
                BaseAttack();
            }
        }

        // Initiates SlamAttack
        public override void BaseAttack()
        {
            if (!isAttacking && !onCooldown)
            {
                StartCoroutine(SlamAttack());
            }
        }

        // Goes throuugh slam attack process
        private IEnumerator SlamAttack()
        {
            isAttacking = true;

            animator.SetBool("IsWalking", false);
            animator.SetTrigger("SlamTrigger");

            // WINDUP
            // Play windup animation if availiable
            Debug.Log("Windup... ");

            // Wait for animation to finish
            yield return new WaitForSeconds(slamAnimationLength);

            // COOLDOWN
            isAttacking = false;
            onCooldown = true;

            yield return new WaitForSeconds(coolDownTime);
            onCooldown = false;
        }

        public void Impact()
        {
            DoImpactHit();
            // Creates shockwave
            Instantiate(shockwavePrefab, slamPoint.position, Quaternion.identity);
        }

        // Creates initial slam hitbox
        private void DoImpactHit()
        {
            Collider[] hits = Physics.OverlapSphere(slamPoint.position, impactRadius, playerLayer);

            foreach(Collider hit in hits)
            {
                // Try to damage player
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

        public override void Death()
        {
            SceneManager.LoadScene("Win Screen");
            base.Death();
        }

        // Makes Monkey King walk towards the player
        private void MoveTowardsPlayer()
        {
            animator.SetBool("IsWalking", true);

            Vector3 direction = (player.position - transform.position);
            direction.y = 0;

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        // Gizmo for initial slam hitbox
        void OnDrawGizmosSelected()
        {
            if (slamPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(slamPoint.position, impactRadius);
        }
    }
}
