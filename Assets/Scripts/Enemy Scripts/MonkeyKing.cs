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
        [Tooltip("Windup time for the slam attack.")]
        [SerializeField] private float windupTime = 0.1f;
        [Tooltip("Cooldown time for the slam attack.")]
        [SerializeField] private float coolDownTime = 0.3f;

        private bool isAttacking = false; // Determines if the Monkey King is attacking
        private bool onCooldown = false; // Determines if the Monkey King's slam is on cooldown

        public override void Update()
        {
            base.Update();
            if (Vector3.Distance(transform.position, player.position) >= chaseRange && !isAttacking && !onCooldown)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
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

            // WINDUP
            // Play windup animation if availiable
            Debug.Log("Windup... ");
            yield return new WaitForSeconds(windupTime);

            // IMPACT FRAME
            Debug.Log("SLAM");
            DoImpactHit();
            // Spawn Shockwave
            Instantiate(shockwavePrefab, slamPoint.position, Quaternion.identity);

            // COOLDOWN
            isAttacking = false;
            onCooldown = true;

            yield return new WaitForSeconds(coolDownTime);
            onCooldown = false;
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
            base.Death();
            SceneManager.LoadScene("Win Screen");
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
