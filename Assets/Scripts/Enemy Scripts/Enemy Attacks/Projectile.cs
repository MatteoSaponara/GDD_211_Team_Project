using UnityEngine; // Assited with AI

namespace game
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("The speed of the projectile")]
        [SerializeField] private float speed = 10f;
        [Tooltip("How long the projectile travels for")]
        [SerializeField] private float lifetime = 3f;
        [Tooltip("Damage the projectile does")]
        [SerializeField] protected float damage = 5f;

        private Rigidbody rb;

        private void Awake()
        {
            // Assigns rigidbody
            rb = GetComponent<Rigidbody>();
        }

        // Sends the projectile in the direction at speed
        public void Launch(Vector3 direction)
        {
            rb.linearVelocity = direction * speed;
        }

        // Destroys game object after lifetime
        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("The player has been hit!");
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
