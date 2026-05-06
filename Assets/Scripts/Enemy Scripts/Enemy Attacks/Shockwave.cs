using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class Shockwave : MonoBehaviour // Created with help from AI
    {
        [Header("Expansion Settings")]
        [Tooltip("Maximum radius of the shockwave.")]
        [SerializeField] private float maxRadius = 15f;

        [Tooltip("How fast the shockwave expands.")]
        [SerializeField] private float expansionSpeed = 12f;

        [Tooltip("Thickness of the shockwave hitbox.")]
        [SerializeField] private float thickness = 1.0f;

        [Header("Damage")]
        [Tooltip("How much damage the shockwave does.")]
        [SerializeField] private int damage = 15;

        [Tooltip("The layer that the player will hit")]
        [SerializeField] private LayerMask hitLayers;

        // Tracks radius of the shockwave
        private float currentRadius = 0f;

        // Tracks previous frame radius for sweep detection
        private float previousRadius = 0f;

        // Allows other scripts to read the radius
        public float CurrentRadius => currentRadius;

        // Tracks what has already been damaged
        private HashSet<PlayerHealth> hitTargets = new HashSet<PlayerHealth>();

        void Update()
        {
            ExpandRing();
            CheckHits();
        }

        void ExpandRing()
        {
            // Store previous radius before expanding
            previousRadius = currentRadius;

            currentRadius += expansionSpeed * Time.deltaTime;

            if (currentRadius >= maxRadius)
            {
                Destroy(gameObject);
            }
        }

        void CheckHits()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                currentRadius,
                hitLayers
            );

            foreach (Collider hit in hits)
            {
                // Gets the closest point on the collider
                Vector3 closestPoint = hit.ClosestPoint(transform.position);

                float dist = Vector3.Distance(
                    transform.position,
                    closestPoint
                );

                // Sweep-based ring detection to prevents fast waves from skipping targets
                if (dist < previousRadius || dist > currentRadius)
                    continue;

                // Gets health from parent object
                var health = hit.GetComponentInParent<PlayerHealth>();

                if (health == null)
                    continue;

                var controller = hit.GetComponentInParent<CharacterController>();

                if (controller != null && !controller.isGrounded)
                    continue;

                // Prevents multiple hits
                if (hitTargets.Contains(health))
                    continue;

                hitTargets.Add(health);

                Debug.Log("Shockwave dealt damage to the player: " + damage);

                health.TakeDamage(damage);
            }
        }

        // Visualize the shockwave ring in Scene view
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, previousRadius);
        }
    }
}